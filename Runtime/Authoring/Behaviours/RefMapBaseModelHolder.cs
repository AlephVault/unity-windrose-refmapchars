using System;
using AlephVault.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;
using AlephVault.Unity.WindRose.RefMapChars.Types;
using AlephVault.Unity.WindRose.RefMapChars.Types.Traits;
using UnityEngine;


namespace AlephVault.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace Behaviours
        {
            /// <summary>
            ///   A base model RefMap applier. It tracks the sex and all
            ///   the involved parts, like this:
            ///   - Sex being M/F. It will typically not be changed later,
            ///     but depending on the game it might.
            ///   - Body being one of the colors. The same applies.
            ///   - Any item trait, being clothes or hair, ... all of
            ///     them have up to 10 color variations.
            ///   - Extra items have no color.
            /// </summary>
            public abstract class RefMapBaseModelHolder : MonoBehaviour
            {
                /// <summary>
                ///   This empty value means: do not use anything in this
                ///   trait (even if hair: it becomes bald).
                /// </summary>
                public const ushort Empty = 0;
                
                /// <summary>
                ///   The bundle. This one is only set at prefab time.
                /// </summary>
                [SerializeField]
                protected RefMapBundle bundle;

                /// <summary>
                ///   The sex to use.
                /// </summary>
                [SerializeField]
                protected RefMapBundle.SexCode sex = RefMapBundle.SexCode.Male;

                /// <summary>
                ///   The body color to use.
                /// </summary>
                [SerializeField]
                private RefMapBody.ColorCode bodyColor = RefMapBody.ColorCode.White;

                /// <summary>
                ///   The hair style. Use <see cref="Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort hair = Empty;

                /// <summary>
                ///   The hair color.
                /// </summary>
                [SerializeField]
                private RefMapAddOn.ColorCode hairColor = RefMapAddOn.ColorCode.Black;

                /// <summary>
                ///   The hat. Use <see cref="Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort hat = Empty;

                /// <summary>
                ///   The hat color.
                /// </summary>
                [SerializeField]
                private RefMapAddOn.ColorCode hatColor = RefMapAddOn.ColorCode.Black;

                /// <summary>
                ///   The necklace. Use <see cref="Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort necklace = Empty;
                
                /// <summary>
                ///   The skilled hand item. Use <see cref="Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort skilledHandItem = Empty;
                
                /// <summary>
                ///   The dumb hand item. Use <see cref="Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort dumbHandItem = Empty;

                protected virtual void Start()
                {
                    Sex = sex;
                    Necklace = necklace;
                    SkilledHandItem = skilledHandItem;
                    DumbHandItem = dumbHandItem;
                    RefreshTexture();
                }

                private void FullUpdate()
                {
                    FullUpdateFromSex(bundle[sex]);
                }

                /// <summary>
                ///   Updates everything from the given sex data.
                /// </summary>
                protected virtual void FullUpdateFromSex(RefMapSex data)
                {
                    ChangeBody(data);
                    ChangeHair(data);
                    ChangeHat(data);
                }

                private void ChangeBody(RefMapSex data)
                {
                    DoChangeBody(new BodyTrait($"{sex}{bodyColor}", data.Body[bodyColor]));
                }

                /// <summary>
                ///   Updates the body trait.
                /// </summary>
                /// <param name="body">The body trait to use</param>
                protected abstract void DoChangeBody(BodyTrait body);

                private void ChangeHair(RefMapSex data)
                {
                    HairTrait hairTrait = null;
                    RefMapSource hairTail = null;
                    if (hair != Empty)
                    {
                        try
                        {
                            try
                            {
                                hairTail = data[RefMapSex.AddOnTypeCode.HairTail][hair][hairColor];
                            }
                            catch (Exception e)
                            {
                            }
                            
                            hairTrait = new HairTrait(
                                $"{sex}{hair}{hairColor}",
                                data[RefMapSex.AddOnTypeCode.Hair][hair][hairColor],
                                hairTail
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    DoChangeHair(hairTrait);
                }

                /// <summary>
                ///   Changes the hair trait.
                /// </summary>
                /// <param name="hair">the hair trait to use</param>
                protected abstract void DoChangeHair(HairTrait hair);
                
                private void ChangeHat(RefMapSex data)
                {
                    HatTrait hatTrait = null;
                    if (hat != Empty)
                    {
                        try
                        {
                            hatTrait = new HatTrait(
                                $"{sex}{hat}{hatColor}",
                                data[RefMapSex.AddOnTypeCode.Hat][hat][hatColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    DoChangeHat(hatTrait);
                }

                /// <summary>
                ///   Changes the hat trait.
                /// </summary>
                /// <param name="hat">the hat trait to use</param>
                protected abstract void DoChangeHat(HatTrait hat);

                private void ChangeNecklace(RefMapSex data)
                {
                    NecklaceTrait necklaceTrait = null;
                    if (necklace != Empty)
                    {
                        try
                        {
                            necklaceTrait = new NecklaceTrait(
                                $"nkl{necklace}",
                                data[RefMapSex.ItemTypeCode.Necklace][necklace]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    DoChangeNecklace(necklaceTrait);
                }

                /// <summary>
                ///   Changes the necklace trait.
                /// </summary>
                /// <param name="necklace">the necklace trait to use</param>
                protected abstract void DoChangeNecklace(NecklaceTrait necklace);

                private void ChangeSkilledHandItem(RefMapSex data)
                {
                    SkilledHandItemTrait skilledHandItemTrait = null;
                    if (skilledHandItem != Empty)
                    {
                        try
                        {
                            skilledHandItemTrait = new SkilledHandItemTrait(
                                $"shi{skilledHandItem}",
                                data[RefMapSex.ItemTypeCode.SkilledHandItem][skilledHandItem]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    DoChangeSkilledHandItem(skilledHandItemTrait);
                }

                /// <summary>
                ///   Changes the skilled hand item trait.
                /// </summary>
                /// <param name="skilledHandItem">the skilled hand item trait to use</param>
                protected abstract void DoChangeSkilledHandItem(SkilledHandItemTrait skilledHandItem);

                private void ChangeDumbHandItem(RefMapSex data)
                {
                    DumbHandItemTrait dumbHandItemTrait = null;
                    if (dumbHandItem != Empty)
                    {
                        try
                        {
                            dumbHandItemTrait = new DumbHandItemTrait(
                                $"dhi{dumbHandItem}",
                                data[RefMapSex.ItemTypeCode.DumbHandItem][dumbHandItem]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    DoChangeDumbHandItem(dumbHandItemTrait);
                }

                /// <summary>
                ///   Changes the dumb hand item trait.
                /// </summary>
                /// <param name="dumbHandItem">the dumb hand item trait to use</param>
                protected abstract void DoChangeDumbHandItem(DumbHandItemTrait dumbHandItem);

                /// <summary>
                ///   Refreshes the texture of this object.
                /// </summary>
                public abstract void RefreshTexture();

                /// <summary>
                ///   the sex of this object, which also triggers the
                ///   full values refresh of the underlying applier.
                /// </summary>
                public RefMapBundle.SexCode Sex
                {
                    get { return sex; }
                    set
                    {
                        sex = value;
                        FullUpdate();
                    }
                }

                /// <summary>
                ///   The body color.
                /// </summary>
                public RefMapBody.ColorCode BodyColor
                {
                    get { return bodyColor; }
                    set
                    {
                        bodyColor = value;
                        ChangeBody(bundle[sex]);
                    }
                }

                /// <summary>
                ///   See <see cref="hair" /> and <see cref="hairColor" />.
                /// </summary>
                public Tuple<ushort, RefMapAddOn.ColorCode> Hair
                {
                    get { return new Tuple<ushort, RefMapAddOn.ColorCode>(hair, hairColor); }
                    set
                    {
                        hair = value?.Item1 ?? Empty;
                        hairColor = value?.Item2 ?? RefMapAddOn.ColorCode.Black;
                        ChangeHair(bundle[sex]);
                    }
                }
                
                /// <summary>
                ///   See <see cref="hat" /> and <see cref="hatColor" />.
                /// </summary>
                public Tuple<ushort, RefMapAddOn.ColorCode> Hat
                {
                    get { return new Tuple<ushort, RefMapAddOn.ColorCode>(hat, hatColor); }
                    set
                    {
                        hat = value?.Item1 ?? Empty;
                        hatColor = value?.Item2 ?? RefMapAddOn.ColorCode.Black;
                        ChangeHat(bundle[sex]);
                    }
                }

                /// <summary>
                ///   See <see cref="necklace" />.
                /// </summary>
                public ushort Necklace
                {
                    get { return necklace; }
                    set
                    {
                        necklace = value;
                        ChangeNecklace(bundle[sex]);
                    }
                }

                /// <summary>
                ///   See <see cref="skilledHandItem" />.
                /// </summary>
                public ushort SkilledHandItem
                {
                    get { return skilledHandItem; }
                    set
                    {
                        skilledHandItem = value;
                        ChangeSkilledHandItem(bundle[sex]);
                    }
                }

                /// <summary>
                ///   See <see cref="dumbHandItem" />.
                /// </summary>
                public ushort DumbHandItem
                {
                    get { return dumbHandItem; }
                    set
                    {
                        dumbHandItem = value;
                        ChangeDumbHandItem(bundle[sex]);
                    }
                }

                /// <summary>
                ///   Applies an entire bulk (typically, differential)
                ///   of settings to this holder.
                /// </summary>
                /// <param name="model">The model to apply</param>
                /// <returns>Whether the model was applied or not</returns>
                public virtual bool BulkApply(IRefMapBaseModel model)
                {
                    if (model == null) return false;
                    if (model.Sex != null) Sex = model.Sex.Value;
                    if (model.BodyColor != null) BodyColor = model.BodyColor.Value;
                    if (model.Necklace != null) Necklace = model.Necklace.Value;
                    if (model.SkilledHandItem != null) SkilledHandItem = model.SkilledHandItem.Value;
                    if (model.DumbHandItem != null) DumbHandItem = model.DumbHandItem.Value;
                    if (model.Hair != null) Hair = model.Hair;
                    if (model.Hat != null) Hat = model.Hat;
                    return true;
                }
            }
        }
    }
}
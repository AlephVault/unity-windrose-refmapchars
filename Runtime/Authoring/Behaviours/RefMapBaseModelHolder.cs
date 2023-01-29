using System;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types.Traits;
using UnityEngine;


namespace GameMeanMachine.Unity.WindRose.RefMapChars
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
            [RequireComponent(typeof(RefMapSimpleApplier))]
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
                private RefMapItem.ColorCode hairColor = RefMapItem.ColorCode.Black;

                /// <summary>
                ///   The hat. Use <see cref="Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort hat = Empty;

                /// <summary>
                ///   The hat color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode hatColor = RefMapItem.ColorCode.Black;

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
                    if (hair == Empty)
                    {
                        DoChangeHair(null);
                    }
                    else
                    {
                        RefMapSource hairTail = null;
                        try
                        {
                            hairTail = data[RefMapSex.ItemTypeCode.HairTail][hair][hairColor];
                        }
                        catch (Exception e)
                        {
                        }

                        HairTrait hairTrait = null;
                        try
                        {
                            hairTrait = new HairTrait(
                                $"{sex}{hair}{hairColor}",
                                data[RefMapSex.ItemTypeCode.Hair][hair][hairColor],
                                hairTail
                            );
                        }
                        catch (Exception e)
                        {
                        }
                        DoChangeHair(hairTrait);
                    }
                }

                /// <summary>
                ///   Changes the hair trait.
                /// </summary>
                /// <param name="hair">the hair trait to use</param>
                protected abstract void DoChangeHair(HairTrait hair);
                
                private void ChangeHat(RefMapSex data)
                {
                    if (hat == 0)
                    {
                        DoChangeHat(null);
                    }
                    else
                    {
                        HatTrait hatTrait = null;
                        try
                        {
                            hatTrait = new HatTrait(
                                $"{sex}{hat}{hatColor}",
                                data[RefMapSex.ItemTypeCode.Hat][hat][hatColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                        DoChangeHat(hatTrait);
                    }
                }

                /// <summary>
                ///   Changes the hat trait.
                /// </summary>
                /// <param name="hat">the hat trait to use</param>
                protected abstract void DoChangeHat(HatTrait hat);

                private void ChangeNecklace(RefMapExtra data)
                {
                    if (necklace == Empty)
                    {
                        DoChangeNecklace(null);
                    }
                    else
                    {
                        NecklaceTrait necklaceTrait = null;
                        try
                        {
                            necklaceTrait = new NecklaceTrait(
                                $"nkl{necklace}",
                                data[necklace]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                        DoChangeNecklace(necklaceTrait);
                    }
                }

                /// <summary>
                ///   Changes the necklace trait.
                /// </summary>
                /// <param name="necklace">the necklace trait to use</param>
                protected abstract void DoChangeNecklace(NecklaceTrait necklace);

                private void ChangeSkilledHandItem(RefMapExtra data)
                {
                    if (skilledHandItem == Empty)
                    {
                        DoChangeSkilledHandItem(null);
                    }
                    else
                    {
                        SkilledHandItemTrait skilledHandItemTrait = null;
                        try
                        {
                            skilledHandItemTrait = new SkilledHandItemTrait(
                                $"shi{skilledHandItem}",
                                data[skilledHandItem]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                        DoChangeSkilledHandItem(skilledHandItemTrait);
                    }
                }

                /// <summary>
                ///   Changes the skilled hand item trait.
                /// </summary>
                /// <param name="skilledHandItem">the skilled hand item trait to use</param>
                protected abstract void DoChangeSkilledHandItem(SkilledHandItemTrait skilledHandItem);

                private void ChangeDumbHandItem(RefMapExtra data)
                {
                    if (dumbHandItem == Empty)
                    {
                        DoChangeDumbHandItem(null);
                    }
                    else
                    {
                        DumbHandItemTrait dumbHandItemTrait = null;
                        try
                        {
                            dumbHandItemTrait = new DumbHandItemTrait(
                                $"dhi{dumbHandItem}",
                                data[dumbHandItem]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                        DoChangeDumbHandItem(dumbHandItemTrait);
                    }
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
                ///   See <see cref="hair" />.
                /// </summary>
                public ushort Hair
                {
                    get { return hair; }
                    set
                    {
                        hair = value;
                        ChangeHair(bundle[sex]);
                    }
                }

                /// <summary>
                ///   See <see cref="hairColor" />.
                /// </summary>
                public RefMapItem.ColorCode HairColor
                {
                    get { return hairColor; }
                    set
                    {
                        hairColor = value;
                        ChangeHair(bundle[sex]);
                    }
                }

                /// <summary>
                ///   See <see cref="hat" />.
                /// </summary>
                public ushort Hat
                {
                    get { return hat; }
                    set
                    {
                        hat = value;
                        ChangeHat(bundle[sex]);
                    }
                }

                /// <summary>
                ///   See <see cref="hatColor" />.
                /// </summary>
                public RefMapItem.ColorCode HatColor
                {
                    get { return hatColor; }
                    set
                    {
                        hatColor = value;
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
                        ChangeNecklace(bundle[RefMapBundle.ExtraItemTypeCode.Necklace]);
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
                        ChangeSkilledHandItem(bundle[RefMapBundle.ExtraItemTypeCode.SkilledHandItem]);
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
                        ChangeDumbHandItem(bundle[RefMapBundle.ExtraItemTypeCode.DumbHanItem]);
                    }
                }
            }
        }
    }
}
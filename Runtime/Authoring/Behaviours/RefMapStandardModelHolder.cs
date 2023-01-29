using System;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types.Traits;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types.Traits.Standard;
using UnityEngine;


namespace GameMeanMachine.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace Behaviours
        {
            /// <summary>
            ///   A simple model RefMap applier. Adds the tracking
            ///   of the cloth parts and their colors.
            /// </summary>
            [RequireComponent(typeof(RefMapStandardApplier))]
            public class RefMapStandardModelHolder : RefMapBaseModelHolder
            {
                // The related applier.
                private RefMapStandardApplier applier;

                /// <summary>
                ///   The boots. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort boots = Empty;

                /// <summary>
                ///   The boots color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode bootsColor = RefMapItem.ColorCode.Black;
                
                /// <summary>
                ///   The pants. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort pants = Empty;

                /// <summary>
                ///   The pants color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode pantsColor = RefMapItem.ColorCode.Black;
                
                /// <summary>
                ///   The shirt. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort shirt = Empty;

                /// <summary>
                ///   The shirt color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode shirtColor = RefMapItem.ColorCode.Black;
                
                /// <summary>
                ///   The chest. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort chest = Empty;

                /// <summary>
                ///   The chest color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode chestColor = RefMapItem.ColorCode.Black;
                
                /// <summary>
                ///   The waist. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort waist = Empty;

                /// <summary>
                ///   The waist color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode waistColor = RefMapItem.ColorCode.Black;
                
                /// <summary>
                ///   The arms. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort arms = Empty;

                /// <summary>
                ///   The arms color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode armsColor = RefMapItem.ColorCode.Black;
                
                /// <summary>
                ///   The long shirt. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort longShirt = Empty;

                /// <summary>
                ///   The long shirt color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode longShirtColor = RefMapItem.ColorCode.Black;
                
                /// <summary>
                ///   The shoulder. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort shoulder = Empty;

                /// <summary>
                ///   The shoulder color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode shoulderColor = RefMapItem.ColorCode.Black;
                
                /// <summary>
                ///   The cloak. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort cloak = Empty;

                /// <summary>
                ///   The cloak color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode cloakColor = RefMapItem.ColorCode.Black;

                protected virtual void Awake()
                {
                    applier = GetComponent<RefMapStandardApplier>();
                }

                protected override void FullUpdateFromSex(RefMapSex data)
                {
                    base.FullUpdateFromSex(data);
                    ChangeBoots(data);
                    ChangePants(data);
                    ChangeShirt(data);
                    ChangeChest(data);
                    ChangeWaist(data);
                    ChangeArms(data);
                    ChangeLongShirt(data);
                    ChangeShoulder(data);
                    ChangeCloak(data);
                }

                /// <summary>
                ///   Updates the body trait.
                /// </summary>
                /// <param name="body">The body trait to use</param>
                protected override void DoChangeBody(BodyTrait body)
                {
                    applier.Use(body, false);
                }

                /// <summary>
                ///   Changes the hair trait.
                /// </summary>
                /// <param name="hair">the hair trait to use</param>
                protected override void DoChangeHair(HairTrait hair)
                {
                    applier.Use(hair, false);
                }

                /// <summary>
                ///   Changes the hat trait.
                /// </summary>
                /// <param name="hat">the hat trait to use</param>
                protected override void DoChangeHat(HatTrait hat)
                {
                    applier.Use(hat, false);
                }

                /// <summary>
                ///   Changes the necklace trait.
                /// </summary>
                /// <param name="necklace">the necklace trait to use</param>
                protected override void DoChangeNecklace(NecklaceTrait necklace)
                {
                    applier.Use(necklace, false);
                }

                /// <summary>
                ///   Changes the skilled hand item trait.
                /// </summary>
                /// <param name="skilledHandItem">the skilled hand item trait to use</param>
                protected override void DoChangeSkilledHandItem(SkilledHandItemTrait skilledHandItem)
                {
                    applier.Use(skilledHandItem, false);
                }

                /// <summary>
                ///   Changes the dumb hand item trait.
                /// </summary>
                /// <param name="dumbHandItem">the dumb hand item trait to use</param>
                protected override void DoChangeDumbHandItem(DumbHandItemTrait dumbHandItem)
                {
                    applier.Use(dumbHandItem, false);
                }

                /// <summary>
                ///   Uses the applier to refresh the texture.
                /// </summary>
                public override void RefreshTexture()
                {
                    applier.RefreshTexture();
                }
                
                private void ChangeBoots(RefMapSex data)
                {
                    BootsTrait bootsTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            bootsTrait = new BootsTrait(
                                $"{sex}{boots}{bootsColor}",
                                data[RefMapSex.ItemTypeCode.Boots][boots][bootsColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(bootsTrait, false);
                }

                /// <summary>
                ///   See <see cref="boots" /> and <see cref="bootsColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Boots
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(boots, bootsColor); }
                    set
                    {
                        boots = value?.Item1 ?? Empty;
                        bootsColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeBoots(bundle[sex]);
                    }
                }

                private void ChangePants(RefMapSex data)
                {
                    PantsTrait pantsTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            pantsTrait = new PantsTrait(
                                $"{sex}{pants}{pantsColor}",
                                data[RefMapSex.ItemTypeCode.Pants][pants][pantsColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(pantsTrait, false);
                }

                /// <summary>
                ///   See <see cref="pants" /> and <see cref="pantsColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Pants
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(pants, pantsColor); }
                    set
                    {
                        pants = value?.Item1 ?? Empty;
                        pantsColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangePants(bundle[sex]);
                    }
                }

                private void ChangeShirt(RefMapSex data)
                {
                    ShirtTrait shirtTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            shirtTrait = new ShirtTrait(
                                $"{sex}{shirt}{shirtColor}",
                                data[RefMapSex.ItemTypeCode.Shirt][shirt][shirtColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(shirtTrait, false);
                }

                /// <summary>
                ///   See <see cref="shirt" /> and <see cref="shirtColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Shirt
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(shirt, shirtColor); }
                    set
                    {
                        shirt = value?.Item1 ?? Empty;
                        shirtColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeShirt(bundle[sex]);
                    }
                }
                
                private void ChangeChest(RefMapSex data)
                {
                    ChestTrait chestTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            chestTrait = new ChestTrait(
                                $"{sex}{chest}{chestColor}",
                                data[RefMapSex.ItemTypeCode.Chest][chest][chestColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(chestTrait, false);
                }

                /// <summary>
                ///   See <see cref="chest" /> and <see cref="chestColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Chest
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(chest, chestColor); }
                    set
                    {
                        chest = value?.Item1 ?? Empty;
                        chestColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeChest(bundle[sex]);
                    }
                }

                private void ChangeWaist(RefMapSex data)
                {
                    WaistTrait waistTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            waistTrait = new WaistTrait(
                                $"{sex}{waist}{waistColor}",
                                data[RefMapSex.ItemTypeCode.Waist][waist][waistColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(waistTrait, false);
                }

                /// <summary>
                ///   See <see cref="waist" /> and <see cref="waistColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Waist
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(waist, waistColor); }
                    set
                    {
                        waist = value?.Item1 ?? Empty;
                        waistColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeWaist(bundle[sex]);
                    }
                }

                private void ChangeArms(RefMapSex data)
                {
                    ArmsTrait armsTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            armsTrait = new ArmsTrait(
                                $"{sex}{arms}{armsColor}",
                                data[RefMapSex.ItemTypeCode.Arms][arms][armsColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(armsTrait, false);
                }

                /// <summary>
                ///   See <see cref="arms" /> and <see cref="armsColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Arms
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(arms, armsColor); }
                    set
                    {
                        arms = value?.Item1 ?? Empty;
                        armsColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeArms(bundle[sex]);
                    }
                }
                
                private void ChangeLongShirt(RefMapSex data)
                {
                    LongShirtTrait longShirtTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            longShirtTrait = new LongShirtTrait(
                                $"{sex}{longShirt}{longShirtColor}",
                                data[RefMapSex.ItemTypeCode.LongShirt][longShirt][longShirtColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(longShirtTrait, false);
                }

                /// <summary>
                ///   See <see cref="longShirt" /> and <see cref="longShirtColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> LongShirt
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(longShirt, longShirtColor); }
                    set
                    {
                        longShirt = value?.Item1 ?? Empty;
                        longShirtColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeLongShirt(bundle[sex]);
                    }
                }
                
                private void ChangeShoulder(RefMapSex data)
                {
                    ShoulderTrait shoulderTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            shoulderTrait = new ShoulderTrait(
                                $"{sex}{shoulder}{shoulderColor}",
                                data[RefMapSex.ItemTypeCode.Shoulder][shoulder][shoulderColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(shoulderTrait, false);
                }

                /// <summary>
                ///   See <see cref="shoulder" /> and <see cref="shoulderColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Shoulder
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(shoulder, shoulderColor); }
                    set
                    {
                        shoulder = value?.Item1 ?? Empty;
                        shoulderColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeShoulder(bundle[sex]);
                    }
                }

                private void ChangeCloak(RefMapSex data)
                {
                    CloakTrait shoulderTrait = null;
                    if (boots != Empty)
                    {
                        try
                        {
                            shoulderTrait = new CloakTrait(
                                $"{sex}{cloak}{cloakColor}",
                                data[RefMapSex.ItemTypeCode.Cloak][cloak][cloakColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    applier.Use(shoulderTrait, false);
                }

                /// <summary>
                ///   See <see cref="cloak" /> and <see cref="cloakColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Cloak
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(cloak, cloakColor); }
                    set
                    {
                        cloak = value?.Item1 ?? Empty;
                        cloakColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeCloak(bundle[sex]);
                    }
                }

                /// <summary>
                ///   Applies an entire bulk (typically, differential)
                ///   of settings to this holder.
                /// </summary>
                /// <param name="model">The model to apply</param>
                public override bool BulkApply(IRefMapBaseModel model)
                {
                    if (!base.BulkApply(model) || !(model is IRefMapStandardModel standardModel))
                    {
                        return false;
                    }
                    if (standardModel.Boots != null) Boots = standardModel.Boots;
                    if (standardModel.Pants != null) Pants = standardModel.Pants;
                    if (standardModel.Shirt != null) Shirt = standardModel.Shirt;
                    if (standardModel.Chest != null) Chest = standardModel.Chest;
                    if (standardModel.Waist != null) Waist = standardModel.Waist;
                    if (standardModel.Arms != null) Arms = standardModel.Arms;
                    if (standardModel.LongShirt != null) LongShirt = standardModel.LongShirt;
                    if (standardModel.Shoulder != null) Shoulder = standardModel.Shoulder;
                    if (standardModel.Cloak != null) Cloak = standardModel.Cloak;
                    return true;
                }
            }
        }
    }
}
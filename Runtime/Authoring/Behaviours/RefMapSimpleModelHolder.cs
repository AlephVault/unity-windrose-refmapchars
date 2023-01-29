using System;
using GameMeanMachine.Unity.WindRose.RefMapChars.Authoring.ScriptableObjects;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types.Traits;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types.Traits.Simple;
using UnityEngine;


namespace GameMeanMachine.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace Behaviours
        {
            /// <summary>
            ///   A simple model RefMap applier. Adds the tracking
            ///   of the cloth and its color.
            /// </summary>
            [RequireComponent(typeof(RefMapSimpleApplier))]
            public class RefMapSimpleModelHolder : RefMapBaseModelHolder
            {
                // The related applier.
                private RefMapSimpleApplier applier;
                
                /// <summary>
                ///   The cloth. Use <see cref="RefMapBaseModelHolder.Empty" /> to clear it out.
                /// </summary>
                [SerializeField]
                private ushort cloth = Empty;

                /// <summary>
                ///   The cloth color.
                /// </summary>
                [SerializeField]
                private RefMapItem.ColorCode clothColor = RefMapItem.ColorCode.Black;
                
                protected virtual void Awake()
                {
                    applier = GetComponent<RefMapSimpleApplier>();
                }

                protected override void FullUpdateFromSex(RefMapSex data)
                {
                    base.FullUpdateFromSex(data);
                    ChangeCloth(data);
                }
                
                private void ChangeCloth(RefMapSex data)
                {
                    ClothTrait clothTrait = null;
                    if (cloth != Empty)
                    {
                        try
                        {
                            clothTrait = new ClothTrait(
                                $"{sex}{cloth}{clothColor}",
                                data[RefMapSex.ItemTypeCode.Cloth][cloth][clothColor]
                            );
                        }
                        catch (Exception e)
                        {
                        }
                        applier.Use(clothTrait, false);
                    }
                    applier.Use((ClothTrait)null, false);
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

                /// <summary>
                ///   See <see cref="cloth" /> and <see cref="clothColor" />.
                /// </summary>
                public Tuple<ushort, RefMapItem.ColorCode> Cloth
                {
                    get { return new Tuple<ushort, RefMapItem.ColorCode>(cloth, clothColor); }
                    set
                    {
                        cloth = value?.Item1 ?? Empty;
                        clothColor = value?.Item2 ?? RefMapItem.ColorCode.Black;
                        ChangeCloth(bundle[sex]);
                    }
                }

                /// <summary>
                ///   Applies an entire bulk (typically, differential)
                ///   of settings to this holder.
                /// </summary>
                /// <param name="model">The model to apply</param>
                public override bool BulkApply(IRefMapBaseModel model)
                {
                    if (!base.BulkApply(model) || !(model is IRefMapSimpleModel simpleModel))
                    {
                        return false;
                    }
                    if (simpleModel.Cloth != null) Cloth = simpleModel.Cloth;
                    return true;
                }
            }
        }
    }
}
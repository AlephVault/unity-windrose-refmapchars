using AlephVault.Unity.SpriteUtils.Types;
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
            ///   A RefMap standard applier adds the 9 missing parts
            ///   to define the cloth traits.
            /// </summary>
            public abstract class RefMapStandardApplier : RefMapBaseApplier,
                IApplier<BootsTrait>, IApplier<PantsTrait>, IApplier<ShirtTrait>,
                IApplier<ChestTrait>, IApplier<WaistTrait>, IApplier<ArmsTrait>,
                IApplier<LongShirtTrait>, IApplier<ShoulderTrait>, IApplier<CloakTrait>,
                IRefMapComposite
            {
                /// <summary>
                ///   Whether the boots should go over the
                ///   pants or not.
                /// </summary>
                [SerializeField]
                protected bool bootsOverPants;

                /// <summary>
                ///   Whether the necklace should go over the
                ///   long shirt or not.
                /// </summary>
                [SerializeField]
                protected bool necklaceOverLongShirt;
                
                /// <summary>
                ///   The boots image.
                /// </summary>
                public RefMapSource Boots => bootsTrait?.Front;

                /// <summary>
                ///   The pants image.
                /// </summary>
                public RefMapSource Pants => pantsTrait?.Front;

                /// <summary>
                ///   The shirt image. Usually, with no arms.
                /// </summary>
                public RefMapSource Shirt => shirtTrait?.Front;

                /// <summary>
                ///   The chest (light armor) image.
                /// </summary>
                public RefMapSource Chest => chestTrait?.Front;

                /// <summary>
                ///   The waist image.
                /// </summary>
                public RefMapSource Waist => waistTrait?.Front;

                /// <summary>
                ///   The arms image.
                /// </summary>
                public RefMapSource Arms => armsTrait?.Front;

                /// <summary>
                ///   The long shirt image.
                /// </summary>
                public RefMapSource LongShirt => longShirtTrait?.Front;

                /// <summary>
                ///   The shoulder image.
                /// </summary>
                public RefMapSource Shoulder => shoulderTrait?.Front;

                /// <summary>
                ///   The cloak image.
                /// </summary>
                public RefMapSource Cloak => cloakTrait?.Front;

                /// <summary>
                ///   Whether the boots should go over the
                ///   pants or not.
                /// </summary>
                public bool BootsOverPants => bootsOverPants;

                /// <summary>
                ///   Whether the necklace should go over the
                ///   long shirt or not.
                /// </summary>
                public bool NecklaceOverLongShirt => necklaceOverLongShirt;

                /// <summary>
                ///   The boots trait.
                /// </summary>
                protected BootsTrait bootsTrait;

                /// <summary>
                ///   The pants trait.
                /// </summary>
                protected PantsTrait pantsTrait;

                /// <summary>
                ///   The shirt trait. Usually with no arms.
                /// </summary>
                protected ShirtTrait shirtTrait;

                /// <summary>
                ///   The chest (light armor) trait.
                /// </summary>
                protected ChestTrait chestTrait;

                /// <summary>
                ///   The waist trait.
                /// </summary>
                protected WaistTrait waistTrait;

                /// <summary>
                ///   The arms trait.
                /// </summary>
                protected ArmsTrait armsTrait;

                /// <summary>
                ///   The long shirt trait.
                /// </summary>
                protected LongShirtTrait longShirtTrait;

                /// <summary>
                ///   The shoulder trait.
                /// </summary>
                protected ShoulderTrait shoulderTrait;

                /// <summary>
                ///   The cloak trait.
                /// </summary>
                protected CloakTrait cloakTrait;
                
                /// <summary>
                ///   The cloth hash involves the 9 cloth parts.
                /// </summary>
                protected string ClothHash()
                {
                    return $"{bootsTrait?.Hash ?? ""}:{pantsTrait?.Hash ?? ""}:{shirtTrait?.Hash ?? ""}:" +
                           $"{chestTrait?.Hash ?? ""}:{waistTrait?.Hash ?? ""}:{armsTrait?.Hash ?? ""}:" +
                           $"{longShirtTrait?.Hash ?? ""}:{shoulderTrait?.Hash ?? ""}:{cloakTrait?.Hash ?? ""}";
                }

                /// <summary>
                ///   The hash involves the 15 parts.
                /// </summary>
                public override string Hash()
                {
                    return $"{bodyTrait?.Hash ?? ""}:{hairTrait?.Hash ?? ""}:{necklaceTrait?.Hash ?? ""}:" +
                           $"{hatTrait?.Hash ?? ""}:{skilledHandItemTrait?.Hash ?? ""}:" +
                           $"{dumbHandItemTrait?.Hash ?? ""}:{ClothHash()}:{BootsOverPants}:{NecklaceOverLongShirt}";
                }

                /// <summary>
                ///   Gets the grid, and uses it.
                /// </summary>
                protected override void RefreshTexture()
                {
                    UseGrid(cache.Get(this));
                }
                
                /// <summary>
                ///   Applies a boots trait. When passing null, it clears
                ///   the boots trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(BootsTrait appliance, bool force = true)
                {
                    bootsTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a pants trait. When passing null, it clears
                ///   the pants trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(PantsTrait appliance, bool force = true)
                {
                    pantsTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a shirt trait. When passing null, it clears
                ///   the shirt trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(ShirtTrait appliance, bool force = true)
                {
                    shirtTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a chest trait. When passing null, it clears
                ///   the chest trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(ChestTrait appliance, bool force = true)
                {
                    chestTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a waist trait. When passing null, it clears
                ///   the waist trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(WaistTrait appliance, bool force = true)
                {
                    waistTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies an arms trait. When passing null, it clears
                ///   the arms trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(ArmsTrait appliance, bool force = true)
                {
                    armsTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a long shirt trait. When passing null, it clears
                ///   the long shirt trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(LongShirtTrait appliance, bool force = true)
                {
                    longShirtTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a shoulder trait. When passing null, it clears
                ///   the shoulder trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(ShoulderTrait appliance, bool force = true)
                {
                    shoulderTrait = appliance;
                    if (force) RefreshTexture();
                }

                /// <summary>
                ///   Applies a cloak trait. When passing null, it clears
                ///   the cloak trait.
                /// </summary>
                /// <param name="appliance">The appliance to set</param>
                /// <param name="force">Whether to force the update or not</param>
                public void Use(CloakTrait appliance, bool force = true)
                {
                    cloakTrait = appliance;
                    if (force) RefreshTexture();
                }
                
                /// <summary>
                ///   Using the grid depends on the selection type
                ///   to make use of, and that depends on the type
                ///   of visual to tie this behaviour to.
                /// </summary>
                /// <param name="grid">The grid to use</param>
                protected abstract void UseGrid(SpriteGrid grid);
            }
        }
    }
}
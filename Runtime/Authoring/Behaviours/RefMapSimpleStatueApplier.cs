using AlephVault.Unity.SpriteUtils.Types;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types.Selectors;
using GameMeanMachine.Unity.WindRose.SpriteUtils.Authoring.Behaviours;
using UnityEngine;


namespace GameMeanMachine.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace Behaviours
        {
            /// <summary>
            ///   Dumps a simple RefMap set into a statue with only
            ///   a single "staying" state.
            /// </summary>
            [RequireComponent(typeof(RoseSpritedSelectionApplier))]
            public class RefMapSimpleStatueApplier : RefMapSimpleApplier
            {
                /// <summary>
                ///   The applier that will take the update.
                /// </summary>
                private RoseSpritedSelectionApplier applier;
                
                private void Awake()
                {
                    applier = GetComponent<RoseSpritedSelectionApplier>();
                }

                /// <summary>
                ///   Uses a <see cref="RefMapStatueSelection"/>
                ///   to parse a grid and generate the states.
                /// </summary>
                /// <param name="grid">The grid to parse</param>
                protected override void UseGrid(SpriteGrid grid)
                {
                    applier.UseSelection(new RefMapStatueSelection(grid));
                }
            }
        }
    }
}
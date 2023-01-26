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
            ///   Dumps a simple RefMap set into a character with both
            ///   moving and staying states.
            /// </summary>
            [RequireComponent(typeof(MultiRoseAnimatedSelectionApplier))]
            public class RefMapSimpleCharacterApplier : RefMapSimpleApplier
            {
                /// <summary>
                ///   How many frames per second use for the
                ///   [re]generated animations.
                /// </summary>
                [SerializeField]
                private uint framesPerSecond = 4;

                /// <summary>
                ///   The applier that will take the update.
                /// </summary>
                [SerializeField]
                private MultiRoseAnimatedSelectionApplier applier;
                
                private void Awake()
                {
                    applier = GetComponent<MultiRoseAnimatedSelectionApplier>();
                }

                /// <summary>
                ///   Uses a <see cref="RefMapCharacterSelection"/>
                ///   to parse a grid and generate the states.
                /// </summary>
                /// <param name="grid">The grid to parse</param>
                protected override void UseGrid(SpriteGrid grid)
                {
                    applier.UseSelection(new RefMapCharacterSelection(grid, framesPerSecond));
                }
            }
        }
    }
}
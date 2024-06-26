using AlephVault.Unity.SpriteUtils.Types;
using AlephVault.Unity.WindRose.RefMapChars.Types.Selections;
using AlephVault.Unity.WindRose.SpriteUtils.Authoring.Behaviours;
using UnityEngine;


namespace AlephVault.Unity.WindRose.RefMapChars
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
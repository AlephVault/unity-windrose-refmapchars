using AlephVault.Unity.SpriteUtils.Types;
using AlephVault.Unity.WindRose.SpriteUtils.Types.Selectors;
using AlephVault.Unity.WindRose.Types;
using UnityEngine;


namespace AlephVault.Unity.WindRose.RefMapChars
{
    namespace Types
    {
        namespace Selections
        {
            /// <summary>
            ///   The selection to spawn a staying-only statue rose.
            /// </summary>
            public class RefMapStatueSelection : RoseSpritedSelection
            {
                public RefMapStatueSelection(SpriteGrid sourceGrid) : base(sourceGrid, new RoseTuple<Vector2Int>(
                    new Vector2Int(0, 3), new Vector2Int(0, 1), 
                    new Vector2Int(0, 2), new Vector2Int(0, 0))
                )
                {
                }
            }
        }
    }
}
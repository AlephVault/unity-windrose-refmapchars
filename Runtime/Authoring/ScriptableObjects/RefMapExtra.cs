using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameMeanMachine.Unity.WindRose.RefMapChars.Types;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace GameMeanMachine.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace ScriptableObjects
        {
            using AlephVault.Unity.Support.Generic.Authoring.Types;
                
            /// <summary>
            ///   A list of the available sources, given an index.
            ///   These are intended to categorize extra objects,
            ///   and they do not have variations, but they have.
            ///   some types, like necklaces or items.
            /// </summary>
            [CreateAssetMenu(fileName = "NewRefMapExtra", menuName = "Wind Rose/RefMap Chars/Extra Items Category (no color variations)", order = 106)]
            public class RefMapExtra : ScriptableObject
            {
                /// <summary>
                ///   A dictionary to use (maps a number to an
                ///   item to represent, where 0 is always to not
                ///   render any element, so will not be used).
                /// </summary>
                [Serializable]
                public class RefMapExtraItemsDictionary : Dictionary<ushort, RefMapSource> {}

                /// <summary>
                ///   The items contained in this extra category.
                /// </summary>
                [SerializeField]
                private RefMapExtraItemsDictionary items = new RefMapExtraItemsDictionary();
                
                /// <summary>
                ///   Gets a <see cref="RefMapSource"/> af a given type.
                /// </summary>
                /// <param name="index">The index to retrieve the extra item</param>
                public RefMapSource this[ushort index] => items[index];
            }
        }
    }    
}

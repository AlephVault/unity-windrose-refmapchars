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
            public class RefMapExtra : ScriptableObject
            {
                /// <summary>
                ///   The available extra item types.
                /// </summary>
                public enum ExtraItemTypeCode
                {
                    Necklace,
                    SkilledHandItem,
                    DumbHanItem
                }
                
                /// <summary>
                ///   A sub-dictionary to use (maps a number to an
                ///   element to represent, where 0 is always to not
                ///   render any element, so will not be used).
                /// </summary>
                [Serializable]
                public class RefMapExtraItemsDictionary : Dictionary<ushort, RefMapSource> {}

                /// <summary>
                ///   The dictionary to use (maps an extra item type
                ///   to a dictionary).
                /// </summary>
                [Serializable]
                public class RefMapExtraItemGroupsDictionary : Dictionary<ExtraItemTypeCode, RefMapExtraItemsDictionary> {}
            
                /// <summary>
                ///   A dictionary of the variations to use for this
                ///   graphical asset.
                /// </summary>
                [SerializeField]
                private RefMapExtraItemGroupsDictionary extraItemGroups = new RefMapExtraItemGroupsDictionary();

                /// <summary>
                ///   Gets a <see cref="RefMapExtraItemsDictionary"/> af a given type.
                /// </summary>
                /// <param name="extraItemTypeCode">The type to retrieve the extra group for</param>
                public RefMapExtraItemsDictionary this[ExtraItemTypeCode extraItemTypeCode] => extraItemGroups[extraItemTypeCode];
            }
        }
    }    
}

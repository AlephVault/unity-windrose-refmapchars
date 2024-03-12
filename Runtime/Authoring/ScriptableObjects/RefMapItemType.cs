using System;
using System.Collections.Generic;
using System.Linq;
using AlephVault.Unity.WindRose.RefMapChars.Types;
using UnityEngine;


namespace AlephVault.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace ScriptableObjects
        {
            using AlephVault.Unity.Support.Generic.Authoring.Types;
            
            /// <summary>
            ///   A list of the available add-ons, given a category
            ///   index. Only 65535 add-ons are allowed. Categories
            ///   (add-on types) belong to a sex, and contain add-ons
            ///   that are defined within.
            /// </summary>
            [CreateAssetMenu(fileName = "NewRefMapItemType", menuName = "AlephVault/Wind Rose/RefMap Chars/Standard Item Category (no color variations)", order = 104)]
            public class RefMapItemType : ScriptableObject
            {
                /// <summary>
                ///   The dictionary to use (maps a byte code to a ref
                ///   map item).
                /// </summary>
                [Serializable]
                public class RefMapItemsDictionary : Dictionary<ushort, RefMapSource> {}
            
                /// <summary>
                ///   A dictionary of the items to use in this category.
                /// </summary>
                [SerializeField]
                private RefMapItemsDictionary items = new RefMapItemsDictionary();

                /// <summary>
                ///   Gets a <see cref="RefMapSource"/> at a given index.
                /// </summary>
                /// <param name="index">The index to retrieve the item for</param>
                public RefMapSource this[ushort index] => items[index];
                
                /// <summary>
                ///   The count of items in the type.
                /// </summary>
                public int Count => items.Count;

                /// <summary>
                ///   Get the available items in the type.
                /// </summary>
                /// <returns>An enumerable of pairs index/item</returns>
                public IEnumerable<KeyValuePair<ushort, RefMapSource>> Items()
                {
                    return from item in items
                           where item.Value != null
                           select item;
                }
            }
        }
    }    
}

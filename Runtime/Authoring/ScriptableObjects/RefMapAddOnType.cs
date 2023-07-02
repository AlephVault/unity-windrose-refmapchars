using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            ///   index. Only 65535 items are allowed. Categories
            ///   (item types) belong to a sex, and contain items
            ///   that are defined within.
            /// </summary>
            [CreateAssetMenu(fileName = "NewRefMapAddOnType", menuName = "Wind Rose/RefMap Chars/Standard Add-On Category (10 colors variations)", order = 104)]
            public class RefMapAddOnType : ScriptableObject
            {
                /// <summary>
                ///   The dictionary to use (maps a byte code to a ref
                ///   map add-on).
                /// </summary>
                [Serializable]
                public class RefMapAddOnsDictionary : Dictionary<ushort, RefMapAddOn> {}
            
                /// <summary>
                ///   A dictionary of the add-ons to use in this category.
                /// </summary>
                [SerializeField]
                private RefMapAddOnsDictionary addOns = new RefMapAddOnsDictionary();

                /// <summary>
                ///   Gets a <see cref="RefMapAddOn"/> at a given index.
                /// </summary>
                /// <param name="index">The index to retrieve the add-on for</param>
                public RefMapAddOn this[ushort index] => addOns[index];
                
                /// <summary>
                ///   The count of add-ons in the type.
                /// </summary>
                public int Count => addOns.Count;

                /// <summary>
                ///   Get the available add-ons in the type.
                /// </summary>
                /// <returns>An enumerable of pairs index/add-on</returns>
                public IEnumerable<KeyValuePair<ushort, RefMapAddOn>> AddOns()
                {
                    return from addOn in addOns
                           where addOn.Value != null
                           select addOn;
                }
                
#if UNITY_EDITOR
                /// <summary>
                ///   Populates the whole add-on type from a directory.
                ///   This path is typically {path}/(Male|Female)/{AddOnType}.
                ///   In this case, populating involves also creating the
                ///   instances of Items and adding them to the dictionary.
                /// </summary>
                /// <param name="path">The path to read from</param>
                /// <param name="itemTypee">The add-on type to read into</param>
                /// <param name="backType">The back type to read into, if a back image is present</param>
                internal static void Populate(string path, RefMapAddOnType itemType, RefMapAddOnType backType = null)
                {
                    ushort idx = 1;
                    while (true)
                    {
                        // First, look for textures of name {idx}_{color}.png.
                        Debug.Log($"Path is: {path}");
                        string[] files = Directory.GetFiles(path, $"{idx}_*.png");
                        if (files.Length != 0)
                        {
                            RefMapAddOn addOn = CreateInstance<RefMapAddOn>();
                            RefMapAddOn.Populate(path, idx, addOn);
                            itemType.addOns.Add(idx, addOn);
                            if (backType)
                            {
                                string[] backFiles = Directory.GetFiles(path, $"{idx}_*_b.png");
                                if (backFiles.Length != 0)
                                {
                                    RefMapAddOn backAddOn = CreateInstance<RefMapAddOn>();
                                    RefMapAddOn.Populate(path, idx, backAddOn, true);
                                    backType.addOns.Add(idx, backAddOn);
                                }
                            }
                            idx += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
#endif
            }
        }
    }    
}

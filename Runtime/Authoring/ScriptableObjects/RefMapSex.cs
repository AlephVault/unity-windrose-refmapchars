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
            ///   A sex directory holds everything that is
            ///   suitable for the standard RefMap resources
            ///   (e.g. weapons are not included). Inside the
            ///   sex traits, there are sub-directories but
            ///   also inside them there are the actual items.
            ///   One particular group is the body trait, which
            ///   is kept aside.
            /// </summary>
            [CreateAssetMenu(fileName = "NewRefMapSex", menuName = "AlephVault/Wind Rose/RefMap Chars/Sex Data", order = 102)]
            public class RefMapSex : ScriptableObject
            {
                /// <summary>
                ///   The available add-on types. In this coding,
                ///   the hair is broken into two different parts:
                ///   front, and tail.
                /// </summary>
                public enum AddOnTypeCode
                {
                    Boots,
                    Pants,
                    Shirt,
                    Chest,
                    LongShirt,
                    Arms,
                    Waist,
                    Shoulder,
                    Cloth,
                    Hair,
                    HairTail,
                    Hat,
                    Cloak
                }

                /// <summary>
                ///   The dictionary to use (maps a category code to a ref
                ///   map add-on type).
                /// </summary>
                [Serializable]
                public class RefMapAddOnTypesDictionary : Dictionary<AddOnTypeCode, RefMapAddOnType> {}

                /// <summary>
                ///   The available item types.
                /// </summary>
                public enum ItemTypeCode
                {
                    Necklace,
                    SkilledHandItem,
                    DumbHandItem
                }

                /// <summary>
                ///   The dictionary to use (maps a category code to a ref
                ///   map item type).
                /// </summary>
                [Serializable]
                public class RefMapItemTypesDictionary : Dictionary<ItemTypeCode, RefMapItemType> {}

                /// <summary>
                ///   The available bodies.
                /// </summary>
                [SerializeField]
                private RefMapBody body;
                
                /// <summary>
                ///   A dictionary of the add-on categories to use in this sex.
                /// </summary>
                [SerializeField]
                private RefMapAddOnTypesDictionary addOnTypes = new RefMapAddOnTypesDictionary();

                /// <summary>
                ///   A dictionary of the item categories to use in this sex.
                /// </summary>
                [SerializeField]
                private RefMapItemTypesDictionary itemTypes = new RefMapItemTypesDictionary();

                /// <summary>
                ///   The available bodies.
                /// </summary>
                public RefMapBody Body => body;

                /// <summary>
                ///   Gets a <see cref="RefMapAddOnType"/> at a given add-on type code.
                /// </summary>
                /// <param name="addOnTypeCode">The add-on type to retrieve the add-ons for</param>
                public RefMapAddOnType this[AddOnTypeCode addOnTypeCode] => addOnTypes[addOnTypeCode];
                
                /// <summary>
                ///   Gets a <see cref="RefMapItemType"/> at a given item type code.
                /// </summary>
                /// <param name="itemTypeCode">The item type to retrieve the items for</param>
                public RefMapItemType this[ItemTypeCode itemTypeCode] => itemTypes[itemTypeCode];
                
                /// <summary>
                ///   The count of add-on types in a sex data.
                /// </summary>
                public int AddOnTypesCount => addOnTypes.Count;

                /// <summary>
                ///   The count of item types in a sex data.
                /// </summary>
                public int ItemTypesCount => itemTypes.Count;

                /// <summary>
                ///   Gets the available add-on types of the sex data.
                /// </summary>
                /// <returns>An enumerable of pairs add-on type code/add-on type</returns>
                public IEnumerable<KeyValuePair<AddOnTypeCode, RefMapAddOnType>> AddOns()
                {
                    return from addOnType in addOnTypes
                           where addOnType.Value != null
                           select addOnType;
                }
                
                /// <summary>
                ///   Gets the available item types of the sex data.
                /// </summary>
                /// <returns>An enumerable of pairs item type code/item type</returns>
                public IEnumerable<KeyValuePair<ItemTypeCode, RefMapItemType>> Items()
                {
                    return from itemType in itemTypes
                           where itemType.Value != null
                           select itemType;
                }
                
#if UNITY_EDITOR
                /// <summary>
                ///   Populates a sex bundle from a given path. This path
                ///   is typically {path}/(Male|Female). This also involves
                ///   creating the body instance, and all the item type
                ///   instances and linking them into the sex bundle.
                /// </summary>
                /// <param name="path">The path to read from</param>
                /// <param name="sex">The sex bundle to read into</param>
                internal static void Populate(string path, RefMapSex sex)
                {
                    // First, the body.
                    RefMapBody body = CreateInstance<RefMapBody>();
                    RefMapBody.Populate(Path.Combine(path, "Base"), body);
                    sex.body = body;
                    
                    // Then, the add-ons.
                    foreach (AddOnTypeCode code in Enum.GetValues(typeof(AddOnTypeCode)))
                    {
                        if (code == AddOnTypeCode.Hair || code == AddOnTypeCode.HairTail ||
                            code == AddOnTypeCode.Cloak) continue;
                        RefMapAddOnType itemType = CreateInstance<RefMapAddOnType>();
                        if (code != AddOnTypeCode.Cloth)
                        {
                            // Full clothes are entirely custom. They must
                            // be built from an entirely separate process
                            // and do not come by default.
                            RefMapAddOnType.Populate(Path.Combine(path, code.ToString()), itemType);
                        }
                        sex.addOnTypes.Add(code, itemType);
                    }
                    RefMapAddOnType hairType = CreateInstance<RefMapAddOnType>();
                    RefMapAddOnType hairTailType = CreateInstance<RefMapAddOnType>();
                    RefMapAddOnType cloakType = CreateInstance<RefMapAddOnType>();
                    RefMapAddOnType.Populate(Path.Combine(path, "Hair"), hairType, hairTailType);
                    sex.addOnTypes.Add(AddOnTypeCode.Hair, hairType);
                    sex.addOnTypes.Add(AddOnTypeCode.HairTail, hairTailType);
                    sex.addOnTypes.Add(AddOnTypeCode.Cloak, cloakType);
                    
                    // Finally, the items.
                    foreach (ItemTypeCode code in Enum.GetValues(typeof(ItemTypeCode)))
                    {
                        sex.itemTypes.Add(code, CreateInstance<RefMapItemType>());
                    }
                }
#endif
            }
        }
    }    
}

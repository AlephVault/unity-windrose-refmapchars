using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlephVault.Unity.WindRose.RefMapChars.Types;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace AlephVault.Unity.WindRose.RefMapChars
{
    namespace Authoring
    {
        namespace ScriptableObjects
        {
            using AlephVault.Unity.Support.Generic.Authoring.Types;
                
            /// <summary>
            ///   A list of the available sources, given an index.
            ///   These are intended to categorize a single object.
            ///   Only 10 variations are allowed. An item belongs
            ///   to a certain sex trait, and a certain asset type.
            ///   An item may, however, be actually a part of a
            ///   bigger trait, whose parts are scattered (e.g.
            ///   a front hair and a back hair would be implemented
            ///   in two different items, but working as a complement
            ///   of each other respectively).
            /// </summary>
            [CreateAssetMenu(fileName = "NewRefMapItem", menuName = "AlephVault/Wind Rose/RefMap Chars/Standard Add-On (10 colors variations)", order = 105)]
            public class RefMapAddOn : ScriptableObject
            {
                /// <summary>
                ///   Each item or body trait (other than the body itself)
                ///   has 10 possible colors.
                /// </summary>
                public enum ColorCode : byte
                {
                    Black,
                    Blue,
                    DarkBrown,
                    Green,
                    LightBrown,
                    Pink,
                    Purple,
                    Red,
                    White,
                    Yellow
                }

                /// <summary>
                ///   The dictionary to use (maps a byte code to a ref
                ///   map source).
                /// </summary>
                [Serializable]
                public class RefMapVariationsDictionary : Dictionary<ColorCode, RefMapSource> {}
            
                /// <summary>
                ///   A dictionary of the variations to use for this
                ///   graphical asset.
                /// </summary>
                [SerializeField]
                private RefMapVariationsDictionary variations = new RefMapVariationsDictionary();

                /// <summary>
                ///   Gets a <see cref="RefMapSource"/> at a given index.
                /// </summary>
                /// <param name="colorCode">The index to retrieve the source for</param>
                public RefMapSource this[ColorCode colorCode] => variations[colorCode];
                
                /// <summary>
                ///   The count of variations in an item.
                /// </summary>
                public int Count => variations.Count;

                /// <summary>
                ///   Gets the available variations of an item.
                /// </summary>
                /// <returns>An enumerable of pairs color/variation</returns>
                public IEnumerable<KeyValuePair<ColorCode, RefMapSource>> Items()
                {
                    return from variation in variations
                        where variation.Value != null
                        select variation;
                }
                
#if UNITY_EDITOR
                private class CreateManyRefMapAddOnsWindow : EditorWindow
                {
                    public string path;
                    public string namePrefix = "NewAddOn";
                    public int howMany = 1; 

                    private void OnGUI()
                    {
                        namePrefix = EditorGUILayout.TextField("New assets prefix", namePrefix).Trim();
                        howMany = EditorGUILayout.IntField("How many?", howMany);
                        if (howMany > 0 && GUILayout.Button("Generate"))
                        {
                            Execute();
                        }
                    }

                    private void Execute()
                    {
                        for (int i = 0; i < howMany; i++)
                        {
                            string name = $"{namePrefix}{i}.asset";
                            AssetDatabase.CreateAsset(CreateInstance<RefMapAddOn>(), Path.Combine(path, name));
                        }
                        Close();
                    }
                }

                [MenuItem("Assets/Create/AlephVault/Wind Rose/RefMap Chars/Standard Add-Ons (Bulk; 10 colors variations)", true, 108)]
                private static bool CanCreateManyRefMapAddOns()
                {
                    UnityEngine.Object obj = Selection.activeObject;
                    if (obj is null)
                    {
                        return false;
                    }

                    string path = AssetDatabase.GetAssetPath(obj);
                    // The object is a directory if (and only if) a
                    // directory exists by this path. Otherwise, the
                    // object is something else.
                    return AssetDatabase.IsValidFolder(path);
                }
                
                [MenuItem("Assets/Create/AlephVault/Wind Rose/RefMap Chars/Standard Add-Ons (Bulk; 10 colors variations)", false, 105)]
                private static void CreateManyRefMapAddOns()
                {
                    // Get the selected object.
                    string path = AssetDatabase.GetAssetPath(Selection.activeObject);

                    CreateManyRefMapAddOnsWindow window = CreateInstance<CreateManyRefMapAddOnsWindow>();
                    window.titleContent = new GUIContent("Create many standard add-ons");
                    window.path = path;
                    window.maxSize = new Vector2(400, 64);
                    window.minSize = new Vector2(400, 64);
                    window.ShowUtility();
                }

                /// <summary>
                ///   Populates a body from a given path. This path is typically
                ///   {path}/(Male|Female)/{ItemType}.
                /// </summary>
                /// <param name="path">The path to read from</param>
                /// <param name="addOn">The body to read into</param>
                /// <param name="back">Whether this asset is populated from _b images</param>
                internal static void Populate(string path, int idx, RefMapAddOn addOn, bool back = false)
                {
                    string suffix = back ? "_b" : "";
                    foreach (ColorCode code in Enum.GetValues(typeof(ColorCode)))
                    {
                        string file = Path.Combine(path, $"{idx}_{code.Name()}{suffix}.png");
                        try
                        {
                            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(file);
                            if (tex == null) throw new Exception();
                            addOn.variations.Add(code, new RefMapSource { Texture = tex });
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Missing texture {idx}_{code}{suffix}.png - skipping");
                        }
                    }
                }
#endif
            }
            
            /// <summary>
            ///   Methods for the <see cref="RefMapAddOn.ColorCode" /> class.
            /// </summary>
            public static class ItemColorCodeMethods
            {
                /// <summary>
                ///   Gives a code name for the color.
                /// </summary>
                /// <param name="code">The color code</param>
                /// <returns>The in-file code name</returns>
                /// <exception cref="ArgumentException">An invalid or unexpected color was provided</exception>
                public static string Name(this RefMapAddOn.ColorCode code)
                {
                    switch (code)
                    {
                        case RefMapAddOn.ColorCode.Black:
                            return "black";
                        case RefMapAddOn.ColorCode.Blue:
                            return "blue";
                        case RefMapAddOn.ColorCode.DarkBrown:
                            return "dbrown";
                        case RefMapAddOn.ColorCode.Green:
                            return "green";
                        case RefMapAddOn.ColorCode.LightBrown:
                            return "lbrown";
                        case RefMapAddOn.ColorCode.Pink:
                            return "pink";
                        case RefMapAddOn.ColorCode.Purple:
                            return "purple";
                        case RefMapAddOn.ColorCode.Red:
                            return "red";
                        case RefMapAddOn.ColorCode.White:
                            return "white";
                        case RefMapAddOn.ColorCode.Yellow:
                            return "yellow";
                        default:
                            throw new ArgumentException($"Invalid item color code: {code}");
                    }
                }
            }
        }
    }    
}

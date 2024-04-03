using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlephVault.Unity.MenuActions.Types;
using AlephVault.Unity.WindRose.RefMapChars.Core;
using AlephVault.Unity.WindRose.RefMapChars.Types;
using UnityEditor;
using UnityEngine;

namespace AlephVault.Unity.WindRose.RefMapChars.MenuActions.Bundles
{
    namespace Editor
    {
        namespace MenuActions
        {
            namespace Tools
            {
                using Authoring.ScriptableObjects;

                /// <summary>
                ///   Defines a menu option to generate a texture.
                /// </summary>
                public static class CreateRefMapClothTexture
                {
                    public class CreateRefMapClothTextureWindow : SmartEditorWindow
                    {
                        // This is just a temporary composite that
                        // will serve the purpose of cloth generation.
                        private class TmpRefMapStandardComposite : IRefMapStandardComposite
                        {
                            // All these properties are not needed.
                            
                            public RefMapSource Body => null;
                            public RefMapSource Hair => null;
                            public RefMapSource HairTail => null;
                            public RefMapSource Necklace => null;
                            public RefMapSource Hat => null;
                            public RefMapSource SkilledHandItem => null;
                            public RefMapSource DumbHandItem => null;
                            public bool NecklaceOverLongShirt => false;

                            public string Hash()
                            {
                                return "";
                            }
                            
                            // These ones will be configured.

                            public RefMapSource Boots { get; set; }
                            public RefMapSource Pants { get; set; }
                            public RefMapSource Shirt { get; set; }
                            public RefMapSource Chest { get; set; }
                            public RefMapSource Waist { get; set; }
                            public RefMapSource Arms { get; set; }
                            public RefMapSource LongShirt { get; set; }
                            public RefMapSource Shoulder { get; set; }
                            public RefMapSource Cloak { get; set; }
                            public bool BootsOverPants { get; set; } 
                        }

                        // A structure encompassing the index and color code.
                        private struct RefMapSourceFields
                        {
                            public ushort Index;
                            public RefMapAddOn.ColorCode Color;

                            public bool RenderProperty(string caption, IEnumerable<ushort> options)
                            {
                                List<int> optionValues = new List<int> { -2 };
                                List<GUIContent> optionCaptions = new List<GUIContent> { new("None") };
                                foreach (ushort option in options)
                                {
                                    optionValues.Add(option);
                                    optionCaptions.Add(new GUIContent(option.ToString()));
                                }
                                
                                bool changed;
                                try
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    changed = UpdateProperty(ref Index, (ushort)EditorGUILayout.IntPopup(
                                        new GUIContent(caption, "Tooltip for popup"), Index,
                                        optionCaptions.ToArray(), optionValues.ToArray()
                                    ));
                                    changed = UpdateProperty(
                                        ref Color, (RefMapAddOn.ColorCode)EditorGUILayout.EnumPopup(
                                            caption + " color", Color
                                        )
                                    ) || changed;
                                }
                                finally
                                {
                                    EditorGUILayout.EndHorizontal();
                                }
                                return changed;
                            }

                            /// <summary>
                            ///   Gets the corresponding source for an add on
                            ///   given the current fields.
                            /// </summary>
                            /// <param name="addOnType">The type this fields set corresponds to</param>
                            /// <returns>The REFMAP source</returns>
                            public RefMapSource GetSource(RefMapAddOnType addOnType)
                            {
                                try
                                {
                                    return PickAddOn(addOnType, Index, Color);
                                }
                                catch (Exception e)
                                {
                                    return null;
                                }
                            }
                        }
                        
                        // The width of the texture.
                        private const int TextureWidth = 128;
                        
                        // The height of the texture.
                        private const int TextureHeight = 192;

                        // A mask to only keep down-looking frames. Loaded by default.
                        private Texture2D maskD;

                        // A mask to only keep sides-looking frames. Loaded by default.
                        private Texture2D maskLR;

                        // A mask to only keep sides|up-looking frames. Loaded by default.
                        private Texture2D maskLRU;

                        // A mask to only keep up-looking frames. Loaded by default.
                        private Texture2D maskU;

                        // The chosen bundle.
                        private RefMapBundle bundle;

                        // The chosen sex.
                        private RefMapBundle.SexCode sexCode = RefMapBundle.SexCode.Male;
                        
                        // The retrieved sex entry based on the chosen sex.
                        private RefMapSex sex;

                        // Whether to use hardware acceleration or not
                        // when generating the texture.
                        private bool useHardwareAcceleration = true;
                        
                        // The final format for the generated texture.
                        private TextureFormat textureFormat = TextureFormat.ARGB32;

                        // The temporary composite that will be maintained.
                        private TmpRefMapStandardComposite composite;

                        // The generated texture.
                        private Texture2D generatedTexture;

                        // Character property: BootsOverPants.
                        private bool bootsOverPants;

                        // Character property: Boots (index and color).
                        private RefMapSourceFields boots;
                        
                        // Character property: Pants (index and color).
                        private RefMapSourceFields pants;

                        // Character property: Shirt (index and color).
                        private RefMapSourceFields shirt;
                        
                        // Character property: Chest (index and color).
                        private RefMapSourceFields chest;
                        
                        // Character property: Waist (index and color).
                        private RefMapSourceFields waist;
                        
                        // Character property: Arms (index and color).
                        private RefMapSourceFields arms;
                        
                        // Character property: LongShirt (index and color).
                        private RefMapSourceFields longShirt;
                        
                        // Character property: Shoulder (index and color).
                        private RefMapSourceFields shoulder;
                        
                        // Character property: Cloak (index and color).
                        private RefMapSourceFields cloak;
                        
                        // Tracks a property to update it given a new value.
                        private static bool UpdateProperty<T>(ref T value, T newValue)
                        {
                            if (!EqualityComparer<T>.Default.Equals(value, newValue))
                            {
                                value = newValue;
                                return true;
                            }

                            return false;
                        }

                        // Makes a texture out of the current composite data.
                        private void MakeTexture() {
                            if (useHardwareAcceleration)
                            {
                                RenderTexture target = new RenderTexture(
                                    TextureWidth, TextureHeight, 0, RenderTextureFormat.ARGB32, 0
                                );
                                RefMapUtils.Paste(
                                    target, composite, maskD, maskLRU, maskLR, maskU
                                );
                                generatedTexture = RefMapUtils.ToTexture2D(target, textureFormat);
                            }
                            else
                            {
                                Texture2D target = new Texture2D(
                                    TextureWidth, TextureHeight, textureFormat, false
                                );
                                RefMapUtils.Paste(
                                    target, composite, maskD, maskLRU, maskLR, maskU
                                );
                                generatedTexture = target;
                            }
                        }
                        
                        private void OnEnable()
                        {
                            if (!maskD) maskD = AssetDatabase.LoadAssetAtPath<Texture2D>(
                                "Packages/com.alephvault.unity.windrose.refmapchars/Runtime/Graphics/mask-d.png"
                            );
                            if (!maskLR) maskLR = AssetDatabase.LoadAssetAtPath<Texture2D>(
                                "Packages/com.alephvault.unity.windrose.refmapchars/Runtime/Graphics/mask-lr.png"
                            );
                            if (!maskLRU) maskLRU = AssetDatabase.LoadAssetAtPath<Texture2D>(
                                "Packages/com.alephvault.unity.windrose.refmapchars/Runtime/Graphics/mask-lru.png"
                            );
                            if (!maskU) maskU = AssetDatabase.LoadAssetAtPath<Texture2D>(
                                "Packages/com.alephvault.unity.windrose.refmapchars/Runtime/Graphics/mask-u.png"
                            );
                        }

                        private void OnDisable()
                        {
                            ClearTexture();
                        }

                        // Gets the default entry for an add-on type.
                        private RefMapSource DefaultAddOn(RefMapSex.AddOnTypeCode addOnTypeCode)
                        {
                            return PickAddOn(sex[addOnTypeCode], -1, RefMapAddOn.ColorCode.Black);
                        }

                        // Picks a specific add-on.
                        private static RefMapSource PickAddOn(
                            RefMapAddOnType addOnType,
                            int addOnIndex,
                            RefMapAddOn.ColorCode colorCode
                        )
                        {
                            if (addOnType.Count == 0 || addOnIndex == -2)
                            {
                                return null;
                            }

                            RefMapAddOn addOn = addOnType[
                                addOnIndex == -1 ? addOnType.AddOns().First().Key : (ushort)addOnIndex
                            ];
                            if (addOn.Count == 0)
                            {
                                return null;
                            }

                            return addOn[colorCode];
                        }

                        // Clears the generated texture.
                        private void ClearTexture()
                        {
                            if (generatedTexture)
                            {
                                DestroyImmediate(generatedTexture);
                                generatedTexture = null;
                            }
                        }
                        
                        protected override float GetSmartWidth()
                        {
                            return 600;
                        }

                        protected override void OnAdjustedGUI()
                        {
                            // First, get the bundle to use.
                            bundle = (RefMapBundle)EditorGUILayout.ObjectField(
                                "Source REFMAP bundle", bundle, typeof(RefMapBundle), false
                            );
                            if (!bundle)
                            {
                                EditorGUILayout.LabelField("You must select a bundle to continue");
                                return;
                            }

                            // Then, get the sex to use.
                            sexCode = (RefMapBundle.SexCode)EditorGUILayout.EnumPopup("Cloth sex", sexCode);
                            sex = bundle[sexCode];
                            if (!sex)
                            {
                                EditorGUILayout.LabelField("There is no data set in the bundle for the chosen sex");
                                return;
                            }
                            
                            // Then, capture some of the texture data.
                            EditorGUILayout.LabelField("Generation-related data");
                            useHardwareAcceleration =
                                EditorGUILayout.ToggleLeft("Use Hardware Acceleration", useHardwareAcceleration);
                            textureFormat =
                                (TextureFormat)EditorGUILayout.EnumPopup("Texture Format", textureFormat);
                            
                            // Then, capture some of the composite/bundle data.
                            EditorGUILayout.LabelField("Cloth-related data");
                            bool compositeChanged = false;
                            if (composite == null)
                            {
                                composite = new TmpRefMapStandardComposite();
                                composite.Boots = DefaultAddOn(RefMapSex.AddOnTypeCode.Boots);
                                composite.Arms = DefaultAddOn(RefMapSex.AddOnTypeCode.Arms);
                                composite.LongShirt = DefaultAddOn(RefMapSex.AddOnTypeCode.LongShirt);
                                composite.Shirt = DefaultAddOn(RefMapSex.AddOnTypeCode.Shirt);
                                composite.Chest = DefaultAddOn(RefMapSex.AddOnTypeCode.Chest);
                                composite.Cloak = DefaultAddOn(RefMapSex.AddOnTypeCode.Cloak);
                                composite.Pants = DefaultAddOn(RefMapSex.AddOnTypeCode.Pants);
                                composite.Waist = DefaultAddOn(RefMapSex.AddOnTypeCode.Waist);
                                composite.Shoulder = DefaultAddOn(RefMapSex.AddOnTypeCode.Shoulder);
                                // Since we're initializing the composite,
                                // this will become true this time.
                                compositeChanged = true;
                            }
                            
                            // 1. Whether the boots should be rendered over the pants.
                            compositeChanged = UpdateProperty(ref bootsOverPants, EditorGUILayout.ToggleLeft(
                                "Render Boots Over Pants", composite.BootsOverPants
                            )) | compositeChanged;
                            composite.BootsOverPants = bootsOverPants;
                            
                            // 2. Render property: boots.
                            compositeChanged = boots.RenderProperty(
                                "Boots", sex[RefMapSex.AddOnTypeCode.Boots].AddOnKeys()
                            ) || compositeChanged;
                            composite.Boots = boots.GetSource(sex[RefMapSex.AddOnTypeCode.Boots]);
                            
                            // 3. Render property: pants.
                            compositeChanged = pants.RenderProperty(
                                "Pants", sex[RefMapSex.AddOnTypeCode.Pants].AddOnKeys()
                            ) || compositeChanged;
                            composite.Pants = pants.GetSource(sex[RefMapSex.AddOnTypeCode.Pants]);

                            // 4. Render property: shirt.
                            compositeChanged = shirt.RenderProperty(
                                "Shirt", sex[RefMapSex.AddOnTypeCode.Shirt].AddOnKeys()
                            ) || compositeChanged;
                            composite.Shirt = shirt.GetSource(sex[RefMapSex.AddOnTypeCode.Shirt]);

                            // 5. Render property: chest.
                            compositeChanged = chest.RenderProperty(
                                "Chest", sex[RefMapSex.AddOnTypeCode.Chest].AddOnKeys()
                            ) || compositeChanged;
                            composite.Chest = chest.GetSource(sex[RefMapSex.AddOnTypeCode.Chest]);

                            // 6. Render property: waist.
                            compositeChanged = waist.RenderProperty(
                                "Waist", sex[RefMapSex.AddOnTypeCode.Waist].AddOnKeys()
                            ) || compositeChanged;
                            composite.Waist = waist.GetSource(sex[RefMapSex.AddOnTypeCode.Waist]);

                            // 7. Render property: arms.
                            compositeChanged = arms.RenderProperty(
                                "Arms", sex[RefMapSex.AddOnTypeCode.Arms].AddOnKeys()
                            ) || compositeChanged;
                            composite.Arms = arms.GetSource(sex[RefMapSex.AddOnTypeCode.Arms]);

                            // 8. Render property: longShirt.
                            compositeChanged = longShirt.RenderProperty(
                                "Long Shirt", sex[RefMapSex.AddOnTypeCode.LongShirt].AddOnKeys()
                            ) || compositeChanged;
                            composite.LongShirt = longShirt.GetSource(sex[RefMapSex.AddOnTypeCode.LongShirt]);

                            // 9. Render property: shoulder.
                            compositeChanged = shoulder.RenderProperty(
                                "Shoulder", sex[RefMapSex.AddOnTypeCode.Shoulder].AddOnKeys()
                            ) || compositeChanged;
                            composite.Shoulder = shoulder.GetSource(sex[RefMapSex.AddOnTypeCode.Shoulder]);

                            // 10. Render property: cloak.
                            compositeChanged = cloak.RenderProperty(
                                "Cloak", sex[RefMapSex.AddOnTypeCode.Cloak].AddOnKeys()
                            ) || compositeChanged;
                            composite.Cloak = cloak.GetSource(sex[RefMapSex.AddOnTypeCode.Cloak]);
                            
                            if (compositeChanged)
                            {
                                ClearTexture();
                                MakeTexture();
                            }

                            if (generatedTexture)
                            {
                                EditorGUILayout.LabelField(
                                    new GUIContent(generatedTexture),
                                    GUILayout.Width(128), GUILayout.Height(192)
                                );
                            }

                            if (GUILayout.Button("Generate"))
                            {
                                string path = EditorUtility.SaveFilePanelInProject(
                                    "Save cloth texture", "Cloth", "png",
                                    "Save the generated cloth texture"
                                );
                                if (path != null)
                                {
                                    SmartExecute(() =>
                                    {
                                        File.WriteAllBytes(path, generatedTexture.EncodeToPNG());
                                        AssetDatabase.Refresh();
                                        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                                        if (textureImporter)
                                        {
                                            Debug.Log("Texture import settings setup started");
                                            textureImporter.isReadable = true;
                                            textureImporter.textureType = TextureImporterType.Sprite;
                                            textureImporter.spriteImportMode = SpriteImportMode.Single;
                                            textureImporter.spritePixelsPerUnit = 32;
                                            textureImporter.filterMode = FilterMode.Point;
                                            textureImporter.wrapMode = TextureWrapMode.Clamp;
                                            textureImporter.alphaIsTransparency = true;

                                            TextureImporterPlatformSettings settings = textureImporter.GetPlatformTextureSettings("Standalone");
                                            settings.format = TextureImporterFormat.Automatic;
                                            textureImporter.SetPlatformTextureSettings(settings);
                                            textureImporter.SaveAndReimport();
                                            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                                            AssetDatabase.Refresh();
                                            Debug.Log("Texture import settings setup done");
                                        }
                                        else
                                        {
                                            Debug.LogWarning("The generated texture does not have an import");
                                        }
                                        Debug.Log("Texture successfully generated");
                                    });
                                }
                            }
                        }
                    }
                    
                    [MenuItem("Assets/Create/Aleph Vault/WindRose/RefMap Chars/Cloth Texture", false, 115)]
                    public static void ExecuteBoilerplate()
                    {
                        CreateRefMapClothTextureWindow window = ScriptableObject.CreateInstance<CreateRefMapClothTextureWindow>();
                        window.titleContent = new GUIContent("REFMAP Cloth Texture generation");
                        window.ShowUtility();
                    }
                }
            }
        }
    }
}
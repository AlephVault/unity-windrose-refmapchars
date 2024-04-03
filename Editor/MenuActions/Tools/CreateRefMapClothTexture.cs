using System;
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
                        private TmpRefMapStandardComposite composite = null;

                        // The generated texture.
                        private Texture2D generatedTexture = null;

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
                            return PickAddOn(addOnTypeCode, -1, RefMapAddOn.ColorCode.Black);
                        }

                        // Picks a specific add-on.
                        private RefMapSource PickAddOn(
                            RefMapSex.AddOnTypeCode addOnTypeCode,
                            int addOnIndex,
                            RefMapAddOn.ColorCode colorCode
                        )
                        {
                            RefMapAddOnType addOnType = sex[addOnTypeCode];
                            if (addOnType.Count == 0)
                            {
                                return null;
                            }

                            RefMapAddOn addOn = addOnType[addOnIndex < 0 ? addOnType.AddOns().First().Key: (ushort)addOnIndex];
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
                            
                            // TODO Implement everything here. Every single assignment.
                            // TODO This needs new fields to be assigned.
                            composite.BootsOverPants = EditorGUILayout.ToggleLeft(
                                "Boots Over Pants", composite.BootsOverPants
                            );
                            if (compositeChanged)
                            {
                                ClearTexture();
                                MakeTexture();
                            }

                            SmartButton("Generate", () =>
                            {
                                // TODO implement the generation.
                            });
                        }
                    }
                    
                    [MenuItem("Assets/Create/Aleph Vault/WindRose/RefMap Chars/Cloth Texture", false, 204)]
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
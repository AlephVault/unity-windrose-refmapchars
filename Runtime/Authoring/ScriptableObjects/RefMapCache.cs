using System;
using AlephVault.Unity.SpriteUtils.Types;
using AlephVault.Unity.TextureUtils.Types;
using AlephVault.Unity.WindRose.RefMapChars.Core;
using AlephVault.Unity.WindRose.RefMapChars.Types;
using Unity.Collections;
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
            /// <summary>
            ///   This cache builds and keeps textures of size
            ///   128x192 to be composed and used as character
            ///   displays. This instance is used on design (e.g.
            ///   to assign to prefabs that should access this
            ///   cache to get generate and then get textures).
            /// </summary>
            [CreateAssetMenu(fileName = "NewRefMapCache", menuName = "Aleph Vault/WindRose/RefMap Chars/Cache", order = 107)]
            public class RefMapCache : ScriptableObject
            {
#if UNITY_EDITOR
                /// <summary>
                ///   This custom editor forces the mask textures
                ///   to be properly changed to the default values
                ///   if they're null.
                /// </summary>
                [CustomEditor(typeof(RefMapCache))]
                public class RefMapCacheEditor : Editor
                {
                    public override void OnInspectorGUI()
                    {
                        DrawDefaultInspector();

                        RefMapCache yourObject = (RefMapCache)target;
                        yourObject.RefreshMasks();
                    }
                }
#endif
                
                /// <summary>
                ///   The size of the last second rescue queue.
                ///   A negative value will imply a shift until
                ///   the size is 0 (same as choosing 0).
                /// </summary>
                [SerializeField]
                private int lastSecondRescueSize = 20;

                /// <summary>
                ///   The pixels per unit for each generated sprite.
                /// </summary>
                [SerializeField]
                private float pixelsPerUnit = 32;

                /// <summary>
                ///   The format for the render textures. Ensure this
                ///   plays nice with <see cref="renderDepth"/> and
                ///   <see cref="finalFormat"/>.
                /// </summary>
                [SerializeField]
                private RenderTextureFormat renderFormat = RenderTextureFormat.ARGB32;

                /// <summary>
                ///   The format for the final textures. Ensure this
                ///   plays nice with <see cref="renderFormat"/>.
                /// </summary>
                [SerializeField]
                private TextureFormat finalFormat = TextureFormat.ARGB32;

                /// <summary>
                ///   The render depth, in pixels. Ensure this plays
                ///   nice with <see cref="renderFormat"/>.
                /// </summary>
                [SerializeField]
                private int renderDepth;

                /// <summary>
                ///   A mask that only shows frames pointing to the
                ///   down side. It is recommended to use the bundled
                ///   texture "mask-d.png" in this property.
                /// </summary>
                [SerializeField]
                private Texture2D maskD;

                /// <summary>
                ///   A mask that does not show frames pointing to
                ///   the down side. It is recommended to use the
                ///   bundled texture "mask-lru.png" in this property.
                /// </summary>
                [SerializeField]
                private Texture2D maskLRU;
                
                /// <summary>
                ///   A mask that only shows frames pointing to the
                ///   left and right sides. It is recommended to use
                ///   the bundled texture "mask-lr.png" in this property.
                /// </summary>
                [SerializeField]
                private Texture2D maskLR;
                
                /// <summary>
                ///   A mask that only shows frames pointing up. It is
                ///   recommended to use the bundled texture "mask-u.png"
                ///   in this property.
                /// </summary>
                [SerializeField]
                private Texture2D maskU;

                /// <summary>
                ///   Tells whether to compose new textures using hardware
                ///   acceleration or not.
                /// </summary>
                public bool UseHardwareAcceleration;
                
                // The texture pool for this cache.
                private TexturePool<string, Texture2D> texturePool;
                
                // The sprite pool for this cache. It extracts
                // stuff from the texture pool.
                private IdentifiedSpriteGridPool<string> spritePool;
                
                private SpriteGrid GridFromTexture(string key, Func<Texture2D> onAbsent)
                {
                    Texture2D usedTexture = texturePool.Use(key, onAbsent, Destroy);
                    return spritePool.Get(key, () =>
                    {
                        return new Tuple<Texture2D, Rect?, Size2D, Size2D, float, Action, Action>(
                            usedTexture, null, new Size2D
                            {
                                Width = RefMapUtils.FrameWidth, Height = RefMapUtils.FrameHeight
                            },
                            new Size2D { Width = 0, Height = 2 }, pixelsPerUnit, () => {}, () =>
                            {
                                texturePool?.Release(key);
                            }
                        );
                    });
                }

                /// <summary>
                ///   Gets the sprite grid associated to a composite. It is
                ///   recommended to use the resulting grid as quick as possible.
                /// </summary>
                /// <param name="composite">The composite to get the grid for</param>
                /// <returns>The appropriate sprite grid</returns>
                public SpriteGrid Get(IRefMapStandardComposite composite)
                {
                    return GridFromTexture(composite.Hash(), () =>
                    {
                        if (UseHardwareAcceleration)
                        {
                            RenderTexture target = new RenderTexture(
                                RefMapUtils.TextureWidth, RefMapUtils.TextureHeight, renderDepth, renderFormat, 0
                            );
                            RefMapUtils.Paste(
                                target, composite, maskD, maskLRU, maskLR, maskU
                            );
                            return RefMapUtils.FixBleeding(RefMapUtils.ToTexture2D(target, finalFormat));
                        }
                        else
                        {
                            Texture2D target = new Texture2D(
                                RefMapUtils.TextureWidth, RefMapUtils.TextureHeight, finalFormat, false
                            );
                            RefMapUtils.Paste(
                                target, composite, maskD, maskLRU, maskLR, maskU
                            );
                            return RefMapUtils.FixBleeding(target);
                        }
                    });
                }
                
                /// <summary>
                ///   Gets the sprite grid associated to a simple composite. It is
                ///   recommended to use the resulting grid as quick as possible.
                /// </summary>
                /// <param name="composite">The composite to get the grid for</param>
                /// <returns>The appropriate sprite grid</returns>
                public SpriteGrid Get(IRefMapSimpleComposite composite)
                {
                    return GridFromTexture(composite.Hash(), () =>
                    {
                        if (UseHardwareAcceleration)
                        {
                            RenderTexture target = new RenderTexture(
                                RefMapUtils.TextureWidth, RefMapUtils.TextureHeight, renderDepth, renderFormat, 0
                            );
                            RefMapUtils.Paste(
                                target, composite, maskD, maskLRU, maskLR, maskU
                            );
                            return RefMapUtils.FixBleeding(RefMapUtils.ToTexture2D(target, finalFormat));
                        }
                        else
                        {
                            Texture2D target = new Texture2D(
                                RefMapUtils.TextureWidth, RefMapUtils.TextureHeight, finalFormat, false
                            );
                            RefMapUtils.Paste(
                                target, composite, maskD, maskLRU, maskLR, maskU
                            );
                            return RefMapUtils.FixBleeding(target);
                        }
                    });
                }
                
#if UNITY_EDITOR
                private void RefreshMasks()
                {
                    if (maskD && maskU && maskLRU && maskLR) return;

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
                    Debug.Log("Updating textures...");
                    EditorUtility.SetDirty(this);
                }
#endif
                
                private void OnEnable()
                {
                    texturePool = new TexturePool<string, Texture2D>(lastSecondRescueSize);
                    spritePool = new IdentifiedSpriteGridPool<string>(lastSecondRescueSize);
                }

                private void OnDisable()
                {
                    spritePool = null;
                    texturePool = null;
                }
            }
        }
    }    
}

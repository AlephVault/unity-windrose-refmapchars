using System;
using System.Collections.Generic;
using System.Linq;
using AlephVault.Unity.TextureUtils.Types;
using AlephVault.Unity.TextureUtils.Utils;
using AlephVault.Unity.WindRose.RefMapChars.Types;
using Unity.Collections;
using UnityEngine;


namespace AlephVault.Unity.WindRose.RefMapChars
{
    namespace Core
    {
        /// <summary>
        ///   The core function is implemented here, which takes the
        ///   textures (all of them are separate parts) that are to
        ///   be merged into a single texture, using certain rendering
        ///   order, to generate a final character. The textures can
        ///   be given separately or as part of a composite object.
        /// </summary>
        public static class RefMapUtils
        {
            public const int TextureWidth = 128;
            public const int TextureHeight = 192;
            public const int FrameWidth = 32;
            public const int FrameHeight = 48;
            private const int DirectionSize = TextureWidth * FrameHeight;
            private const int AntiBleedBufferSize = TextureWidth * 2;
            private static Color32[] bleedingBuffer = InitAntiBleedingBuffer();
            private const int AntiBleedDirectionSize = DirectionSize + AntiBleedBufferSize;

            private static Color32[] InitAntiBleedingBuffer()
            {
                Color32[] buffer = new Color32[AntiBleedBufferSize];
                for (int i = 0; i < AntiBleedBufferSize; i++)
                {
                    buffer[i] = new Color32(0, 0, 0, 0);
                }

                return buffer;
            }
            
            private static void Paste(Texture target, params Texture2DSource[] sources)
            {
                if (target == null) throw new ArgumentNullException(nameof(target));
                if (target.width != 128 || target.height != 192)
                {
                    throw new ArgumentException("The target texture must be 128x192");
                }
                Textures.Paste2D(target, true, (
                    from source in sources
                    where source != null select source
                ).ToArray());
            }
            
            /// <summary>
            ///   Takes a target texture and a composite object to paste all the
            ///   parts (in the appropriate order and using the appropriate masks)
            ///   into it, to form a new texture (to be cached and used).
            /// </summary>
            /// <param name="target">The target texture to render everything into</param>
            /// <param name="composite">The composite object to render</param>
            /// <param name="maskD">A mask that only shows down-oriented frames</param>
            /// <param name="maskLRU">A mask that does not show down-oriented frames</param>
            /// <param name="maskLR">A mask that only shows side-oriented frames</param>
            /// <param name="maskU">A mask that only shows up-oriented frames</param>
            public static void Paste(
                Texture target, IRefMapStandardComposite composite, Texture2D maskD, Texture2D maskLRU,
                Texture2D maskLR, Texture2D maskU
            )
            {
                Paste(
                    target,
                    composite?.SkilledHandItem?.ToTexture2DSource(maskU),
                    composite?.DumbHandItem?.ToTexture2DSource(maskLRU),
                    composite?.HairTail?.ToTexture2DSource(maskD),
                    composite?.Cloak?.ToTexture2DSource(maskD),
                    composite?.Body?.ToTexture2DSource(),
                    (composite?.BootsOverPants ?? false ?
                        composite.Pants : composite?.Boots)?.ToTexture2DSource(),
                    (composite?.BootsOverPants ?? false ?
                        composite.Boots : composite?.Pants)?.ToTexture2DSource(),
                    composite?.Shirt?.ToTexture2DSource(),
                    composite?.Chest?.ToTexture2DSource(),
                    (composite?.NecklaceOverLongShirt ?? false ?
                        composite.LongShirt : composite?.Necklace)?.ToTexture2DSource(),
                    (composite?.NecklaceOverLongShirt ?? false ?
                        composite.Necklace : composite?.LongShirt)?.ToTexture2DSource(),
                    composite?.Shoulder?.ToTexture2DSource(),
                    composite?.Waist?.ToTexture2DSource(),
                    composite?.Arms?.ToTexture2DSource(),
                    composite?.Hair?.ToTexture2DSource(),
                    composite?.SkilledHandItem?.ToTexture2DSource(maskLR),
                    composite?.Cloak?.ToTexture2DSource(maskLRU),
                    composite?.HairTail?.ToTexture2DSource(maskLRU),
                    composite?.Hat?.ToTexture2DSource(),
                    composite?.DumbHandItem?.ToTexture2DSource(maskD),
                    composite?.SkilledHandItem?.ToTexture2DSource(maskD)
                );
            }

            /// <summary>
            ///   Takes a target texture and a simple composite object to paste all the
            ///   parts (in the appropriate order and using the appropriate masks) into
            ///   it, to form a new texture (to be cached and used). In this mode, the
            ///   clothes make use of less iterations since they come in a single sprite
            ///   instead of broken in parts.
            /// </summary>
            /// <param name="target">The target texture to render everything into</param>
            /// <param name="composite">The simple composite object to render</param>
            /// <param name="maskD">A mask that only shows down-oriented frames</param>
            /// <param name="maskLRU">A mask that does not show down-oriented frames</param>
            /// <param name="maskLR">A mask that only shows side-oriented frames</param>
            /// <param name="maskU">A mask that only shows up-oriented frames</param>
            public static void Paste(
                Texture target, IRefMapSimpleComposite composite, Texture2D maskD, Texture2D maskLRU,
                Texture2D maskLR, Texture2D maskU
            )
            {
                Paste(
                    target,
                    composite?.SkilledHandItem?.ToTexture2DSource(maskU),
                    composite?.DumbHandItem?.ToTexture2DSource(maskLRU),
                    composite?.HairTail?.ToTexture2DSource(maskD),
                    composite?.Body?.ToTexture2DSource(),
                    composite?.Cloth?.ToTexture2DSource(),
                    composite?.Necklace?.ToTexture2DSource(),
                    composite?.Hair?.ToTexture2DSource(),
                    composite?.SkilledHandItem?.ToTexture2DSource(maskLR),
                    composite?.HairTail?.ToTexture2DSource(maskLRU),
                    composite?.Hat?.ToTexture2DSource(),
                    composite?.DumbHandItem?.ToTexture2DSource(maskD),
                    composite?.SkilledHandItem?.ToTexture2DSource(maskD)
                );
            }
            
            /// <summary>
            ///   Converts a <see cref="RenderTexture" /> to a <see cref="Texture2D"/>
            ///   by telling a render format in the target texture.
            /// </summary>
            /// <param name="renderTexture">The texture to convert</param>
            /// <param name="finalFormat">The format to use for the new texture</param>
            /// <returns>The new texture</returns>
            public static Texture2D ToTexture2D(RenderTexture renderTexture, TextureFormat finalFormat)
            {
                Texture2D tex = new Texture2D(TextureWidth, TextureHeight, finalFormat, false);
                // ReadPixels looks at the active RenderTexture.
                RenderTexture.active = renderTexture;
                tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                tex.Apply();
                return tex;
            }
            
            /// <summary>
            ///   Creates a new texture based on a previous texture but adds
            ///   some anti-bleeding margin for the new texture.
            /// </summary>
            /// <param name="sourceTexture">The source texture</param>
            /// <returns>A new texture which adds anti-bleeding margin</returns>
            public static Texture2D FixBleeding(Texture2D sourceTexture)
            {
                Texture2D fixedImage = new Texture2D(
                    TextureWidth, TextureHeight + 8, sourceTexture.format, 
                    false
                );
                NativeArray<Color32> sourcePixels = sourceTexture.GetPixelData<Color32>(0);
                NativeArray<Color32> fixedPixels = fixedImage.GetPixelData<Color32>(0);
                for (int i = 0; i < 4; i++)
                {
                    fixedPixels.Slice(i * AntiBleedDirectionSize, DirectionSize).CopyFrom(
                        sourcePixels.Slice(i * DirectionSize, DirectionSize)
                    );
                    fixedPixels.Slice(i * AntiBleedDirectionSize + DirectionSize, AntiBleedBufferSize).CopyFrom(
                        bleedingBuffer
                    );
                }
                fixedImage.Apply();
                return fixedImage;
            }

        }
    }
}

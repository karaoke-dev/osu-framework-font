// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace osu.Framework.Graphics.Sprites
{
    public interface ILyricTexture
    {
        Texture CreateTexture(int width, int height, Direction direction);
    }

    public class SolidTexture : ILyricTexture
    {
        public Color4 SolidColor { get; set; }

        public Texture CreateTexture(int width, int height, Direction direction)
        {
            var argb = SolidColor.ToArgb();
            byte a = (byte)(argb >> 24);
            byte r = (byte)(argb >> 16);
            byte g = (byte)(argb >> 8);
            byte b = (byte)(argb >> 0);

            var rawData = new[] { r, g, b, a };

            // Create image and convert to sample
            var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(rawData, 1, 1);
            image.Mutate(x => x.Resize(width, height));
            var texture = new Texture(width, height);
            texture.SetData(new TextureUpload(image));
            return texture;
        }
    }

    public class Mixedexture : ILyricTexture
    {
        /// <summary>
        /// Percentage is from 0 to 1
        /// </summary>
        public IReadOnlyDictionary<float, SRGBColour> Colors { get; set; }

        public MixedType Type { get; set; }

        public Texture CreateTexture(int width, int height, Direction direction)
        {
            var targetSize = direction == Direction.Horizontal ? width : height;

            // create a texture is targetSize*1
            var singleLineSample = createSingaleLineSample(targetSize);

            var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(singleLineSample, targetSize, 1);
            image.Mutate(x => x.Resize(targetSize, direction == Direction.Horizontal ? height : width));

            // Rotate the image if is vertical
            if (direction == Direction.Vertical)
                image.Mutate(x => x.Rotate(90));

            var texture = new Texture(width, height);
            texture.SetData(new TextureUpload(image));
            return texture;
        }

        private byte[] createSingaleLineSample(int size)
        {
            var rawData = new byte[size * sizeof(int)];
            var colors = Colors.ToDictionary(k => (int)(k.Key * size), v => v.Value);

            for (int i = 0; i < rawData.Length; i += 4)
            {
                var index = i / 4;
                var firstColor = colors.LastOrDefault(x => x.Key <= index);
                var secondColor = colors.FirstOrDefault(x => x.Key > index);

                var firstColorOpacity = ((float)secondColor.Key - index) / (secondColor.Key - firstColor.Key);
                var mixedColor = Type == MixedType.Gradient
                    ? (Color4)firstColor.Value
                    : (Color4)(firstColor.Value * firstColorOpacity + secondColor.Value * (1 - firstColorOpacity));

                var argb = mixedColor.ToArgb();
                byte a = (byte)(argb >> 24);
                byte r = (byte)(argb >> 16);
                byte g = (byte)(argb >> 8);
                byte b = (byte)(argb >> 0);

                rawData[i] = r;
                rawData[i + 1] = g;
                rawData[i + 2] = b;
                rawData[i + 3] = a;
            }

            return rawData;
        }

        public enum MixedType
        {
            Gradient,

            MilleFeuille
        }
    }
}

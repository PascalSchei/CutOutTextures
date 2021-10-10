using System.Collections.Generic;

namespace CutOutTextures {
    public class PixelColor {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;

        public PixelColor(List<byte> pixelColor) {
            Blue = pixelColor[0];
            Green = pixelColor[1];
            Red = pixelColor[2];
            Alpha = pixelColor[3];
        }

        internal List<byte> GetValues() {
            return new List<byte> {
                Blue,
                Green,
                Red,
                Alpha
            };
        }
    }

}
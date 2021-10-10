using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CutOutTextures {
    internal class BitmapTransformed {

        private PixelColor[,] originalPic;
        private PixelColor[,] resultPic;
        private Point pA;
        private Point pB;
        private Point pC;
        private Point pD;
        public BitmapSource resultImage;

        public BitmapTransformed(BitmapImage bitmap, System.Windows.Point a, System.Windows.Point b, System.Windows.Point c, System.Windows.Point d, double zoom) {
            originalPic = BitmapSourceToArray(bitmap);
           

            pA = ConvertPoint(a, zoom);
            pB = ConvertPoint(b, zoom);
            pC = ConvertPoint(c, zoom);
            pD = ConvertPoint(d, zoom);
            CreateNewEmptyBitmap();
            StartTransformation();
            BitmapSource bmpSource = BitmapSourceFromArray(resultPic);
            resultImage = bmpSource;
        }

        private PixelColor[,] BitmapSourceToArray(BitmapSource source) {
            // Stride = (width) x (bytes per pixel)
            int stride = (int)source.PixelWidth * (source.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[(int)source.PixelHeight * stride];
            source.CopyPixels(pixels, stride, 0);
            var pixelLIst = pixels.ToList();
            int width = source.PixelWidth;
            int height = source.PixelHeight;
            List<PixelColor> pixelColorsList = new List<PixelColor>();
            for (int i = 0; i < pixelLIst.Count; i += 4) {
                List<byte> items = pixelLIst.Skip(i).Take(4).ToList();
                pixelColorsList.Add(new PixelColor(items));
            }
            originalPic = new PixelColor[width, height];
            int vertikal = 0;
            int horizontal = 0;
            foreach (var item in pixelColorsList) {
                originalPic[horizontal, vertikal] = item;
                horizontal++;
                if (horizontal >= width) {
                    horizontal = 0;
                    vertikal++;
                }
            }
            return originalPic;
        }


        private Point ConvertPoint(System.Windows.Point point, double zoom) {
            int x = (int)(point.X / zoom);
            int y = (int)(point.Y / zoom);

            return new Point(x,y);
        }


        private void StartTransformation() {
            int stepsVertical = resultPic.GetLength(1);
            //Vektor linke seite
            for (int vStep = 0; vStep < stepsVertical; vStep++) {
                StepsOnVektor(pA, pD, stepsVertical, vStep, out Point pOrigAD);
                StepsOnVektor(pB, pC, stepsVertical, vStep, out Point pOrigBC);
                int stepsHorizontal = resultPic.GetLength(0);
                for (int i = 0; i < stepsHorizontal; i++) {
                    StepsOnVektor(pOrigAD, pOrigBC, stepsHorizontal, i, out Point pOrig);
                    resultPic[i, vStep] = originalPic[pOrig.X, pOrig.Y];
                }
            }
        }

        private BitmapSource BitmapSourceFromArray(PixelColor[,] resultPic) {
            List<byte> pixelList = new List<byte>();

            for (int y = 0; y < resultPic.GetLength(1); y++) {
                for (int x = 0; x < resultPic.GetLength(0); x++) {
                    List<byte> point = resultPic[x, y].GetValues();
                    pixelList.AddRange(point);
                }
            }

            byte[] pixels = pixelList.Cast<byte>().ToArray();
            int width = resultPic.GetLength(0);
            int height = resultPic.GetLength(1);
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            bitmap.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), pixels, width * (bitmap.Format.BitsPerPixel / 8), 0);
            return bitmap;
        }


        private void StepsOnVektor(Point pStart, Point pEnd, int stepsVertical, int vStep, out Point pOrigAC) {
            StepsOnVektor(pStart, pEnd, stepsVertical, vStep, out pOrigAC, out Point nichtVerwendet);
        }

        private void StepsOnVektor(Point pStart, Point pEnd, int stepsHorizontal, int i, out Point pOrig, out Point pResult) {
            Point vektor = pStart.Minus(pEnd);
            var temp = vektor.Multiply(i);
            int distance = (int)pStart.Distance(pEnd);
            pResult = temp.Divided(stepsHorizontal);
            pOrig = pStart.Plus(pResult);
        }

        private void CreateNewEmptyBitmap() {
            //finde die groessten Seitenlaengen
            double distanceA = Distance(pA, pB);
            double distanceB = Distance(pB, pC);
            double distanceC = Distance(pC, pD);
            double distanceD = Distance(pD, pA);
            double horizontal = Math.Max(distanceA, distanceC);
            double vertical = Math.Max(distanceB, distanceD);
            resultPic = new PixelColor[(int)horizontal, (int)vertical];
        }

        private double Distance(Point a, Point b) {
            return a.Distance(b);
        }
    }

}
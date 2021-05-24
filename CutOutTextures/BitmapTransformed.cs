using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace CutOutTextures {
    internal class BitmapTransformed {

        private Bitmap originalPic;
        private Point pA;
        private Point pB;
        private Point pC;
        private Point pD;

        public Bitmap transformed;

        public BitmapTransformed(BitmapImage bitmap, Point a, Point b, Point c, Point d) {
            this.originalPic = BitmapImage2Bitmap(bitmap);
            this.pA = a;
            this.pB = b;
            this.pC = c;
            this.pD = d;
        }

        private void BuildTransformed() {
            CreateNewBitmap();
            StartTransformation();
        }

        private void StartTransformation() {


            //Eine Horizontale Linie
            int stepsHorizontal = (int)originalPic.Width;
            Point vektorAB = pA.Minus(pB);
            for (int i = 0; i < stepsHorizontal; i++) {
                var temp = vektorAB.Multiply(i);
                Point pos = temp.Divided((int)pA.Distance(pB));
                Point pOrig = pA.Plus(pos);
                Point pResult = new Point(stepsHorizontal, 0);
                Color color = originalPic.GetPixel(pOrig.X, pOrig.Y);
                transformed.SetPixel(pResult.X, pResult.Y, color);
            }


        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage) {
            using (MemoryStream outStream = new MemoryStream()) {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }

        private Point Vektor(Point pA, Point pB) {
            return new Point(pB.X - pA.X, pB.Y - pA.Y);
        }

        private void CreateNewBitmap() {
            //finde die groessten Seitenlaengen
            double distanceA = Distance(pA, pB);
            double distanceB = Distance(pB, pC);
            double distanceC = Distance(pC, pD);
            double distanceD = Distance(pD, pA);
            double horizontal = Math.Max(distanceA, distanceC);
            double vertical = Math.Max(distanceB, distanceD);
            transformed = new Bitmap((int)horizontal, (int)vertical);
        }

        private double Distance(Point a, Point b) {
            return a.Distance(b);
        }
    }

    public static class PointExtension {

        public static Point Plus(this Point pA, Point pB) {
            return new Point(pB.X + pA.X, pB.Y + pA.Y);
        }

        public static Point Minus(this Point pA, Point pB) {
            return new Point(pB.X - pA.X, pB.Y - pA.Y);
        }

        public static Point Multiply(this Point pA, int length) {
            return new Point(pA.X * length, pA.Y * length);
        }

        public static Point Divided(this Point pA, int length) {
            return new Point(pA.X / length, pA.Y / length);
        }

        public static double Distance(this Point a, Point b) {
            double x2 = Math.Pow(a.X - b.X, 2);
            double y2 = Math.Pow(a.Y - b.Y, 2);
            double res = Math.Sqrt(x2 + y2);
            return res;
        }
    }




}
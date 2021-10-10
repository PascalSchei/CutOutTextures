using System;
using System.Drawing;

namespace CutOutTextures {
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
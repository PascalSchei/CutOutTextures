using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CutOutTextures {
    class Viereck {

        public bool defined = false;
        public Point? pointA;
        public Point? pointB;
        public Point? pointC;
        public Point? pointD;
        private Line lineA;
        private Line lineB;
        private Line lineC;
        private Line lineD;
        private Canvas canvas;

        public Viereck(Canvas canvas1) {
            pointA = null;
            pointB = null;
            pointC = null;
            pointD = null;
            canvas = canvas1;
        }

        public void AddPoint(Point point) {
            if (pointA == null) {
                pointA = point;
                return;
            }
            if (pointB == null) {
                pointB = point;
                lineA = AddNewLIne(pointA, pointB);
                return;
            }
            if (pointC == null) {
                pointC = point;
                lineB = AddNewLIne(pointB, pointC);
                return;
            }
            pointD = point;
            lineC = AddNewLIne(pointC, pointD);
            lineD = AddNewLIne(pointD, pointA);
            defined = true;
        }


        private Line AddNewLIne(Point? p1, Point? p2) {
            Line line = new Line();
            line.Visibility = Visibility.Visible;
            line.StrokeThickness = 4;
            line.Stroke = Brushes.Black;
            line.X1 = p1.Value.X;
            line.Y1 = p1.Value.Y;
            line.X2 = p2.Value.X;
            line.Y2 = p2.Value.Y;
            canvas.Children.Add(line);
            return line;
        }

    }
}
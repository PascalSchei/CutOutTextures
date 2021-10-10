using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Timer = System.Timers.Timer;

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
        private Ellipse ellipseA;


        public Viereck(Canvas canvas1) {

            //form button           
            canvas = canvas1;
            canvas.MouseUp += EllipseKlickUp;
            canvas.MouseMove += UpdatePosition;

            pointA = null;
            pointB = null;
            pointC = null;
            pointD = null;

        }



        public void AddPoint(Point point) {
            if (pointA == null) {
                pointA = point;
                AddElllipse(point);
                return;
            }
            if (pointB == null) {
                pointB = point;
                lineA = AddNewLIne(pointA, pointB);
                AddElllipse(point);
                return;
            }
            if (pointC == null) {
                pointC = point;
                lineB = AddNewLIne(pointB, pointC);
                AddElllipse(point);
                return;
            }
            pointD = point;
            lineC = AddNewLIne(pointC, pointD);
            lineD = AddNewLIne(pointD, pointA);
            defined = true;


        }

        private void AddElllipse(Point? pointA) {
            Ellipse neu = new Ellipse();
            ellipseA = neu;
            canvas.Children.Add(ellipseA);
            neu.Visibility = Visibility.Visible;
            neu.Stroke = Brushes.Red;
            neu.StrokeThickness = 2;


            Canvas.SetLeft(ellipseA, pointA.Value.X - radius);
            Canvas.SetTop(ellipseA, pointA.Value.Y - radius);

            neu.Width = 2 * radius;
            neu.Height = 2 * radius;

            ellipseA.MouseRightButtonDown += EllipseKlickDown;
            ellipseA.MouseEnter += (s, e) => ellipseA.Fill = Brushes.Red;
            ellipseA.MouseLeave += (s, e) => ellipseA.Fill = Brushes.Transparent;

        }


        private void loopTimerEvent(object sender, System.Timers.ElapsedEventArgs e) {
            Point mouseClick = Mouse.GetPosition(canvas);
            Canvas.SetLeft(ellipseA, mouseClick.X - radius);
            Canvas.SetTop(ellipseA, mouseClick.Y - radius);
            //canvas.UpdateLayout();
        }

        private void UpdatePosition(object sender, MouseEventArgs e) {
            if (IsMouseDown) {
                Point mouseClick = e.GetPosition(canvas);
                Canvas.SetLeft(ellipseA, mouseClick.X - radius);
                Canvas.SetTop(ellipseA, mouseClick.Y - radius);
            }
        }


        int radius = 8;
        bool IsMouseDown;


        private void EllipseKlickDown(object sender, MouseButtonEventArgs e) {
            IsMouseDown = true;
        }

        private void EllipseKlickUp(object sender, MouseButtonEventArgs e) {
            IsMouseDown = false;
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
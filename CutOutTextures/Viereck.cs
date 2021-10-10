using System;
using System.Collections.Generic;
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
        public BeweglicherPunkt pointA;
        public BeweglicherPunkt pointB;
        public BeweglicherPunkt pointC;
        public BeweglicherPunkt pointD;
        private Line lineA;
        private Line lineB;
        private Line lineC;
        private Line lineD;
        private Canvas canvas;
        List<BeweglicherPunkt> allePunkte;


        public Viereck(Canvas canvas1) {

            //form button           
            canvas = canvas1;
            pointA = null;
            pointB = null;
            pointC = null;
            pointD = null;

            allePunkte = new List<BeweglicherPunkt> {
                pointA,
                pointB,
                pointC,
                pointD
            };

        }





        public void AddPoint(Point point) {
            if (pointA == null) {
                pointA = new BeweglicherPunkt(point, canvas);
                return;
            }
            if (pointB == null) {
                pointB = new BeweglicherPunkt(point, canvas);
                pointA.AddEndPunkt(pointB);
                pointB.AddStartPunkt(pointA);
                lineA = AddLineToCanvas(pointA, pointB);
                pointA.AddLineStart(lineA);
                pointB.AddLineEnd(lineA);
                return;
            }
            if (pointC == null) {
                pointC = new BeweglicherPunkt(point, canvas);
                pointB.AddEndPunkt(pointC);
                pointC.AddStartPunkt(pointB);
                lineB = AddLineToCanvas(pointB, pointC);
                pointB.AddLineStart(lineB);
                pointC.AddLineEnd(lineB);
                return;
            }
            if (pointD == null) {
                pointD = new BeweglicherPunkt(point, canvas);
                pointC.AddEndPunkt(pointD);
                pointD.AddStartPunkt(pointC);
                pointD.AddEndPunkt(pointA);
                pointA.AddStartPunkt(pointD);
                lineC = AddLineToCanvas(pointC, pointD);
                pointC.AddLineStart(lineC);
                pointD.AddLineEnd(lineC);
                lineD = AddLineToCanvas(pointD, pointA);
                pointD.AddLineStart(lineD);
                pointA.AddLineEnd(lineD);
                defined = true;
            }
        }




        private Line AddLineToCanvas(BeweglicherPunkt p1, BeweglicherPunkt p2) {
            Line line = new Line();
            line.Visibility = Visibility.Visible;
            line.StrokeThickness = 4;
            line.Stroke = Brushes.Black;
            line.X1 = p1.PPoint.X;
            line.Y1 = p1.PPoint.Y;
            line.X2 = p2.PPoint.X;
            line.Y2 = p2.PPoint.Y;
            canvas.Children.Add(line);
            return line;
        }

    }




    public class BeweglicherPunkt {
        public Point PPoint { get; set; }
        Ellipse PEllipse;
        Canvas PCanvas;

        BeweglicherPunkt start;
        BeweglicherPunkt end;

        Line lineStart;
        Line lineEnd;

        int radius = 8;
        public bool IsMouseDown;

        public BeweglicherPunkt(Point point, Canvas canvas) {
            PPoint = point;
            PCanvas = canvas;
            canvas.MouseUp += EllipseKlickUp;
            canvas.MouseMove += UpdatePosition;
            AddElllipse();
        }

        internal void AddStartPunkt(BeweglicherPunkt point) {
            start = point;
        }

        internal void AddEndPunkt(BeweglicherPunkt point) {
            end = point;
        }


        private void AddElllipse() {
            PEllipse = new Ellipse();
            PCanvas.Children.Add(PEllipse);            
            PEllipse.Visibility = Visibility.Visible;
            PEllipse.Stroke = Brushes.Red;
            PEllipse.StrokeThickness = 2;
            Canvas.SetLeft(PEllipse, PPoint.X - radius);
            Canvas.SetTop(PEllipse, PPoint.Y - radius);
            PEllipse.Width = 2 * radius;
            PEllipse.Height = 2 * radius;
            PEllipse.MouseRightButtonDown += EllipseKlickDown;
            PEllipse.MouseEnter += (s, e) => PEllipse.Fill = Brushes.Red;
            PEllipse.MouseLeave += (s, e) => PEllipse.Fill = Brushes.Transparent;

        }

        private void EllipseKlickUp(object sender, MouseButtonEventArgs e) {
            IsMouseDown = false;
        }


        private void UpdatePosition(object sender, MouseEventArgs e) {
            if (IsMouseDown) {
                Point mousePos = e.GetPosition(PCanvas);
                PPoint = mousePos;
                UpdateLine();
                UpdateEllipse();
                PCanvas.UpdateLayout();
            }
        }

        private void UpdateLine() {
            if (lineStart != null) {
                lineStart.X1 = PPoint.X;
                lineStart.Y1 = PPoint.Y;
                lineStart.X2 = end.PPoint.X;
                lineStart.Y2 = end.PPoint.Y;
            }
            if (lineEnd != null) {
                lineEnd.X1 = start.PPoint.X;
                lineEnd.Y1 = start.PPoint.Y;
                lineEnd.X2 = PPoint.X;
                lineEnd.Y2 = PPoint.Y;

            }
        }

        private void UpdateEllipse() {
            Canvas.SetLeft(PEllipse, PPoint.X - radius);
            Canvas.SetTop(PEllipse, PPoint.Y - radius);
        }

        private void EllipseKlickDown(object sender, MouseButtonEventArgs e) {
            PEllipse = (Ellipse)sender;
            IsMouseDown = true;
        }

        internal void AddLineStart(Line plineStart) {
            lineStart = plineStart;
            //ZIndex erneuern
            PCanvas.Children.Remove(PEllipse);
            PCanvas.Children.Add(PEllipse);
        }

        internal void AddLineEnd(Line pLineEnd) {
            lineEnd = pLineEnd;
            //ZIndex erneuern
            PCanvas.Children.Remove(PEllipse);
            PCanvas.Children.Add(PEllipse);
        }


    }

}
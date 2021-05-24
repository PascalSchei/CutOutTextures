using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CutOutTextures {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            viereck = new Viereck(canvas1);
        }

        private FileInfo openFile;
        private Viereck viereck;

        private void Button_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                openFile = new FileInfo(openFileDialog.FileName);
            }
            ShowImage();

        }

        private void ShowImage() {
            if (openFile == null) {
                return;
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(openFile.FullName);
            bitmap.EndInit();
            imageviewer.Source = bitmap;
        }


        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Point mouseClick = e.GetPosition(canvas1);
            viereck.AddPoint(mouseClick);
            GetBitmap();
        }

        private void GetBitmap() {
            if (viereck.defined) {
                Point a = canvas1.TranslatePoint(viereck.pointA.Value, imageviewer);
                Point b = canvas1.TranslatePoint(viereck.pointB.Value, imageviewer);
                Point c = canvas1.TranslatePoint(viereck.pointC.Value, imageviewer);
                Point d = canvas1.TranslatePoint(viereck.pointD.Value, imageviewer);
            }
        }
    }

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
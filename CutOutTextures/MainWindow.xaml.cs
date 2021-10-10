using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace CutOutTextures {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {


        private BitmapImage bitmap;
        public MainWindow() {
            InitializeComponent();
            viereck = new Viereck(canvas1);

            canvas1.MouseUp += UpdateImage;

        }



        private FileInfo openFile;
        private Viereck viereck;




        private void Button_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true) {
                openFile = new FileInfo(openFileDialog.FileName);
            }
            ShowImage();
            viereck = new Viereck(canvas1);
            canvas1.Children.Clear();
        }

        private void ShowImage() {
            if (openFile == null) {
                return;
            }
            bitmap = new BitmapImage();
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


        private void UpdateImage(object sender, MouseButtonEventArgs e) {
            GetBitmap();
        }

        private void GetBitmap() {
            if (!viereck.defined) {
                return;
            }
            if (bitmap == null) {
                return;
            }
            double zoom = imageviewer.ActualHeight / bitmap.Height;
            Point a = canvas1.TranslatePoint(viereck.pointA.PPoint, imageviewer);
            Point b = canvas1.TranslatePoint(viereck.pointB.PPoint, imageviewer);
            Point c = canvas1.TranslatePoint(viereck.pointC.PPoint, imageviewer);
            Point d = canvas1.TranslatePoint(viereck.pointD.PPoint, imageviewer);

            BitmapTransformed transformed = new BitmapTransformed(bitmap, a, b, c, d, zoom);
            imageResult.Source = transformed.resultImage;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            viereck = new Viereck(canvas1);
            canvas1.Children.Clear();
        }
    }
}
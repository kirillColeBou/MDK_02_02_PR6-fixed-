using System;
using System.Collections.Generic;
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

namespace RegIn_Тепляков.Elements
{
    /// <summary>
    /// Логика взаимодействия для ElementCapture.xaml
    /// </summary>
    public partial class ElementCapture : UserControl
    {
        public CorrectCapture HandlerCorrectCapture;
        public delegate void CorrectCapture();
        string strCapture = "";
        int ElementWidth = 280;
        int ElementHeight = 50;

        public ElementCapture()
        {
            InitializeComponent();
            CreateCapture();
        }

        public void CreateCapture()
        {
            InputCapture.Text = "";
            Capture.Children.Clear();
            strCapture = "";
            CreateBackground();
            Background();
        }

        private void GenerateRandom(bool flag)
        {
            Random random = new Random();
            int n = flag ? 100 : 4;
            var fontSize = flag ? random.Next(10, 16) : 30;
            for (int i = 0; i < n; i++)
            {
                int back = random.Next(0, 10);
                var margin = flag ? new Thickness(random.Next(0, ElementWidth - 20), random.Next(0, ElementHeight - 20), 0, 0) : new Thickness(ElementWidth / 2 - 60 + i * 30, random.Next(-10, 10), 0, 0);
                Label label = new Label()
                {
                    Content = back,
                    FontSize = fontSize,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255))),
                    Margin = margin
                };
                if (flag) Capture.Children.Add(label);
                else { strCapture += back.ToString(); Capture.Children.Add(label); }
            }
        }

        #region CreateCapture
        public void CreateBackground() => GenerateRandom(true);

        public void Background() => GenerateRandom(false);
        #endregion

        public bool OnCapture() => strCapture == InputCapture.Text;

        private void EnterCapture(object sender, KeyEventArgs e)
        {
            if (InputCapture.Text.Length == 4)
                if (!OnCapture()) CreateCapture();
                else if (HandlerCorrectCapture != null) HandlerCorrectCapture.Invoke();
        }
    }
}

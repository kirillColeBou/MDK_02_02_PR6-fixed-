using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegIn_Тепляков.Pages
{
    /// <summary>
    /// Логика взаимодействия для Recovery.xaml
    /// </summary>
    public partial class Recovery : Page
    {
        string OldLogin;
        bool IsCapture = false;

        public Recovery()
        {
            InitializeComponent();
            MainWindow.mainWindow.userLogin.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.userLogin.HandlerInCorrectLogin += InCorrectLogin;
            Capture.HandlerCorrectCapture += CorrectCapture;
        }

        void animation(bool flag)
        {
            BitmapImage img = new BitmapImage();
            ImageSource src = img;
            if (flag)
            {
                MemoryStream ms = new MemoryStream(MainWindow.mainWindow.userLogin.Image);
                img.BeginInit();
                img.StreamSource = ms;
                img.EndInit();
                src = img;
            }
            DoubleAnimation start = new DoubleAnimation();
            start.From = 1;
            start.To = 0;
            start.Duration = TimeSpan.FromSeconds(0.6);
            start.Completed += delegate
            {
                if (flag) IUser.Source = src;
                else IUser.Source = new BitmapImage(new Uri("pack://application:,,,/Images/ic-user.png"));
                DoubleAnimation end = new DoubleAnimation();
                end.From = 0;
                end.To = 1;
                end.Duration = TimeSpan.FromSeconds(1.2);
                IUser.BeginAnimation(OpacityProperty, start);
            };
            IUser.BeginAnimation(OpacityProperty, start);
        }

        public void CorrectLogin()
        {
            if (OldLogin != TbLogin.Text)
            {
                SetNotification("Hi, " + MainWindow.mainWindow.userLogin.Name, Brushes.Black);
                try
                {
                    animation(true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                };
                OldLogin = TbLogin.Text;
                SendNewPassword();
            }
        }

        public void InCorrectLogin()
        {
            if (LNameUser.Content.ToString() != "")
            {
                LNameUser.Content = "";
                animation(false);
            }
            if (TbLogin.Text.Length > 0) SetNotification("Login is incorrect", Brushes.Red);
        }

        public void CorrectCapture()
        {
            Capture.IsEnabled = false;
            IsCapture = true;
            SendNewPassword();
        }

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) MainWindow.mainWindow.userLogin.GetUserLogin(TbLogin.Text);
        }

        private void SetLogin(object sender, RoutedEventArgs e) => MainWindow.mainWindow.userLogin.GetUserLogin(TbLogin.Text);

        public void SendNewPassword()
        {
            if (IsCapture)
            {
                if (MainWindow.mainWindow.userLogin.Password != String.Empty)
                {
                    animation(false);
                    SetNotification("An email has been sent to your email.", Brushes.Black);
                    MainWindow.mainWindow.userLogin.CreateNewPassword();
                }
            }
        }

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Login());
    }
}

using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Imaging = Aspose.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace RegIn_Тепляков.Pages
{
    /// <summary>
    /// Логика взаимодействия для Regin.xaml
    /// </summary>
    public partial class Regin : Page
    {
        OpenFileDialog opf = new OpenFileDialog();
        bool BCorrectLogin = false;
        bool BCorrectPassword = false;
        bool BCorrectConfirmPassword = false;
        bool BSetImages = false;

        public Regin()
        {
            InitializeComponent();
            MainWindow.mainWindow.userLogin.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.userLogin.HandlerInCorrectLogin += InCorrectLogin;
            opf.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg";
            opf.RestoreDirectory = true;
            opf.Title = "Choose a photo for your avatar";
        }

        private void CorrectLogin()
        {
            SetNotification("Login already in use.", Brushes.Red);
            BCorrectLogin = false;
        }

        private void InCorrectLogin() => SetNotification("", Brushes.Black);

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SetLogin();
        }

        private void SetLogin(object sender, RoutedEventArgs e) => SetLogin();

        void SetLogin()
        {
            Regex regex = new Regex(@"([a-zA-Z0-9._-]{4,}@[a-zA-Z0-9._-]{2,}\.[a-zA-Z0-9._-]{2,})");
            BCorrectLogin = regex.IsMatch(TbLogin.Text);
            if (regex.IsMatch(TbLogin.Text))
            {
                SetNotification("", Brushes.Black);
                MainWindow.mainWindow.userLogin.GetUserLogin(TbLogin.Text);
            }
            else SetNotification("Invalid login", Brushes.Red);
            OnRegin();
        }

        private void SetPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SetPassword();
        }

        private void SetPassword(object sender, RoutedEventArgs e) => SetPassword();

        void SetPassword()
        {
            Regex regex = new Regex(@"(?=.*[0-9])(?=.*[!@#$%^&?*\-_=])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&?*\-_=]{10,}");
            BCorrectLogin = regex.IsMatch(TbPassword.Password);
            if (regex.IsMatch(TbPassword.Password) == true)
            {
                SetNotification("", Brushes.Black);
                if (TbConfirmPassword.Password.Length > 0) ConfirmPassword(true);
                OnRegin();
            }
            else SetNotification("Invalid password", Brushes.Red);
        }

        private void ConfirmPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ConfirmPassword();
        }

        private void ConfirmPassword(object sender, RoutedEventArgs e) => ConfirmPassword();

        void ConfirmPassword(bool pass = false)
        {
            BCorrectConfirmPassword = TbConfirmPassword.Password == TbPassword.Password;
            if (TbConfirmPassword.Password != TbPassword.Password) SetNotification("Password don't match", Brushes.Red);
            else
            {
                SetNotification("", Brushes.Black);
                if (!pass) SetPassword();
            }
        }

        void OnRegin()
        {
            if (!BCorrectLogin || TbName.Text.Length == 0 || !BCorrectPassword || !BCorrectConfirmPassword) return;
            MainWindow.mainWindow.userLogin.Login = TbLogin.Text;
            MainWindow.mainWindow.userLogin.Password = TbPassword.Password;
            MainWindow.mainWindow.userLogin.Name = TbName.Text;
            if (BSetImages) MainWindow.mainWindow.userLogin.Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\IUser.jpg");
            MainWindow.mainWindow.userLogin.DateUpdate = DateTime.Now;
            MainWindow.mainWindow.userLogin.DateCreate = DateTime.Now;
            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Regin));
        }

        private void SetName(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }

        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            if (opf.ShowDialog() == true)
            {
                using (Imaging.Image image = Imaging.Image.Load(opf.FileName))
                {
                    int NewWidth = 0; int NewHeight = 0;
                    if (image.Width > image.Height)
                    {
                        NewWidth = (int)(image.Width / (256f / image.Height));
                        NewHeight = 256;
                    }
                    else
                    {
                        NewWidth = 256;
                        NewHeight = (int)(image.Height * (256f / image.Width));
                    }
                    image.Resize(NewWidth, NewHeight);
                    image.Save("IUser.jpg");
                }
                using (Imaging.RasterImage rasterImage = (Imaging.RasterImage)Imaging.Image.Load("IUser.jpg"))
                {
                    if (!rasterImage.IsCached) rasterImage.CacheData();
                    int X = 0; int Width = 256; int Y = 0; int Height = 256;
                    if (rasterImage.Width > rasterImage.Height) X = (int)((rasterImage.Width - 256f) / 2);
                    else Y = (int)((rasterImage.Height - 256f) / 2);
                    Imaging.Rectangle rectangle = new Imaging.Rectangle(X, Y, Width, Height);
                    rasterImage.Crop(rectangle);
                    rasterImage.Save("IUser.jpg");
                }
                DoubleAnimation StartAnimation = new DoubleAnimation();
                StartAnimation.From = 1;
                StartAnimation.To = 0;
                StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                StartAnimation.Completed += delegate
                {
                    IUser.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\IUser.jpg"));
                    DoubleAnimation EndAnimation = new DoubleAnimation();
                    EndAnimation.From = 0;
                    EndAnimation.To = 1;
                    EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                    IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                };
                IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
                BSetImages = true;
            }
            else BSetImages = false;
        }

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Login());
    }
}

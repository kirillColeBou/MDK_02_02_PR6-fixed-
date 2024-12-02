using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace RegIn_Тепляков.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPinCode.xaml
    /// </summary>
    public partial class AddPinCode : Page
    {
        public enum TypeConfirmation
        {
            Login, Regin
        }

        TypeConfirmation ThisTypeConfirmation;
        private static string typeConfirmation;
        public AddPinCode(string _typeConfirmation)
        {
            InitializeComponent();
            typeConfirmation = _typeConfirmation;
            ChangeLabel();
        }

        void ChangeLabel()
        {
            if (typeConfirmation == "Login")
            {
                ThisTypeConfirmation = TypeConfirmation.Login;
                if (MainWindow.mainWindow.userLogin.PinCode != String.Empty) { BTextPinCode.Content = "Enter your pin code."; BAddPinCode.Visibility = Visibility.Hidden; LAnotherTime.Visibility = Visibility.Hidden; }
                else BTextPinCode.Content = "Add a pin code for quick authorization.";
            }
            else if (typeConfirmation == "Regin")
            {
                ThisTypeConfirmation = TypeConfirmation.Regin;
                BTextPinCode.Content = "Add a pin code for quick authorization.";
            }
        }

        private void SetPinCode(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SetPinCode();
        }

        void SetPinCode()
        {
            Regex regex = new Regex(@"\d{4}");
            if (regex.IsMatch(TbPinCode.Text) == true)
            {
                if (ThisTypeConfirmation == TypeConfirmation.Login)
                {
                    if(MainWindow.mainWindow.userLogin.PinCode != String.Empty && TbPinCode.Text == MainWindow.mainWindow.userLogin.PinCode) MainWindow.mainWindow.OpenPage(new Main());
                    else if(MainWindow.mainWindow.userLogin.PinCode == String.Empty)
                    {
                        MainWindow.mainWindow.userLogin.AddPinCode(TbPinCode.Text);
                        MainWindow.mainWindow.OpenPage(new Main());
                    }
                }
                else if (ThisTypeConfirmation == TypeConfirmation.Regin)
                {
                    MainWindow.mainWindow.userLogin.AddPinCode(TbPinCode.Text);
                    MainWindow.mainWindow.OpenPage(new Main());
                }
                SetNotification("", Brushes.Black);
            }
            else SetNotification("Invalid pin-code", Brushes.Red);
        }

        private void AddPincode(object sender, RoutedEventArgs e) => SetPinCode();

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LPinCode.Content = Message;
            LPinCode.Foreground = _Color;
        }

        private void OpenMain(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Main());
    }
}

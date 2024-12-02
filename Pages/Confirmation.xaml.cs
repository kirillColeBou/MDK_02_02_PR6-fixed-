using RegIn_Тепляков.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Page
    {
        public enum TypeConfirmation
        {
            Login, Regin
        }

        TypeConfirmation ThisTypeConfirmation;

        public int Code = 0;

        public Confirmation(TypeConfirmation TypeConfirmation)
        {
            InitializeComponent();
            ThisTypeConfirmation = TypeConfirmation;
            SendMailCode();
        }

        public void SendMailCode()
        {
            Code = new Random().Next(100000, 999999);
            Classes.SendMail.SendMessage($"Login code: {Code}", MainWindow.mainWindow.userLogin.Login);
            Thread TSendMailCode = new Thread(TimerSendMailCode);
            TSendMailCode.Start();
        }

        public void TimerSendMailCode()
        {
            for (int i = 0; i < 60; i++)
            {
                Dispatcher.Invoke(() => LTimer.Content = $"A second message can be sent after {60 - i} seconds");
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() =>
            {
                BSendMessage.IsEnabled = true;
                LTimer.Content = "";
            });
        }

        private void SendMail(object sender, RoutedEventArgs e) => SendMailCode();

        private void SetCode(object sender, KeyEventArgs e)
        {
            if (TbCode.Text.Length == 6) SetCode();
        }

        private void SetCode(object sender, RoutedEventArgs e) => SetCode();

        void SetCode()
        {
            string typeConfirmation;
            if (TbCode.Text == Code.ToString() && TbCode.IsEnabled == true)
            {
                TbCode.IsEnabled = false;
                if (ThisTypeConfirmation == TypeConfirmation.Login) { typeConfirmation = "Login"; MainWindow.mainWindow.OpenPage(new AddPinCode(typeConfirmation)); }
                else
                {
                    MainWindow.mainWindow.userLogin.SetUser();
                    typeConfirmation = "Regin"; 
                    MainWindow.mainWindow.OpenPage(new AddPinCode(typeConfirmation));
                }
            }
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e) => MainWindow.mainWindow.OpenPage(new Login());
    }
}

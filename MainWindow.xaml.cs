using RegIn_Тепляков.Classes;
using RegIn_Тепляков.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace RegIn_Тепляков
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;
        public User userLogin = new User();

        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
            OpenPage(new Login());
        }

        public void OpenPage(Page page)
        {
            DoubleAnimation start = new DoubleAnimation();
            start.From = 1;
            start.To = 0;
            start.Duration = TimeSpan.FromSeconds(0.6);
            start.Completed += delegate
            {
                frame.Navigate(page);
                DoubleAnimation end = new DoubleAnimation();
                end.From = 0;
                end.To = 1;
                end.Duration = TimeSpan.FromSeconds(1.2);
                frame.BeginAnimation(Frame.OpacityProperty, end);
            };
            frame.BeginAnimation(Frame.OpacityProperty, start);
        }
    }
}

using LiLo_Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace LiLo_WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TimeUpdater();
        }

        async void TimeUpdater()
        {
            while(true)
            {
                Time.Text = DateTime.Now.ToString();
                await Task.Delay(1000);
            }
        }
    }
}

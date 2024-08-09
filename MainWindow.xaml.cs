using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using YoutubeExplode;
using YoutubeExplode.Converter;
using System.IO;
using Microsoft.Win32;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;
using System;



namespace YoutubeDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml  sds
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();


            MainContentControl.Content = new DownloadView();
        }

        
    }
}
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

namespace YoutubeDownloader
{
    /// <summary>
    /// Logika interakcji dla klasy AboutView.xaml
    /// </summary>
    public partial class AboutView : UserControl
    {
        public DownloadView downloadView { get; set; }

        public AboutView()
        {

            InitializeComponent();
        }

        private void Homepage_Button(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Content = downloadView;
        }
    }
}

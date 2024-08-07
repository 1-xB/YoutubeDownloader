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
using System.Windows;
using Microsoft.Win32;
using YoutubeExplode.Common;
using Xabe.FFmpeg;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml  sds
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private bool _start = true;



        public bool IsVideoDownloaderClicked = true;
        
        public MainWindow()
        {
            InitializeComponent();

            if (_start)
            {
                VideoLocalization.Text = $"C:\\Users\\{Environment.UserName}\\Videos\\";
                _start = false;
            }
            
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Downloading");

            string videoUrl = YoutubeLink.Text;
            string outputPath = VideoLocalization.Text;
            string ffmpegPath = "ffmpeg.exe";

            char[] illegalsybols = new char[] { '\\', '/', ':', '?', '<', '>', '|' };

            

            


            if (IsVideoDownloaderClicked)
            {
                try
                {
                    var youtube = new YoutubeClient();

                    var video = await youtube.Videos.GetAsync(videoUrl);

                    string title = video.Title;

                    outputPath = GetOutput(title, outputPath);

                    await youtube.Videos.DownloadAsync(
                        videoUrl,
                        outputPath,
                        builder =>
                        {
                            builder.SetPreset(YoutubeExplode.Converter.ConversionPreset.UltraFast)
                                   .SetFFmpegPath(ffmpegPath);
                        });

                    MessageBox.Show("Pobieranie zakończone pomyślnie! ");
                }

                catch (Exception ex)
                {
                    MessageBox.Show(outputPath + "Wystąpił błąd podczas pobierania filmu: " + ex.Message);
                }
            }

            else
            {
                

                try
                {
                    MessageBox.Show("t");

                    string playlistUrl = YoutubeLink.Text;

                    var youtube = new YoutubeClient();

                    var videos = await youtube.Playlists.GetVideosAsync(playlistUrl);

                    foreach (var video in videos)
                    {

                        string title = video.Title;

                        outputPath = GetOutput(title, outputPath);

                        await youtube.Videos.DownloadAsync(video.Url, outputPath, builder =>
                        {
                            builder.SetPreset(YoutubeExplode.Converter.ConversionPreset.UltraFast)
                                   .SetFFmpegPath(ffmpegPath);
                        });

                        outputPath = VideoLocalization.Text;
                    }



                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd" +  ex.Message);
                }
            }
            

        }

        private string GetOutput(string title, string outputPath)
        {
            char[] illegalsybols = new char[] { '\\', '/', ':', '?', '<', '>', '|' };

            if (!string.IsNullOrEmpty(title))
            {
                foreach (char symbol in illegalsybols)
                {
                    title = title.Replace(symbol.ToString(), "");
                }

                outputPath += title.Trim().ToLower().Substring(0, title.Length / 2);

                if (File.Exists(outputPath + ".mp4"))
                {
                    outputPath += "-copy";
                }

                outputPath += ".mp4";// Pobieranie filmu i zapisywanie go
                return outputPath;
            }

            else
            {
                return outputPath;
            }
        }

        private void VideoDownloaderButton_Click(object sender, RoutedEventArgs e)
        {

            if (!IsVideoDownloaderClicked)
            {
                VideoDownloader.Foreground = new SolidColorBrush(Colors.Red);
                PlaylistDownloader.Foreground = new SolidColorBrush(Colors.Gray);
                IsVideoDownloaderClicked = true;
            }

        }

        private void PlaylistDownloaderButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsVideoDownloaderClicked)
            {
                PlaylistDownloader.Foreground = new SolidColorBrush(Colors.Red);
                VideoDownloader.Foreground = new SolidColorBrush(Colors.Gray);
                IsVideoDownloaderClicked = false;
            }
        }

        private void ChangeButton(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            dialog.DefaultDirectory = @"C:\";
            dialog.ShowDialog();
            VideoLocalization.Text = dialog.FolderName + "\\";
        }
    }
}
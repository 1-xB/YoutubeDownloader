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
    /// Logika interakcji dla klasy DownloadView.xaml
    /// </summary>
    public partial class DownloadView : UserControl
    {
        private bool _start = true;
        public bool IsVideoDownloaderClicked = true;

        public DownloadView()
        {
            InitializeComponent();

            if (_start)
            {
                VideoLocalization.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\";
                _start = false;
            }
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateProgressBar(0);
            OffOnButtons(false);

            string videoUrl = YoutubeLink.Text;
            string outputPath = VideoLocalization.Text;
            string ffmpegPath = @"ffmpeg.exe";

            StackPanelProgressBar.Visibility = Visibility.Visible;
            AboutVideoStackPanel.Visibility = Visibility.Visible;




            if (IsVideoDownloaderClicked)
            {
                try
                {
                    // inicjacja youtubeclient
                    var youtube = new YoutubeClient();
                    UpdateProgressBar(progressBar.Value + 25);

                    // pobieranie informacji o video
                    var video = await youtube.Videos.GetAsync(videoUrl);

                    AuthorTextBlock.Text = video.Author.ToString();
                    TitleTextBlock.Text = video.Title;

                    UpdateProgressBar(progressBar.Value + 25);



                    string title = video.Title;
                    // pobieranie dobrej ścieżki pliku 
                    outputPath = GetOutput(title, outputPath);
                    UpdateProgressBar(progressBar.Value + 25);



                    await youtube.Videos.DownloadAsync(videoUrl, outputPath, builder =>
                    {

                        builder.SetPreset(ConversionPreset.Medium)
                               .SetFFmpegPath(ffmpegPath);

                    });

                    UpdateProgressBar(progressBar.Value + 25);



                    MessageBox.Show("Download completed successfully! ");

                }

                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while downloading the video: " + ex.Message);
                }
            }

            else
            {


                try
                {

                    string playlistUrl = YoutubeLink.Text;

                    var youtube = new YoutubeClient();

                    var videos = await youtube.Playlists.GetVideosAsync(playlistUrl);

                    int videoNumber = 0;

                    PlaylistVideoCounter.Visibility = Visibility.Visible;


                    foreach (var video in videos)
                    {
                        AuthorTextBlock.Text = video.Author.ToString();
                        TitleTextBlock.Text = video.Title;

                        UpdateProgressBar(progressBar.Value + 35, videos.Count, ++videoNumber);
                        await Task.Delay(200);
                        string title = video.Title;

                        UpdateProgressBar(progressBar.Value + 20);
                        await Task.Delay(200);

                        outputPath = GetOutput(title, outputPath);

                        UpdateProgressBar(progressBar.Value + 25);

                        await youtube.Videos.DownloadAsync(video.Url, outputPath, builder =>
                        {

                            builder.SetPreset(ConversionPreset.Medium)
                                   .SetFFmpegPath(ffmpegPath);

                        });
                        UpdateProgressBar(progressBar.Value + 20);




                        outputPath = VideoLocalization.Text;

                        await Task.Delay(200);
                        UpdateProgressBar(0);



                    }





                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd" + ex.Message);

                }
            }
            UpdateProgressBar(0);
            StackPanelProgressBar.Visibility = Visibility.Hidden;
            AboutVideoStackPanel.Visibility = Visibility.Hidden;
            OffOnButtons(true);

        }

        private void OffOnButtons(bool offOn)
        {

            if (offOn)
            {
                Download_button.Content = "Download";
            }
            else
            {
                Download_button.Content = "Download in progress...";
            }

            VideoDownloader.IsEnabled = offOn;
            PlaylistDownloader.IsEnabled = offOn;
            YoutubeLink.IsEnabled = offOn;
            VideoLocalization.IsEnabled = offOn;
            Download_button.IsEnabled = offOn;

        }

        private void UpdateProgressBar(double newProgress, int playlistLenght = 0, int videoNumber = 0)
        {
            progressBar.Value = newProgress;
            PercentProgressBar.Text = $"{newProgress}%";

            if (!IsVideoDownloaderClicked && playlistLenght != 0)
            {
                PlaylistVideoCounter.Text = $"{videoNumber}/{playlistLenght}";
            }


        }

        private string GetOutput(string title, string outputPath)
        {
            char[] illegalsybols = new char[] { '\\', '/', ':', '?', '<', '>', '|', '(', ')', '"', '`' };

            if (!string.IsNullOrEmpty(title))
            {
                foreach (char symbol in illegalsybols)
                {
                    title = title.Replace(symbol.ToString(), "");
                }

                if (title.Length <= 250)
                {
                    outputPath += title.Trim().ToLower();
                }

                else
                {
                    outputPath += title.Trim().ToLower().Substring(0, title.Length / 2);
                }



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

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Content = new AboutView();
        }
    }
}

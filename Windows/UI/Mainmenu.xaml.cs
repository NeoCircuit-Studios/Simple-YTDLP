using APPLogManager;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Windows.Foundation.Diagnostics;


namespace Simple_YTDLP.Windows.UI
{
    /// <summary>
    /// Interaction logic for Mainmenu.xaml
    /// </summary>
    public partial class Mainmenu : UserControl
    {
        public Mainmenu()
        {
            InitializeComponent();

            Installtools();
        }

        private async Task Installtools()
        {
            string extractPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NeoCircuit-Studios", "Simple-YTDLP", "tools"
            );

            var exeFiles = new[]
            {
                Path.Combine(extractPath, "ffmpeg.exe"),
                Path.Combine(extractPath, "ffplay.exe"),
                Path.Combine(extractPath, "ffprobe.exe"),
                Path.Combine(extractPath, "yt-dlp.exe")
            };

            string appDir = AppContext.BaseDirectory;

            var zipFiles = new Dictionary<string, string>
            {
                ["ffmpeg.exe"] = Path.Combine(appDir, "Core", "ThirdParty", "bin.1.tmp.guustPKG"),
                ["ffplay.exe"] = Path.Combine(appDir, "Core", "ThirdParty", "bin.2.tmp.guustPKG"),
                ["ffprobe.exe"] = Path.Combine(appDir, "Core", "ThirdParty", "bin.3.tmp.guustPKG"),
                ["yt-dlp.exe"] = Path.Combine(appDir, "Core", "ThirdParty", "bin.6.tmp.guustPKG")
            };

            if (exeFiles.All(File.Exists))
            {
                LogManager.LogToFile("All tools are already installed. Skipping installation.");
                //ProgressBar.Value = 100;

                InstallingText.Visibility = Visibility.Collapsed;
                ProgressBar.Visibility = Visibility.Visible;

                await ready();
                return;
            }

            Directory.CreateDirectory(extractPath);

            await Task.WhenAll(
                FadeIn(Background, 0, 300),
                FadeIn(InstallingText, 0, 300),
                FadeIn(ProgressBar, 0, 300)
            );

            ProgressBar.Value = 0;
            await Task.Delay(500);

            int totalSteps = zipFiles.Count * 2;
            int currentStep = 0;

            foreach (var kvp in zipFiles)
            {
                string exeName = kvp.Key;
                string zipPath = kvp.Value;

                if (!File.Exists(zipPath))
                {
                    LogManager.LogToFile($"Missing zip: {exeName}");
                    System.Diagnostics.Process.Start("updater.exe", $"-reinstall=\"{exeName}\""); //path is not okey
                }
            }

            foreach (var kvp in zipFiles)
            {
                string exeName = kvp.Key;
                string zipPath = kvp.Value;

                if (File.Exists(zipPath))
                {
                    try
                    {
                        LogManager.LogToFile($"Start unzipping {zipPath}...");
                        await Task.Run(() => ZipFile.ExtractToDirectory(zipPath, extractPath, overwriteFiles: true));
                        LogManager.LogToFile($"[{zipPath}] Unzipped to: {extractPath}");
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogToFile($"Failed to unzip {zipPath}: {ex.Message}");
                        System.Windows.MessageBox.Show(
                            $"A fatal error occurred! Could not unzip [{zipPath}].\nRecommended: reinstall the application.",
                            "Simple-YTDLP: BIG ERROR",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                }

                currentStep++;
                ProgressBar.Value = (currentStep * 100) / totalSteps;
                await Task.Delay(100);
            }

            foreach (var exeName in zipFiles.Keys)
            {
                string exePath = Path.Combine(extractPath, exeName);
                if (!File.Exists(exePath))
                {
                    LogManager.LogToFile($"ERROR: {exeName} not found after extraction!");
                }
                else
                {
                    LogManager.LogToFile($"{exeName} is present.");
                }

                currentStep++;
                ProgressBar.Value = (currentStep * 100) / totalSteps;
                await Task.Delay(50);
            }

            ProgressBar.Value = 100;
            LogManager.LogToFile("All tools are installed successfully.");

            InstallingText.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Visible;

            await ready();
            return;
        }


        private async Task ready()
        {
            var fadeOut = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(500),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            InstallingText.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            ProgressBar.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            await Task.WhenAll(
                FadeIn(infoText, 0, 300),
                FadeIn(YTLinkTextBox, 0, 300),
                FadeIn(okBUt, 0, 300)
            );
        }

        bool hasEnteredValidLink0 = false;
        string linkki = "NULL";
        string baseFolderName = "NSYTDLR_Playlist0";
        private string currentDownloadFolder = ""; // store the actual folder used

        bool isdone = false;
        private async void Okbut_Click(object sender, RoutedEventArgs e)
        {
            if (hasEnteredValidLink0 == true)
            {
                YTLinkTextBox.Text = linkki;
                LogManager.LogToFile($"{linkki}");
            }

            var fadeOut = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(500),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            infoText.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            YTLinkTextBox.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            okBUt.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            Background.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            Background.Source = new BitmapImage(new Uri("pack://application:,,,/Core/APP/sys/Downloading.png"));

            await Task.Delay(800);

            await Task.WhenAll(
                FadeIn(Background, 0, 300),
                FadeIn(downloadingTEXR, 0, 300),
                FadeIn(LogTextBox, 0, 300),
                FadeIn(LoadingIcon, 0, 300)
            );

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string folderName = baseFolderName;
            string fullPath = Path.Combine(desktopPath, folderName);

            int counter = 1;
            while (Directory.Exists(fullPath))
            {
                folderName = baseFolderName + counter; // "NSYTDLR_Playlist01", "NSYTDLR_Playlist02", ...
                fullPath = Path.Combine(desktopPath, folderName);
                counter++;
            }

            Directory.CreateDirectory(fullPath);
            currentDownloadFolder = fullPath; 
            LogManager.LogToFile("Folder created: " + fullPath);



            string toolsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NeoCircuit-Studios", "Simple-YTDLP", "tools"
            );
            string ytdlpPath = Path.Combine(toolsPath, "yt-dlp.exe");
            string outputTemplate = Path.Combine(fullPath, "%(title)s.%(ext)s"); 
            string arguments = $"-x --audio-format mp3 -o \"{outputTemplate}\" \"{linkki}\"";

            LogManager.LogToFile($"Starting= '{ytdlpPath}'");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = ytdlpPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };


            Process proc = new Process();
            proc.StartInfo = psi;
            proc.EnableRaisingEvents = true;

            proc.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    //gets the output of yt-dlp.exe to LogTextBox
                    Dispatcher.Invoke(() =>
                    {
                        LogTextBox.Text += e.Data + Environment.NewLine;
                        LogTextBox.ScrollToEnd();
                    });
                    LogManager.LogToFile(e.Data, "INFO");

                    var match = System.Text.RegularExpressions.Regex.Match(e.Data, @"(\d{1,3}\.\d)%");
                    if (match.Success)
                    {
                        double progress = double.Parse(match.Groups[1].Value);
                        Dispatcher.Invoke(() => ProgressBar.Value = progress);
                    }
                }
            };

            proc.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Dispatcher.Invoke(() =>
                    {
                        LogTextBox.Text += "ERROR: " + e.Data + Environment.NewLine;
                        LogTextBox.ScrollToEnd();
                    });
                    LogManager.LogToFile(e.Data, "ERROR");
                }
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            await Task.Run(() => proc.WaitForExit()); // ensures process fully exits
            {
                Dispatcher.Invoke(() =>
                {
                    LogTextBox.Text += "Download finished!" + Environment.NewLine;
                    isdone = true;

                });
            };

            if (isdone == true)
            {
                await Task.WhenAll(
                    FadeIn(thxBUt, 0, 300),
                    FadeIn(openfolderBUT, 0,300)
                );

                LoadingIcon.BeginAnimation(UIElement.OpacityProperty, fadeOut);
                LogTextBox.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            }
        }


        private async void THXbut_Click(object sender, EventArgs e)
        {
            LogManager.LogToFile("user stopped the app");
            Application.Current.Shutdown();
        }

        private async void Folderopen_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentDownloadFolder) && Directory.Exists(currentDownloadFolder))
            {
                Process.Start("explorer.exe", currentDownloadFolder);
            }
            else
            {
                LogManager.LogToFile("Folder not found or download not started yet.");
            }
        }


        private void YTLinkTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (YTLinkTextBox.Text == "Public Youtube of Youtube Music link 'https://youtu.be/xvF'")
            {
                YTLinkTextBox.Text = "";
            }
        }

        string DefaultLinkText = "Public Youtube of Youtube Music link 'https://youtu.be/xvF'";
        private void YTLinkTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(YTLinkTextBox.Text) && YTLinkTextBox.Text != DefaultLinkText)
            {
                hasEnteredValidLink0 = true;
                linkki = YTLinkTextBox.Text; // <-- save the actual link here

            }
            else
            {
                hasEnteredValidLink0 = false;
                YTLinkTextBox.Text = DefaultLinkText;
                linkki = "NULL"; // reset if invalid

            }
        }

        private async Task FadeIn(UIElement element, double durationMs = 300, double delayMs = 0)
        {
            // 100 = Opacity, 300 = fade in duration

            if (delayMs > 0)
                await Task.Delay((int)delayMs);

            element.Visibility = Visibility.Visible;

            var animation = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            element.BeginAnimation(UIElement.OpacityProperty, animation);
            await Task.Delay((int)durationMs); // ensure fade completes before moving on
        }
    }
}

using APPLogManager;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace Simple_YTDLP.Windows.UI
{
    /// <summary>
    /// Interaction logic for Mainmenu.xaml
    /// </summary>
    public partial class Mainmenu : UserControl
    {
        bool Stop = false;
        public Mainmenu()
        {
            InitializeComponent();

            InstallingText.Visibility = Visibility.Collapsed;
            InstallingText.Opacity = 0;

            ProgressBar.Visibility = Visibility.Collapsed;
            ProgressBar.Opacity = 0;

            infoText.Visibility = Visibility.Collapsed;
            infoText.Opacity = 0;

            YTLinkTextBox.Visibility = Visibility.Collapsed;
            YTLinkTextBox.Opacity = 0;

            okBUt.Visibility = Visibility.Collapsed;
            okBUt.Opacity = 0;

            updatePlaylistBUt.Visibility = Visibility.Collapsed;
            updatePlaylistBUt.Opacity = 0;

            VraagText.Visibility = Visibility.Collapsed;
            VraagText.Opacity = 0;

            MP3BUt.Visibility = Visibility.Collapsed;   
            MP3BUt.Opacity = 0;

            MP4BUt.Visibility = Visibility.Collapsed;
            MP4BUt.Opacity = 0;

            downloadingTEXR.Visibility = Visibility.Collapsed;
            downloadingTEXR.Opacity = 0;

            LogTextBox.Visibility = Visibility.Collapsed;
            LogTextBox.Opacity = 0;

            stopbutt.Visibility = Visibility.Collapsed;
            stopbutt.Opacity = 0;

            thxBUt.Visibility = Visibility.Collapsed;
            thxBUt.Opacity = 0;

            LoadingIcon.Visibility = Visibility.Collapsed;
            LoadingIcon.Opacity = 0;

            openfolderBUT.Visibility = Visibility.Collapsed;
            openfolderBUT.Opacity = 0;


            this.Loaded += Mainmenu_Loaded; 
        }

        private async void Mainmenu_Loaded(object sender, RoutedEventArgs e)
        {
            await Installtools();
        }


        private async Task Reload()
        {
            LogManager.LogToFile("Reloading Mainmenu...");

            Background.Visibility = Visibility.Collapsed;
            Background.Source = new BitmapImage(new Uri("pack://application:,,,/Core/APP/sys/Mainmenu.jpg"));
            Background.Opacity = 0;
            InstallingText.Visibility = Visibility.Collapsed;
            ProgressBar.Visibility = Visibility.Collapsed;
            infoText.Visibility = Visibility.Collapsed;
            YTLinkTextBox.Visibility = Visibility.Collapsed;
            downloadingTEXR.Visibility = Visibility.Collapsed;
            okBUt.Visibility = Visibility.Collapsed;
            thxBUt.Visibility = Visibility.Collapsed;
            stopbutt.Visibility = Visibility.Collapsed;
            openfolderBUT.Visibility = Visibility.Collapsed;
            LogTextBox.Visibility = Visibility.Collapsed;
            LoadingIcon.Visibility = Visibility.Collapsed;
            VraagText.Visibility = Visibility.Collapsed;
            MP3BUt.Visibility = Visibility.Collapsed;
            MP4BUt.Visibility = Visibility.Collapsed;
            updatePlaylistBUt.Visibility = Visibility.Collapsed;

            stopbutt.IsEnabled = false;
            okBUt.IsEnabled = true;
            Stop = false;
            hasEnteredValidLink0 = false;
            updatePlaylistBUt.IsEnabled = true;
            linkki = "NULL";
            YTLinkTextBox.Text = DefaultLinkText;
            downloadingTEXR.Text = "Playlist Downloaden..";

            await Task.Delay(1000); // give UI time to refresh

            await Installtools();
        }

        private async Task Installtools()
        {

            Background.Visibility = Visibility.Visible;

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
                FadeIn(Background, 200, 300),
                FadeIn(InstallingText, 200, 300),
                FadeIn(ProgressBar, 200, 300)
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
                    //System.Diagnostics.Process.Start("updater.exe", $"-reinstall=\"{exeName}\""); //path is not okey
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

        bool isupdatingplaylistcool = false;
        string savedFolder = "";
        string savedUrl = "";
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
            string fullConfigPath = Path.Combine(configPath, configname);

            savedFolder = "";
            savedUrl = "";
            isupdatingplaylistcool = false;

            if (File.Exists(fullConfigPath))
            {
                try
                {
                    var lines = File.ReadAllLines(fullConfigPath);
                    if (lines.Length >= 2)
                    {
                        string folderCandidate = lines[0];
                        string urlCandidate = lines[1];

                        if (Directory.Exists(folderCandidate))
                        {
                            savedFolder = folderCandidate;
                        }
                        else
                        {
                            LogManager.LogToFile($"Saved folder does not exist: {folderCandidate}");
                        }

                        if (!string.IsNullOrWhiteSpace(urlCandidate) && urlCandidate != DefaultLinkText)
                        {
                            savedUrl = urlCandidate;
                        }
                        else
                        {
                            LogManager.LogToFile($"Saved URL is invalid: {urlCandidate}");
                        }

                        if (!string.IsNullOrEmpty(savedFolder) && !string.IsNullOrEmpty(savedUrl))
                        {
                            isupdatingplaylistcool = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogToFile($"Failed to read config: {ex.Message}");
                }
            }
            else
            {
                LogManager.LogToFile("Config file not found.");
            }

            await Task.WhenAll(
                FadeIn(infoText, 200, 300),
                FadeIn(YTLinkTextBox, 200, 300),
                FadeIn(okBUt, 200, 300)
            );

            if (isupdatingplaylistcool)
            {
                await FadeIn(updatePlaylistBUt);
                LogManager.LogToFile("Update button enabled (valid playlist found).");
            }
            else
            {
                LogManager.LogToFile("No valid saved playlist, update button will not be shown.");
                updatePlaylistBUt.Visibility = Visibility.Collapsed;
                updatePlaylistBUt.IsEnabled = false;
            }
        }

        string configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "NeoCircuit-Studios", "Simple-YTDLP");
        string configname = "lastfolder.txt";


        bool hasEnteredValidLink0 = false;
        string linkki = "NULL";
        string baseFolderName = "NS-SYTDLR_Playlist0";
        private string currentDownloadFolder = ""; 

        bool isDone = false;


        private async void Okbut_Click(object sender, RoutedEventArgs e)
        {
            if (!hasEnteredValidLink0)
            {
                YTLinkTextBox.Text = DefaultLinkText;
                LogManager.LogToFile("Invalid link entered.");
                return;
            }

            YTLinkTextBox.Text = linkki;

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

            {
                //was a bug : did not hide the input UI when clicking OK
                okBUt.Visibility = Visibility.Collapsed;
                okBUt.IsEnabled = false;
            }
            infoText.Visibility = Visibility.Collapsed;
            YTLinkTextBox.Visibility = Visibility.Collapsed;
            okBUt.Visibility = Visibility.Collapsed;
            updatePlaylistBUt.Visibility = Visibility.Collapsed;

            await Task.WhenAll(
                FadeIn(VraagText, 100, 300),
                FadeIn(MP3BUt, 100, 300),
                FadeIn(MP4BUt, 100, 300)
            );
            MP4BUt.IsEnabled = false; // Disable MP4 button for now                                       !!!!!       
        }

        private async void MP3but_Click(object sender, EventArgs e)
        {
            var fadeOut = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(500),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            VraagText.Visibility = Visibility.Collapsed;
            MP3BUt.Visibility = Visibility.Collapsed;
            MP4BUt.Visibility = Visibility.Collapsed;
            updatePlaylistBUt.Visibility = Visibility.Collapsed;


            ////////////////////////////////////////////////////
            
            Background.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            Background.Source = new BitmapImage(new Uri("pack://application:,,,/Core/APP/sys/Downloading.jpg"));
            await Task.Delay(800);

            await Task.WhenAll(
                FadeIn(Background, 200, 300),
                FadeIn(downloadingTEXR, 200, 300),
                FadeIn(LogTextBox, 200, 300),
                FadeIn(LoadingIcon, 200, 300)
            );

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderName = baseFolderName;
            string fullPath = Path.Combine(desktopPath, folderName);
            int counter = 1;
            while (Directory.Exists(fullPath))
            {
                folderName = $"{baseFolderName}{counter}";
                fullPath = Path.Combine(desktopPath, folderName);
                counter++;
            }
            Directory.CreateDirectory(fullPath);
            currentDownloadFolder = fullPath;
            LogManager.LogToFile("Folder created: " + fullPath);

            Directory.CreateDirectory(configPath);
            string fullConfigPath = Path.Combine(configPath, configname);

            // Save both folder and URL
            File.WriteAllText(fullConfigPath, $"{currentDownloadFolder}{Environment.NewLine}{linkki}");
            LogManager.LogToFile("Config file updated: " + fullConfigPath);

            string toolsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                            "NeoCircuit-Studios", "Simple-YTDLP", "tools");
            string ytdlpPath = Path.Combine(toolsPath, "yt-dlp.exe");
            string outputTemplate = Path.Combine(fullPath, "%(title)s.%(ext)s");
            string arguments = $"--no-check-formats -f bestaudio --extract-audio --audio-format mp3 -o \"{outputTemplate}\" \"{linkki}\"";

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
            Process proc = new Process { StartInfo = psi, EnableRaisingEvents = true };

            var logQueue = new ConcurrentQueue<string>();
            DispatcherTimer logTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            logTimer.Tick += (s, evt) =>
            {
                while (logQueue.TryDequeue(out var line))
                {
                    LogTextBox.AppendText(line + Environment.NewLine);
                    LogTextBox.ScrollToEnd();
                }
            };
            logTimer.Start();

            proc.OutputDataReceived += (s, evt) =>
            {
                if (!string.IsNullOrEmpty(evt.Data))
                {
                    logQueue.Enqueue(evt.Data);
                    LogManager.LogToFile(evt.Data, "INFO");

                    var match = System.Text.RegularExpressions.Regex.Match(evt.Data, @"(\d{1,3}\.\d)%");
                    if (match.Success && double.TryParse(match.Groups[1].Value, out double progress))
                    {
                        Dispatcher.BeginInvoke(() => ProgressBar.Value = progress);
                    }
                }
            };

            proc.ErrorDataReceived += (s, evt) =>
            {
                if (!string.IsNullOrEmpty(evt.Data))
                {
                    logQueue.Enqueue("ERROR: " + evt.Data);
                    LogManager.LogToFile(evt.Data, "ERROR");
                }
            };

            proc.Exited += (s, evt) =>
            {
                isDone = true;
                Dispatcher.Invoke(() =>
                {
                    LogTextBox.AppendText("Download finished!" + Environment.NewLine);
                    LogTextBox.ScrollToEnd();
                    LogManager.LogToFile("Download finished.", "INFO");

                    //ProgressBar.Value = 100;
                });
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            await Task.Run(() => proc.WaitForExit());

            logTimer.Stop();

            Dispatcher.BeginInvoke(() =>
            {
                LogTextBox.AppendText("Download finished!" + Environment.NewLine);
                isDone = true;
            });

            if (isDone && Stop == false)
            {
                var fadeOutUI = new System.Windows.Media.Animation.DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromMilliseconds(500),
                    FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
                };
                LoadingIcon.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
                LogTextBox.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
                stopbutt.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);

                await Task.WhenAll(
                    FadeIn(thxBUt, 0, 300),
                    FadeIn(openfolderBUT, 0, 300)
                );

            }
        }
        private async void MP4but_Click(object sender, EventArgs e)
        {
        }

        private async void updateplaylistbut_Click(object sender, EventArgs e)
        {
            var fadeOut = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(500),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            VraagText.Visibility = Visibility.Collapsed;
            MP3BUt.Visibility = Visibility.Collapsed;
            MP4BUt.Visibility = Visibility.Collapsed;
            updatePlaylistBUt.Visibility = Visibility.Collapsed;
            infoText.Visibility = Visibility.Collapsed;
            YTLinkTextBox.Visibility = Visibility.Collapsed;
            okBUt.Visibility = Visibility.Collapsed;
            updatePlaylistBUt.Visibility = Visibility.Collapsed;

            okBUt.IsEnabled = false;
            updatePlaylistBUt.IsEnabled = false;



            if (!string.IsNullOrEmpty(savedFolder) && !string.IsNullOrEmpty(savedUrl))
            {
                ////////////////////////////////////////////////////
                Background.BeginAnimation(UIElement.OpacityProperty, fadeOut);
                Background.Source = new BitmapImage(new Uri("pack://application:,,,/Core/APP/sys/Downloading.jpg"));
                await Task.Delay(800);

                downloadingTEXR.Text = "Playlist Updaten..";

                await Task.WhenAll(
                    FadeIn(Background, 200, 300),
                    FadeIn(downloadingTEXR, 200, 300),
                    FadeIn(LogTextBox, 200, 300),
                    FadeIn(LoadingIcon, 200, 300)
                );
  
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fullPath = Path.Combine(desktopPath, savedFolder);
                Directory.CreateDirectory(fullPath);

                // Use savedFolder and savedUrl instead of creating new ones
                currentDownloadFolder = savedFolder;
                LogManager.LogToFile("Using folder from config: " + currentDownloadFolder);
                LogManager.LogToFile("Using URL from config: " + savedUrl);

                string toolsPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "NeoCircuit-Studios", "Simple-YTDLP", "tools"
                );
                string ytdlpPath = Path.Combine(toolsPath, "yt-dlp.exe");
                string outputTemplate = Path.Combine(currentDownloadFolder, "%(title)s.%(ext)s");
                string arguments = $"--no-check-formats -f bestaudio --extract-audio --audio-format mp3 -o \"{outputTemplate}\" \"{savedUrl}\"";

                LogManager.LogToFile($"Starting= '{ytdlpPath}' with arguments: {arguments}");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = ytdlpPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                Process proc = new Process { StartInfo = psi, EnableRaisingEvents = true };

                var logQueue = new ConcurrentQueue<string>();
                DispatcherTimer logTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
                logTimer.Tick += (s, evt) =>
                {
                    while (logQueue.TryDequeue(out var line))
                    {
                        LogTextBox.AppendText(line + Environment.NewLine);
                        LogTextBox.ScrollToEnd();
                    }
                };
                logTimer.Start();

                proc.OutputDataReceived += (s, evt) =>
                {
                    if (!string.IsNullOrEmpty(evt.Data))
                    {
                        logQueue.Enqueue(evt.Data);
                        LogManager.LogToFile(evt.Data, "INFO");

                        var match = System.Text.RegularExpressions.Regex.Match(evt.Data, @"(\d{1,3}\.\d)%");
                        if (match.Success && double.TryParse(match.Groups[1].Value, out double progress))
                        {
                            Dispatcher.BeginInvoke(() => ProgressBar.Value = progress);
                        }
                    }
                };

                proc.ErrorDataReceived += (s, evt) =>
                {
                    if (!string.IsNullOrEmpty(evt.Data))
                    {
                        logQueue.Enqueue("ERROR: " + evt.Data);
                        LogManager.LogToFile(evt.Data, "ERROR");
                    }
                };

                proc.Exited += (s, evt) =>
                {
                    isDone = true;
                    Dispatcher.Invoke(() =>
                    {
                        LogTextBox.AppendText("Download finished!" + Environment.NewLine);
                        LogTextBox.ScrollToEnd();
                        LogManager.LogToFile("Download finished.", "INFO");
                    });
                };

                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                await Task.Run(() => proc.WaitForExit());

                logTimer.Stop();

                Dispatcher.BeginInvoke(() =>
                {
                    LogTextBox.AppendText("Download finished!" + Environment.NewLine);
                    isDone = true;
                });

                if (isDone && Stop == false)
                {
                    var fadeOutUI = new System.Windows.Media.Animation.DoubleAnimation
                    {
                        From = 1.0,
                        To = 0.0,
                        Duration = TimeSpan.FromMilliseconds(500),
                        FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
                    };
                    LoadingIcon.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
                    LogTextBox.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);

                    await Task.WhenAll(
                        FadeIn(thxBUt, 100, 300),
                        FadeIn(openfolderBUT, 100, 300)
                    );
                }
            }
            else
            {
                LogManager.LogToFile("Saved folder or URL is not valid. Cannot start download.");
            }
        }

        private async void THXbut_Click(object sender, EventArgs e)
        {
            //LogManager.LogToFile("user stopped the app");
            //Application.Current.Shutdown();

            var fadeOutUI = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(500),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            thxBUt.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
            openfolderBUT.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
            LogTextBox.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
            downloadingTEXR.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
            Background.Visibility = Visibility.Collapsed;

            await Reload();
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

        private async void stopbut_Click(object sender, EventArgs e)
        {
            Stop = true;
            stopbutt.IsEnabled = false;
            LogManager.LogToFile("User requested to stop the download.");

            // --- Kill yt-dlp.exe if still running ---
            try
            {
                foreach (var proc in Process.GetProcessesByName("yt-dlp"))
                {
                    proc.Kill();   // force kill
                    proc.WaitForExit();
                    LogManager.LogToFile($"Killed yt-dlp.exe (PID {proc.Id})", "INFO");
                }
            }
            catch (Exception ex)
            {
                LogManager.LogToFile($"Failed to kill yt-dlp.exe: {ex.Message}", "ERROR");
            }

            var fadeOutUI = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(500),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            thxBUt.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
            openfolderBUT.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
            LogTextBox.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
            downloadingTEXR.BeginAnimation(UIElement.OpacityProperty, fadeOutUI);
            Background.Visibility = Visibility.Collapsed;
            openfolderBUT.Visibility = Visibility.Collapsed;
            thxBUt.Visibility = Visibility.Collapsed;

            await Reload();
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
                linkki = YTLinkTextBox.Text;

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

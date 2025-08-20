using APPLogManager;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Simple_YTDLP.Windows.UI
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        string currentVersion = VersionInfo.Version;

        public Splash()
        {
            InitializeComponent();

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait; // set cursor to wait

            this.Title = "Simple_YTDLP - NeoCircuit-Studios - BETA";
            {
                Background.Opacity = 1.0;
                Background.Source = new BitmapImage(new Uri("pack://application:,,,/Core/APP/sys/Splash.jpg"));
            }

            this.Loaded += async (s, e) => await StartSplash(); // safer and faster

        }

        private async Task Loading()
        {
            LogManager.LogToFile("----------Start--------------", "INFO");
            LogManager.LogToFile("@NeoCircuit-Studios@", "INFO");
            LogManager.LogToFile($"Simple_YTDLP {currentVersion}", "INFO");
            LogManager.LogToFile("Copyright (C) 2025 NeoCircuit Studios", "INFO");

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP");
            string tmpDir = Path.Combine(folderPath, "TMP");
            string updaterDir = Path.Combine(folderPath, "updater");

            Directory.CreateDirectory(folderPath);
            Directory.CreateDirectory(tmpDir);
            Directory.CreateDirectory(updaterDir);

            string filePath = Path.Combine(folderPath, "firstboot.guustFlag");
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                LogManager.LogToFile("firstboot..", "INFO");
            }
            else
            {
                LogManager.LogToFile("Not firstboot..", "INFO");
                // Local version file (in working dir)
                if (!File.Exists("version.guustGV"))
                    File.WriteAllText("version.guustGV", currentVersion);
            }

            loadingText.Text = "Zoeken Naar Update...";

            // Paths
            string installedVersionPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "version.guustGV");
            string updateVersionPath = Path.Combine(tmpDir, "version.guustGV");

            string installedVersionPathUpdater = Path.Combine(updaterDir, "version.guustGV");
            string updateVersionPathUpdater = Path.Combine(updaterDir, "TMP", "version.guustGV");

            string exeName = "SYTDLP-Updater.exe";
            string updaterPath = Path.Combine(updaterDir, exeName);

            // URLs
            string urlMainVersion = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/version.guustGV";
            string urlUpdaterVersion = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/version.guustGV";
            string urlUpdaterExe = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP-updater/update/SYTDLP-Updater.exe";

            // ---- Helper ----
            async Task DownloadAsync(string url, string path)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    using (HttpClient client = new HttpClient())
                    {
                        var data = await client.GetByteArrayAsync(url);
                        await File.WriteAllBytesAsync(path, data);
                    }
                    LogManager.LogToFile($"Downloaded [{url}] to [{path}]", "INFO");
                }
                catch (Exception ex)
                {
                    LogManager.LogToFile($"Download failed: {url} - {ex.Message}", "ERROR");
                    throw;
                }
            }

            // ---- Download new main version ----
            await DownloadAsync(urlMainVersion, updateVersionPath);

            Version installedVer = File.Exists(installedVersionPath) ? new Version(File.ReadAllText(installedVersionPath).Trim()) : new Version("0.0.0.0");
            Version updateVer = File.Exists(updateVersionPath) ? new Version(File.ReadAllText(updateVersionPath).Trim()) : new Version("0.0.0.0");

            if (VersionUtils.IsNewerVersion(installedVer.ToString(), updateVer.ToString()))
            {
                LogManager.LogToFile("Update available!", "INFO");
                LogManager.LogToFile($"Local: '{installedVer}' vs Server:'{updateVer}' ", "INFO");

                await FadeIn(newversion, 300); 

                newversion.Text = $"Nieuw:{updateVer}";
                loadingText.Text = "Update Gevonden!";

                if (File.Exists(updaterPath))
                {
                    LogManager.LogToFile("Updater found, but downloading anyway..", "INFO");
                    loadingText.Text = "Downloaden...";

                    await DownloadAsync(urlUpdaterExe, Path.Combine(updaterDir, exeName));
    
                    // ---- Check updater version ----
                    Version installedUpdaterVer = File.Exists(installedVersionPathUpdater) ? new Version(File.ReadAllText(installedVersionPathUpdater).Trim()) : new Version("0.0.0.0");
                    Version updateUpdaterVer = File.Exists(updateVersionPathUpdater) ? new Version(File.ReadAllText(updateVersionPathUpdater).Trim()) : new Version("0.0.0.0");

                    if (installedUpdaterVer < updateUpdaterVer)
                    {
                        LogManager.LogToFile("Update available for updater!", "INFO");

                        await DownloadAsync(urlUpdaterExe, Path.Combine(updaterDir, exeName));
                        await DownloadAsync(urlUpdaterVersion, Path.Combine(updaterDir, "version.guustGV"));

                        LogManager.LogToFile("Updater updated, starting updater.", "INFO");
                    }
                    else
                    {
                        LogManager.LogToFile("Updater is up to date.", "INFO");
                    }
                }
                else
                {
                    LogManager.LogToFile("Updater not found, downloading...", "INFO");
                    await DownloadAsync(urlUpdaterExe, Path.Combine(updaterDir, exeName));
                }

                // ---- Start updater ----

                loadingText.Text = "Wachten...";

                Directory.Delete(Path.Combine(updaterDir, "Logs")); // delete old version file
                LogManager.LogToFile("Deleting old updater logs..", "INFO");

                await Task.Delay(500); // wait a bit before starting updater

                LogManager.LogToFile("Starting.. " + exeName);
                Process.Start(new ProcessStartInfo
                {
                    FileName = updaterPath,
                    Arguments = "-updateme",
                    UseShellExecute = true
                });
                LogManager.LogToFile("Update started, exiting application.", "INFO");
                Application.Current.Shutdown();
            }
            else
            {
                loadingText.Text = "Starten..";
            }
        }


        private async Task StartSplash()
        {
            version.Text = VersionInfo.Version;

            await Task.Delay(450); // wait

            await Task.WhenAll(
                FadeIn(NS, 300),
                FadeIn(version, 300)
            );

            await FadeIn(APPTitle, 300, 700);

            await Task.WhenAll(
                FadeIn(loadingText, 300, 500),
                FadeIn(LoadingIcon, 300, 500)
            );

            await Loading();
            await Task.Delay(2000); // wait before fade out //loading time


            version.Visibility = Visibility.Collapsed;
            APPTitle.Visibility = Visibility.Collapsed;
            NS.Visibility = Visibility.Collapsed;
            loadingText.Visibility = Visibility.Collapsed;
            LoadingIcon.Visibility = Visibility.Collapsed;


            // Smooth fade out background
            var fadeOut = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromMilliseconds(500),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            Background.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            await Task.Delay(500);

            Background.Opacity = 0.0;

            await Task.Delay(400); //load lol

            Mouse.OverrideCursor = null; // reset cursor to default // Very important!!!!

            Mainmenu mainmenu = new Mainmenu(); // filename, Disclaimer is the variablename, Disclaimer is the class name

            this.Content = mainmenu; // "mainmenu" in the variablename

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

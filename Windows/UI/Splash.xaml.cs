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
                Background.Source = new BitmapImage(new Uri("pack://application:,,,/Core/APP/sys/Splash.png"));
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
            string filePath = Path.Combine(folderPath, "firstboot.guustFlag");
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (File.Exists(filePath) == false) // this crap does not help much rn
            {
                File.Create(filePath).Close(); // Create empty file and close the stream
                LogManager.LogToFile("firstboot..", "INFO");
            }
            else
            {
                LogManager.LogToFile("Not firstboot..", "INFO");
                //getlang(); // this is handy to know if it is the first boot or not.. to set like a language or something..
            }

            version.Text = currentVersion; 

            loadingText.Text = "Checking for Updates....";

            string installedVersionPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "version.guustGV");
            string updateVersionPath = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "TMP", "version.guustGV");

            string installedText = File.Exists(installedVersionPath) ? File.ReadAllText(installedVersionPath).Trim() : "0.0.0.0";
            string updateText = File.Exists(updateVersionPath) ? File.ReadAllText(updateVersionPath).Trim() : "0.0.0.0";

            string installedTextupdater = File.Exists(installedVersionPath) ? File.ReadAllText(installedVersionPath).Trim() : "0.0.0.0";
            string updateTextupdater = File.Exists(updateVersionPath) ? File.ReadAllText(updateVersionPath).Trim() : "0.0.0.0";

            string installedVersionPathUpdater = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "updater", "version.guustGV");
            string updateVersionPathUpdater = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "updater", "TMP", "version.guustGV");

            string url1 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/update/main/version.guustGV";
            string urlupdater = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/update/updater/version.guustGV";
            string exeName = "SYTDLP-Updater.exe";

            string updaterPath = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "updater", exeName + ".exe");

            if (File.Exists("version.guustGV"))
            {
                //File.WriteAllText("version.guustGV", currentVersion);
            }
            else
            {
                File.WriteAllText("version.guustGV", currentVersion);
            }

            using (HttpClient client = new HttpClient())
            {
                async Task DownloadAsync(string url, string path)
                {
                    var data = await client.GetByteArrayAsync(url);
                    await File.WriteAllBytesAsync(path, data);
                    LogManager.LogToFile($"Downloaded [{url}] to [{path}]", "INFO");
                }

                await DownloadAsync(url1, Path.Combine(updateVersionPath, "version.guustGV"));
            }


            if (new Version(File.ReadAllText(installedVersionPath).Trim())
                < new Version(File.ReadAllText(updateVersionPath).Trim()))
            {
                LogManager.LogToFile("Update available!", "INFO");
                LogManager.LogToFile($"Local: '{installedVersionPath}' vs Server:'{updateVersionPath}' ", "INFO");

                if (File.Exists(Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "updater", exeName + ".exe")))
                {
                    if (new Version(File.ReadAllText(installedTextupdater).Trim())
                        < new Version(File.ReadAllText(updateTextupdater).Trim()))
                    {
                        LogManager.LogToFile("Update available for updater!", "INFO");
                        LogManager.LogToFile($"Local: '{installedVersionPathUpdater}' vs Server: '{updateVersionPathUpdater}' ", "INFO");

                        string savedirupdater = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "updater");
                        string urlupdateupdater = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/update/updater/version.guustGV";
                        string urlversionupdater = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/updater/SYTDLP-Updater.exe";

                        LogManager.LogToFile("Downloading update for updater..", "INFO");

                        using (HttpClient client = new HttpClient())
                        {
                            async Task DownloadAsync(string url, string path)
                            {
                                var data = await client.GetByteArrayAsync(url);
                                await File.WriteAllBytesAsync(path, data);
                                LogManager.LogToFile($"Downloaded [{urlupdateupdater}] to [{savedirupdater}]", "INFO");
                            }

                            await DownloadAsync(urlupdateupdater, Path.Combine(savedirupdater, "SYTDLP-Updater.exe"));
                        }

                        LogManager.LogToFile("Update for updater downloaded, now downloading version file..", "INFO");

                        using (HttpClient client = new HttpClient())
                        {
                            async Task DownloadAsync(string url, string path)
                            {
                                var data = await client.GetByteArrayAsync(url);
                                await File.WriteAllBytesAsync(path, data);
                                LogManager.LogToFile($"Downloaded [{url}] to [{path}]", "INFO");
                            }

                            await DownloadAsync(url1, Path.Combine(urlversionupdater, "version.guustGV"));
                        }

                        LogManager.LogToFile("Update for updater downloaded, starting updater.", "INFO");

                        LogManager.LogToFile("Starting.. " + exeName);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = updaterPath,
                            Arguments = "-updateme",
                            UseShellExecute = false
                        });
                        LogManager.LogToFile("Update started, exiting application.", "INFO");
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        LogManager.LogToFile("Updater is up to date, no need to update.", "INFO");

                        LogManager.LogToFile("Starting.. " + exeName);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = updaterPath,
                            Arguments = "-updateme",
                            UseShellExecute = false
                        });
                        LogManager.LogToFile("Update started, exiting application.", "INFO");
                        Application.Current.Shutdown();
                    }
                }
                else
                {
                    LogManager.LogToFile("Updater not found, downloading updater..", "INFO");
                    string savedir = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "updater");
                    string urlinstallupdater = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/updater/SYTDLP-Updater.exe";

                    using (HttpClient client = new HttpClient())
                    {
                        async Task DownloadAsync(string url, string path)
                        {
                            var data = await client.GetByteArrayAsync(url);
                            await File.WriteAllBytesAsync(path, data);
                            LogManager.LogToFile($"Downloaded [{urlinstallupdater}] to [{savedir}]", "INFO");
                        }

                        await DownloadAsync(urlinstallupdater, Path.Combine(savedir, "SYTDLP-Updater.exe"));
                    }

                    LogManager.LogToFile("Updater downloaded, starting the updater.", "INFO");
                    LogManager.LogToFile("Starting.. " + exeName);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = updaterPath,
                        Arguments = "-updateme",
                        UseShellExecute = false
                    });
                    LogManager.LogToFile("Update started, exiting application.", "INFO");
                    Application.Current.Shutdown();
                }
            }
        }

        private async Task StartSplash()
        {
            await Task.Delay(500); // wait

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
            await Task.Delay(4500); // wait before fade out //loading time


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

            await Task.Delay(800); //load lol

            Mouse.OverrideCursor = null; // reset cursor to default // Very important!!!!

            /*
                var basePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "NeoCircuit-Studios", "NeoAppFlixToolKit", "Saved"
                );

                var userFilePath = Path.Combine(basePath, "user.guustGSF");

                if (File.Exists(userFilePath))
                {
                    // skip the main menu if user file exists
                } 
                else
                {
                    // Show the main menu to set username
                }

            */


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

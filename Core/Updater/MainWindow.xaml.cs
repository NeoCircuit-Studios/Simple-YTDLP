using APPLogManager;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Security.Policy;
using System.Windows;
using System.Xml.Linq;
using static Updater.App;

namespace Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += async (s, e) =>
            {
                if (AppState.Fromapp)
                {
                    UpdateBUT.Visibility = Visibility.Collapsed;
                    statusTEXT.Visibility = Visibility.Visible;
                    progress.Visibility = Visibility.Visible;
                    Background.Visibility = Visibility.Visible;
                    Background.Opacity = 1;

                    statusTEXT.Text = "Laden..";
                    LogManager.LogToFile("Loading...");

                    await Install();
                    return;
                }
            };
        }


        //%appdata%\NeoCircuit-Studios\Simple-YTDLP\Updater\Updater.exe
        private async void updateBUT_Click(object sender, RoutedEventArgs e)
        {
            UpdateBUT.Visibility = Visibility.Collapsed;
            statusTEXT.Visibility = Visibility.Visible;
            progress.Visibility = Visibility.Visible;
            Background.Visibility = Visibility.Visible;
            Background.Opacity= 1;

            statusTEXT.Text = "Laden..";

            await Install();
            return;
        }

        private async Task Install()
        {
            LogManager.LogToFile("---------- Install Started --------------", "INFO");

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP");
            string tmpDir = Path.Combine(folderPath, "TMP");

            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string installedVersionPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "version.guustGV");

            string updateVersionPath = Path.Combine(tmpDir, "version.guustGV");
            string versionUrl = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/version.guustGV";

            string url1 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/update0.pack.guustPKG";
            string url2 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/Core/ThirdParty/bin.1.tmp.guustPKG";
            string url3 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/Core/ThirdParty/bin.2.tmp.guustPKG";
            string url4 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/Core/ThirdParty/bin.3.tmp.guustPKG";
            string url5 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/Core/ThirdParty/bin.6.tmp.guustPKG";

            string savedir1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");
            string savedir2 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Core", "ThirdParty");

            string sourceFile1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "install0.pack.guustPKG");
            string extractPath1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");
            string checkPath1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");

            Directory.CreateDirectory(tmpDir);


            // --- Helper for safe version parsing ---
            Version SafeParseVersion(string text)
            {
                try
                {
                    return new Version(text.Trim());
                }
                catch
                {
                    return new Version("0.0.0.0");
                }
            }

            // --- Download latest version file ---
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var data = await client.GetByteArrayAsync(versionUrl);
                    await File.WriteAllBytesAsync(updateVersionPath, data);
                    LogManager.LogToFile($"Downloaded latest version file from {versionUrl} to {updateVersionPath}", "INFO");
                }
            }
            catch (Exception ex)
            {
                LogManager.LogToFile($"Failed to download version file: {ex.Message}", "ERROR");
                return; // stop update
            }

            progress.Value = 10;

            // --- Read versions ---
            string installedText = File.Exists(installedVersionPath) ? File.ReadAllText(installedVersionPath) : "0.0.0.0";
            string updateText = File.Exists(updateVersionPath) ? File.ReadAllText(updateVersionPath) : "0.0.0.0";

            string ShortcutPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe.lnk");
            string shortcutName = "Simple-YTDLP.lnk";
            string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), shortcutName);
            string startMenuDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), "NeoCircuit-Studios");
            string startMenuPath = Path.Combine(startMenuDir, shortcutName);

            Directory.CreateDirectory(startMenuDir); // make sure the folder exists

            Version installedVer = SafeParseVersion(installedText);
            Version latestVer = SafeParseVersion(updateText);

            LogManager.LogToFile($"Installed version: {installedVer}", "INFO");
            LogManager.LogToFile($"Latest version: {latestVer}", "INFO");

            // --- Compare ---
            if (installedVer < latestVer)
            {
                LogManager.LogToFile("Update available! Starting update procedure...", "INFO");

                LogManager.LogToFile("Downloading update package...", "INFO");  

                statusTEXT.Text = "Downloaden...";

                using (HttpClient client = new HttpClient())
                {
                    async Task<bool> DownloadAsync(string url, string path)
                    {
                        try
                        {
                            var data = await client.GetByteArrayAsync(url);
                            await File.WriteAllBytesAsync(path, data);
                            LogManager.LogToFile($"Downloaded [{url}] to [{path}]", "INFO");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogToFile($"Failed to download [{url}]: {ex.Message}", "ERROR");
                            MessageBox.Show($"Failed to download: {url}\n\n{ex.Message}",
                                            "Download Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                    Directory.CreateDirectory(savedir2);

                    if (!await DownloadAsync(url1, Path.Combine(savedir1, "install0.pack.guustPKG"))) return;
                    progress.Value = 25;
                    //if (!await DownloadAsync(url2, Path.Combine(savedir2, "bin.1.tmp.guustPKG"))) return;
                    progress.Value = 30;
                    //if (!await DownloadAsync(url3, Path.Combine(savedir2, "bin.2.tmp.guustPKG"))) return;
                    progress.Value = 35;
                    //if (!await DownloadAsync(url4, Path.Combine(savedir2, "bin.3.tmp.guustPKG"))) return;
                    progress.Value = 40;
                    //if (!await DownloadAsync(url5, Path.Combine(savedir2, "bin.6.tmp.guustPKG"))) return;
                    progress.Value = 45;
                }
                statusTEXT.Text = "Installeren..";

                LogManager.LogToFile("Installing update package...", "INFO");

                ZipFile.ExtractToDirectory(sourceFile1, extractPath1, overwriteFiles: true);

                File.Copy(ShortcutPath, desktopPath, overwrite: true);
                File.Copy(ShortcutPath, startMenuPath, overwrite: true);

                LogManager.LogToFile("Copied shortcut to Desktop and Start Menu.", "INFO");
                progress.Value = 60;

                LogManager.LogToFile("Update package extracted successfully.", "INFO");

                if (File.Exists(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe")) && (Directory.Exists(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Core"))))
                {
                    progress.Value = 75;
                }
                else
                {
                    LogManager.LogToFile("Failed to update Simple-YTDLP. Please try again.", "ERROR");
                    MessageBox.Show("Failed to update Simple-YTDLP. Please try again.", "Simple-YTDLP Updater", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                LogManager.LogToFile("updating version file for main app...", "INFO");
                File.Copy(updateVersionPath, Path.Combine(extractPath1, "version.guustGV"), overwrite: true);
                progress.Value = 99;
                await Task.Delay(200);

                File.Delete(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "install0.pack.guustPKG"));
                Directory.Delete(tmpDir, recursive: true);

                LogManager.LogToFile("Deleted temporary directorys..", "DEBUG");

                progress.Value = 100;
                LogManager.LogToFile("Update finished.", "INFO");

                LogManager.LogToFile("---------- Install Finished --------------", "INFO");

                LogManager.LogToFile("Starting main app ");

                Process.Start(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe"));

                statusTEXT.Text = "wachten..";

                await Task.Delay(1000);

                Application.Current.Shutdown();

            }
            else
            {
                LogManager.LogToFile("No update needed. Installed version is up to date.", "INFO");
                statusTEXT.Text = "Geen update nodig.";
                progress.Value = 100;
            }
        }
    }
}
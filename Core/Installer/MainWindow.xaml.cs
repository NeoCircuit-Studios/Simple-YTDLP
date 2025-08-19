using APPLogManager2;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using static Installer.App;

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string FolderPath = Path.Combine(AppDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "installer");
            string FilePath = Path.Combine(FolderPath, "firstboot.guustFlag");

            LogManager.LogToFile("----------Start--------------", "INFO");

            LogManager.LogToFile("@NeoCircuit-Studios@", "INFO");
            LogManager.LogToFile($"Simple_YTDLP - Installer", "INFO");
            LogManager.LogToFile("Copyright (C) 2025 NeoCircuit Studios", "INFO");


            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            if (File.Exists(FilePath) == false) 
            {
                File.Create(FilePath).Close(); // Create empty file and close the stream
                LogManager.LogToFile("firstboot..", "INFO");
            }
            else
            {
                LogManager.LogToFile("Not firstboot..", "INFO");
                //getlang(); // this is handy to know if it is the first boot or not.. to set like a language or something..
            }
        }

        private void InstallBUT_Click(object sender, RoutedEventArgs e)
        {
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            string filePath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");
            string dirPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");

            if (File.Exists(filePath))
            {
                var result = MessageBox.Show(
                    "Simple-YTDLP is al geïnstalleerd... toch installeren? (did zal de app opnieuw installeren)",
                    "Simple-YTDLP Installer",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Information);

                if (result == MessageBoxResult.No)
                {
                    LogManager.LogToFile("User chose not to reinstall Simple-YTDLP.", "INFO");
                    return;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    LogManager.LogToFile("User cancelled the installation.", "INFO");
                    return;
                }

                // If Yes, continue installation...
                LogManager.LogToFile("User chose to reinstall Simple-YTDLP.", "INFO");
                install();
            }
            else
            {
                install();
            }
        }


        private async Task install()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            LogManager.LogToFile("Starten Met Installeren..", "INFO");

            InstallBUT.Visibility = Visibility.Collapsed;

            statusTEXT.Visibility = Visibility.Visible;

            statusTEXT.Text = "Downloaden..";

            progress.Visibility = Visibility.Visible;

            progress.Value = 1;

            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            string filePath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");
            string dirPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                LogManager.LogToFile("Created directory: " + dirPath, "INFO");
            }

            string url1 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/update0.pack.guustPKG";
            string url2 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/Core/ThirdParty/bin.1.tmp.guustPKG";
            string url3 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/Core/ThirdParty/bin.2.tmp.guustPKG";
            string url4 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/Core/ThirdParty/bin.3.tmp.guustPKG";
            string url5 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/Core/ThirdParty/bin.6.tmp.guustPKG";
            string savedir1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");
            string savedir2 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Core", "ThirdParty");

            if (!Directory.Exists(savedir1))
            {
                Directory.CreateDirectory(dirPath);
                LogManager.LogToFile("Created directory: " + savedir1, "INFO");
            }

            if (!Directory.Exists(savedir2))
            {
                Directory.CreateDirectory(savedir2);
                LogManager.LogToFile("Created directory: " + savedir2, "INFO");
            }

            LogManager.LogToFile("Downloading files from GitHub...", "INFO");

            async Task<bool> DownloadAsync(string url, string path, int retries = 5, int timeoutSeconds = 200)
            {
                for (int attempt = 1; attempt <= retries; attempt++)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

                            var data = await client.GetByteArrayAsync(url);
                            await File.WriteAllBytesAsync(path, data);

                            LogManager.LogToFile($"Downloaded [{url}] to [{path}]", "INFO");
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogToFile($"Attempt {attempt}/{retries} failed for [{url}]: {ex.Message}", "WARN");

                        if (attempt == retries)
                        {
                            MessageBox.Show($"Failed to download: {url}\n\n{ex.Message}",
                                            "Download Error, ik zal opnieuw proberen..", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }

                        // small delay before retry
                        await Task.Delay(2000);
                    }
                }

                return false;
            }

            string file1 = Path.Combine(savedir1, "update0.pack.guustPKG");
            string file2 = Path.Combine(savedir2, "bin.1.tmp.guustPKG");
            string file3 = Path.Combine(savedir2, "bin.2.tmp.guustPKG");
            string file4 = Path.Combine(savedir2, "bin.3.tmp.guustPKG");
            string file5 = Path.Combine(savedir2, "bin.6.tmp.guustPKG");

            // Download each file sequentially
            if (!await DownloadAsync(url1, file1)) return;
            if (!await DownloadAsync(url2, file2)) return;
            if (!await DownloadAsync(url3, file3)) return;
            if (!await DownloadAsync(url4, file4)) return;
            if (!await DownloadAsync(url5, file5)) return;

            statusTEXT.Text = "Installeren..";

            progress.Value = 1;

            LogManager.LogToFile("Installing Simple-YTDLP..", "INFO");

            string sourceFile1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "update0.pack.guustPKG");
            string extractPath1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");
            string checkPath1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");
            string checkPath1_1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");

            string ShortcutPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe.lnk");
            string shortcutName = "Simple-YTDLP.lnk";
            string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),shortcutName);
            string startMenuDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), "NeoCircuit-Studios");
            string startMenuPath = Path.Combine(startMenuDir, shortcutName);

            Directory.CreateDirectory(startMenuDir); // make sure the folder exists


            ZipFile.ExtractToDirectory(sourceFile1, extractPath1, overwriteFiles: true);

            LogManager.LogToFile("installed update0.pack.guustPKG to " + extractPath1, "INFO");

            progress.Value = 30;

            File.Copy(ShortcutPath, desktopPath, overwrite: true);
            File.Copy(ShortcutPath, startMenuPath, overwrite: true);

            LogManager.LogToFile("Copied shortcut to Desktop and Start Menu.", "INFO");

            progress.Value = 50;

            if (File.Exists(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe")) && (Directory.Exists(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Core"))))
            {
                progress.Value = 98;
            }
            else
            {
                LogManager.LogToFile("Failed to install Simple-YTDLP. Please try again.", "ERROR");
                MessageBox.Show("Installatie van Simple-YTDLP is mislukt. Probeer het opnieuw.", "Simple-YTDLP Installer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            File.Delete(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "update0.pack.guustPKG"));

            LogManager.LogToFile("Deleted update0.pack.guustPKG from " + extractPath1, "INFO");

            progress.Value = 100;

            progress.Visibility = Visibility.Collapsed;

            statusTEXT.Visibility = Visibility.Collapsed;

            InstallBUT.Visibility = Visibility.Collapsed;

            Mouse.OverrideCursor = null;

            MessageBox.Show("Simple-YTDLP is succesvol geïnstalleerd!", "Simple-YTDLP Installer", MessageBoxButton.OK, MessageBoxImage.Information);

            openBUT.Visibility = Visibility.Visible;
            statusTEXT.Text = "Klaar.";
        }

        private void openBUT_Click(object sender, RoutedEventArgs e)
        {
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string filePath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");
            if (File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                LogManager.LogToFile("Opened Simple-YTDLP.", "INFO");
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("Simple-YTDLP is niet correct geïnstalleerd!", "Simple-YTDLP Installer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
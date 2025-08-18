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
        string currentVersion = VersionInfo.Version;

        public MainWindow()
        {
            InitializeComponent();


            string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string FolderPath = Path.Combine(AppDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "installer");
            string FilePath = Path.Combine(FolderPath, "firstboot.guustFlag");


            LogManager.LogToFile("----------Start--------------", "INFO");

            LogManager.LogToFile("@NeoCircuit-Studios@", "INFO");
            LogManager.LogToFile($"Simple_YTDLP - Installer {currentVersion}", "INFO");
            LogManager.LogToFile("Copyright (C) 2025 NeoCircuit Studios", "INFO");


            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            if (File.Exists(FilePath) == false) // this crap does not help much rn
            {
                File.Create(FilePath).Close(); // Create empty file and close the stream
                LogManager.LogToFile("firstboot..", "INFO");
            }
            else
            {
                LogManager.LogToFile("Not firstboot..", "INFO");
                //getlang(); // this is handy to know if it is the first boot or not.. to set like a language or something..
            }

            if (File.Exists("version.guustGV"))
            {
                //currentVersion = File.ReadAllText("version.guustGV");
                File.WriteAllText("version.guustGV", currentVersion);
            }
            else
            {
                File.WriteAllText("version.guustGV", currentVersion);
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
                    "Simple-YTDLP is already installed... Install anyway? (this will reinstall the app)",
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

            LogManager.LogToFile("Starting installing..", "INFO");

            InstallBUT.Visibility = Visibility.Collapsed;

            statusTEXT.Visibility = Visibility.Visible;

            statusTEXT.Text = "Downloading..";

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

            string url1 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/main/install0.pack.guustPKG";
            string url2 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/main/Core/ThirdParty/bin.1.tmp.guustPKG";
            string url3 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/main/Core/ThirdParty/bin.2.tmp.guustPKG";
            string url4 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/main/Core/ThirdParty/bin.3.tmp.guustPKG";
            string url5 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/main/Core/ThirdParty/bin.6.tmp.guustPKG";
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

                if (!await DownloadAsync(url1, Path.Combine(savedir1, "install0.pack.guustPKG"))) return;
                progress.Value = 25;

                if (!await DownloadAsync(url2, Path.Combine(savedir2, "bin.1.tmp.guustPKG"))) return;
                progress.Value = 50;

                if (!await DownloadAsync(url3, Path.Combine(savedir2, "bin.2.tmp.guustPKG"))) return;
                progress.Value = 75;

                if (!await DownloadAsync(url4, Path.Combine(savedir2, "bin.3.tmp.guustPKG"))) return;
                progress.Value = 80;

                if (!await DownloadAsync(url5, Path.Combine(savedir2, "bin.6.tmp.guustPKG"))) return;
                progress.Value = 100;
            }

            statusTEXT.Text = "Installing..";

            progress.Value = 1;

            LogManager.LogToFile("Installing Simple-YTDLP..", "INFO");


            string sourceFile1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "install0.pack.guustPKG");
            string extractPath1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");
            string checkPath1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");
            string checkPath1_1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");

            //string sourceFile2 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "install0.pack.guustPKG");
            //string extractPath2 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "install0.pack.guustPKG");

            ZipFile.ExtractToDirectory(sourceFile1, extractPath1, overwriteFiles: true);

            LogManager.LogToFile("installed install0.pack.guustPKG to " + extractPath1, "INFO");

            progress.Value = 50;

            if (File.Exists(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe")) && (Directory.Exists(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Core"))))
            {
                progress.Value = 98;
            }
            else
            {
                LogManager.LogToFile("Failed to install Simple-YTDLP. Please try again.", "ERROR");
                MessageBox.Show("Failed to install Simple-YTDLP. Please try again.", "Simple-YTDLP Installer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            File.Delete(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "install0.pack.guustPKG"));

            LogManager.LogToFile("Deleted install0.pack.guustPKG from " + extractPath1, "INFO");

            progress.Value = 100;

            progress.Visibility = Visibility.Collapsed;

            statusTEXT.Visibility = Visibility.Collapsed;

            InstallBUT.Visibility = Visibility.Collapsed;


            Mouse.OverrideCursor = null;

            MessageBox.Show("Simple-YTDLP has been installed successfully!", "Simple-YTDLP Installer", MessageBoxButton.OK, MessageBoxImage.Information);

            openBUT.Visibility = Visibility.Visible;

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
                MessageBox.Show("Simple-YTDLP is not installed correctly.", "Simple-YTDLP Installer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
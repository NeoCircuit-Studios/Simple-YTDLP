using APPLogManager;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
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

                    statusTEXT.Text = "Loading..";

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

            statusTEXT.Text = "Loading..";

            await Install();
            return;
        }

        private async Task Install()
        {
            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string installedVersionPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "version.guustGV");
            string updateVersionPath = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "TMP", "version.guustGV");

            string installedText = File.Exists(installedVersionPath) ? File.ReadAllText(installedVersionPath).Trim() : "0.0.0.0";
            string updateText = File.Exists(updateVersionPath) ? File.ReadAllText(updateVersionPath).Trim() : "0.0.0.0";
            string Urlverison = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/version.guustGV";

            string exeName = "Simple-YTDLP";

            if (new Version(File.ReadAllText(installedVersionPath).Trim())
               < new Version(File.ReadAllText(updateVersionPath).Trim()))
            {
                // updatefile = %appdata%\NeoCircuit-Studios\Simple-YTDLP\update0.pack.guustPKG

                if (File.Exists(Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "TMP", "update0.pack.guustPKG")))
                {
                    statusTEXT.Text = $"Closing {exeName}...";
                    progress.Value = 30;
                    LogManager.LogToFile("Closing " + exeName);

                    var processes = Process.GetProcessesByName(exeName);

                    foreach (var process in processes)
                    {
                        try
                        {
                            process.Kill();
                            process.WaitForExit();
                            LogManager.LogToFile($"Closed {exeName}.exe (PID: {process.Id})");
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogToFile($"Could not close {exeName}.exe: {ex.Message}");
                        }
                    }

                    if (!processes.Any())
                    {
                        LogManager.LogToFile($"{exeName}.exe was not running.");
                    }

                    progress.Value = 50;
                    statusTEXT.Text = "Updating";

                    string sourceFile1 = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "TMP", "update0.pack.guustPKG");
                    string extractPath1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");

                    ZipFile.ExtractToDirectory(sourceFile1, extractPath1, overwriteFiles: true);

                    LogManager.LogToFile("installed update0.pack.guustPKG to " + extractPath1, "INFO");

                    LogManager.LogToFile("Update completed successfully.");
                    statusTEXT.Text = "Done..";
                    progress.Value = 100;

                    await Task.Delay(200);

                    progress.Value = 10;

                    statusTEXT.Text = $"starting {exeName}...";
                    LogManager.LogToFile("Starting " + exeName);
                    Process.Start(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", exeName + ".exe"));
                    progress.Value = 100;

                    LogManager.LogToFile("Update finished successfully.");

                    statusTEXT.Text = "waiting..";

                    await Task.Delay(1500);

                    Application.Current.Shutdown();
                }
                else
                {
                    statusTEXT.Text = "Update file not found!";
                    progress.Value = 10;

                    LogManager.LogToFile("Update file not found at " + Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "update0.pack.guustPKG"), "ERROR");

                    string url1 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/Simple-YTDLP/update/update0.pack.guustPKG";

                    string savedir1 = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "TMP");


                    using (HttpClient client = new HttpClient())
                    {
                        async Task DownloadAsync(string url, string path)
                        {
                            var data = await client.GetByteArrayAsync(url);
                            await File.WriteAllBytesAsync(path, data);
                            LogManager.LogToFile($"Downloaded [{url}] to [{path}]", "INFO");
                        }

                        await DownloadAsync(url1, Path.Combine(savedir1, "update0.pack.guustPKG"));
                        progress.Value = 20;
                    }

                    statusTEXT.Text = $"Closing {exeName}...";
                    progress.Value = 30;
                    LogManager.LogToFile("Closing " + exeName);

                    var processes = Process.GetProcessesByName(exeName);

                    foreach (var process in processes)
                    {
                        try
                        {
                            process.Kill();
                            process.WaitForExit();
                            LogManager.LogToFile($"Closed {exeName}.exe (PID: {process.Id})");
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogToFile($"Could not close {exeName}.exe: {ex.Message}");
                        }
                    }

                    if (!processes.Any())
                    {
                        LogManager.LogToFile($"{exeName}.exe was not running.");
                    }

                    progress.Value = 50;
                    statusTEXT.Text = "Updating";

                    string sourceFile1 = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP", "TMP", "update0.pack.guustPKG");
                    string extractPath1 = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");

                    ZipFile.ExtractToDirectory(sourceFile1, extractPath1, overwriteFiles: true);

                    LogManager.LogToFile("installed update0.pack.guustPKG to " + extractPath1, "INFO");

                    LogManager.LogToFile("Update completed successfully.");
                    statusTEXT.Text = "Done..";
                    progress.Value = 100;

                    await Task.Delay(200);

                    progress.Value = 10;

                    statusTEXT.Text = $"starting {exeName}...";
                    LogManager.LogToFile("Starting " + exeName);
                    Process.Start(Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", exeName + ".exe"));
                    progress.Value = 100;

                    LogManager.LogToFile("Update finished successfully.");

                    statusTEXT.Text = "waiting..";

                    await Task.Delay(1500);

                    Application.Current.Shutdown();
                }
            }
            else
            {
                LogManager.LogToFile("No update needed. Installed version is up to date.", "INFO");
                if (!AppState.Fromapp)
                {
                    MessageBox.Show("No update needed. Installed version is up to date.", "Simple-YTDLP Updater", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                statusTEXT.Text = "No update needed... waiting 10 sec";
                progress.Value = 100;

                await Task.Delay(10000);

                Application.Current.Shutdown();
            }
        }
    }
}
using APPLogManager2;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
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



            LogManager.LogToFile("Starting installing..", "INFO");

            if (File.Exists(filePath))
            {
                MessageBox.Show("Simple-YTDLP is already installed... Install anyway? (this will reinstall the app)", "Simple-YTDLP Installer", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            }
            else
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                    LogManager.LogToFile("Created directory: " + dirPath, "INFO");
                }

                string url = "https://raw.githubusercontent.com/username/repo/branch/path/to/file.txt";
                string savePath = @"C:\path\to\save\file.txt";

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, savePath);
                }

                Console.WriteLine("Downloaded successfully!");


            }

        }



    }
}
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

            if (File.Exists(filePath))
            {
                MessageBox.Show("Simple-YTDLP is already installed... Install anyway? (this will reinstall the app)", "Simple-YTDLP Installer", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                install();
            }
            else
            {
                install();
            }
        }

        private void install()
        {
            LogManager.LogToFile("Starting installing..", "INFO");

            string programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

            string filePath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP", "Simple-YTDLP.exe");
            string dirPath = Path.Combine(programFilesX86, "NeoCircuit-Studios", "Simple-YTDLP");

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                LogManager.LogToFile("Created directory: " + dirPath, "INFO");
            }

            string url1 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/install0.pack.guustPKG";
            string url2 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/Core/ThirdParty/bin.1.tmp.guustPKG";
            string url3 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/Core/ThirdParty/bin.2.tmp.guustPKG";
            string url4 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/Core/ThirdParty/bin.3.tmp.guustPKG";
            string url5 = "https://github.com/NeoCircuit-Studios/Simple-YTDLP/raw/refs/heads/main/pkg/install/Core/ThirdParty/bin.6.tmp.guustPKG";
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

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url1, savedir1);
                LogManager.LogToFile($"Downloaded [{url1}] to [{savedir1}]", "INFO");
                client.DownloadFile(url2, savedir2);
                LogManager.LogToFile($"Downloaded [{url2}] to [{savedir2}]", "INFO");
                client.DownloadFile(url3, savedir2);
                LogManager.LogToFile($"Downloaded [{url3}] to [{savedir2}]", "INFO");
                client.DownloadFile(url4, savedir2);
                LogManager.LogToFile($"Downloaded [{url4}] to [{savedir2}]", "INFO");
                client.DownloadFile(url5, savedir2);
                LogManager.LogToFile($"Downloaded [{url5}] to [{savedir2}]", "INFO");

            }
        }



    }
}
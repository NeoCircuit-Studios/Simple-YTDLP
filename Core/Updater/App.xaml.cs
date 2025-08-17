using APPLogManager;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool _willshutthefuckup = false;
        public static string RunningTMPfilename = "Simple-YTDLP-UPDATER.guustTMP";
        private FileStream? _lockFileStream; // nullable to allow null assignment

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_lockFileStream != null)
            {
                _lockFileStream.Close();
                _lockFileStream.Dispose();
                _lockFileStream = null;

                try
                {
                    File.Delete(Path.Combine(Path.GetTempPath(), RunningTMPfilename));
                    LogManager.LogToFile("--------APP is STOPPED-------", "INFO");
                }
                catch (Exception ex)
                {
                    LogManager.LogToFile($"Failed to delete running file on exit: {ex.Message}", "ERROR");
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP");

            string tempFilePath = Path.Combine(Path.GetTempPath(), RunningTMPfilename);

            // Set shutdown mode to shut down when the main window closes
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            LogManager.LogToFile("----------Start--------------", "INFO");

            LogManager.LogToFile("@NeoCircuit-Studios@", "INFO");
            LogManager.LogToFile($"Simple_YTDLP - Installer {VersionInfo.Version}", "INFO");
            LogManager.LogToFile("Copyright (C) 2025 NeoCircuit Studios", "INFO");


            try
            {
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);

                    LogManager.LogToFile("Deleted leftover temp lock file on startup.", "DEBUG");
                }
            }
            catch (Exception ex)
            {
                LogManager.LogToFile($"Failed to delete leftover temp lock file on startup: {ex.Message}", "ERROR");
            }

            // Then continue with your existing lock file creation
            try
            {
                _lockFileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                // Could not open with exclusive lock - assume app is already running
                LogManager.LogToFile("Application is already running.", "ERROR");
                if (!_willshutthefuckup)
                {
                    System.Windows.MessageBox.Show("Application is already running.", "Simple-YTDLP Updater: ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Shutdown();
                return;
            }

            if (e.Args.Contains("-shutup"))
            {
                _willshutthefuckup = true;
                LogManager.LogToFile("shut up mode on : Will not show any messages to the user.", "WARRING");
            }

            if (e.Args.Contains("-reset"))
            {
            }


            if (e.Args.Contains("-updateme"))
            {
                LogManager.LogToFile("Update requested via command line argument.", "DEBUG");
                AppState.Fromapp = true;
            }


            base.OnStartup(e);

            // Create your splash window manually
            var MainWindow = new MainWindow();

            // Assign to MainWindow property (important!)
            this.MainWindow = MainWindow;

            // Attach Closing event handler if needed
            MainWindow.Closing += OnWindowClosing;

            // Show the window
            MainWindow.Show();
        }

        public static class ConsoleHelper
        {
            [DllImport("kernel32.dll")]
            public static extern bool AllocConsole();
        }

        public static class VersionInfo
        {
            public static string Version = "1.0.0.0"; // Default version, can be updated dynamically

            //public static string Version =>
            //Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";
        }

        public static class AppState
        {
            public static string CurrentWindow { get; set; } = "";
            public static string UserName { get; set; } = "NULL";
            public static bool UserAgreedToTerms { get; set; } = false;
            public static bool Fromapp { get; set; } = false; 
            public static int TmP { get; set; } = 0;
        }
    }
}

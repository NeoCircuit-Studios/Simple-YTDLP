using APPLogManager;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace Simple_YTDLP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static bool _willshutthefuckup = false;
        public static string RunningTMPfilename = "Simple-YTDLP.guustTMP";
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
                    System.Windows.MessageBox.Show("Application is already running.", "Simple-YTDLP: ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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


            base.OnStartup(e);

            // Create your splash window manually
            var splash = new Windows.UI.Splash();

            // Assign to MainWindow property (important!)
            this.MainWindow = splash;

            // Attach Closing event handler if needed
            splash.Closing += OnWindowClosing;

            // Show the window
            splash.Show();
        }
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
        public static int TmP { get; set; } = 0; 
    }

    public static class DebugSettings
    {
        public static bool IsDebugMode { get; set; } = false;
    }
}

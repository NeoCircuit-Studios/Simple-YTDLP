using APPLogManager;
using System.Diagnostics;
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
        public static string RunningTMPfilename = "Simple-YTDLP-V6.guustTMP";
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

            // --- Kill yt-dlp.exe if still running ---
            try
            {
                foreach (var proc in Process.GetProcessesByName("yt-dlp"))
                {
                    proc.Kill();   // force kill
                    proc.WaitForExit();
                    LogManager.LogToFile($"Killed yt-dlp.exe (PID {proc.Id})", "INFO");
                }
            }
            catch (Exception ex)
            {
                LogManager.LogToFile($"Failed to kill yt-dlp.exe: {ex.Message}", "ERROR");
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folderPath = Path.Combine(appDataPath, "NeoCircuit-Studios", "Simple-YTDLP");

            string tempFilePath = Path.Combine(Path.GetTempPath(), RunningTMPfilename);

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
        private static readonly string versionFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            "NeoCircuit-Studios", "Simple-YTDLP", "version.guustGV");

        public static string Version
        {
            get
            {
                try
                {
                    if (File.Exists(versionFilePath))
                    {
                        string text = File.ReadAllText(versionFilePath).Trim();
                        return string.IsNullOrWhiteSpace(text) ? "0.0.0.0" : text;
                    }
                    else
                    {
                        return "0.0.0.0"; // default if file not found
                    }
                }
                catch
                {
                    return "0.0.0.0"; // fallback if error
                }
            }
        }
    }

    public static class VersionUtils
    {
        public static bool IsNewerVersion(string installed, string update)
        {
            if (Version.TryParse(installed, out Version? installedVer) &&
                Version.TryParse(update, out Version? updateVer))
            {
                return updateVer > installedVer;
            }
            return false;
        }
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

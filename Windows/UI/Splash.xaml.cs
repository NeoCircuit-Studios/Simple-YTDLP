using APPLogManager;
using System;
using System.IO;
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

            version.Text = currentVersion; // Set the version text in the UI

            loadingText.Text = "Checking for Updates....";

            if (File.Exists("version.guustGV"))
            {
                //currentVersion = File.ReadAllText("version.guustGV");
                File.WriteAllText("version.guustGV", currentVersion);
            }
            else
            {
                File.WriteAllText("version.guustGV", currentVersion);
            }

            // download bla bla
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

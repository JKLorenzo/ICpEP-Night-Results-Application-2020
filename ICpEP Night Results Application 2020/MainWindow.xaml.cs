using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace ICpEP_Night_Results_Application_2020
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public FireBaseHelper firebase;
        MediaElement me;
        BackgroundWorker progressAnimator;
        GlobalKeyboardHook _globalKeyboardHook;
        int view = 0;

        public MainWindow()
        {
            InitializeComponent();

            firebase = new FireBaseHelper();

            playBackgroundVideo();

            firebase.startVotedListener(this);

            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();

            progressAnimator = new BackgroundWorker();
            progressAnimator.DoWork += ProgressAnimator_DoWork;
            progressAnimator.ProgressChanged += ProgressAnimator_ProgressChanged;
            progressAnimator.WorkerReportsProgress = true;
            progressAnimator.RunWorkerAsync();

            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private BitmapImage pixelBin(String path)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnDemand;
            bi.CreateOptions = BitmapCreateOptions.DelayCreation;
            bi.DecodePixelHeight = 614;
            bi.DecodePixelWidth = 614;
            bi.UriSource = new Uri("pack://application:,,/" + path, UriKind.RelativeOrAbsolute);
            bi.EndInit();
            bi.Freeze();
            return bi;
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            Console.WriteLine(e.KeyboardData.VirtualCode);
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
            {
                switch (e.KeyboardData.VirtualCode)
                {
                    case 37:
                        if (view == 1)
                        {
                            Console.WriteLine("Setting to concealed images");
                            img1m.Source = pixelBin("Resources/male icon.png");
                            img2m.Source = pixelBin("Resources/male icon.png");
                            img5m.Source = pixelBin("Resources/male icon.png");

                            img1f.Source = pixelBin("Resources/female icon.png");
                            img2f.Source = pixelBin("Resources/female icon.png");
                            img5f.Source = pixelBin("Resources/female icon.png");
                            view = 0;
                        }
                        break;
                    case 39:
                        if (view == 0)
                        {
                            Console.WriteLine("Setting to shown images");
                            img1m.Source = pixelBin("Resources/1m.png");
                            img2m.Source = pixelBin("Resources/2m.png");
                            img5m.Source = pixelBin("Resources/5m.png");

                            img1f.Source = pixelBin("Resources/1f.png");
                            img2f.Source = pixelBin("Resources/2f.png");
                            img5f.Source = pixelBin("Resources/5f.png");
                            view = 1;
                        }
                        break;
                }
            }
        }
        private void myMainWindow_Closing(object sender, CancelEventArgs e)
        {
            _globalKeyboardHook?.Dispose();
        }


        #region Animations

        double p1m = 0, p1f = 0, p2m = 0, p2f = 0, p5m = 0, p5f = 0;
        private void ProgressAnimator_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 11:
                    pbar1m.Value = (double)e.UserState;
                    tb1m.Text = String.Format("{0}%", ((double)e.UserState).ToString("0"));
                    break;
                case 12:
                    pbar1f.Value = (double)e.UserState;
                    tb1f.Text = String.Format("{0}%", ((double)e.UserState).ToString("0"));
                    break;
                case 21:
                    pbar2m.Value = (double)e.UserState;
                    tb2m.Text = String.Format("{0}%", ((double)e.UserState).ToString("0"));
                    break;
                case 22:
                    pbar2f.Value = (double)e.UserState;
                    tb2f.Text = String.Format("{0}%", ((double)e.UserState).ToString("0"));
                    break;
                case 51:
                    pbar5m.Value = (double)e.UserState;
                    tb5m.Text = String.Format("{0}%", ((double)e.UserState).ToString("0"));
                    break;
                case 52:
                    pbar5f.Value = (double)e.UserState;
                    tb5f.Text = String.Format("{0}%", ((double)e.UserState).ToString("0"));
                    break;
            }
        }
        
        private void ProgressAnimator_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (p1m < (firebase.votesM[0] * 1.0 / firebase.totalVotes) * 100)
                {
                    p1m += 0.1;
                    progressAnimator.ReportProgress(11, p1m);
                }
                if (p1m > (firebase.votesM[0] * 1.0 / firebase.totalVotes) * 100)
                {
                    p1m -= 0.1;
                    progressAnimator.ReportProgress(11, p1m);
                }

                if (p1f < (firebase.votesF[0] * 1.0 / firebase.totalVotes) * 100)
                {
                    p1f += 0.1;
                    progressAnimator.ReportProgress(12, p1f);
                }
                if (p1f > (firebase.votesF[0] * 1.0 / firebase.totalVotes) * 100)
                {
                    p1f -= 0.1;
                    progressAnimator.ReportProgress(12, p1f);
                }


                if (p2m < (firebase.votesM[1] * 1.0 / firebase.totalVotes) * 100)
                {
                    p2m += 0.1;
                    progressAnimator.ReportProgress(21, p2m);
                }
                if (p2m > (firebase.votesM[1] * 1.0 / firebase.totalVotes) * 100)
                {
                    p2m -= 0.1;
                    progressAnimator.ReportProgress(21, p2m);
                }

                if (p2f < (firebase.votesF[1] * 1.0 / firebase.totalVotes) * 100)
                {
                    p2f += 0.1;
                    progressAnimator.ReportProgress(22, p2f);
                }
                if (p2f > (firebase.votesF[1] * 1.0 / firebase.totalVotes) * 100)
                {
                    p2f -= 0.1;
                    progressAnimator.ReportProgress(22, p2f);
                }


                if (p5m < (firebase.votesM[2] * 1.0 / firebase.totalVotes) * 100)
                {
                    p5m += 0.1;
                    progressAnimator.ReportProgress(51, p5m);
                }
                if (p5m > (firebase.votesM[2] * 1.0 / firebase.totalVotes) * 100)
                {
                    p5m -= 0.1;
                    progressAnimator.ReportProgress(51, p5m);
                }

                if (p5f < (firebase.votesF[2] * 1.0 / firebase.totalVotes) * 100)
                {
                    p5f += 0.1;
                    progressAnimator.ReportProgress(52, p5f);
                }
                if (p5f > (firebase.votesF[2] * 1.0 / firebase.totalVotes) * 100)
                {
                    p5f -= 0.1;
                    progressAnimator.ReportProgress(52, p5f);
                }
                System.Threading.Thread.Sleep(1);
            }
        }

        #endregion

        #region Background Video Controls

        private void playBackgroundVideo()
        {
            VisualBrush vb = new VisualBrush();
            me = new MediaElement();
            me.Stretch = Stretch.UniformToFill;
            me.Width = 1920;
            me.Height = 1080;
            me.LoadedBehavior = MediaState.Manual;
            Console.WriteLine(System.IO.Path.Combine(Environment.CurrentDirectory, "BackgroundVideo.mp4"));
            me.MediaEnded += me_OnMediaEnded;
            me.Source = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "BackgroundVideo.mp4"), UriKind.Relative);
            me.Volume = 0;
            me.Play();
            me.SpeedRatio = 1;
            vb.Visual = me;
            // Set Background from Black to Vid
            grid.Background = vb;
        }

        private void me_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            me.Position = new TimeSpan(0, 0, 0);
            me.Play();
        }

        #endregion

        #region Window Scaling
        public static readonly DependencyProperty ScaleValueProperty = DependencyProperty.Register("ScaleValue", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));

        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                return mainWindow.OnCoerceScaleValue((double)value);
            else
                return value;
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = o as MainWindow;
            if (mainWindow != null)
                mainWindow.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            if (double.IsNaN(value))
                return 1.0f;

            value = Math.Max(0.1, value);
            return value;
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {

        }

        public double ScaleValue
        {
            get
            {
                return (double)GetValue(ScaleValueProperty);
            }
            set
            {
                SetValue(ScaleValueProperty, value);
            }
        }

        private void MainGrid_SizeChanged(object sender, EventArgs e)
        {
            CalculateScale();
        }

        private void CalculateScale()
        {
            double yScale = ActualHeight / 785f;
            double xScale = ActualWidth / 745f;
            double value = Math.Min(xScale, yScale);
            ScaleValue = (double)OnCoerceScaleValue(myMainWindow, value);
        }
        #endregion
    }
}

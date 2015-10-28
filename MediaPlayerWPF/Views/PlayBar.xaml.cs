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
using System.Windows.Threading;

namespace MediaPlayerWPF.Views
{
    /// <summary>
    /// Interaction logic for PlayBar.xaml
    /// </summary>
    public partial class PlayBar : UserControl
    {
        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        public ICommand PauseCommand
        {
            get { return (ICommand)GetValue(PauseCommandProperty); }
            set { SetValue(PauseCommandProperty, value); }
        }

        public ICommand StopCommand
        {
            get { return (ICommand)GetValue(StopCommandProperty); }
            set { SetValue(StopCommandProperty, value); }
        }

        public ICommand OpenCommand
        {
            get { return (ICommand)GetValue(OpenCommandProperty); }
            set { SetValue(OpenCommandProperty, value); }
        }

        public double MediaVolume
        {
            get { return (double)GetValue(MediaVolumeProperty); }
            set { SetValue(MediaVolumeProperty, value); }
        }

        public double MediaProgress
        {
            get { return (double)GetValue(MediaProgressProperty); }
            set { SetValue(MediaProgressProperty, value); }
        }

        public double MediaDuration
        {
            get { return (double)GetValue(MediaDurationProperty); }
            set { SetValue(MediaDurationProperty, value); }
        }

        public bool AutoHide
        {
            get { return (bool)GetValue(AutoHideProperty); }
            set { SetValue(AutoHideProperty, value); }
        }

        public static readonly DependencyProperty AutoHideProperty = DependencyProperty.Register("AutoHide", typeof(bool), typeof(PlayBar), new PropertyMetadata(false));


        public static readonly DependencyProperty MediaDurationProperty = DependencyProperty.Register("MediaDuration", typeof(double), typeof(PlayBar), new PropertyMetadata(0d));


        public static readonly DependencyProperty MediaProgressProperty = DependencyProperty.Register("MediaProgress", typeof(double), typeof(PlayBar), new PropertyMetadata(0d));


        public static readonly DependencyProperty MediaVolumeProperty = DependencyProperty.Register("MediaVolume", typeof(double), typeof(PlayBar), new PropertyMetadata(0.5d));


        public static readonly DependencyProperty OpenCommandProperty = DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(PlayBar), new PropertyMetadata(null));


        public static readonly DependencyProperty StopCommandProperty = DependencyProperty.Register("StopCommand", typeof(ICommand), typeof(PlayBar), new PropertyMetadata(null));


        public static readonly DependencyProperty PauseCommandProperty = DependencyProperty.Register("PauseCommand", typeof(ICommand), typeof(PlayBar), new PropertyMetadata(null));


        public static readonly DependencyProperty PlayCommandProperty = DependencyProperty.Register("PlayCommand", typeof(ICommand), typeof(PlayBar), new PropertyMetadata(null));


        private DispatcherTimer autoHideTimer;

        public PlayBar()
        {
            InitializeComponent();
        }

        private void InitAutohideTimer()
        {
            autoHideTimer = new DispatcherTimer();
            autoHideTimer.Interval = TimeSpan.FromMilliseconds(50);
            autoHideTimer.Tick += OnAutoHideTimer_Tick;
        }

        private void OnAutoHideTimer_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05;
            if (this.Opacity <= 0)
            {
                autoHideTimer.Stop();
            }
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (AutoHide)
            {
                if (autoHideTimer == null)
                {
                    InitAutohideTimer();
                }

                this.Opacity = 1;
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {

            if (AutoHide)
            {
                if (autoHideTimer == null)
                {
                    InitAutohideTimer();
                }

                this.Opacity = 0;
                autoHideTimer.Stop();
            }
        }
    }
}

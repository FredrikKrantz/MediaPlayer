using MediaPlayerWPF.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MediaPlayerWPF.View_models
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MediaElement MediaFile { get; private set; }

        private double volume;
        public double Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                MediaFile.Volume = value;
                UpdateUI("Volume");
            }
        }

        private double progress;
        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;

                MediaFile.Pause();

                MediaFile.Position = TimeSpan.FromSeconds(value);
                progressTimer.Tick += StartPlayAfterAwhile;

                UpdateUI("Progress");

            }
        }

        void StartPlayAfterAwhile(object sender, EventArgs e)
        {
            MediaFile.Play();
            progressTimer.Tick -= StartPlayAfterAwhile; //För att inte stacka metoden om man drar i baren flera gånger
        }

        private DispatcherTimer progressTimer;

        private double duration;
        public double Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                UpdateUI("Duration");
            }
        }

        public MainWindowViewModel()
        {
            MediaFile = new MediaElement() { LoadedBehavior = MediaState.Manual };
            MediaFile.MediaOpened += MediaFile_MediaOpened;

            progressTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200),
            };
            progressTimer.Tick += OnProgressTimer_Tick;
        }

        void OnProgressTimer_Tick(object sender, EventArgs e)
        {
            progress = MediaFile.NaturalDuration.TimeSpan.TotalSeconds;
            UpdateUI("Progress");
        }

        void MediaFile_MediaOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            Duration = MediaFile.NaturalDuration.TimeSpan.TotalSeconds;
        }

        #region Buttons
        private RelayCommand openCommand;
        public ICommand Open
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new RelayCommand(OpenMediaFile);
                }
                return openCommand;
            }
        }

        private RelayCommand pauseCommand;
        public ICommand Pause
        {
            get
            {
                if (pauseCommand == null)
                {
                    pauseCommand = new RelayCommand(OnMediaPause);
                }
                return pauseCommand;
            }
        }

        private RelayCommand stopCommand;
        public ICommand Stop
        {
            get
            {
                if (stopCommand == null)
                {
                    stopCommand = new RelayCommand(OnMediaStop);
                }
                return stopCommand;
            }
        }

        private RelayCommand playCommand;
        public ICommand Play
        {
            get
            {
                if (playCommand == null)
                {
                    playCommand = new RelayCommand(OnMediaPlay);
                }
                return playCommand;
            }
        }

        private void OpenMediaFile(object param)
        {
            OnMediaPause(param);

            var mediaFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Media Files | *.AVI;*.WMV; | All Files | *.*"
            };

            if (mediaFileDialog.ShowDialog() == true)
            {
                progressTimer.Stop();
                MediaFile.Source = null;
                MediaFile.Source = new Uri(mediaFileDialog.FileName);
                UpdateUI("MediaFile");
                OnMediaPlay(param);
            }
        }

        private void OnMediaPause(object param)
        {
            MediaFile.Pause();
            progressTimer.Stop();
        }

        private void OnMediaStop(object param)
        {
            MediaFile.Stop();
            progressTimer.Stop();
        }

        private void OnMediaPlay(object param)
        {
            MediaFile.Play();
            progressTimer.Start();
            UpdateUI("MediaFile");
        }
        #endregion
    }
}

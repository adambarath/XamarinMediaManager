using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaManager;
using MediaManager.Library;
using MediaManager.Media;
using MediaManager.Playback;
using MediaManager.Player;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace ElementPlayer.Core.ViewModels
{
    public class RepeatShuffleTestViewModel : BaseViewModel
    {
        public IMediaManager MediaManager => CrossMediaManager.Current;

        public RepeatShuffleTestViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            MediaManager.MediaItemFailed += Current_MediaItemFailed;
            MediaManager.MediaItemFinished += Current_MediaItemFinished;
            MediaManager.MediaItemChanged += Current_MediaItemChanged;
            MediaManager.StateChanged += Current_StatusChanged;
            MediaManager.BufferedChanged += Current_BufferingChanged;
            MediaManager.PositionChanged += Current_PositionChanged;
        }

        #region Event handlers

        private void Current_MediaItemFailed(object sender, MediaItemFailedEventArgs e)
        {
            Log.Debug($"Media item failed: {e.MediaItem.Title}, Message: {e.Message}, Exception: {e.Exeption?.ToString()};");
        }

        private void Current_MediaItemFinished(object sender, MediaItemEventArgs e)
        {
            Log.Debug($"Media item finished: {e.MediaItem.Title};");
        }

        private void Current_MediaItemChanged(object sender, MediaItemEventArgs e)
        {
            Log.Debug($"Media item changed, new item title: {e.MediaItem.Title};");
            RaisePropertyChanged(nameof(CurrentTitle));
        }

        private void Current_StatusChanged(object sender, StateChangedEventArgs e)
        {
            Log.Debug($"Status changed: {System.Enum.GetName(typeof(MediaPlayerState), e.State)};");
        }

        private void Current_BufferingChanged(object sender, BufferedChangedEventArgs e)
        {
            Log.Debug($"Total buffered time is {e.Buffered};");
        }

        private void Current_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            Log.Debug($"Current position is {e.Position};");
            RaisePropertyChanged(nameof(Position));
            RaisePropertyChanged(nameof(TotalPlayed));
        }

        #endregion

        public override string Title => "RepeatShuffleTestViewModel";

        public IList<string> Mp3UrlList => new[]{
            "https://ia800806.us.archive.org/15/items/Mp3Playlist_555/AaronNeville-CrazyLove.mp3",
            "https://ia800605.us.archive.org/32/items/Mp3Playlist_555/CelineDion-IfICould.mp3",
            "https://ia800605.us.archive.org/32/items/Mp3Playlist_555/Daughtry-Homeacoustic.mp3",
            "https://storage.googleapis.com/uamp/The_Kyoto_Connection_-_Wake_Up/01_-_Intro_-_The_Way_Of_Waking_Up_feat_Alan_Watts.mp3",
            "https://aphid.fireside.fm/d/1437767933/02d84890-e58d-43eb-ab4c-26bcc8524289/d9b38b7f-5ede-4ca7-a5d6-a18d5605aba1.mp3"
            };

        public IMediaItem Current => MediaManager.MediaQueue.Current;

        public string CurrentTitle => Current?.GetTitle();

        public int Buffered => Convert.ToInt32(MediaManager.Buffered.TotalSeconds);

        public int Duration => Convert.ToInt32(MediaManager.Duration.TotalSeconds);

        public int Position => Convert.ToInt32(MediaManager.Position.TotalSeconds);

        public float FloatedPosition => (float)Position / (float)Duration;

        public string TotalDuration => MediaManager.Duration.ToString(@"mm\:ss");

        public string TotalPlayed => MediaManager.Position.ToString(@"mm\:ss");

        public override async Task Initialize()
        {
            await base.Initialize();

            await MediaManager.Play(Mp3UrlList);
        }

        public MvxAsyncCommand PlayPauseCommand => new MvxAsyncCommand(MediaManager.PlayPause);

        public MvxAsyncCommand NextCommand => new MvxAsyncCommand(MediaManager.PlayNext);

        public MvxAsyncCommand PrevCommand => new MvxAsyncCommand(MediaManager.PlayPrevious);

        public MvxAsyncCommand ToggleShuffleCommand => new MvxAsyncCommand(async () =>
        {
            MediaManager.ToggleShuffle();
        });

        public MvxAsyncCommand ToggleRepeatCommand => new MvxAsyncCommand(async () =>
        {
            MediaManager.ToggleRepeat();
        });
    }
}

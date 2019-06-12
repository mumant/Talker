namespace Talker
{
	class TalkerController
	{
		private ITalker _talker;
		private ITalkerView _view;
		private ISettingsManager _settingsManager;

		public TalkerController(ITalker talker, ITalkerView view, ISettingsManager settingsManager)
		{
			_talker = talker;
			_view = view;
			_settingsManager = settingsManager;

			Init();
		}

		private void Init()
		{
			InitTalker();
			InitView();
		}

		private void InitTalker()
		{
			_settingsManager.Restore(_talker);
		}

		private void InitView()
		{
			_view.ViewClosing += () =>
			{
				_talker.Stop();
				_settingsManager.Save(_talker);
				_talker.Dispose();
			};

			_view.IntervalChanged += interval => _talker.Interval = interval;
			_view.RepeatChanged += repeat => _talker.Repeat = repeat;
			_view.RandomChanged += random => _talker.Random = random;
			_view.SpeedChanged += speed => _talker.Speed = speed;
			_view.VolumeChanged += volume => _talker.Volume = volume;
			_view.VoiceChanged += voice => _talker.Voice = voice;
			_view.TimbreChanged += timbre => _talker.Timbre = timbre;
			_view.RecentFilesChanged += recentFiles => _settingsManager.RecentFiles = recentFiles;
			_view.GetRecentFilesCommand += () => _settingsManager.RecentFiles;
			_view.StartCommand += _talker.Start;
			_view.StopCommand += _talker.Stop;
			_view.PauseCommand += _talker.Pause;
			_view.ContinueCommand += _talker.Continue;
			_view.NextCommand += _talker.Next;
			_view.LoadFileCommand += file => _talker.LoadFile(file);

			_view.SetTalker(_talker);
		}
	}
}
using System;
using System.Collections.Generic;

namespace Talker
{
	interface ITalkerView
	{
		event Action ViewClosing;
		event Action<uint> IntervalChanged;
		event Action<bool> RepeatChanged;
		event Action<bool> RandomChanged;
		event Action<int> SpeedChanged;
		event Action<int> VolumeChanged;
		event Action<string> VoiceChanged;
		event Action<int> TimbreChanged;
		event Action<List<string>> RecentFilesChanged;

		event Action StartCommand;
		event Action StopCommand;
		event Action PauseCommand;
		event Action ContinueCommand;
		event Action NextCommand;
		event Func<string, bool> LoadFileCommand;
		event Func<List<string>> GetRecentFilesCommand;

		void SetTalker(ITalker talker);
	}
}

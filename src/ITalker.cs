using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;

namespace Talker
{
	interface ITalker : IDisposable
	{
		event Action<TalkerStatus> StateChanged;
		event Action<bool> ReadyChanged;
		event Action<int> SpeakElementChanged;
		event Action TextBaseLoaded;

		uint Interval { get; set; }
		bool Random { get; set; }
		bool Repeat { get; set; }
		int Speed { get; set; }
		int Volume { get; set; }
		string Voice { get; set; }
		int Timbre { get; set; }
		TextBase TextBase { get; }

		bool IsReady { get; }
		TalkerStatus State { get; }

		int MinTimbreValue { get; }
		int MaxTimbreValue { get; }

		int MinVolumeValue { get; }
		int MaxVolumeValue { get; }

		int MinRateValue { get; }
		int MaxRateValue { get; }

		ReadOnlyCollection<InstalledVoice> InstalledVoices { get; }

		void Start();
		void Stop();
		void Pause();
		void Continue();
		void Next();

		bool LoadFile(string file);
	}
}
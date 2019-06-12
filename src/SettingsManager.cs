using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Talker
{
	class SettingsManager : ISettingsManager
	{
		private List<string> _recentFiles = new List<string>();

		public void Restore(ITalker talker)
		{
			talker.Interval = Properties.Settings.Default.Interval;
			talker.Random = Properties.Settings.Default.Random;
			talker.Repeat = Properties.Settings.Default.Repeat;
			talker.Speed = Properties.Settings.Default.Speed;
			talker.Volume = Properties.Settings.Default.Volume;
			talker.Voice = Properties.Settings.Default.Voice;
			talker.Timbre = Properties.Settings.Default.Timbre;
			_recentFiles = new List<string>((Properties.Settings.Default.RecentFiles ?? new StringCollection()).Cast<string>());
		}

		public void Save(ITalker talker)
		{
			Properties.Settings.Default.Interval = talker.Interval;
			Properties.Settings.Default.Random = talker.Random;
			Properties.Settings.Default.Repeat = talker.Repeat;
			Properties.Settings.Default.Speed = talker.Speed;
			Properties.Settings.Default.Volume = talker.Volume;
			Properties.Settings.Default.Voice = talker.Voice;
			Properties.Settings.Default.Timbre = talker.Timbre;

			StringCollection files = new StringCollection();
			files.AddRange(_recentFiles.ToArray());
			Properties.Settings.Default.RecentFiles = files;

			Properties.Settings.Default.Save();
		}

		public List<string> RecentFiles
		{
			get { return new List<string>(_recentFiles); }

			set
			{
				if (value != null)
				{
					_recentFiles = new List<string>(value);
				}
			}
		}
	}
}
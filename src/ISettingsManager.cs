using System.Collections.Generic;

namespace Talker
{
	interface ISettingsManager
	{
		List<string> RecentFiles { get; set; }

		void Restore(ITalker model);
		void Save(ITalker model);
	}
}
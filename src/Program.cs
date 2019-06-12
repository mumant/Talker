using System;
using System.Windows.Forms;

namespace Talker
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			MainForm view = new MainForm();
			TalkerController controller = new TalkerController(new Talker(), view, new SettingsManager());

			Application.Run(view);
		}
	}
}

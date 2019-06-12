using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Talker
{
	partial class MainForm : Form, ITalkerView
	{
		public event Action ViewClosing;
		public event Action<uint> IntervalChanged;
		public event Action<bool> RepeatChanged;
		public event Action<bool> RandomChanged;
		public event Action<int> SpeedChanged;
		public event Action<int> VolumeChanged;
		public event Action<string> VoiceChanged;
		public event Action<int> TimbreChanged;
		public event Action StartCommand;
		public event Action StopCommand;
		public event Action PauseCommand;
		public event Action ContinueCommand;
		public event Action NextCommand;
		public event Func<string, bool> LoadFileCommand;
		public event Action<List<string>> RecentFilesChanged;
		public event Func<List<string>> GetRecentFilesCommand;

		private ITalker _talker;
		private const int _maxRecentFiles = 5;
		private EditorForm _editorForm;

		public MainForm()
		{
			InitializeComponent();
			InitEvents();
		}

		private void InitEvents()
		{
			Closing += delegate
			{
				ViewClosing?.Invoke();
			};

			numericInterval.ValueChanged += delegate
			{
				IntervalChanged?.Invoke((uint)numericInterval.Value);
			};

			checkRepeat.CheckedChanged += delegate
			{
				RepeatChanged?.Invoke(checkRepeat.Checked);
			};

			checkRandom.CheckedChanged += delegate
			{
				RandomChanged?.Invoke(checkRandom.Checked);
			};

			trackSpeed.ValueChanged += delegate
			{
				SpeedChanged?.Invoke(trackSpeed.Value);
				UpdateLabelSpeed();
			};

			trackVolume.ValueChanged += delegate
			{
				VolumeChanged?.Invoke(trackVolume.Value);
				UpdateLabelVolume();
			};

			comboVoice.SelectedValueChanged += delegate
			{
				VoiceChanged?.Invoke(comboVoice.SelectedItem?.ToString());
			};

			trackTimbre.ValueChanged += delegate
			{
				TimbreChanged?.Invoke(trackTimbre.Value);
				UpdateLabelTimbre();
			};
		}

		public void SetTalker(ITalker talker)
		{
			_talker = talker;
			_talker.StateChanged += _ => UpdateControls();
			_talker.ReadyChanged += _ => UpdateControls();

			InitControls();
			UpdateControls();
			UpdateLabelSpeed();
			UpdateLabelVolume();
			UpdateLabelTimbre();
			UpdateRecentFiles(RecentFiles);
		}

		private void InitControls()
		{
			LoadInstalledVoices();

			trackTimbre.Minimum = _talker.MinTimbreValue;
			trackTimbre.Maximum = _talker.MaxTimbreValue;

			trackVolume.Minimum = _talker.MinVolumeValue;
			trackVolume.Maximum = _talker.MaxVolumeValue;

			trackSpeed.Minimum = _talker.MinRateValue;
			trackSpeed.Maximum = _talker.MaxRateValue;

			numericInterval.Value = _talker.Interval;
			checkRandom.Checked = _talker.Random;
			checkRepeat.Checked = _talker.Repeat;
			trackSpeed.Value = _talker.Speed;
			trackVolume.Value = _talker.Volume;
			comboVoice.SelectedItem = _talker.Voice;
			trackTimbre.Value = _talker.Timbre;
		}

		public void UpdateControls()
		{
			buttonStartStop.Enabled = _talker.IsReady;
			buttonPauseContinue.Enabled = _talker.State != TalkerStatus.Stop;
			buttonNext.Enabled = _talker.State != TalkerStatus.Stop;

			numericInterval.Enabled = _talker.IsReady;
			checkRandom.Enabled = _talker.IsReady;
			checkRepeat.Enabled = _talker.IsReady;
			comboVoice.Enabled = _talker.IsReady;
			trackSpeed.Enabled = _talker.IsReady;
			trackVolume.Enabled = _talker.IsReady;
			trackTimbre.Enabled = _talker.IsReady;

			buttonStartStop.Text = _talker.State == TalkerStatus.Stop ? "Начать" : "Остановить";
			buttonPauseContinue.Text = _talker.State == TalkerStatus.Pause ? "Возобновить" : "Пауза";
		}

		private void UpdateLabelSpeed()
		{
			labelSpeed.Text = "Скорость: " + trackSpeed.Value;
		}

		private void UpdateLabelVolume()
		{
			labelVolume.Text = "Громкость: " + trackVolume.Value;
		}

		private void UpdateLabelTimbre()
		{
			labelTimbre.Text = "Тембр: " + trackTimbre.Value;
		}

		private void LoadInstalledVoices()
		{
			comboVoice.Items.Clear();

			foreach (var voice in _talker.InstalledVoices)
			{
				if (voice.Enabled)
				{
					comboVoice.Items.Add(voice.VoiceInfo.Description);
				}
			}
		}

		private bool LoadFile(string file)
		{
			if (LoadFileCommand == null || !LoadFileCommand(file))
			{
				MessageBox.Show("Не удалось загрузить файл " + file, GetAssemblyName());
				return false;
			}

			this.Text = GetAssemblyName() + " - " + Path.GetFileName(file);
			return true;
		}

		private string GetAssemblyName()
		{
			var titleAttribute = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)?[0] as AssemblyTitleAttribute;
			return titleAttribute?.Title;
		}

		private void Start()
		{
			StartCommand?.Invoke();
		}

		private void Stop()
		{
			StopCommand?.Invoke();
		}

		private void Pause()
		{
			PauseCommand?.Invoke();
		}

		private void Continue()
		{
			ContinueCommand?.Invoke();
		}

		private void Next()
		{
			NextCommand?.Invoke();
		}

		private void menuItemOpen_Click(object sender, EventArgs e)
		{
			Stop();

			using (OpenFileDialog fileDialog = new OpenFileDialog())
			{
				fileDialog.Filter = "UTF-8 (*.*)|*.*";

				if (fileDialog.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				if (LoadFile(fileDialog.FileName))
				{
					AddFileToRecent(fileDialog.FileName);
				}
			}
		}

		private void buttonStartStop_Click(object sender, EventArgs e)
		{
			if (_talker.State == TalkerStatus.Stop)
			{
				Start();
			}
			else if (_talker.State == TalkerStatus.Run || _talker.State == TalkerStatus.Pause)
			{
				Stop();
			}
		}

		private void buttonPause_Click(object sender, EventArgs e)
		{
			if (_talker.State == TalkerStatus.Run)
			{
				Pause();
			}
			else if (_talker.State == TalkerStatus.Pause)
			{
				Continue();
			}
		}

		private void buttonNext_Click(object sender, EventArgs e)
		{
			Next();
		}

		private void menuItemEditor_Click(object sender, EventArgs e)
		{
			if (_editorForm == null)
			{
				_editorForm = new EditorForm(){ Owner = this };
				_editorForm.SetTalker(_talker);
			}

			if (!_editorForm.Visible)
			{
				_editorForm.Visible = true;
			}
		}

		private List<string> RecentFiles
		{
			get
			{
				return GetRecentFilesCommand?.Invoke() ?? new List<string>();
			}

			set
			{
				RecentFilesChanged?.Invoke(value);
			}
		}

		private void OnRecentFileClick(object sender, EventArgs e)
		{
			Stop();

			if (sender is ToolStripItem meniItem)
			{
				var recentFiles = RecentFiles;

				int index = recentFiles.IndexOf(meniItem.Text);

				if (LoadFile(meniItem.Text))
				{
					if (index > 0)
					{
						recentFiles.Insert(0, meniItem.Text);
						index++;
					}
					else
					{
						index = -1;
					}
				}

				if (index != -1)
				{
					recentFiles.RemoveAt(index);
					RecentFiles = recentFiles;
					UpdateRecentFiles(recentFiles);
				}
			}
		}

		private bool AddFileToRecent(string filePath)
		{
			var recentFiles = RecentFiles;

			if (recentFiles.Contains(filePath))
			{
				return false;
			}

			recentFiles.Insert(0, filePath);
			RecentFiles = recentFiles;
			UpdateRecentFiles(recentFiles);

			return true;
		}

		private void UpdateRecentFiles(List<string> recentFiles)
		{
			if (recentFiles.Count > _maxRecentFiles)
			{
				while (recentFiles.Count > _maxRecentFiles)
				{
					recentFiles.RemoveAt(recentFiles.Count - 1);
				}

				RecentFiles = recentFiles;
			}

			int recentFilesSeparatorIndex = menuItemFile.DropDownItems.IndexOf(menuItemRecentFilesSeparator);

			if (recentFilesSeparatorIndex != -1)
			{
				int insertIndex = recentFilesSeparatorIndex + 1;

				while (menuItemFile.DropDownItems.Count > insertIndex)
				{
					menuItemFile.DropDownItems.RemoveAt(insertIndex);
				}

				foreach (string filePath in recentFiles)
				{
					if (File.Exists(filePath))
					{
						menuItemFile.DropDownItems.Add(filePath).Click += OnRecentFileClick;
					}
				}

				menuItemRecentFilesSeparator.Visible = menuItemFile.DropDownItems.Count > insertIndex;
			}
		}
	}
}
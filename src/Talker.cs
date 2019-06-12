using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Windows.Media;
using System.Media;
using System.Linq;
using System.IO;

namespace Talker
{
	class Talker : ITalker
	{
		private TalkerStatus _state = TalkerStatus.Stop;

		private int _currentTextBaseIndex = -1;

		private TextBase _textBase;

		private SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
		private MediaPlayer _player = new MediaPlayer();

		private Task<SpeakPosition> _taskAudioFilePreparation;

		private const float _minVolumeValueInternal = 0f;
		private const float _maxVolumeValueInternal = 1f;

		private const int _minRateValue = -10;
		private const int _maxRateValue = 10;

		private const float _minRateValueInternal = 0f;
		private const float _maxRateValueInternal = 2f;

		private double _playerRateInternal;

		private const int _minVolumeValue = 0;
		private const int _maxVolumeValue = 100;

		private const int _minTimbreValue = -10;
		private const int _maxTimbreValue = 10;
		private const int _timbreDisableValue = (_maxTimbreValue + _minTimbreValue) / 2;

		public int MinTimbreValue => _minTimbreValue;
		public int MaxTimbreValue => _maxTimbreValue;
		public int MinVolumeValue => _minVolumeValue;
		public int MaxVolumeValue => _maxVolumeValue;
		public int MinRateValue => _minRateValue;
		public int MaxRateValue => _maxRateValue;

		private const float _minPitchShiftCoeff = 0.5f;
		private const float _maxPitchShiftCoeff = 1.5f;

		private int _timbre = _timbreDisableValue;
		private float _pitchShiftCoeff = 0f;

		private double _volume;

		private Random _random = new Random(Guid.NewGuid().GetHashCode());
		private List<int> _randomIndexes;

		private string _audioFilePath = GetTempAudioFile();
		private string _audioFilePathNext = GetTempAudioFile();

		private uint _interval = 0;
		private bool _isRepeat = false;
		private bool _isRandom = false;

		private SynchronizationContext _context;

		public event Action<TalkerStatus> StateChanged;
		public event Action<bool> ReadyChanged;
		public event Action<int> SpeakElementChanged;
		public event Action TextBaseLoaded;

		private class SpeakPosition
		{
			public int randomIndex;
			public int textBaseIndex;
			public bool isRandom;
			public string filePath;
		}

		public Talker()
		{
			TextBase = new TextBase();
			TextBase.Loaded += () => TextBaseLoaded?.Invoke();

			_synthesizer.Volume = 100;
			_volume = _player.Volume;

			_playerRateInternal = _player.SpeedRatio;

			_player.MediaOpened += delegate
			{
				// требуется переустановка настроек после открытия
				_player.Volume = _volume;
				_player.SpeedRatio = _playerRateInternal;
				_player.Balance = 0;
			};

			_player.MediaEnded += delegate { SpeakCompleted(); };
		}

		public void Dispose()
		{
			_synthesizer?.Dispose();
			_player?.Close();
			DeleteTempFiles();
		}

		private void DeleteTempFiles()
		{
			DeleteFile(_audioFilePath);
			DeleteFile(_audioFilePathNext);
		}

		private void SpeakCompleted()
		{
			if (_context != null)
			{
				_context.Post( _ => SpeakNext(), null);
			}
			else
			{
				SpeakNext();
			}
		}

		public ReadOnlyCollection<InstalledVoice> InstalledVoices => _synthesizer.GetInstalledVoices();

		public string Voice
		{
			get
			{
				return _synthesizer.Voice?.Description;
			}

			set
			{
				InstalledVoice installedVoice = InstalledVoices.FirstOrDefault(voice => voice.VoiceInfo.Description == value);

				if (installedVoice != null && _synthesizer.Voice.Name != installedVoice.VoiceInfo.Name)
				{
					_synthesizer.SelectVoice(installedVoice.VoiceInfo.Name);
					ResetTaskAudioFilePreparation();
				}
			}
		}

		private void OnTextBaseLoaded()
		{
			ReadyChanged?.Invoke(IsReady);
		}

		private void OnTextBaseItemDeleted(int itemIndex)
		{
			if (State == TalkerStatus.Stop)
			{
				return;
			}

			if (Random)
			{
				_randomIndexes = null;
			}
			else
			{
				if (_currentTextBaseIndex > -1 && _currentTextBaseIndex >= itemIndex)
				{
					_currentTextBaseIndex--;
				}
			}

			ResetTaskAudioFilePreparation();

			if (!IsReady)
			{
				Stop();
			}
		}

		public bool LoadFile(string file)
		{
			return TextBase.LoadFile(file);
		}

		public TextBase TextBase
		{
			get { return _textBase; }
			set
			{
				if (_textBase != null)
				{
					_textBase.Loaded -= OnTextBaseLoaded;
					_textBase.ItemDeleted -= OnTextBaseItemDeleted;
				}

				_textBase = value;

				_textBase.Loaded += OnTextBaseLoaded;
				_textBase.ItemDeleted += OnTextBaseItemDeleted;
			}
		}

		public TalkerStatus State
		{
			get { return _state; }
			private set
			{
				_state = value;
				StateChanged?.Invoke(value);
			}
		}

		public uint Interval
		{
			get { return _interval; }
			set
			{
				_interval = value;
				ResetTaskAudioFilePreparation();
			}
		}

		public bool Repeat
		{
			get { return _isRepeat; }
			set
			{
				_isRepeat = value;
				ResetTaskAudioFilePreparation();
			}
		}

		public bool Random
		{
			get { return _isRandom; }
			set
			{
				_isRandom = value;
				ResetTaskAudioFilePreparation();
			}
		}

		public int Speed
		{
			get { return _synthesizer.Rate; }
			set
			{
				_synthesizer.Rate = CheckInt(value, _minRateValue, _maxRateValue);
				ResetTaskAudioFilePreparation();
			}
		}

		public int PlayerSpeed
		{
			get
			{
				return ConvertInternalFloatToInt((float)_player.SpeedRatio, _minRateValue, _maxRateValue, _minRateValueInternal, _maxRateValueInternal);
			}

			set
			{
				_playerRateInternal = _player.SpeedRatio = ConvertIntToInternalFloat(value, _minRateValue, _maxRateValue, _minRateValueInternal, _maxRateValueInternal);
			}
		}

		public int Volume
		{
			get
			{
				return ConvertInternalFloatToInt((float)/*player.Volume*/_volume, _minVolumeValue, _maxVolumeValue, _minVolumeValueInternal, _maxVolumeValueInternal);
			}

			set
			{
				_volume = _player.Volume = ConvertIntToInternalFloat(value, _minVolumeValue, _maxVolumeValue, _minVolumeValueInternal, _maxVolumeValueInternal);
			}
		}

		public int Timbre
		{
			get { return _timbre; }
			set
			{
				_timbre = CheckInt(value, _minTimbreValue, _maxTimbreValue);
				_pitchShiftCoeff = ConvertIntToInternalFloat(_timbre, _minTimbreValue, _maxTimbreValue, _minPitchShiftCoeff, _maxPitchShiftCoeff);
				ResetTaskAudioFilePreparation();
			}
		}

		public bool IsReady => _textBase.Count != 0; 

		public void Start()
		{
			if (!IsReady)
			{
				return;
			}

			if (_context == null)
			{
				_context = SynchronizationContext.Current;
			}

			State = TalkerStatus.Run;
			_currentTextBaseIndex = -1;
			SpeakNext();
		}

		public void Stop()
		{
			_synthesizer.SpeakAsyncCancelAll();
			_player.Stop();
			_player.Close();
			State = TalkerStatus.Stop;
			_randomIndexes = null;
			ResetTaskAudioFilePreparation();
		}

		public void Pause()
		{
			_player.Pause();
			State = TalkerStatus.Pause;
		}

		public void Continue()
		{
			_player.Play();
			State = TalkerStatus.Run;
		}

		public void Next()
		{
			if (State == TalkerStatus.Pause)
			{
				State = TalkerStatus.Run;
			}

			SpeakNext();
		}

		private SpeakPosition GetNextSpeakPosition()
		{
			SpeakPosition speakPosition = new SpeakPosition();

			while (true)
			{
				bool isEnd = false;

				if (Random)
				{
					if (_randomIndexes == null)
					{
						_randomIndexes = Enumerable.Range(0, _textBase.Count).ToList();
					}

					if (_randomIndexes.Count > 0)
					{
						speakPosition.isRandom = true;
						speakPosition.randomIndex = _random.Next(_randomIndexes.Count);
						speakPosition.textBaseIndex = _randomIndexes[speakPosition.randomIndex];
					}
					else
					{
						isEnd = true;
					}
				}
				else
				{
					speakPosition.isRandom = false;
					speakPosition.textBaseIndex = _currentTextBaseIndex + 1;
					isEnd = speakPosition.textBaseIndex >= _textBase.Count;
				}

				if (!isEnd)
				{
					return speakPosition;
				}

				if (Repeat && IsReady)
				{
					_randomIndexes = null;
					_currentTextBaseIndex = -1;
					continue;
				}

				return null;
			}
		}

		private void SpeakNext()
		{
			_player.Stop();

			if (State != TalkerStatus.Run)
			{
				return;
			}

			if (_taskAudioFilePreparation == null)
			{
				_player.Close();
				_taskAudioFilePreparation = PrepareAudioFile();
			}

			SpeakPosition speakPosition = _taskAudioFilePreparation.Result;

			if (speakPosition != null)
			{
				PlayAudio(speakPosition.filePath);

				_currentTextBaseIndex = speakPosition.textBaseIndex;
				SpeakElementChanged?.Invoke(speakPosition.textBaseIndex);

				if (speakPosition.isRandom)
				{
					_randomIndexes.RemoveAt(speakPosition.randomIndex);
				}

				_taskAudioFilePreparation = PrepareAudioFile();
			}
			else
			{
				Stop();
			}
		}

		private void ResetTaskAudioFilePreparation()
		{
			_taskAudioFilePreparation = null;
		}

		private Task<SpeakPosition> PrepareAudioFile()
		{
			SpeakPosition speakPosition = GetNextSpeakPosition();

			if (speakPosition == null)
			{
				return Task.FromResult<SpeakPosition>(null);
			}

			string text = _textBase[speakPosition.textBaseIndex].Text;

			return Task.Run(() =>
			{
				try
				{
					Swap(ref _audioFilePath, ref _audioFilePathNext);
					speakPosition.filePath = _audioFilePathNext;

					if (!SpeakToFile(speakPosition.filePath, text))
					{
						return null;
					}

					return speakPosition;
				}
				catch (System.OperationCanceledException)
				{
					return null;
				}
			});
		}

		private bool SpeakToFile(string filePath, string text)
		{
			using (MemoryStream streamAudio = new MemoryStream())
			{
				SpeakPromptToStream(BuildPrompt(text), streamAudio);

				if (Timbre != _timbreDisableValue)
				{
					Wave.WaveTimbreShifter.Shift(streamAudio.GetBuffer(), _pitchShiftCoeff);
				}

				if (!SaveStreamToFile(filePath, streamAudio))
				{
					return false;
				}
			}

			return true;
		}

		private PromptBuilder BuildPrompt(string text)
		{
			PromptBuilder prompt = new PromptBuilder();

			prompt.StartVoice(_synthesizer.Voice);
			prompt.EndVoice();

			prompt.StartSentence();
			prompt.AppendText(text);
			prompt.EndSentence();

			if (Interval > 0)
			{
				prompt.AppendBreak(TimeSpan.FromSeconds(Interval));
			}

			return prompt;
		}

		private void SpeakPromptToStream(PromptBuilder prompt, Stream stream)
		{
			_synthesizer.SetOutputToWaveStream(stream);
			_synthesizer.Speak(prompt);
			_synthesizer.SetOutputToNull();
			stream.Position = 0;
		}

		private static void SpeakStreamUsingSoundPlayer(Stream streamAudio)
		{
			SoundPlayer soundPlayer = new SoundPlayer();
			soundPlayer.Stream = streamAudio;
			soundPlayer.PlaySync();
		}

		private void PlayAudio(string filePath)
		{
			_player.Close();
			_player.Open(new Uri(filePath));
			_player.Play();
		}

		private static string GetTempAudioFile()
		{
			string filePath;
			do
			{
				filePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".wav";
			} while (File.Exists(filePath));
			return filePath;
		}

		private static void DeleteFile(string filePath)
		{
			if (filePath != null)
			{
				try
				{
					File.Delete(filePath);
				}
				catch { }
			}
		}

		private static bool SaveStreamToFile(string filePath, MemoryStream streamAudio)
		{
			try
			{
				using (FileStream audioFile = new FileStream(filePath, FileMode.Create))
					audioFile.Write(streamAudio.GetBuffer(), 0, (int)streamAudio.Length);
			}
			catch
			{
				return false;
			}

			return true;
		}

		private static int CheckInt(int value, int min, int Max)
		{
			return Math.Min(Math.Max(value, min), Max);
		}

		private static float ConvertIntToInternalFloat(int value, int minValue, int maxValue, float minInternalValue, float maxInternalValue)
		{
			return (CheckInt(value, minValue, maxValue) - minValue)
				* (maxInternalValue - minInternalValue)
				/ (maxValue - minValue) + minInternalValue;
		}

		private static int ConvertInternalFloatToInt(float value, int minValue, int maxValue, float minInternalValue, float maxInternalValue)
		{
			return (int)Math.Round((value - minInternalValue)
				* (maxValue - minValue)
				/ (maxInternalValue - minInternalValue) + minValue);
		}

		private static void Swap<T>(ref T first, ref T second)
		{
			T temp = first;
			first = second;
			second = temp;
		}
	}
}
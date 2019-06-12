using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Talker
{
	public struct TextBaseElement
	{
		public TextBaseElement(string text)
		{
			Text = text;
		}

		public string Text { get; set; }
	}

	public class TextBase : IEnumerable<TextBaseElement>
	{
		private List<TextBaseElement> textList = new List<TextBaseElement>();

		public int Count => textList.Count;

		public TextBaseElement this[int index] => textList[index];

		public event Action Loaded;

		public event Action<int> ItemDeleted;

		public bool LoadFile(string file)
		{
			try
			{
				textList.Clear();

				StreamReader stream;
				try
				{
					stream = new StreamReader(file);
				}
				catch
				{
					return false;
				}

				using (stream)
				{
					string line;
					while ((line = stream.ReadLine()) != null)
					{
						textList.Add(new TextBaseElement(line));
					}
				}

				return true;
			}
			finally
			{
				Loaded?.Invoke();
			}
		}

		public void RemoveAt(int index)
		{
			if (index >= 0 && index < Count)
			{
				textList.RemoveAt(index);
				ItemDeleted?.Invoke(index);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return textList.GetEnumerator();
		}

		IEnumerator<TextBaseElement> IEnumerable<TextBaseElement>.GetEnumerator()
		{
			return textList.GetEnumerator();
		}
	}
}
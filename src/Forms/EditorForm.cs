using System.Drawing;
using System.Windows.Forms;

namespace Talker
{
	partial class EditorForm : Form
	{
		private ITalker _talker;
		private int _lastSelectedElement = -1;

		public EditorForm()
		{
			InitializeComponent();
		}

		public void SetTalker(ITalker talker)
		{
			_talker = talker;
			_talker.SpeakElementChanged += SelectCurrentElement;
			_talker.TextBaseLoaded += LoadTextBase;
			LoadTextBase();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Visible = false;
			}
		}

		public void SelectCurrentElement(int index)
		{
			listView.Select();
			SetElementSelected(_lastSelectedElement, false);
			SetElementSelected(index, true);
			_lastSelectedElement = index;
		}

		private void SetElementSelected(int index, bool selected)
		{
			if (index >= 0 && listView.Items.Count > index)
			{
				var item = listView.Items[index];
				item.BackColor = selected ? Color.DodgerBlue : Color.White;
				item.ForeColor = selected ? Color.White : Color.Black;

				if (selected)
				{
					item.EnsureVisible();
				}
			}
		}

		private void LoadTextBase()
		{
			listView.Items.Clear();

			foreach (var element in _talker.TextBase)
			{
				listView.Items.Add(element.Text);
			}

			_lastSelectedElement = -1;
		}
	}
}
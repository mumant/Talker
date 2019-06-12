namespace Talker
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonStartStop = new System.Windows.Forms.Button();
			this.groupSettings = new System.Windows.Forms.GroupBox();
			this.labelTimbre = new System.Windows.Forms.Label();
			this.checkRepeat = new System.Windows.Forms.CheckBox();
			this.labelVolume = new System.Windows.Forms.Label();
			this.trackTimbre = new System.Windows.Forms.TrackBar();
			this.trackVolume = new System.Windows.Forms.TrackBar();
			this.labelSpeed = new System.Windows.Forms.Label();
			this.comboVoice = new System.Windows.Forms.ComboBox();
			this.labelVoice = new System.Windows.Forms.Label();
			this.checkRandom = new System.Windows.Forms.CheckBox();
			this.numericInterval = new System.Windows.Forms.NumericUpDown();
			this.labelInterval = new System.Windows.Forms.Label();
			this.trackSpeed = new System.Windows.Forms.TrackBar();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonPauseContinue = new System.Windows.Forms.Button();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItemRecentFilesSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemEditor = new System.Windows.Forms.ToolStripMenuItem();
			this.groupSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackTimbre)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackSpeed)).BeginInit();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonStartStop
			// 
			this.buttonStartStop.Location = new System.Drawing.Point(6, 162);
			this.buttonStartStop.Name = "buttonStartStop";
			this.buttonStartStop.Size = new System.Drawing.Size(78, 25);
			this.buttonStartStop.TabIndex = 0;
			this.buttonStartStop.Text = "Начать";
			this.buttonStartStop.UseVisualStyleBackColor = true;
			this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
			// 
			// groupSettings
			// 
			this.groupSettings.Controls.Add(this.labelTimbre);
			this.groupSettings.Controls.Add(this.checkRepeat);
			this.groupSettings.Controls.Add(this.labelVolume);
			this.groupSettings.Controls.Add(this.trackTimbre);
			this.groupSettings.Controls.Add(this.trackVolume);
			this.groupSettings.Controls.Add(this.labelSpeed);
			this.groupSettings.Controls.Add(this.comboVoice);
			this.groupSettings.Controls.Add(this.labelVoice);
			this.groupSettings.Controls.Add(this.checkRandom);
			this.groupSettings.Controls.Add(this.numericInterval);
			this.groupSettings.Controls.Add(this.labelInterval);
			this.groupSettings.Controls.Add(this.trackSpeed);
			this.groupSettings.Location = new System.Drawing.Point(7, 26);
			this.groupSettings.Name = "groupSettings";
			this.groupSettings.Size = new System.Drawing.Size(381, 128);
			this.groupSettings.TabIndex = 1;
			this.groupSettings.TabStop = false;
			this.groupSettings.Text = "Настройки";
			// 
			// labelTimbre
			// 
			this.labelTimbre.AutoSize = true;
			this.labelTimbre.Location = new System.Drawing.Point(130, 75);
			this.labelTimbre.Name = "labelTimbre";
			this.labelTimbre.Size = new System.Drawing.Size(43, 13);
			this.labelTimbre.TabIndex = 13;
			this.labelTimbre.Text = "Тембр:";
			// 
			// checkRepeat
			// 
			this.checkRepeat.AutoSize = true;
			this.checkRepeat.Location = new System.Drawing.Point(252, 47);
			this.checkRepeat.Name = "checkRepeat";
			this.checkRepeat.Size = new System.Drawing.Size(80, 17);
			this.checkRepeat.TabIndex = 11;
			this.checkRepeat.Text = "Повторять";
			this.checkRepeat.UseVisualStyleBackColor = true;
			// 
			// labelVolume
			// 
			this.labelVolume.AutoSize = true;
			this.labelVolume.Location = new System.Drawing.Point(257, 75);
			this.labelVolume.Name = "labelVolume";
			this.labelVolume.Size = new System.Drawing.Size(65, 13);
			this.labelVolume.TabIndex = 10;
			this.labelVolume.Text = "Громкость:";
			// 
			// trackTimbre
			// 
			this.trackTimbre.AutoSize = false;
			this.trackTimbre.Location = new System.Drawing.Point(127, 91);
			this.trackTimbre.Minimum = -10;
			this.trackTimbre.Name = "trackTimbre";
			this.trackTimbre.Size = new System.Drawing.Size(125, 32);
			this.trackTimbre.TabIndex = 12;
			this.trackTimbre.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// trackVolume
			// 
			this.trackVolume.AutoSize = false;
			this.trackVolume.Location = new System.Drawing.Point(252, 91);
			this.trackVolume.Maximum = 100;
			this.trackVolume.Name = "trackVolume";
			this.trackVolume.Size = new System.Drawing.Size(125, 32);
			this.trackVolume.TabIndex = 9;
			this.trackVolume.TickFrequency = 5;
			this.trackVolume.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.trackVolume.Value = 100;
			// 
			// labelSpeed
			// 
			this.labelSpeed.AutoSize = true;
			this.labelSpeed.Location = new System.Drawing.Point(8, 75);
			this.labelSpeed.Name = "labelSpeed";
			this.labelSpeed.Size = new System.Drawing.Size(58, 13);
			this.labelSpeed.TabIndex = 8;
			this.labelSpeed.Text = "Скорость:";
			// 
			// comboVoice
			// 
			this.comboVoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboVoice.FormattingEnabled = true;
			this.comboVoice.Location = new System.Drawing.Point(49, 45);
			this.comboVoice.Name = "comboVoice";
			this.comboVoice.Size = new System.Drawing.Size(197, 21);
			this.comboVoice.TabIndex = 6;
			// 
			// labelVoice
			// 
			this.labelVoice.AutoSize = true;
			this.labelVoice.Location = new System.Drawing.Point(6, 50);
			this.labelVoice.Name = "labelVoice";
			this.labelVoice.Size = new System.Drawing.Size(37, 13);
			this.labelVoice.TabIndex = 5;
			this.labelVoice.Text = "Голос";
			// 
			// checkRandom
			// 
			this.checkRandom.AutoSize = true;
			this.checkRandom.Location = new System.Drawing.Point(252, 17);
			this.checkRandom.Name = "checkRandom";
			this.checkRandom.Size = new System.Drawing.Size(126, 17);
			this.checkRandom.TabIndex = 4;
			this.checkRandom.Text = "Случайный порядок";
			this.checkRandom.UseVisualStyleBackColor = true;
			// 
			// numericInterval
			// 
			this.numericInterval.Location = new System.Drawing.Point(183, 16);
			this.numericInterval.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			this.numericInterval.Name = "numericInterval";
			this.numericInterval.Size = new System.Drawing.Size(63, 20);
			this.numericInterval.TabIndex = 3;
			// 
			// labelInterval
			// 
			this.labelInterval.AutoSize = true;
			this.labelInterval.Location = new System.Drawing.Point(5, 19);
			this.labelInterval.Name = "labelInterval";
			this.labelInterval.Size = new System.Drawing.Size(177, 13);
			this.labelInterval.TabIndex = 1;
			this.labelInterval.Text = "Интервал м/у предложениями (с)";
			// 
			// trackSpeed
			// 
			this.trackSpeed.AutoSize = false;
			this.trackSpeed.Location = new System.Drawing.Point(4, 91);
			this.trackSpeed.Minimum = -10;
			this.trackSpeed.Name = "trackSpeed";
			this.trackSpeed.Size = new System.Drawing.Size(125, 32);
			this.trackSpeed.TabIndex = 7;
			this.trackSpeed.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(187, 162);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(78, 25);
			this.buttonNext.TabIndex = 2;
			this.buttonNext.Text = "Следующий";
			this.buttonNext.UseVisualStyleBackColor = true;
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonPauseContinue
			// 
			this.buttonPauseContinue.Location = new System.Drawing.Point(91, 162);
			this.buttonPauseContinue.Name = "buttonPauseContinue";
			this.buttonPauseContinue.Size = new System.Drawing.Size(89, 25);
			this.buttonPauseContinue.TabIndex = 3;
			this.buttonPauseContinue.Text = "Пауза";
			this.buttonPauseContinue.UseVisualStyleBackColor = true;
			this.buttonPauseContinue.Click += new System.EventHandler(this.buttonPause_Click);
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemEditor});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(396, 24);
			this.mainMenu.TabIndex = 4;
			// 
			// menuItemFile
			// 
			this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpen,
            this.menuItemRecentFilesSeparator});
			this.menuItemFile.Name = "menuItemFile";
			this.menuItemFile.Size = new System.Drawing.Size(48, 20);
			this.menuItemFile.Text = "Файл";
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Name = "menuItemOpen";
			this.menuItemOpen.Size = new System.Drawing.Size(121, 22);
			this.menuItemOpen.Text = "Открыть";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);
			// 
			// menuItemRecentFilesSeparator
			// 
			this.menuItemRecentFilesSeparator.Name = "menuItemRecentFilesSeparator";
			this.menuItemRecentFilesSeparator.Size = new System.Drawing.Size(118, 6);
			// 
			// menuItemEditor
			// 
			this.menuItemEditor.Name = "menuItemEditor";
			this.menuItemEditor.Size = new System.Drawing.Size(49, 20);
			this.menuItemEditor.Text = "Текст";
			this.menuItemEditor.Click += new System.EventHandler(this.menuItemEditor_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(396, 194);
			this.Controls.Add(this.groupSettings);
			this.Controls.Add(this.buttonPauseContinue);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonStartStop);
			this.Controls.Add(this.mainMenu);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.mainMenu;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Talker";
			this.groupSettings.ResumeLayout(false);
			this.groupSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackTimbre)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackSpeed)).EndInit();
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonStartStop;
		private System.Windows.Forms.GroupBox groupSettings;
		private System.Windows.Forms.Label labelInterval;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonPauseContinue;
		private System.Windows.Forms.NumericUpDown numericInterval;
		private System.Windows.Forms.CheckBox checkRandom;
		private System.Windows.Forms.ComboBox comboVoice;
		private System.Windows.Forms.Label labelVoice;
		private System.Windows.Forms.TrackBar trackSpeed;
		private System.Windows.Forms.Label labelVolume;
		private System.Windows.Forms.TrackBar trackVolume;
		private System.Windows.Forms.Label labelSpeed;
		private System.Windows.Forms.CheckBox checkRepeat;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem menuItemFile;
		private System.Windows.Forms.ToolStripMenuItem menuItemOpen;
		private System.Windows.Forms.ToolStripMenuItem menuItemEditor;
		private System.Windows.Forms.ToolStripSeparator menuItemRecentFilesSeparator;
		private System.Windows.Forms.Label labelTimbre;
		private System.Windows.Forms.TrackBar trackTimbre;
	}
}


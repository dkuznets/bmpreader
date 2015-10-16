namespace BPview
{
	partial class Form1
	{
		/// <summary>
		/// Требуется переменная конструктора.
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
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.открыть3 = new System.Windows.Forms.ToolStripMenuItem();
			this.открыть1 = new System.Windows.Forms.ToolStripMenuItem();
			this.открыть2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.сервисToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolExchange = new System.Windows.Forms.ToolStripMenuItem();
			this.toolCompare = new System.Windows.Forms.ToolStripMenuItem();
			this.Stat = new System.Windows.Forms.ToolStripMenuItem();
			this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.сервисToolStripMenuItem,
            this.справкаToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(530, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// файлToolStripMenuItem
			// 
			this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открыть3,
            this.открыть1,
            this.открыть2,
            this.toolStripSeparator,
            this.выходToolStripMenuItem});
			this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
			this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.файлToolStripMenuItem.Text = "&Файл";
			// 
			// открыть3
			// 
			this.открыть3.Image = ((System.Drawing.Image)(resources.GetObject("открыть3.Image")));
			this.открыть3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.открыть3.Name = "открыть3";
			this.открыть3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
			this.открыть3.Size = new System.Drawing.Size(210, 22);
			this.открыть3.Text = "&Открыть файл(ы)";
			this.открыть3.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// открыть1
			// 
			this.открыть1.Image = ((System.Drawing.Image)(resources.GetObject("открыть1.Image")));
			this.открыть1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.открыть1.Name = "открыть1";
			this.открыть1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
			this.открыть1.Size = new System.Drawing.Size(210, 22);
			this.открыть1.Text = "&Открыть файл 1";
			this.открыть1.Visible = false;
			this.открыть1.Click += new System.EventHandler(this.открыть1_Click);
			// 
			// открыть2
			// 
			this.открыть2.Image = ((System.Drawing.Image)(resources.GetObject("открыть2.Image")));
			this.открыть2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.открыть2.Name = "открыть2";
			this.открыть2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
			this.открыть2.Size = new System.Drawing.Size(210, 22);
			this.открыть2.Text = "&Открыть файл 2";
			this.открыть2.Visible = false;
			this.открыть2.Click += new System.EventHandler(this.открыть2_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(207, 6);
			// 
			// выходToolStripMenuItem
			// 
			this.выходToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("выходToolStripMenuItem.Image")));
			this.выходToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
			this.выходToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.выходToolStripMenuItem.Text = "Вы&ход";
			this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
			// 
			// сервисToolStripMenuItem
			// 
			this.сервисToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolItem1,
            this.toolItem2,
            this.toolExchange,
            this.toolCompare,
            this.Stat});
			this.сервисToolStripMenuItem.Name = "сервисToolStripMenuItem";
			this.сервисToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.сервисToolStripMenuItem.Text = "&Сервис";
			this.сервисToolStripMenuItem.DropDownOpening += new System.EventHandler(this.сервисToolStripMenuItem_DropDownOpening);
			this.сервисToolStripMenuItem.Click += new System.EventHandler(this.сервисToolStripMenuItem_Click);
			// 
			// toolItem1
			// 
			this.toolItem1.Enabled = false;
			this.toolItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolItem1.Image")));
			this.toolItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolItem1.Name = "toolItem1";
			this.toolItem1.Size = new System.Drawing.Size(221, 22);
			this.toolItem1.Click += new System.EventHandler(this.toolItem1_Click);
			// 
			// toolItem2
			// 
			this.toolItem2.Enabled = false;
			this.toolItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolItem2.Image")));
			this.toolItem2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolItem2.Name = "toolItem2";
			this.toolItem2.Size = new System.Drawing.Size(221, 22);
			this.toolItem2.Click += new System.EventHandler(this.toolItem2_Click);
			// 
			// toolExchange
			// 
			this.toolExchange.Enabled = false;
			this.toolExchange.Image = ((System.Drawing.Image)(resources.GetObject("toolExchange.Image")));
			this.toolExchange.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolExchange.Name = "toolExchange";
			this.toolExchange.Size = new System.Drawing.Size(221, 22);
			this.toolExchange.Text = "Поменять файлы местами";
			this.toolExchange.Click += new System.EventHandler(this.toolExchange_Click);
			// 
			// toolCompare
			// 
			this.toolCompare.Enabled = false;
			this.toolCompare.Image = ((System.Drawing.Image)(resources.GetObject("toolCompare.Image")));
			this.toolCompare.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolCompare.Name = "toolCompare";
			this.toolCompare.Size = new System.Drawing.Size(221, 22);
			this.toolCompare.Text = "Сравнить файлы";
			this.toolCompare.Click += new System.EventHandler(this.toolCompare_Click);
			// 
			// Stat
			// 
			this.Stat.Enabled = false;
			this.Stat.Image = ((System.Drawing.Image)(resources.GetObject("Stat.Image")));
			this.Stat.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Stat.Name = "Stat";
			this.Stat.Size = new System.Drawing.Size(221, 22);
			this.Stat.Text = "Статистика";
			this.Stat.Click += new System.EventHandler(this.qqToolStripMenuItem_Click);
			// 
			// справкаToolStripMenuItem
			// 
			this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem});
			this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
			this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
			this.справкаToolStripMenuItem.Text = "Справка";
			// 
			// оПрограммеToolStripMenuItem
			// 
			this.оПрограммеToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("оПрограммеToolStripMenuItem.Image")));
			this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
			this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.оПрограммеToolStripMenuItem.Text = "О программе";
			this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "map";
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.ShowReadOnly = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(12, 27);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(512, 640);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(530, 676);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(546, 714);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(546, 714);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Сравнение таблиц битых пикселов";
			this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Form1_HelpButtonClicked);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem открыть1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem сервисToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ToolStripMenuItem toolItem1;
		private System.Windows.Forms.ToolStripMenuItem toolItem2;
		private System.Windows.Forms.ToolStripMenuItem toolCompare;
		private System.Windows.Forms.ToolStripMenuItem открыть2;
		private System.Windows.Forms.ToolStripMenuItem Stat;
		private System.Windows.Forms.ToolStripMenuItem открыть3;
		private System.Windows.Forms.ToolStripMenuItem toolExchange;
		private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
	}
}


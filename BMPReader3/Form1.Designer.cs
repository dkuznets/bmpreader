namespace BMPReader3
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this._stop = new System.Windows.Forms.Button();
            this._play = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this._pause = new System.Windows.Forms.Button();
            this.trb_contr1 = new System.Windows.Forms.TrackBar();
            this.tbNavigation = new System.Windows.Forms.TrackBar();
            this.rb24 = new System.Windows.Forms.RadioButton();
            this.rb48 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flip_90 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.flip_h = new System.Windows.Forms.CheckBox();
            this.flip_v = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.textB_to = new System.Windows.Forms.TextBox();
            this.textB_from = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trb_contr2 = new System.Windows.Forms.TrackBar();
            this.bt_screenshot = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cntr2 = new System.Windows.Forms.CheckBox();
            this.cntr1 = new System.Windows.Forms.CheckBox();
            this.histo_bar2 = new System.Windows.Forms.TrackBar();
            this.histo_bar1 = new System.Windows.Forms.TrackBar();
            this.cntr3 = new System.Windows.Forms.CheckBox();
            this.txt = new System.Windows.Forms.CheckBox();
            this.video = new System.Windows.Forms.CheckBox();
            this.tt_rgb = new System.Windows.Forms.ToolTip(this.components);
            this.tt_contr = new System.Windows.Forms.ToolTip(this.components);
            this.tt_flip = new System.Windows.Forms.ToolTip(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.flash_find = new System.Windows.Forms.CheckBox();
            this.cb_RA = new System.Windows.Forms.CheckBox();
            this.tb_Delay = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trb_contr1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbNavigation)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trb_contr2)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.histo_bar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.histo_bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Delay)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "bmp";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Файлы BMP|*.bmp|Все файлы|*.*";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // button1
            // 
            this.button1.AllowDrop = true;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(11, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 23);
            this.button1.TabIndex = 99;
            this.button1.TabStop = false;
            this.button1.Text = "Открыть файл";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(147, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 640);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // _stop
            // 
            this._stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this._stop.Image = ((System.Drawing.Image)(resources.GetObject("_stop.Image")));
            this._stop.Location = new System.Drawing.Point(11, 691);
            this._stop.Name = "_stop";
            this._stop.Size = new System.Drawing.Size(40, 23);
            this._stop.TabIndex = 8;
            this._stop.UseVisualStyleBackColor = true;
            this._stop.Click += new System.EventHandler(this._stop_Click);
            // 
            // _play
            // 
            this._play.Image = ((System.Drawing.Image)(resources.GetObject("_play.Image")));
            this._play.Location = new System.Drawing.Point(53, 691);
            this._play.Name = "_play";
            this._play.Size = new System.Drawing.Size(40, 23);
            this._play.TabIndex = 9;
            this._play.UseVisualStyleBackColor = true;
            this._play.Click += new System.EventHandler(this._play_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // _pause
            // 
            this._pause.Image = ((System.Drawing.Image)(resources.GetObject("_pause.Image")));
            this._pause.Location = new System.Drawing.Point(95, 691);
            this._pause.Name = "_pause";
            this._pause.Size = new System.Drawing.Size(40, 23);
            this._pause.TabIndex = 10;
            this._pause.UseVisualStyleBackColor = true;
            this._pause.Click += new System.EventHandler(this._pause_Click);
            // 
            // trb_contr1
            // 
            this.trb_contr1.AutoSize = false;
            this.trb_contr1.LargeChange = 1;
            this.trb_contr1.Location = new System.Drawing.Point(9, 35);
            this.trb_contr1.Maximum = 200;
            this.trb_contr1.Minimum = 1;
            this.trb_contr1.Name = "trb_contr1";
            this.trb_contr1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trb_contr1.Size = new System.Drawing.Size(26, 137);
            this.trb_contr1.TabIndex = 12;
            this.trb_contr1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tt_contr.SetToolTip(this.trb_contr1, "Контрастирование средствами Image");
            this.trb_contr1.Value = 25;
            this.trb_contr1.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // tbNavigation
            // 
            this.tbNavigation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNavigation.Enabled = false;
            this.tbNavigation.LargeChange = 100;
            this.tbNavigation.Location = new System.Drawing.Point(147, 693);
            this.tbNavigation.Name = "tbNavigation";
            this.tbNavigation.Size = new System.Drawing.Size(512, 45);
            this.tbNavigation.SmallChange = 100;
            this.tbNavigation.TabIndex = 25;
            this.tbNavigation.TickFrequency = 100;
            this.tbNavigation.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbNavigation.Scroll += new System.EventHandler(this.tbNavigation_Scroll);
            this.tbNavigation.ValueChanged += new System.EventHandler(this.tbNavigation_ValueChanged);
            // 
            // rb24
            // 
            this.rb24.AutoSize = true;
            this.rb24.Checked = true;
            this.rb24.Location = new System.Drawing.Point(6, 17);
            this.rb24.Name = "rb24";
            this.rb24.Size = new System.Drawing.Size(52, 17);
            this.rb24.TabIndex = 31;
            this.rb24.TabStop = true;
            this.rb24.Text = "rgb24";
            this.tt_rgb.SetToolTip(this.rb24, "Отображение 24 бит (8R+8G+8B на пиксель)");
            this.rb24.UseVisualStyleBackColor = true;
            // 
            // rb48
            // 
            this.rb48.AutoSize = true;
            this.rb48.Location = new System.Drawing.Point(65, 17);
            this.rb48.Name = "rb48";
            this.rb48.Size = new System.Drawing.Size(52, 17);
            this.rb48.TabIndex = 32;
            this.rb48.Text = "rgb48";
            this.tt_rgb.SetToolTip(this.rb48, "Отображение 48 бит (16R+16G+16B на пиксель)");
            this.rb48.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flip_90);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Controls.Add(this.flip_h);
            this.groupBox1.Controls.Add(this.flip_v);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.rb24);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rb48);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(11, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(124, 264);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Картинка";
            // 
            // flip_90
            // 
            this.flip_90.Image = ((System.Drawing.Image)(resources.GetObject("flip_90.Image")));
            this.flip_90.Location = new System.Drawing.Point(82, 70);
            this.flip_90.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.flip_90.Name = "flip_90";
            this.flip_90.Size = new System.Drawing.Size(39, 26);
            this.flip_90.TabIndex = 52;
            this.tt_flip.SetToolTip(this.flip_90, "Отражение изображения справа налево");
            this.flip_90.UseVisualStyleBackColor = true;
            this.flip_90.CheckedChanged += new System.EventHandler(this.flip_90_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(51, 244);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 13);
            this.label11.TabIndex = 51;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(51, 228);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(0, 13);
            this.label12.TabIndex = 50;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(51, 212);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(0, 13);
            this.label13.TabIndex = 49;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(51, 196);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(0, 13);
            this.label14.TabIndex = 48;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(51, 180);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 13);
            this.label15.TabIndex = 47;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(6, 244);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 13);
            this.label10.TabIndex = 46;
            this.label10.Text = "fps";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(6, 228);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 45;
            this.label9.Text = "Кадр";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(6, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 44;
            this.label6.Text = "Эксп";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(6, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Макс";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(6, 180);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 42;
            this.label8.Text = "Мин";
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new System.Drawing.Point(6, 146);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(111, 32);
            this.checkBox4.TabIndex = 41;
            this.checkBox4.Text = "Инфо на экране";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // flip_h
            // 
            this.flip_h.Image = ((System.Drawing.Image)(resources.GetObject("flip_h.Image")));
            this.flip_h.Location = new System.Drawing.Point(42, 70);
            this.flip_h.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.flip_h.Name = "flip_h";
            this.flip_h.Size = new System.Drawing.Size(40, 26);
            this.flip_h.TabIndex = 40;
            this.tt_flip.SetToolTip(this.flip_h, "Отражение изображения справа налево");
            this.flip_h.UseVisualStyleBackColor = true;
            // 
            // flip_v
            // 
            this.flip_v.Image = ((System.Drawing.Image)(resources.GetObject("flip_v.Image")));
            this.flip_v.Location = new System.Drawing.Point(6, 70);
            this.flip_v.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.flip_v.Name = "flip_v";
            this.flip_v.Size = new System.Drawing.Size(34, 26);
            this.flip_v.TabIndex = 39;
            this.tt_flip.SetToolTip(this.flip_v, "Отражение изображения сверху вниз");
            this.flip_v.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(6, 39);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(99, 32);
            this.checkBox1.TabIndex = 38;
            this.checkBox1.Text = "Перестановка пикселей";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Высота";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "Ширина";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Кадров";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 2000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // button4
            // 
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.Location = new System.Drawing.Point(4, 55);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(114, 23);
            this.button4.TabIndex = 36;
            this.button4.Text = "Сохранить";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textB_to
            // 
            this.textB_to.Location = new System.Drawing.Point(66, 32);
            this.textB_to.Name = "textB_to";
            this.textB_to.Size = new System.Drawing.Size(52, 20);
            this.textB_to.TabIndex = 38;
            this.textB_to.Text = "0";
            this.textB_to.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textB_to_KeyPress);
            // 
            // textB_from
            // 
            this.textB_from.Location = new System.Drawing.Point(4, 32);
            this.textB_from.Name = "textB_from";
            this.textB_from.Size = new System.Drawing.Size(52, 20);
            this.textB_from.TabIndex = 39;
            this.textB_from.Text = "0";
            this.textB_from.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textB_from_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textB_from);
            this.groupBox2.Controls.Add(this.textB_to);
            this.groupBox2.Location = new System.Drawing.Point(11, 310);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(124, 98);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сохранить кадры";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(4, 82);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(113, 10);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 41;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "До";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "От";
            // 
            // trb_contr2
            // 
            this.trb_contr2.AutoSize = false;
            this.trb_contr2.LargeChange = 1;
            this.trb_contr2.Location = new System.Drawing.Point(34, 35);
            this.trb_contr2.Maximum = 4000;
            this.trb_contr2.Minimum = 1;
            this.trb_contr2.Name = "trb_contr2";
            this.trb_contr2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trb_contr2.Size = new System.Drawing.Size(26, 137);
            this.trb_contr2.TabIndex = 43;
            this.trb_contr2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tt_contr.SetToolTip(this.trb_contr2, "Пороговое попиксельное контрастирование ");
            this.trb_contr2.Value = 25;
            // 
            // bt_screenshot
            // 
            this.bt_screenshot.Enabled = false;
            this.bt_screenshot.Image = ((System.Drawing.Image)(resources.GetObject("bt_screenshot.Image")));
            this.bt_screenshot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bt_screenshot.Location = new System.Drawing.Point(11, 662);
            this.bt_screenshot.Name = "bt_screenshot";
            this.bt_screenshot.Size = new System.Drawing.Size(124, 23);
            this.bt_screenshot.TabIndex = 44;
            this.bt_screenshot.Text = "Скриншот";
            this.bt_screenshot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bt_screenshot.UseVisualStyleBackColor = true;
            this.bt_screenshot.Click += new System.EventHandler(this.bt_screenshot_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cntr2);
            this.groupBox3.Controls.Add(this.cntr1);
            this.groupBox3.Controls.Add(this.histo_bar2);
            this.groupBox3.Controls.Add(this.histo_bar1);
            this.groupBox3.Controls.Add(this.trb_contr1);
            this.groupBox3.Controls.Add(this.trb_contr2);
            this.groupBox3.Controls.Add(this.cntr3);
            this.groupBox3.Location = new System.Drawing.Point(11, 412);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(124, 174);
            this.groupBox3.TabIndex = 45;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Контрастность";
            // 
            // cntr2
            // 
            this.cntr2.AutoSize = true;
            this.cntr2.Checked = true;
            this.cntr2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cntr2.Location = new System.Drawing.Point(37, 20);
            this.cntr2.Name = "cntr2";
            this.cntr2.Size = new System.Drawing.Size(29, 17);
            this.cntr2.TabIndex = 108;
            this.cntr2.Text = " ";
            this.cntr2.UseVisualStyleBackColor = true;
            this.cntr2.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // cntr1
            // 
            this.cntr1.AutoSize = true;
            this.cntr1.Checked = true;
            this.cntr1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cntr1.Location = new System.Drawing.Point(12, 20);
            this.cntr1.Name = "cntr1";
            this.cntr1.Size = new System.Drawing.Size(29, 17);
            this.cntr1.TabIndex = 107;
            this.cntr1.Text = " ";
            this.cntr1.UseVisualStyleBackColor = true;
            this.cntr1.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // histo_bar2
            // 
            this.histo_bar2.AutoSize = false;
            this.histo_bar2.LargeChange = 1;
            this.histo_bar2.Location = new System.Drawing.Point(88, 35);
            this.histo_bar2.Maximum = 2000;
            this.histo_bar2.Name = "histo_bar2";
            this.histo_bar2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.histo_bar2.Size = new System.Drawing.Size(26, 137);
            this.histo_bar2.TabIndex = 106;
            this.histo_bar2.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // histo_bar1
            // 
            this.histo_bar1.AutoSize = false;
            this.histo_bar1.LargeChange = 1;
            this.histo_bar1.Location = new System.Drawing.Point(63, 35);
            this.histo_bar1.Maximum = 16383;
            this.histo_bar1.Name = "histo_bar1";
            this.histo_bar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.histo_bar1.Size = new System.Drawing.Size(26, 137);
            this.histo_bar1.TabIndex = 105;
            this.histo_bar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.histo_bar1.Value = 16383;
            // 
            // cntr3
            // 
            this.cntr3.AutoSize = true;
            this.cntr3.Location = new System.Drawing.Point(66, 20);
            this.cntr3.Name = "cntr3";
            this.cntr3.Size = new System.Drawing.Size(49, 17);
            this.cntr3.TabIndex = 104;
            this.cntr3.Text = "Гист";
            this.cntr3.UseVisualStyleBackColor = true;
            this.cntr3.CheckedChanged += new System.EventHandler(this.cntr7_CheckedChanged);
            // 
            // txt
            // 
            this.txt.AutoSize = true;
            this.txt.Location = new System.Drawing.Point(11, 593);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(112, 17);
            this.txt.TabIndex = 104;
            this.txt.Text = "Сохранять в CSV";
            this.txt.UseVisualStyleBackColor = true;
            this.txt.CheckedChanged += new System.EventHandler(this.txt_CheckedChanged);
            // 
            // video
            // 
            this.video.AutoSize = true;
            this.video.Location = new System.Drawing.Point(11, 616);
            this.video.Name = "video";
            this.video.Size = new System.Drawing.Size(108, 17);
            this.video.TabIndex = 105;
            this.video.Text = "Сохранять в AVI";
            this.video.UseVisualStyleBackColor = true;
            this.video.CheckedChanged += new System.EventHandler(this.video_CheckedChanged);
            // 
            // tt_rgb
            // 
            this.tt_rgb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.tt_rgb.ToolTipTitle = "Переключатель режима отображения";
            // 
            // tt_contr
            // 
            this.tt_contr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.tt_contr.ToolTipTitle = "Настройка контраста";
            // 
            // tt_flip
            // 
            this.tt_flip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.tt_flip.ToolTipTitle = "Переворот изображения";
            // 
            // timer3
            // 
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // flash_find
            // 
            this.flash_find.AutoSize = true;
            this.flash_find.Location = new System.Drawing.Point(11, 639);
            this.flash_find.Name = "flash_find";
            this.flash_find.Size = new System.Drawing.Size(107, 17);
            this.flash_find.TabIndex = 107;
            this.flash_find.Text = "Поиск вспышки";
            this.flash_find.UseVisualStyleBackColor = true;
            this.flash_find.CheckedChanged += new System.EventHandler(this.flash_find_CheckedChanged);
            // 
            // cb_RA
            // 
            this.cb_RA.AutoSize = true;
            this.cb_RA.Enabled = false;
            this.cb_RA.Location = new System.Drawing.Point(147, 666);
            this.cb_RA.Name = "cb_RA";
            this.cb_RA.Size = new System.Drawing.Size(139, 17);
            this.cb_RA.TabIndex = 108;
            this.cb_RA.Text = "Разностный алгоритм";
            this.cb_RA.UseVisualStyleBackColor = true;
            this.cb_RA.CheckedChanged += new System.EventHandler(this.cb_RA_CheckedChanged);
            // 
            // tb_Delay
            // 
            this.tb_Delay.AutoSize = false;
            this.tb_Delay.LargeChange = 100;
            this.tb_Delay.Location = new System.Drawing.Point(489, 666);
            this.tb_Delay.Maximum = 1000;
            this.tb_Delay.Name = "tb_Delay";
            this.tb_Delay.Size = new System.Drawing.Size(170, 27);
            this.tb_Delay.SmallChange = 10;
            this.tb_Delay.TabIndex = 109;
            this.tb_Delay.TickFrequency = 100;
            this.tb_Delay.Value = 1000;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 722);
            this.Controls.Add(this.tb_Delay);
            this.Controls.Add(this.cb_RA);
            this.Controls.Add(this.flash_find);
            this.Controls.Add(this.txt);
            this.Controls.Add(this.video);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.bt_screenshot);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbNavigation);
            this.Controls.Add(this._pause);
            this.Controls.Add(this._play);
            this.Controls.Add(this._stop);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(810, 760);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(682, 760);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Form1_HelpButtonClicked);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.LocationChanged += new System.EventHandler(this.Form1_LocationChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trb_contr1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbNavigation)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trb_contr2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.histo_bar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.histo_bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Delay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button _stop;
        private System.Windows.Forms.Button _play;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button _pause;
        private System.Windows.Forms.TrackBar trb_contr1;
        private System.Windows.Forms.TrackBar tbNavigation;
        private System.Windows.Forms.RadioButton rb24;
        private System.Windows.Forms.RadioButton rb48;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textB_to;
        private System.Windows.Forms.TextBox textB_from;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trb_contr2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button bt_screenshot;
        private System.Windows.Forms.CheckBox flip_h;
        private System.Windows.Forms.CheckBox flip_v;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.ToolTip tt_rgb;
		private System.Windows.Forms.ToolTip tt_contr;
        private System.Windows.Forms.ToolTip tt_flip;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label10;
		private System.Windows.Forms.CheckBox flip_90;
        private System.Windows.Forms.CheckBox video;
        private System.Windows.Forms.CheckBox txt;
		private System.Windows.Forms.CheckBox cntr3;
		private System.Windows.Forms.Timer timer3;
		private System.Windows.Forms.TrackBar histo_bar1;
		private System.Windows.Forms.CheckBox flash_find;
		private System.Windows.Forms.CheckBox cb_RA;
		private System.Windows.Forms.CheckBox cntr1;
		private System.Windows.Forms.TrackBar histo_bar2;
		private System.Windows.Forms.CheckBox cntr2;
		private System.Windows.Forms.TrackBar tb_Delay;
	}
}


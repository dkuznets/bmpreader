using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using iofile = System.IO.File;
using iopath = System.IO.Path;
using newBMPLib;
using System.Drawing.Imaging;
using BPview.Resources;
using System.Diagnostics;
using AForge.Video.VFW;
using AForge.Video.DirectShow;
using System.Threading;


namespace BMPReader3
{
	public partial class Form1 : Form
	{

		#region Global vars

		FileStream fs = null;
		OLSFileHeader OLSFH;
		partOLSInfoHeader OLSIH;
		long flength;
		long num_frames = 0;
		int expo = 0;
		byte[] ImageData;
		UInt16[] ImageData16;
		UInt16[] BackData16;
		UInt16[] ra_BackData16;
		byte[] ra_ImageData;
		byte[] ra_ImageData2;
		List<uint> FlashData;

		int iwidth = 0;
		int iheight = 0;

		int dataSize;
		int dataSize16;
		long frame_counter = 0;
		float _contrastFactor = 1f;
		enum PlayState
		{
			stop,
			play,
			pause
		}

		bool savestate = true;
		bool screenshot = false;

		PlayState State = PlayState.stop;
		int _min_pix = 0, _max_pix = 0;
		int cur_minpix = 0, cur_maxpix = 0;

		UInt16[] data_save_min, data_save_max;
		UInt16[] idata;
		Byte[] idata8;

		const UInt16 RSHT = 7;

		public readonly int corr_contr = 2;

		long prevfcount = 0, fps = 0;

		String FileName = "";

		public Bitmap glav_image = null;
		public Bitmap back_glav_image = null;
		public Bitmap marker_image = null;
		public Bitmap hover_image = null;
		public Bitmap ra_image = null;

		Font drawFontHW = new Font("Microsoft Sans Serif", 10);
//        SolidBrush drawBrushHW = new SolidBrush(Color.RoyalBlue);
        SolidBrush drawBrushHW = new SolidBrush(Color.Red);

		histo2 hst2 = new histo2();
		marker mark = new marker();
		hover_window hoverW = new hover_window();
		ffw flashfindW = new ffw();
		ra raW = new ra();

		public int[] hist1 = new int[256];
		public int[] hist2 = new int[256];
		public int[] hist3 = new int[256];

		Boolean mouse_down = false;
		Boolean mouseR_down = false;
		int X1, Y1, X2, Y2, X3, Y3;
		Rectangle marker_rect;

		Boolean save_video = false;
		Boolean save_txt = false;
		FileStream save_fs = null;
		StreamWriter sw;
		int txt_frame_counter = 0;
		long maxnumsaveframe = 0;
		public AVIWriter writer;
		//public VideoFileWriter writerFF;
		String[] cdec = new String[10] { "CDVC", "CVID", "DIV5", "DIVX", "XVID", "X264", "MPG4", "WVC1", "3IVD", "AVI " };
		string avifile = String.Empty;

		List<UInt32[]> txt_list;

		Boolean nomouseclick = false;
		Boolean load_file = false;
		Boolean test_first = false;
		Boolean test_ra_first = false;
		Boolean test_ra = false;
		Boolean bigimage = false;
        public Boolean show_mark = false;

		#endregion

		#region Загрузка/выгрузка данных в структуру
		public T ReadStruct<T>(FileStream fs)
		{
			byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
			fs.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();
			return temp;
		}

		public static T BuffToStruct<T>(byte[] arr)
		{
			GCHandle gch = GCHandle.Alloc(arr, GCHandleType.Pinned);
			IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
			T ret = (T)Marshal.PtrToStructure(ptr, typeof(T));
			gch.Free();
			return default(T);
		}

		public static byte[] StructToBuff<T>(T value) where T : struct
		{
			byte[] arr = new byte[Marshal.SizeOf(value)]; // создать массив
			GCHandle gch = GCHandle.Alloc(arr, GCHandleType.Pinned); // зафиксировать в памяти
			IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0); // и взять его адрес
			Marshal.StructureToPtr(value, ptr, true); // копировать в массив
			gch.Free(); // снять фиксацию
			return arr;
		}
		#endregion

		#region Открытие/закрытие формы
		public Form1()
		{
			InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			onStop();
			_stop.Enabled = false;
			_play.Enabled = false;
			_pause.Enabled = false;
			openFileDialog1.FileName = "";
			groupBox2.Enabled = false;
			groupBox1.Enabled = false;
			groupBox3.Enabled = false;
			trb_contr1.Minimum = 0;
			trb_contr1.Maximum = 50;
			trb_contr1.Value = 25;
			trb_contr2.Minimum = 0;
			trb_contr2.Maximum = 4000;
			trb_contr2.Value = 0;

#if DEBUG
			if (Environment.CommandLine != "" && File.Exists(Environment.GetCommandLineArgs()[1]))
			{
				FileName = Environment.GetCommandLineArgs()[1];
				mouse_down = false;
				nomouseclick = true;
				openFile(FileName);
			}
#endif
			histo_bar1.Enabled = false;
			histo_bar2.Enabled = false;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (savestate)
				e.Cancel = false;
			else
			{
				onStop();
				if (fs != null)
					fs.Close();
				e.Cancel = true;
			}
		}
		#endregion

		#region flip_image
		private void flip_image()
		{
			// Перевороты главной картинки
			if (flip_v.Checked)
			{
				glav_image.RotateFlip(RotateFlipType.RotateNoneFlipY);
			}
			if (flip_h.Checked)
			{
				glav_image.RotateFlip(RotateFlipType.RotateNoneFlipX);
			}

/*			if (flip_90.Checked)
			{
				this.Width = 682 + 128;
				pictureBox1.Width = 640;
				pictureBox1.Height = 512;
				glav_image.RotateFlip(RotateFlipType.Rotate270FlipNone);
			}
			else
			{
				this.Width = 682;
				pictureBox1.Width = 512;
				pictureBox1.Height = 640;
				glav_image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
			}
 */
			// Перевороты картинки разностного алгоритма
			if (flip_v.Checked)
			{
				ra_image.RotateFlip(RotateFlipType.RotateNoneFlipY);
			}
			if (flip_h.Checked)
			{
				ra_image.RotateFlip(RotateFlipType.RotateNoneFlipX);
			}
			//if (flip_90.Checked)
			//{
			//	this.Width = 682 + 128;
			//	pictureBox1.Width = 640;
			//	pictureBox1.Height = 512;
			//	glav_image.RotateFlip(RotateFlipType.Rotate270FlipNone);
			//}
			//else
			//{
			//	this.Width = 682;
			//	pictureBox1.Width = 512;
			//	pictureBox1.Height = 640;
			//	glav_image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
			//}
		}
		#endregion

		#region ContrastImage
		private void ContrastImage()
		{
			if(checkBox2.Checked)
			{
				if (glav_image == null)
					return;
				//Bitmap bm = new Bitmap(glav_image.Width, glav_image.Height);
				Graphics g = Graphics.FromImage(glav_image);
				ImageAttributes ia = new ImageAttributes();
				ColorMatrix cm = new ColorMatrix(new float[][]{  new float[]{_contrastFactor,0f,0f,0f,0f},
								  new float[]{0f,_contrastFactor,0f,0f,0f},
								  new float[]{0f,0f,_contrastFactor,0f,0f},
								  new float[]{0f,0f,0f,1f,0f},
								  new float[]{0.001f,0.001f,0.001f,0f,1f}});
				ia.SetColorMatrix(cm);
				g.DrawImage(glav_image, new Rectangle(0, 0, glav_image.Width, glav_image.Height), 0, 0, glav_image.Width, glav_image.Height, GraphicsUnit.Pixel, ia);
				g.Dispose();
				ia.Dispose();
				}
		}
		#endregion

		#region DisplayData
		private void DisplayData()
		{
			if (checkBox4.Checked && glav_image != null)
			{
				Graphics g = Graphics.FromImage(glav_image);
				//Font drawFont = new Font("Courier", 8);
				Font drawFont = new Font("Microsoft Sans Serif", 8);
				SolidBrush drawBrush = new SolidBrush(Color.LimeGreen);
				SolidBrush drawBrush2 = new SolidBrush(Color.Aqua);
				SolidBrush drawBrushBlack = new SolidBrush(Color.Black);
				Pen drawPen = new Pen(Color.Aqua);
				SolidBrush drawBrushBG = new SolidBrush(glav_image.GetPixel(213, 2));

				g.FillRectangle(drawBrushBG, 214, 3, 37, 13);
				g.DrawRectangle(drawPen, 214, 3, 37, 13);
				switch (State)
				{
					case PlayState.stop:
						g.DrawString("Stop", drawFont, drawBrush2, new PointF(215.0F, 3.0F));
						//                        g.FillRectangle(drawBrushBG, 5, 3, 170, 25);
						break;
					case PlayState.play:
						g.DrawString("Play", drawFont, drawBrush2, new PointF(215.0F, 3.0F));
						break;
					case PlayState.pause:
						g.DrawString("Pause", drawFont, drawBrush2, new PointF(215.0F, 3.0F));
						break;
					default:
						break;
				}
				g.DrawString("Мин " + cur_minpix.ToString() + " / Макс " + cur_maxpix.ToString() + " Эксп " + expo.ToString(), drawFont, drawBrush, new PointF(5.0F, 3.0F));
				g.DrawString("Кадр " + textB_from.Text + " из " + (num_frames - 1).ToString() + " fps " + fps.ToString(), drawFont, drawBrush, new PointF(5.0F, 15.0F));
				//g.DrawString("Кадр " + frame_counter.ToString() + " из " + (num_frames - 1).ToString() + " fps " + fps.ToString(), drawFont, drawBrush, new PointF(5.0F, 15.0F));
				g.Dispose();
			}
		}
		#endregion

		#region Основная программа по таймеру
		private unsafe void main_prog()
		{
			Boolean image_bpp = false; // 0 - 8 bit color, 1 - 16 bit color
			//UInt16 t3 = 0;

			if (State == PlayState.play)
				textB_from.Text = frame_counter.ToString();

			if (State == PlayState.play || State == PlayState.pause)
			{
				fs.Seek(OLSFH.Size * frame_counter, SeekOrigin.Begin);
				OLSFH = ReadStruct<OLSFileHeader>(fs);
			}
			if (frame_counter < tbNavigation.Maximum)
				tbNavigation.Value = (int)frame_counter;

			//if (frame_counter == num_frames)
			//{
			//    video.CheckState = CheckState.Unchecked;
			//}

			int corr = trb_contr2.Value;
			if (OLSFH.Type == 0 || frame_counter == num_frames - 1)
			{
				State = PlayState.pause;
				onPause();
			}
			if (State != PlayState.stop)
			{
				expo = (int)OLSFH.TimeExposure;
				if (OLSFH.Type != 0x4D42 /* BM in ASCII */)
					throw new IOException("Прочитан неверный заголовок файла.");

				fs.Seek(OLSFH.OffBits - (UInt32)Marshal.SizeOf(typeof(OLSFileHeader)), SeekOrigin.Current);

				UInt32 nImageBytes = OLSFH.Size - OLSFH.OffBits;

				if (nImageBytes <= Int32.MaxValue)
					dataSize = Convert.ToInt32(nImageBytes);
				else
					throw new NotImplementedException();

				ImageData = new Byte[dataSize];
				fs.Read(ImageData, 0, dataSize);

				#region Создание и преобразование массивов
				ImageData16 = new UInt16[dataSize / 2];
				GCHandle gch = GCHandle.Alloc(ImageData16, GCHandleType.Pinned); // зафиксировать в памяти
				IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(ImageData16, 0); // и взять его адрес
				Marshal.Copy(ImageData, 0, ptr, ImageData.Length);
				gch.Free(); // снять фиксацию
				idata = new UInt16[dataSize / 2];
				#endregion

				Boolean pix_ex = checkBox1.Checked;

				#region перестановка пикселов
				if (pix_ex) // перестановка пикселов
				{
					UInt16 _tmp1, _tmp2;
					for (int j = 0; j < ImageData16.Length; j += 4)
					{
						_tmp1 = ImageData16[j];
						_tmp2 = ImageData16[j + 2];
						ImageData16[j] = ImageData16[j + 1];
						ImageData16[j + 2] = ImageData16[j + 3];
						ImageData16[j + 1] = _tmp1;
						ImageData16[j + 3] = _tmp2;
					}
				}
				ImageData16.CopyTo(idata, 0);
				#endregion

				#region поиск вспышки
				if (flash_find.Checked)
				{
					if (frame_counter > 1 && !test_first)
					{
						int fl = flashfindW.trackBar1.Value;
						int fl_max = flashfindW.trackBar1.Maximum;
						for (int j = 0; j < BackData16.Length; j++)
						{
//							if ((ImageData16[j] > (BackData16[j] + fl)) && (ImageData16[j] < (BackData16[j] + fl_max)))
							if (ImageData16[j] > (BackData16[j] + fl))
							{
//								flashfindW.textBox1.AppendText(frame_counter.ToString() + " " + j.ToString() + " " + ImageData16[j] + " " + BackData16[j] + "\r\n");
								flashfindW.textBox1.AppendText(frame_counter.ToString() + " " + j.ToString() + "\r\n");
								FlashData.Add((uint)j);
							}
						}
					}
					ImageData16.CopyTo(BackData16, 0);
					test_first = false;
				}
//				flashfindW.textBox1.AppendText("-- " + frame_counter.ToString() + " " + ImageData16[0] + " " + BackData16[0] + " " + idata[0] + " --\r\n");
				#endregion

				#region Разностный алгоритм
				ra_ImageData = new Byte[dataSize / 2];
				ra_ImageData2 = new Byte[dataSize / 2];

				if (cb_RA.Checked)
				{
					if (frame_counter > 1 && !test_ra_first)
					{
						for (int j = 0; j < ra_BackData16.Length; j++)
						{
							if ((ImageData16[j] - ra_BackData16[j]) <= (Math.Sqrt(ImageData16[j]) * raW.trackBar1.Value / 100))
								ra_ImageData[j] = 0;
							else
								ra_ImageData[j] = 255;
						}
						if(raW.cb_RA2P.Checked)
						{
							byte[] aaa = new byte[8];
							ra_ImageData.CopyTo(ra_ImageData2, 0);
							for (int j = 0; j < ra_ImageData.Length; j++)
							{
								if (ra_ImageData[j] == 255)
								{
									aaa[0] = (byte)(((j - 1 - OLSIH.Width) < 0) ? 0 : ra_ImageData2[j - 1 - OLSIH.Width]);
									aaa[1] = (byte)(((j - OLSIH.Width) < 0) ? 0 : ra_ImageData2[j - OLSIH.Width]);
									aaa[2] = (byte)(((j + 1 - OLSIH.Width) < 0) ? 0 : ra_ImageData2[j + 1 - OLSIH.Width]);
									aaa[3] = (byte)(((j - 1) < 0) ? 0 : ra_ImageData2[j - 1]);
									aaa[4] = (byte)(((j + 1) >= OLSIH.Width * OLSIH.Height) ? 0 : ra_ImageData2[j + 1]);
									aaa[5] = (byte)(((j - 1 + OLSIH.Width) >= OLSIH.Width * OLSIH.Height) ? 0 : ra_ImageData2[j - 1 + OLSIH.Width]);
									aaa[6] = (byte)(((j + OLSIH.Width) >= OLSIH.Width * OLSIH.Height) ? 0 : ra_ImageData2[j + OLSIH.Width]);
									aaa[7] = (byte)(((j + 1 + OLSIH.Width) >= OLSIH.Width * OLSIH.Height) ? 0 : ra_ImageData2[j + 1 + OLSIH.Width]);
									for (int k = 0; k < 8; k++)
										if (aaa[k] > 0)
										{
											ra_ImageData[j] = 0;
											break;
										}
								}
							}
						}
					}
					ImageData16.CopyTo(ra_BackData16, 0);
					test_ra_first = false;
				}
				#endregion

				#region если включена запись и режим play пишем массив и добавляем его в лист
				// если включена запись и режим play пишем массив и добавляем его в лист
				if (save_txt && State == PlayState.play)
				{
					txt_frame_counter++;
					UInt32[] txt_array = new UInt32[glav_image.Width * glav_image.Height + 2];
					txt_array[0] = (UInt32)frame_counter;
					txt_array[1] = (UInt32)expo;
					for (int k = 2; k < txt_array.Length; k++)
						txt_array[k] = ImageData16[k - 2];
					txt_list.Add(txt_array);
					//                    Array.Clear(txt_array, 0, txt_array.Length);
					if (txt_frame_counter == 256)
						txt.CheckState = CheckState.Unchecked;
				}
				#endregion

				dataSize16 = ImageData16.Length;
				//Boolean minmaxpix = (frame_counter % 1 == 0) ? true : false;

				iwidth = (int)OLSIH.Width;
				iheight = (int)OLSIH.Height;

				#region Создание картинки и переменных в соответствии с разрядностью 8 или 16 бит
				if (rb24.Checked)
				{
					image_bpp = false;
					glav_image = new Bitmap(iwidth, iheight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				}
				if (rb48.Checked)
				{
					image_bpp = true;
					glav_image = new Bitmap(iwidth, iheight, System.Drawing.Imaging.PixelFormat.Format48bppRgb);
				}

				Rectangle rect = new Rectangle(0, 0, iwidth, iheight);
				BitmapData bmpData = glav_image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, glav_image.PixelFormat);

				byte[] rgbValues8 = new byte[iwidth * iheight * 3];
				byte* curpos8 = (byte*)bmpData.Scan0;
				byte tmpVal81;

				UInt16[] rgbValues16 = new UInt16[iwidth * iheight * 3];
				UInt16* curpos16 = (UInt16*)bmpData.Scan0;
				UInt16 tmpVal16;

				ra_image = new Bitmap(iwidth, iheight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				BitmapData ra_bmpData = ra_image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, ra_image.PixelFormat);
				byte* ra_curpos8 = (byte*)ra_bmpData.Scan0;
				byte ra_tmpVal8 = 0;


				#endregion

				int _minpix = 65535, _maxpix = 0;
				UInt16 _minp = 0, _maxp = 0;

				Boolean _cntr7 = cntr7.Checked;

				UInt16 predel = 16383;

				UInt16 t1 = (UInt16)histo_bar2.Value, t2 = (UInt16)histo_bar1.Value;
				//Byte t81 = 0, t82 = (Byte)(histo_bar1.Value >> RSHT);

				#region основной цикл обработки четверной цикл
				int j1, j2, j3, j4;
				for (int j = 0; j < ImageData16.Length; j += 4)
				{
					#region Всякая обработка, контрастирование, сбор данных на гистограмму
					j1 = j;
					j2 = j + 1;
					j3 = j + 2;
					j4 = j + 3;

					if (ImageData16[j1] < _minpix)
						_minpix = ImageData16[j1];
					if (ImageData16[j1] > _maxpix)
						_maxpix = ImageData16[j1];

					if (ImageData16[j2] < _minpix)
						_minpix = ImageData16[j2];
					if (ImageData16[j2] > _maxpix)
						_maxpix = ImageData16[j2];

					if (ImageData16[j3] < _minpix)
						_minpix = ImageData16[j3];
					if (ImageData16[j3] > _maxpix)
						_maxpix = ImageData16[j3];

					if (ImageData16[j4] < _minpix)
						_minpix = ImageData16[j4];
					if (ImageData16[j4] > _maxpix)
						_maxpix = ImageData16[j4];

					UInt16 _av_minp = (UInt16)((cur_minpix + _minp) / 2);
					UInt16 _av_maxp = (UInt16)((cur_maxpix + _maxp) / 2);
					if (checkBox3.Checked)
					{
						ImageData16[j1] = (UInt16)((ImageData16[j1] - corr) * corr_contr + corr); // Контрастирование
						ImageData16[j2] = (UInt16)((ImageData16[j2] - corr) * corr_contr + corr); // Контрастирование
						ImageData16[j3] = (UInt16)((ImageData16[j3] - corr) * corr_contr + corr); // Контрастирование
						ImageData16[j4] = (UInt16)((ImageData16[j4] - corr) * corr_contr + corr); // Контрастирование
					}
					if (cntr7.Checked)
					{
						// убираем фон
						ImageData16[j1] = (UInt16)(((ImageData16[j1] - _av_minp) < 0) ? 0 : (ImageData16[j1] - _av_minp)); // Контрастирование
						ImageData16[j2] = (UInt16)(((ImageData16[j2] - _av_minp) < 0) ? 0 : (ImageData16[j2] - _av_minp)); // Контрастирование
						ImageData16[j3] = (UInt16)(((ImageData16[j3] - _av_minp) < 0) ? 0 : (ImageData16[j3] - _av_minp)); // Контрастирование
						ImageData16[j4] = (UInt16)(((ImageData16[j4] - _av_minp) < 0) ? 0 : (ImageData16[j4] - _av_minp)); // Контрастирование

						if (ImageData16[j1] < t1)
							ImageData16[j1] = t1;
						if (ImageData16[j1] > t2)
							ImageData16[j1] = t2;

						if (ImageData16[j2] < t1)
							ImageData16[j2] = t1;
						if (ImageData16[j2] > t2)
							ImageData16[j2] = t2;

						if (ImageData16[j3] < t1)
							ImageData16[j3] = t1;
						if (ImageData16[j3] > t2)
							ImageData16[j3] = t2;

						if (ImageData16[j4] < t1)
							ImageData16[j4] = t1;
						if (ImageData16[j4] > t2)
							ImageData16[j4] = t2;

						ImageData16[j1] = (UInt16)(ImageData16[j1] * (Double)predel / (t2 - t1));
						ImageData16[j2] = (UInt16)(ImageData16[j2] * (Double)predel / (t2 - t1));
						ImageData16[j3] = (UInt16)(ImageData16[j3] * (Double)predel / (t2 - t1));
						ImageData16[j4] = (UInt16)(ImageData16[j4] * (Double)predel / (t2 - t1));
					}

					#endregion
					#region Вывод данных в массив картинки
					if(test_ra)
					{
						*(ra_curpos8++) = ra_ImageData[j1];
						*(ra_curpos8++) = ra_ImageData[j1];
						*(ra_curpos8++) = ra_ImageData[j1];
						*(ra_curpos8++) = ra_ImageData[j2];
						*(ra_curpos8++) = ra_ImageData[j2];
						*(ra_curpos8++) = ra_ImageData[j2];
						*(ra_curpos8++) = ra_ImageData[j3];
						*(ra_curpos8++) = ra_ImageData[j3];
						*(ra_curpos8++) = ra_ImageData[j3];
						*(ra_curpos8++) = ra_ImageData[j4];
						*(ra_curpos8++) = ra_ImageData[j4];
						*(ra_curpos8++) = ra_ImageData[j4];
					}
					if (!image_bpp)
					{
						tmpVal81 = (byte)(((ImageData16[j1] >> RSHT) > 255) ? 255 : ImageData16[j1] >> RSHT);
						*(curpos8++) = tmpVal81;
						*(curpos8++) = tmpVal81;
						*(curpos8++) = tmpVal81;
						tmpVal81 = (byte)(((ImageData16[j2] >> RSHT) > 255) ? 255 : ImageData16[j2] >> RSHT);
						*(curpos8++) = tmpVal81;
						*(curpos8++) = tmpVal81;
						*(curpos8++) = tmpVal81;
						tmpVal81 = (byte)(((ImageData16[j3] >> RSHT) > 255) ? 255 : ImageData16[j3] >> RSHT);
						*(curpos8++) = tmpVal81;
						*(curpos8++) = tmpVal81;
						*(curpos8++) = tmpVal81;
						tmpVal81 = (byte)(((ImageData16[j4] >> RSHT) > 255) ? 255 : ImageData16[j4] >> RSHT);
						*(curpos8++) = tmpVal81;
						*(curpos8++) = tmpVal81;
						*(curpos8++) = tmpVal81;
					}
					else
					{
						//tmpVal16 = (UInt16)ImageData16[j];
						tmpVal16 = (UInt16)(ImageData16[j1] >> 1);
						*(curpos16++) = tmpVal16;
						*(curpos16++) = tmpVal16;
						*(curpos16++) = tmpVal16;
						tmpVal16 = (UInt16)(ImageData16[j2] >> 1);
						*(curpos16++) = tmpVal16;
						*(curpos16++) = tmpVal16;
						*(curpos16++) = tmpVal16;
						tmpVal16 = (UInt16)(ImageData16[j3] >> 1);
						*(curpos16++) = tmpVal16;
						*(curpos16++) = tmpVal16;
						*(curpos16++) = tmpVal16;
						tmpVal16 = (UInt16)(ImageData16[j4] >> 1);
						*(curpos16++) = tmpVal16;
						*(curpos16++) = tmpVal16;
						*(curpos16++) = tmpVal16;
					}
					#endregion

					_maxp = (UInt16)cur_maxpix;
					_minp = (UInt16)cur_minpix;
				}
				#endregion
				glav_image.UnlockBits(bmpData);
				ra_image.UnlockBits(ra_bmpData);
				cur_minpix = _minpix;
				cur_maxpix = _maxpix;


				flip_image();
				ContrastImage();

				back_glav_image = new Bitmap(glav_image);
				DisplayData();

				pictureBox1.Image = glav_image;

                if (mark.Visible)
                {
                    out_marker_image();
                }

				raW.pictureBox1.Image = ra_image;

				#region Вывод результатов поиска вспышки на картинку
				if (flash_find.Checked)
				{
					using (Graphics g = Graphics.FromImage(glav_image))
					{
						Pen drawPenFlash = new Pen(Color.Red);
//						FlashData.Sort(delegate(uint q1, uint q2)
//							{return q1.CompareTo(q2);});
//						String ss = String.Empty;
						int x = 0, y = 0;
						foreach (uint qq in FlashData)
						{
							if (qq != 0)
							{
								if (!flip_90.Checked)
								{
									y = (int)(qq / glav_image.Width);
									x = (int)(qq - y * glav_image.Width);
									if (flip_v.Checked && flip_h.Checked)
									{
										y = (int)(qq / glav_image.Width);
										x = (int)(glav_image.Width - (qq - y * glav_image.Width));
										y = (int)(glav_image.Height - y);
									}
									else
									{
										if (flip_v.Checked)
										{
											y = (int)(qq / glav_image.Width);
											x = (int)(qq - y * glav_image.Width);
											y = (int)(glav_image.Height - y);
										}
										if (flip_h.Checked)
										{
											y = (int)(qq / glav_image.Width);
											x = (int)(glav_image.Width - (qq - y * glav_image.Width));
										}
									}
								}
								else
								{
									if (flip_v.Checked && flip_h.Checked)
									{
									}
									else
									{
										if (flip_v.Checked)
										{
										}
										if (flip_h.Checked)
										{
										}
									}
								}

								if (x > 9 && y > 9 && x < (iwidth - 10) && y < (iheight - 10))
									g.DrawEllipse(drawPenFlash, x - 10, y - 10, 20, 20);
								else if (x < 10)
									x = 10;
								else if (y < 10)
									y = 10;
								else if (x > (iwidth - 10))
									x = iwidth - 10;
								else if (y > (iheight - 10))
									y = iheight - 10;
								g.DrawEllipse(drawPenFlash, x - 10, y - 10, 20, 20);
							}							
						}
						g.Dispose();
					}
				}
				#endregion

				//pictureBox1.Image = glav_image.GetThumbnailImage(pictureBox1.Width, pictureBox1.Height, null, IntPtr.Zero);

			}
			else
			{
				//onPause();
				//video.CheckState = CheckState.Unchecked;
				//glav_image = back_glav_image;
				//DisplayData();
				//pictureBox1.Image = glav_image;
			}

			label15.Text = cur_minpix.ToString();
			label14.Text = cur_maxpix.ToString();
			label13.Text = expo.ToString();
			label12.Text = frame_counter.ToString();
			if (State == PlayState.play)
				textB_from.Text = frame_counter.ToString();
			label11.Text = fps.ToString();

			#region Выделение части картинки
			// Выделение части картинки
			if (mouse_down && pictureBox1.Image != null && marker_rect.Width > 1 && marker_rect.Height > 1)
			{
				Graphics g = Graphics.FromImage(pictureBox1.Image);
				Pen drawPenMark = new Pen(Color.Red);
				g.DrawRectangle(drawPenMark, marker_rect.X - 1, marker_rect.Y - 1, marker_rect.Width + 2, marker_rect.Height + 2);
				g.Dispose();
				pictureBox1.Refresh();
			}
			#endregion
#if DEBUG
/*
			#region построение гистограммы
			// построение гистограммы
			if (histo.Checked)
			{
				if (hst2.checkBox1.Checked)
					hst2.chart1.Series["Series1"].Points.DataBindY(hist1);
				else
					hst2.chart1.Series["Series1"].Points.Clear();
				//hst2.chart1.Series["Series3"].Points.DataBindY(hist3);

				for (int i = 0; i < hist1.Length; i++)
				{
					//hist1[i] = (hist1[i] < (cur_minpix + cur_minpix / 10) || hist1[i] > (cur_maxpix - cur_maxpix / 10)) ? hist1[i] : 0;
					//hist2[i] = (hist2[i] < (cur_minpix + cur_minpix / 10) || hist2[i] > (cur_maxpix - cur_maxpix / 10)) ? hist2[i] : 0;
					//                    hist3[i] = (hist3[i] > 50) ? hist3[i] : 0;
				}
				if (hst2.checkBox2.Checked)
					hst2.chart1.Series["Series2"].Points.DataBindY(hist2);
				else
					hst2.chart1.Series["Series2"].Points.Clear();
				if (hst2.checkBox3.Checked)
					hst2.chart1.Series["Series3"].Points.DataBindY(hist3);
				else
					hst2.chart1.Series["Series3"].Points.Clear();
				hst2.chart1.DataBind();
				Array.Clear(hist1, 0, 256);
				Array.Clear(hist2, 0, 256);
				Array.Clear(hist3, 0, 256);
			}
			#endregion
 */
#endif
			#region screenshot
			if (screenshot)
			{
#if DEBUG
				pictureBox1.Image.Save(FileName.Replace(".bmp", "_" + frame_counter + ".png"), ImageFormat.Png);
#endif
				pictureBox1.Image.Save(FileName.Replace(".bmp", "_" + frame_counter + ".bmp"), ImageFormat.Bmp);
				screenshot = false;
				bt_screenshot.Enabled = true;
			}
			#endregion

			#region Сохранение видео
			if (save_video && State == PlayState.play)
			{
				if (pictureBox1.Image != null && writer != null)
					if(!cb_RA.Checked)
						writer.AddFrame((Bitmap)pictureBox1.Image);
					else
					{
						Bitmap glue_image = new Bitmap(iwidth * 2, iheight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
						Graphics g = Graphics.FromImage(glue_image);
						g.DrawImage(glav_image, new Point(0, 0));
						g.DrawImage(ra_image, new Point(iwidth, 0));
						writer.AddFrame(glue_image);
						g.Dispose();
						glue_image.Dispose();
					}
			}
			#endregion

			if (State == PlayState.play)
				frame_counter++;

		}

		#region обработка главного таймера
		private void timer1_Tick(object sender, EventArgs e)
		{
			main_prog();
			if (State == PlayState.stop)
			{
				timer1.Enabled = false;
				glav_image = back_glav_image;
				fps = 0;
				DisplayData();
				pictureBox1.Refresh();
				pictureBox1.Image = glav_image;
				pictureBox1.Refresh();
			}
			if (load_file && frame_counter == 1)
			{
				load_file = false;
				onPause();
				//frame_counter = 0;
			}
		}
		#endregion

		#endregion

		#region Кнопки Play/Pause/Stop
		private void _stop_Click(object sender, EventArgs e)
		{
			onStop();
			video.CheckState = CheckState.Unchecked;
			txt.CheckState = CheckState.Unchecked;
			flashfindW.textBox1.Clear();
			if(FlashData != null)
				FlashData.Clear();
		}

		private void _play_Click(object sender, EventArgs e)
		{
			onStart();
		}

		private void _pause_Click(object sender, EventArgs e)
		{
			onPause();
		}
		#endregion

		#region screenshot
		private void bt_screenshot_Click(object sender, EventArgs e)
		{
			if (pictureBox1.Image != null)
			{
				bt_screenshot.Enabled = false;
				screenshot = true;
			}
		}
		#endregion

		#region HELP
		private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			Form ab = new AboutBox1();
			ab.ShowDialog();
		}
		#endregion

		#region Вырезка
		private async void button4_Click(object sender, EventArgs e)
		{
			if (FileName != "")
			{
				maxnumsaveframe = Convert.ToInt32(textB_to.Text);
				FileStream cropfs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
				int len = (int)OLSFH.Size * ((int)maxnumsaveframe - Convert.ToInt32(textB_from.Text));
				cropfs.Seek(OLSFH.Size * Convert.ToInt64(textB_from.Text), SeekOrigin.Begin);

				button4.Enabled = false;
				savestate = false;

				String newname = FileName.Replace(".bmp", "_" + textB_from.Text + "-" + textB_to.Text + ".bmp");
				FileStream newfs = new FileStream(newname, FileMode.Create);
                if ((Convert.ToInt32(textB_to.Text) - Convert.ToInt32(textB_from.Text)) > 100)
                {
                    int nn = (Convert.ToInt32(textB_to.Text) - Convert.ToInt32(textB_from.Text)) / 100 + 1;
				    progressBar1.Maximum = nn - 1;
				    progressBar1.Value = 0;
				    byte[] result = new byte[OLSFH.Size * 100];
				    for (int i = 0; i < nn; i++)
				    {
					    await cropfs.ReadAsync(result, 0, (int)OLSFH.Size * 100);
					    await newfs.WriteAsync(result, 0, result.Length);
					    progressBar1.Value = i;
				    }
                }
                else
                {
                    int nn = Convert.ToInt32(textB_to.Text) - Convert.ToInt32(textB_from.Text);
                    progressBar1.Maximum = nn - 1;
                    progressBar1.Value = 0;
                    byte[] result = new byte[OLSFH.Size * nn];
                    await cropfs.ReadAsync(result, 0, (int)OLSFH.Size * nn);
                    await newfs.WriteAsync(result, 0, result.Length);
                }
				newfs.Close();
				cropfs.Close();
				MessageBox.Show("Файл сохранен под именем: \r\n" + newname, "Файл сохранен", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				savestate = true;
				button4.Enabled = true;
			}
		}
		#endregion

		#region Тракбар контраста
		private void trackBar2_Scroll(object sender, EventArgs e)
		{
			_contrastFactor = 0.04f * this.trb_contr1.Value;
		}
		#endregion

		#region tbNavigation
		private void tbNavigation_Scroll(object sender, EventArgs e)
		{
			frame_counter = tbNavigation.Value;
		}
		private void tbNavigation_ValueChanged(object sender, EventArgs e)
		{
			//MessageBox.Show("!!");
			textB_from.Text = tbNavigation.Value.ToString();
			label12.Text = tbNavigation.Value.ToString();
		}
		#endregion

		#region подсчет фпс
		private void timer2_Tick(object sender, EventArgs e)
		{
			fps = (frame_counter - prevfcount) / 2;
			prevfcount = frame_counter;
		}
		#endregion

		#region Состояния
		public void onStop()
		{
			_play.Enabled = true;
			_stop.Enabled = false;
			_pause.Enabled = false;
			State = PlayState.stop;
			frame_counter = 0;
			tbNavigation.Value = 0;
		}

		private void onStart()
		{
			if (FileName != "")
			{
				if (fs != null)
					fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
				if (frame_counter != 0)
					fs.Seek(OLSFH.Size * tbNavigation.Value, SeekOrigin.Begin);
				State = PlayState.play;
				timer1.Enabled = true;
				_play.Enabled = false;
				_stop.Enabled = true;
				_pause.Enabled = true;
			}
		}

		private void onPause()
		{
			_play.Enabled = true;
			_stop.Enabled = true;
			_pause.Enabled = false;
			State = PlayState.pause;
			textB_from.Text = frame_counter.ToString();
			//glav_image = back_glav_image;
			DisplayData();
			////pictureBox1.Image = glav_image;

		}
		#endregion

		#region Открытие файла

		#region Кнопка открытия файла
		private void button1_Click(object sender, EventArgs e)
		{
			mouse_click_OFF();
			openFileDialog1.ShowDialog();
			mouse_click_OFF();
		}
		#endregion

		#region FileDialog1_FileOk
		private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
		{
			FileName = openFileDialog1.FileName;
			mouse_down = false;
			nomouseclick = true;
			openFile(FileName);
		}
		#endregion

		#region Процедура открытия файла
		private void openFile(String FName)
		{
			if (fs != null)
				fs.Close();
			this.Text = FName;

			FileInfo fi = new FileInfo(FName);
			flength = fi.Length;
			//toolStripStatusLabel1.Text = "Файл " + fi.Name + " Длина " + Convert.ToString(flength);
			fs = new FileStream(FName, FileMode.Open, FileAccess.Read);

			OLSFH = ReadStruct<OLSFileHeader>(fs);
			OLSIH = ReadStruct<partOLSInfoHeader>(fs);

			if (OLSFH.Type != 0x4D42 /* BM in ASCII */)
				throw new IOException("Прочитан неверный заголовок файла.");

			num_frames = flength / OLSFH.Size;
			expo = (int)OLSFH.TimeExposure;

			label2.Text = "Кадров - " + num_frames.ToString();
			label3.Text = "Ширина - " + OLSIH.Width + " пикс.";
			label4.Text = "Высота - " + OLSIH.Height + " пикс.";


			data_save_min = new UInt16[num_frames + 2];
			data_save_max = new UInt16[num_frames + 2];

			fi = null;
			fs.Seek(OLSFH.OffBits, SeekOrigin.Begin);

			Int32 cx = (Int32)OLSIH.Width;
			Int32 cy = (Int32)OLSIH.Height;

			iwidth = (Int32)OLSIH.Width;
			iheight = (Int32)OLSIH.Height;


			#region Костылик для картинки 640х512
			if (iwidth == 640 && iheight == 512)
			{
				this.Width = 682 + 128;
				pictureBox1.Width = 640;
				pictureBox1.Height = 512;
				flip_90.Enabled = false;
				bigimage = true;
			}
			else
			{
				this.Width = 682;
				pictureBox1.Width = 512;
				pictureBox1.Height = 640;
				flip_90.Enabled = true;
				bigimage = false;
			}

			#endregion


			UInt32 nImageBytes = OLSFH.Size - OLSFH.OffBits;
			if (nImageBytes <= Int32.MaxValue)
				dataSize = Convert.ToInt32(nImageBytes);
			else
				throw new NotImplementedException();

			ImageData = new Byte[dataSize];
			fs.Read(ImageData, 0, dataSize);

			fs.Close();
			//            onStop();
#if DEBUG
			load_file = true;
			onStart();
#else
            onStart();
#endif
			tbNavigation.Enabled = true;

			for (UInt32 i = 1; i < dataSize - 1; i += 2)
			{
				UInt16 tmp = (UInt16)(ImageData[i + 1] + (UInt16)(ImageData[i] << 8));
				if (tmp < _min_pix)
					_min_pix = tmp;
				if (tmp > _max_pix)
					_max_pix = tmp;
			}

			tbNavigation.Minimum = 0;
			tbNavigation.Maximum = (int)(num_frames - 1);
			frame_counter = 0;

			groupBox2.Enabled = true;
			groupBox1.Enabled = true;
			groupBox3.Enabled = true;
			bt_screenshot.Enabled = true;

			cb_RA.Enabled = true;
			
			textB_from.Text = (frame_counter).ToString();
			textB_to.Text = (num_frames - 1).ToString();

			mouse_down = false;
			X1 = 0;
			Y1 = 0;
			X2 = 0;
			Y2 = 0;
			mouse_click_ON();
			nomouseclick = false;
		}
		#endregion

		#endregion

		#region Мультимедиа-клава
		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.MediaPlayPause:
					if (State == PlayState.stop || State == PlayState.pause)
						onStart();
					else if (State == PlayState.play)
						onPause();
					break;
				case Keys.MediaStop:
					onStop();
					break;
				case Keys.MediaNextTrack:
					frame_counter += num_frames / 20;
					if (frame_counter > num_frames)
						frame_counter = num_frames;
					break;
				case Keys.MediaPreviousTrack:
					frame_counter -= num_frames / 20;
					if (frame_counter < 0)
						frame_counter = 0;
					break;
				case Keys.Space:
					if (State == PlayState.stop || State == PlayState.pause)
						onStart();
					else if (State == PlayState.play)
						onPause();
					break;
			}
		}
		#endregion

		#region Обработка drag'n'drop
		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			string[] item = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (item[0].EndsWith(".bmp"))
				openFile(FileName = item[0]);
			//MessageBox.Show(item[0]);
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("FileDrop"))
				e.Effect = DragDropEffects.Link;
		}
		#endregion

		#region маленькая картинка с увеличением
		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			#region обработка правой кнопки
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if (pictureBox1.Image != null)
				{
					mouseR_down = true;
					if (bigimage)
					{
						X3 = e.X;
						Y3 = e.Y;
					}
					else
					{
						X3 = e.X / 2;
						Y3 = e.Y / 2;
					}
					hoverW.StartPosition = FormStartPosition.Manual;
					hoverW.Location = new Point(Cursor.Position.X, Cursor.Position.Y);

					hover_image = new Bitmap(10, 10);
					using (Graphics g = Graphics.FromImage(hover_image))
					{
						g.DrawImage(pictureBox1.Image, 0, 0, new Rectangle(0, 0, 100, 100), GraphicsUnit.Pixel);
					}
					hoverW.pictureBox1.Image = hover_image.GetThumbnailImage(hoverW.pictureBox1.Width, hoverW.pictureBox1.Height, null, IntPtr.Zero);

					out_line_text_hover_win();

					hoverW.Show();
					hoverW.BringToFront();
					this.Cursor = System.Windows.Forms.Cursors.Cross;
				}
			}
			#endregion
			#region обработка левой кнопки
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (pictureBox1.Image != null)
				{
					if(bigimage)
					{
						X1 = e.X;
						Y1 = e.Y;
					}
					else
					{
						X1 = e.X / 2;
						Y1 = e.Y / 2;
					}
					mouse_down = true;
                    marker_rect = Rectangle.Empty;
                }
			}
			#endregion
		}

		#region перекрестие и яркость на hover
		private void out_line_text_hover_win()
		{
			using (Graphics g = Graphics.FromImage(hoverW.pictureBox1.Image))
			{
				Pen drawPenMark2 = new Pen(Color.Indigo);
				g.DrawLine(drawPenMark2, 50, 20, 50, 80);
				g.DrawLine(drawPenMark2, 0, 50, 100, 50);
				StringFormat sf = new StringFormat(StringFormatFlags.NoClip);
				sf.Alignment = StringAlignment.Center;
				X3 = (X3 < 0) ? 0 : X3;
				X3 = (X3 >= glav_image.Width) ? glav_image.Width - 1 : X3;
				Y3 = (Y3 < 0) ? 0 : Y3;
				Y3 = (Y3 >= glav_image.Height) ? glav_image.Height - 1 : Y3;
				int hX, hY, hH, hW;

				if (!flip_90.Checked)
				{
					hH = glav_image.Height - 1;
					hW = glav_image.Width - 1;
					hX = X3;
					hY = Y3;

					if (flip_v.Checked && flip_h.Checked)
					{
						hX = hW - X3;
						hY = hH - Y3;
					}
					else
					{
						if (flip_v.Checked)
						{
							hX = X3;
							hY = hH - Y3;
						}
						if (flip_h.Checked)
						{
							hX = hW - X3;
							hY = Y3;
						}
					}
				}
				else
				{
					hH = glav_image.Height - 1;
					hW = glav_image.Width - 1;
					hX = hH - Y3;
					hY = X3;
					if (flip_v.Checked && flip_h.Checked)
					{
						hY = hW - X3;
						hX = Y3;
					}
					else
					{
						if (flip_v.Checked)
						{
							hX = hH - Y3;
							hY = hW - X3;
						}
						if (flip_h.Checked)
						{
							hY = X3;
							hX = Y3;
						}
					}
				}
	
				g.DrawString("L=" + idata[hY * iwidth + hX].ToString(), drawFontHW, drawBrushHW, new PointF(50.0F, 80.0F), sf);
				g.DrawString("N=" + (hY * iwidth + hX).ToString(), drawFontHW, drawBrushHW, new PointF(50.0F, 0.0F), sf);
				//g.DrawString("X3=" + X3.ToString() + " Y3=" + Y3.ToString(), drawFontHW, drawBrushHW, new PointF(50.0F, 35.0F), sf);
				//g.DrawString("hX=" + hX.ToString() + " hY=" + hY.ToString(), drawFontHW, drawBrushHW, new PointF(50.0F, 50.0F), sf);
				g.Dispose();
			}
		}
		#endregion

		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			#region обработка правой кнопки
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				mouseR_down = false;
				hoverW.Hide();
				this.Cursor = System.Windows.Forms.Cursors.Default;
			}
			#endregion
			#region обработка левой кнопки
			if (pictureBox1.Image == null || nomouseclick || e.Button != System.Windows.Forms.MouseButtons.Left)
				return;
			if (bigimage)
			{
				X2 = e.X;
				Y2 = e.Y;
			}
			else
			{
				X2 = e.X / 2;
				Y2 = e.Y / 2;
			}
			if (marker_rect.Width > 0 && marker_rect.Height > 0)
			{
                out_marker_image();
/*
                marker_image = new Bitmap(marker_rect.Width, marker_rect.Height);
                using (Graphics g = Graphics.FromImage(marker_image))
                {
                    g.DrawImage(pictureBox1.Image, 0, 0, marker_rect, GraphicsUnit.Pixel);
                }

				int resize = 8;
				if (marker_rect.Width < 20 || marker_rect.Height < 20)
					resize = 16;
				if (marker_rect.Width > 100 || marker_rect.Height > 100)
					resize = 4;
				mark.Text = "Увеличение х" + (resize / 2).ToString();

				// масштабируем размер окна
				mark.pictureBox1.Width = marker_image.Width * resize;
				mark.pictureBox1.Height = marker_image.Height * resize;
				mark.Width = marker_image.Width * resize + 18;
				if (mark.Width < 130)
					mark.Width = 130;
				mark.Height = marker_image.Height * resize + 76;

				// масштабируем картинку под размер окна
                mark.pictureBox1.Image = marker_image.GetThumbnailImage(mark.pictureBox1.Width, mark.pictureBox1.Height, null, IntPtr.Zero);
*/
				// позиционирование окна
				this.StartPosition = FormStartPosition.Manual;
				Point form1_def = new Point();
				form1_def = this.Location;
				this.StartPosition = FormStartPosition.CenterScreen;
				mark.Location = new Point(form1_def.X + 10, form1_def.Y + 10);
				mark.Show();
                show_mark = true;
				mark.BringToFront();
/*
				UInt16 _minpix = 65000, _maxpix = 0;

				int mX = marker_rect.X, mY = marker_rect.Y;
				int mH = marker_rect.Height, mW = marker_rect.Width;
				int W = glav_image.Width, H = glav_image.Height;

				if (!flip_90.Checked)
				{
					if (flip_v.Checked && flip_h.Checked)
					{
						mX = W - marker_rect.X - mW;
						mY = H - marker_rect.Y - mH;
					}
					else
					{
						if (flip_v.Checked)
						{
							mX = marker_rect.X;
							mY = H - marker_rect.Y - mH;
						}
						if (flip_h.Checked)
						{
							mX = W - marker_rect.X - mW;
							mY = marker_rect.Y;
						}
					}
					for (int i = mY; i < mY + mH; i++)
						for (int j = mX; j < mX + mW; j++)
						{
							if (idata[i * W + j] < _minpix)
								_minpix = idata[i * W + j];
							if (idata[i * W + j] > _maxpix)
								_maxpix = idata[i * W + j];
						}
					mark.label4.Text = _minpix.ToString();
					mark.label3.Text = _maxpix.ToString();
				}
				else
				{
					mark.label4.Text = "Нет";
					mark.label3.Text = "Нет";
				}
 */
			}
			else
			{
				//MessageBox.Show("X = " + X1.ToString() + " Y = " + Y1.ToString() + "\r\n" +
				//                "Номер = " + (Y1 * 256 + X1).ToString() + " Яркость = " + idata[Y1 * 256 + X1].ToString());
			}
			mouse_down = false;
			//marker_rect = Rectangle.Empty;
			#endregion
		}

		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			#region обработка правой кнопки
			if (e.Button == System.Windows.Forms.MouseButtons.Right && pictureBox1.Image != null)
			{
				if (bigimage)
				{
					X3 = e.X;
					Y3 = e.Y;
				}
				else
				{
					X3 = e.X / 2;
					Y3 = e.Y / 2;
				}
				hover_image = new Bitmap(10, 10);
				using (Graphics g = Graphics.FromImage(hover_image))
				{
					g.DrawImage(pictureBox1.Image, 0, 0, new Rectangle(X3 - 5, Y3 - 5, 10, 10), GraphicsUnit.Pixel);
				}
				hoverW.pictureBox1.Image = hover_image.GetThumbnailImage(hoverW.pictureBox1.Width, hoverW.pictureBox1.Height, null, IntPtr.Zero);
				out_line_text_hover_win();

				hoverW.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
				hoverW.Show();
			}
			#endregion
			#region обработка левой кнопки
			if (e.Button == System.Windows.Forms.MouseButtons.Left && pictureBox1.Image != null)
			{
				if (bigimage)
				{
					int rX1 = X1, rX2 = e.X, rY1 = Y1, rY2 = e.Y;

					if (rX2 >= glav_image.Width)
						rX2 = glav_image.Width - 2;
					if (rY2 >= glav_image.Height)
						rY2 = glav_image.Height - 2;
					if (rX2 < 0)
						rX2 = 2;
					if (rY2 < 0)
						rY2 = 2;

					int rW = Math.Abs(rX2 - X1), rH = Math.Abs(rY2 - Y1), rX = X1, rY = Y1;
					if (X1 > e.X)
						rX = rX1 - rW;
					if (Y1 > e.Y)
						rY = rY1 - rH;
					marker_rect = new Rectangle(rX, rY, rW, rH);
				}
				else
				{
					int rX1 = X1, rX2 = e.X / 2, rY1 = Y1, rY2 = e.Y / 2;

					if (rX2 >= glav_image.Width)
						rX2 = glav_image.Width - 2;
					if (rY2 >= glav_image.Height)
						rY2 = glav_image.Height - 2;
					if (rX2 < 0)
						rX2 = 2;
					if (rY2 < 0)
						rY2 = 2;

					int rW = Math.Abs(rX2 - X1), rH = Math.Abs(rY2 - Y1), rX = X1, rY = Y1;
					if (X1 > e.X / 2)
						rX = rX1 - rW;
					if (Y1 > e.Y / 2)
						rY = rY1 - rH;
					marker_rect = new Rectangle(rX, rY, rW, rH);
				}
			}
			#endregion
		}
		#endregion

		#region проверка установленных кодеков и создание видеофала
		private void video_CheckedChanged(object sender, EventArgs e)
		{
			if (pictureBox1.Image == null)
			{
				video.CheckState = CheckState.Unchecked;
				return;
			}
			if (video.Checked)
			{
				set_codecs();
				save_video = true;
			}
			else
			{
				save_video = false;
				writer.Close();
				if (writer.Codec != "DIB ")
					MessageBox.Show("Имя видеофайла: " + avifile + "\r\n\r\nКодек: " + writer.Codec, "Сохранение видео", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				else
					MessageBox.Show("Имя видеофайла: " + avifile + "\r\n\r\nКодек: не установлен" + "\r\n\r\nФайл сохранен без сжатия!!!", "Сохранение видео", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				avifile = String.Empty;
			}
		}
		private void set_codecs()
		{
			writer = new AVIWriter();
			writer.Quality = 100;
			int detect = 99;
			avifile = NewFileName(FileName, ".avi");
			for (int i = 0; i < 10; i++)
			{
				writer.Codec = cdec[i];
				try
				{
					if(!cb_RA.Checked)
						writer.Open(avifile, pictureBox1.Image.Width, pictureBox1.Image.Height);
					else
						writer.Open(avifile, pictureBox1.Image.Width * 2, pictureBox1.Image.Height);
					writer.Close();
					detect = i;
					break;
				}
				catch (Exception)
				{
				}
				finally
				{
					writer.Close();
				}
			}

			if (detect == 99)
				writer.Codec = "DIB ";
			else
				writer.Codec = cdec[detect];

			//MessageBox.Show(detect.ToString() + "-" + writer.Codec);
			if (!cb_RA.Checked)
				writer.Open(avifile, pictureBox1.Image.Width, pictureBox1.Image.Height);
			else
				writer.Open(avifile, pictureBox1.Image.Width * 2, pictureBox1.Image.Height);
		}
		#endregion

		#region ручная установка начального кадра с проверкой валидности
		private void textB_from_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				if (long.TryParse(textB_from.Text, out frame_counter))
				{
					if (frame_counter > num_frames - 1)
					{
						frame_counter = 1;
						textB_from.Text = "1";
					}
				}
				else
				{
					textB_from.Text = "1";
					frame_counter = 1;
				}
			}
		}
		#endregion

		#region ручная установка конечного кадра с проверкой валидности
		private void textB_to_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				if (long.TryParse(textB_to.Text, out maxnumsaveframe))
				{
					if (maxnumsaveframe > num_frames - 1)
					{
						maxnumsaveframe = num_frames - 1;
						textB_to.Text = (num_frames - 1).ToString();
					}
				}
				else
				{
					maxnumsaveframe = num_frames - 1;
					textB_to.Text = (num_frames - 1).ToString();
				}
			}
		}
		#endregion

		#region сохранение данных в csv
		//private async void txt_CheckedChanged(object sender, EventArgs e)
		private void txt_CheckedChanged(object sender, EventArgs e)
		{
			String nfn;
			if (pictureBox1.Image == null)
			{
				txt.CheckState = CheckState.Unchecked;
				return;
			}
			if (txt.Checked)
			{
				//txt_array = 
				txt_list = new List<UInt32[]>();
				save_txt = true;
				txt_frame_counter = 0;
			}
			else
			{
				txt.Text = "Идет сохранение...";
				txt.Enabled = false;
				nfn = NewFileName(FileName, ".csv");
				save_txt = false;
				String sss = String.Empty;
				save_fs = new FileStream(nfn, FileMode.Create, FileAccess.Write);
				sw = new StreamWriter(save_fs);
				for (int i = 0; i < glav_image.Width * glav_image.Height + 2; i++)
				{
					sss = String.Empty;
					foreach (UInt32[] item in txt_list)
						sss += item[i].ToString() + ";";
					sss += "\r\n";
					sw.Write(sss);
					//await sw.WriteAsync(sss);
				}
				sw.Close();
				save_fs.Close();
				save_txt = false;
				MessageBox.Show("Имя файла: " + nfn +
								"\r\n\r\nКадров сохранено: " + txt_frame_counter.ToString() +
								"\r\n\r\nВнимание! Для сохранения совместимости с версиями " +
								"\r\nMS Office 2007 и ниже, количество кадров ограничено 256!" +
								"\r\n\r\nФормат таблицы: \r\n1-я строка - номер кадра" +
								"\r\n2-я строка - экспозиция" +
								"\r\nследующие строки - пиксели по порядку",
								"Сохранение данных в файл csv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txt.Enabled = true;
				txt.Text = "Сохранять в CSV";
			}
		}
		#endregion

		#region создание нового имени файла из оригинального открытого с учетом дублей
		private String NewFileName(String fullfilename, String newextention)
		{
			if (!iofile.Exists(fullfilename.Replace(".bmp", "_00" + newextention)))
				return fullfilename.Replace(".bmp", "_00" + newextention);
			DirectoryInfo dir = new DirectoryInfo(iopath.GetDirectoryName(fullfilename));
			String lastname;
			int lastnum = 0;
			String sss = "";
			foreach (FileInfo item in dir.GetFiles("*" + newextention))
			{
				//MessageBox.Show(item.FullName);
				if (item.FullName.StartsWith(fullfilename.Replace(".bmp", "")))
				{
					lastname = item.FullName;
					lastnum = Convert.ToInt32(lastname.Replace(fullfilename.Replace(".bmp", ""), "").Replace(newextention, "").Remove(0, 1));
					sss += lastname + "\r\n";
				}
			}
			//MessageBox.Show(sss + "\r\n" + (lastnum + 1).ToString());
			return fullfilename.Replace(".bmp", "") + "_" + (lastnum + 1).ToString("D2") + newextention;
		}
		#endregion

		#region Гистограмма
/*		private void cntr4_CheckedChanged(object sender, EventArgs e)
		{
			if (histo.Checked)
			{
				hst2.Show(this);
			}
			else
			{
				hst2.Hide();
			}
		}
		public Bitmap equalizing(Bitmap _Bmp)
		{
			Rectangle rect = new Rectangle(0, 0, _Bmp.Width, _Bmp.Height);
			System.Drawing.Imaging.BitmapData bmpData = _Bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, _Bmp.PixelFormat);
			IntPtr ptr = bmpData.Scan0;
			int bytes = bmpData.Stride * _Bmp.Height;
			byte[] grayValues = new byte[bytes];
			int[] R = new int[256];
			byte[] N = new byte[256];
			byte[] left = new byte[256];
			byte[] right = new byte[256];
			System.Runtime.InteropServices.Marshal.Copy(ptr, grayValues, 0, bytes);

			for (int i = 0; i < grayValues.Length; i++)
			{
				++R[grayValues[i]];
			}

			byte z = 0;
			int Hint = 0;
			int Havg = grayValues.Length / R.Length;
			for (int i = 0; i < N.Length - 1; i++)
			{
				N[i] = 0;
			}
			for (int j = 0; j < R.Length; j++)
			{
				left[j] = z;
				Hint += R[j];
				while (Hint > Havg)
				{
					Hint -= Havg;
					z++;
				}
				right[j] = z;
				N[j] = (byte)((left[j] + right[j]) / 2);
			}
			for (int i = 0; i < grayValues.Length; i++)
			{
				if (left[grayValues[i]] == right[grayValues[i]])
					grayValues[i] = (byte)left[grayValues[i]];
				else
					grayValues[i] = (byte)N[grayValues[i]];
			}

			System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, ptr, bytes);
			_Bmp.UnlockBits(bmpData);
			return _Bmp;
		}
 */
		#endregion

		#region Отмена и восстановление подписки на события кликов pictureBox1
		private void mouse_click_OFF()
		{
			this.pictureBox1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
		}
		private void mouse_click_ON()
		{
			timer3.Enabled = true;
		}

		private void timer3_Tick(object sender, EventArgs e)
		{
			timer3.Enabled = false;
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
		}
		#endregion

		#region Контрастирование по гистограмме
		private void cntr7_CheckedChanged(object sender, EventArgs e)
		{
			if (cntr7.Checked)
			{
				histo_bar1.Enabled = true;
//				histo_bar1.Value = histo_bar1.Maximum;
//				trb_contr2.Enabled = false;
				histo_bar2.Enabled = true;
			}
			else
			{
				histo_bar1.Enabled = false;
				histo_bar2.Enabled = false;
//				histo_bar1.Value = histo_bar1.Maximum;
//				trb_contr2.Enabled = true;
			}
		}
		#endregion

		#region окно Поиск вспышки
		private void flash_find_CheckedChanged(object sender, EventArgs e)
		{
			if (flash_find.Checked)
			{
				onStop();
				Point form1_def = new Point();
				form1_def = this.Location;
				flashfindW.Location = new Point(form1_def.X + this.Width, form1_def.Y);
				flashfindW.Show(this);
				flashfindW.BringToFront();
				flashfindW.label2.Text = flashfindW.trackBar1.Value.ToString();
				test_first = true;
				flashfindW.textBox1.Clear();
				BackData16 = new UInt16[dataSize / 2];
				FlashData = new List<uint>();
			}
			else
			{
				flashfindW.Hide();
			}
		}
		private void Form1_LocationChanged(object sender, EventArgs e)
		{
			flashfindW.Location = new Point(this.Location.X + this.Width, this.Location.Y);
			raW.Location = new Point(this.Location.X + this.Width, this.Location.Y);
		}

		private void flip_90_CheckedChanged(object sender, EventArgs e)
		{
			if(!flip_90.Checked)
				flashfindW.Location = new Point(this.Location.X + 682, this.Location.Y);
			else
				flashfindW.Location = new Point(this.Location.X + 810, this.Location.Y);
		}

		#endregion

		#region окно Разностный алгоритм
		private void cb_RA_CheckedChanged(object sender, EventArgs e)
		{
			if (cb_RA.Checked)
			{
				onStop();
				if (FileName != "")
					_play.Enabled = true;
				else
					_play.Enabled = false;
				Point form1_def = new Point();
				form1_def = this.Location;
				raW.Location = new Point(form1_def.X + this.Width, form1_def.Y);
				raW.Show(this);
				raW.BringToFront();
				test_ra_first = true;
				ra_BackData16 = new UInt16[dataSize / 2];
				test_ra = true;
			}
			else
			{
				raW.Hide();
				test_ra = false;
			}
		}
		#endregion

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox2.Checked)
				trb_contr1.Enabled = true;
			else
				trb_contr1.Enabled = false;
		}

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                trb_contr2.Enabled = true;
            else
                trb_contr2.Enabled = false;
        }

        private void out_marker_image()
        {
            if (marker_rect.Width > 0 && marker_rect.Height > 0)
            {
                marker_image = new Bitmap(marker_rect.Width, marker_rect.Height);
                using (Graphics g = Graphics.FromImage(marker_image))
                {
                    g.DrawImage(pictureBox1.Image, 0, 0, marker_rect, GraphicsUnit.Pixel);
                }

                int resize = 8;
                if (marker_rect.Width < 20 || marker_rect.Height < 20)
                    resize = 16;
                if (marker_rect.Width > 100 || marker_rect.Height > 100)
                    resize = 4;
                mark.Text = "Увеличение х" + (resize / 2).ToString();

                // масштабируем размер окна
                mark.pictureBox1.Width = marker_image.Width * resize;
                mark.pictureBox1.Height = marker_image.Height * resize;
                mark.Width = marker_image.Width * resize + 18;
                if (mark.Width < 130)
                    mark.Width = 130;
                mark.Height = marker_image.Height * resize + 76;

                // масштабируем картинку под размер окна
                mark.pictureBox1.Image = marker_image.GetThumbnailImage(mark.pictureBox1.Width, mark.pictureBox1.Height, null, IntPtr.Zero);

                UInt16 _minpix = 65000, _maxpix = 0;

                int mX = marker_rect.X, mY = marker_rect.Y;
                int mH = marker_rect.Height, mW = marker_rect.Width;
                int W = glav_image.Width, H = glav_image.Height;

                if (!flip_90.Checked)
                {
                    if (flip_v.Checked && flip_h.Checked)
                    {
                        mX = W - marker_rect.X - mW;
                        mY = H - marker_rect.Y - mH;
                    }
                    else
                    {
                        if (flip_v.Checked)
                        {
                            mX = marker_rect.X;
                            mY = H - marker_rect.Y - mH;
                        }
                        if (flip_h.Checked)
                        {
                            mX = W - marker_rect.X - mW;
                            mY = marker_rect.Y;
                        }
                    }
                    for (int i = mY; i < mY + mH; i++)
                        for (int j = mX; j < mX + mW; j++)
                        {
                            if (idata[i * W + j] < _minpix)
                                _minpix = idata[i * W + j];
                            if (idata[i * W + j] > _maxpix)
                                _maxpix = idata[i * W + j];
                        }
                    mark.label4.Text = _minpix.ToString();
                    mark.label3.Text = _maxpix.ToString();
                }
                else
                {
                    mark.label4.Text = "Нет";
                    mark.label3.Text = "Нет";
                }
            }
        }


	}

}

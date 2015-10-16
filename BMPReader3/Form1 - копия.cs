using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using newBMPLib;
using System.Drawing.Imaging;

namespace BMPReader
{
	public partial class Form1 : Form
	{
		FileStream fs;
		OLSFileHeader OLSFH;
		partOLSInfoHeader OLSIH;
		long flength;
		UInt64 N_Time;
		UInt64 framesCounter;
		byte[] ImageData;
        Int32 dataSize;
        int frame_counter = 0;
        float _contrastFactor = 1f;
        float _saturation = 1;
//        Boolean play = false;
        float _brightness = 0.5F;
		//float mulR = 0.5F, mulG = 0.5F, mulB = 0.5F;
        enum PlayState {stop, play, pause}

        PlayState State = PlayState.stop;
        UInt16 _min_pix = 65535, _max_pix = 0;
        UInt32 counter = 0;
        public Form1()
		{
			InitializeComponent();
		}

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

 		private void button1_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
				return;

            toolStripStatusLabel3.Text = "Stop";
            timer1.Enabled = false;
            if (State != PlayState.stop)
            {
                _play.Enabled = true;
                State = PlayState.stop;
                fs.Close();
            }

            _play.Enabled = true;
			tbNavigation.Enabled = true;

			toolStripStatusLabel1.Text = openFileDialog1.FileName;
			FileInfo fi = new FileInfo(openFileDialog1.FileName);
			flength = fi.Length;
			toolStripStatusLabel1.Text = "Файл " + fi.Name + " Длина " + Convert.ToString(flength);
			fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
			
			OLSFH = ReadStruct<OLSFileHeader>(fs);
			OLSIH = ReadStruct<partOLSInfoHeader>(fs);

            if (OLSFH.Type != 0x4D42 /* BM in ASCII */)
                throw new IOException("Прочитан неверный заголовок файла.");

			Console.WriteLine("FileName = {0}\nFileLength = {1}\n\n", fi.Name, fi.Length);
			Console.WriteLine("Size = {0}\nExp = {1}\nOffset = {2}\n", OLSFH.Size, OLSFH.TimeExposure, OLSFH.OffBits);
			Console.WriteLine("Size = {0}\nWidth = {1}\nHeight = {2}\nN_Time = {3}\nframesCounter = {4}\n", OLSIH.Size, OLSIH.Width, OLSIH.Height, OLSIH.N_Time, OLSIH.framesCounter);
			Console.WriteLine("NumCadr = {0}\n", Convert.ToString(flength / OLSFH.Size));

			toolStripStatusLabel1.Text += " Кадров " + Convert.ToString(flength / OLSFH.Size);
			fi = null;
			//fs.Seek((long)Math.Floor((Double)(flength / OLSFH.Size)) * OLSFH.Size - OLSFH.Size, SeekOrigin.Begin);
			//OLSFH = ReadStruct<OLSFileHeader>(fs);
			//OLSIH = ReadStruct<partOLSInfoHeader>(fs);
			//Console.WriteLine("Last cadr:\nN_Time2 = {0}\nframesCounter2 = {1}\nFrames = {2}\n\n\n", OLSIH.N_Time, OLSIH.framesCounter, OLSIH.framesCounter - framesCounter + 1);

			//long totalHeaderSize = Marshal.SizeOf(typeof(OLSFileHeader)) + Marshal.SizeOf(typeof(OLSInfoHeader));
			long totalHeaderSize = OLSFH.OffBits;

			UInt32 nBytesPass = OLSFH.OffBits - (UInt32)totalHeaderSize;
			fs.Seek(OLSFH.OffBits, SeekOrigin.Begin);

			Int32 cx = (Int32)OLSIH.Width;
			Int32 cy = (Int32)OLSIH.Height;

			UInt32 nImageBytes = OLSFH.Size - OLSFH.OffBits;
//			Int32 dataSize;
			if (nImageBytes <= Int32.MaxValue)
				dataSize = Convert.ToInt32(nImageBytes);
			else throw new NotImplementedException();

			//byte[] data = br.ReadBytes(dataSize);
			ImageData = new Byte[dataSize];
			fs.Read(ImageData, 0, dataSize);

			fs.Close();
            CalcImage();
			tbNavigation.Minimum = 0;
			tbNavigation.Maximum = (int)(flength / OLSFH.Size);
			frame_counter = 0;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(fs != null)
				fs.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//textBox1.Text += "\r\n======  " + Convert.ToString(Marshal.SizeOf(typeof(T))) + " === " + Convert.ToString(buffer.Length) + " ======\r\n";
			for (int i = 0; i < 100; i++)
				textBox1.Text += String.Format("{0,2:D2} - 0x{1,2:X2} - 0x{2,2:X2}\r\n", i, i, ImageData[i]);

		}

		private void button3_Click(object sender, EventArgs e)
		{
            UInt16 qq = 1000;
            UInt16 ww = 2000;
            MessageBox.Show((qq - ww).ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
			CalcImage();
            MessageBox.Show(_min_pix.ToString() + " " + _max_pix.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bitmap test = new Bitmap((int)OLSIH.Width, (int)OLSIH.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, (int)OLSIH.Width, (int)OLSIH.Height);
            BitmapData bmpData = test.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, test.PixelFormat);

            int numBytes = test.Width * test.Height * 3; 
            byte[] rgbValues = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, numBytes);
            int ii = 0;
            for (int i = 0; i < rgbValues.Length; i += 3)
            {
                rgbValues[i] = ImageData[ii];
                rgbValues[i + 1] = ImageData[ii];
                rgbValues[i + 2] = ImageData[ii];
                ii++;
            }
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);
            test.UnlockBits(bmpData);
            pictureBox1.Image = (Image)test;
            pictureBox1.Refresh();
//            test.Save(@"test6.bmp", ImageFormat.Bmp);
        }

#region Image24bpp
        private void Image24bpp()
        {
            Bitmap test = new Bitmap((int)OLSIH.Width, (int)OLSIH.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Rectangle rect = new Rectangle(0, 0, (int)OLSIH.Width, (int)OLSIH.Height);
            BitmapData bmpData = test.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, test.PixelFormat);

            int numBytes = test.Width * test.Height * 3;
            byte[] rgbValues = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, numBytes);
            for (int i = 0; i < test.Width * 20; i++)
            {
                ImageData[i] = 0xFF; // B
            }
            int ii = 0;
            for (int i = 0; i < rgbValues.Length; i += 3)
            {
                rgbValues[i + 0] = (byte)(ImageData[ii + 1]); // B
                rgbValues[i + 1] = (byte)(ImageData[ii + 1]); // G
                rgbValues[i + 2] = (byte)(ImageData[ii + 1]); // R
                ii += 2;
            }
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);
            test.UnlockBits(bmpData);

            Graphics g = Graphics.FromImage(test);
            Rectangle dest = new Rectangle(0, 0, test.Width, test.Height);
            Rectangle src = new Rectangle(0, test.Height, test.Width, -test.Height);
            g.DrawImage(test, dest, src, GraphicsUnit.Pixel);

            ShowImage(test);
        }
#endregion
#region Image48bpp
		private void Image48bpp()
        {
            Bitmap test = new Bitmap((int)OLSIH.Width, (int)OLSIH.Height, System.Drawing.Imaging.PixelFormat.Format48bppRgb);
            Rectangle rect = new Rectangle(0, 0, (int)OLSIH.Width, (int)OLSIH.Height);
            BitmapData bmpData = test.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, test.PixelFormat);

            int numBytes = test.Width * test.Height * 6;
            byte[] rgbValues = new byte[numBytes];
            Marshal.Copy(bmpData.Scan0, rgbValues, 0, numBytes);
            //            int ii = ImageData.Length - 1;
            int ii = 0;
            for (int i = 0; i < rgbValues.Length; i += 6)
            {
                rgbValues[i + 0] = (byte)(ImageData[ii]);
                rgbValues[i + 1] = (byte)(ImageData[ii + 1]);
                rgbValues[i + 2] = (byte)(ImageData[ii]);
                rgbValues[i + 3] = (byte)(ImageData[ii + 1]);
                rgbValues[i + 4] = (byte)(ImageData[ii]);
                rgbValues[i + 5] = (byte)(ImageData[ii + 1]);
                //rgbValues[i + 0] = (byte)(ImageData[ii + 1]);
                //rgbValues[i + 1] = (byte)(ImageData[ii]);
                //rgbValues[i + 2] = (byte)(ImageData[ii + 1]);
                //rgbValues[i + 3] = (byte)(ImageData[ii]);
                //rgbValues[i + 4] = (byte)(ImageData[ii + 1]);
                //rgbValues[i + 5] = (byte)(ImageData[ii]);
                //ii--;
                ii += 2;
            }
            Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);
            test.UnlockBits(bmpData);

            // Переворот картинки
            Graphics g = Graphics.FromImage(test);
            Rectangle dest = new Rectangle(0, 0, test.Width, test.Height);
            Rectangle src = new Rectangle(0, test.Height, test.Width, -test.Height);
            g.DrawImage(test, dest, src, GraphicsUnit.Pixel);

            ShowImage(test);
        }
#endregion

		private void ShowImage(Image _image)
        {
            if (checkBox1.Checked)
				_image = ContrastImage(_image);
			if (checkBox3.Checked)
                _image = SaturationImage(_image);
            if (checkBox2.Checked)
                _image = BrightnessImage(_image);

			if (checkBox4.Checked)
                _image = DataOnImage(_image);
            pictureBox1.Image = (Image)_image;
            pictureBox1.Refresh();
        }

		private void CalcImage()
		{
			//UInt16 tmp1 = Convert.ToUInt16(ImageData[0] + Convert.ToUInt16(ImageData[1] << 8));
			for (UInt32 i = 1; i < dataSize-1; i += 2)
			{
                //UInt16 tmp = Convert.ToUInt16(ImageData[i + 1] + Convert.ToUInt16(ImageData[i] << 8));
                UInt16 tmp = (UInt16)(ImageData[i + 1] + (UInt16)(ImageData[i] << 8));
                if (tmp < _min_pix)
					_min_pix = tmp;
				if (tmp > _max_pix)
					_max_pix = tmp;
			}
            counter++;
		}

        private void Contrast2()
        {
            for (UInt32 i = 1; i < dataSize - 1; i += 2)
            {
                UInt16 tmp = (UInt16)(ImageData[i + 1] + (UInt16)(ImageData[i] << 8));
                if (tmp < _min_pix)
                    tmp = 0;
                else
                    tmp -= _min_pix;
                if (tmp < 0)
                    tmp = 0;
                tmp *= 2;

                ImageData[i + 1] = (byte)tmp;
                ImageData[i] = (Byte)(tmp >> 8);
            }
        }

#region ContrastImage
        private Bitmap ContrastImage(Image _image)
        {
            if (_image == null)
                return null;
            Bitmap bm = new Bitmap(_image.Width, _image.Height); //create a new image
            Graphics g = Graphics.FromImage(bm); //ready to draw on it
            ImageAttributes ia = new ImageAttributes();
            //create the scaling matrix
            ColorMatrix cm = new ColorMatrix(new float[][]{  new float[]{_contrastFactor,0f,0f,0f,0f},
                              new float[]{0f,_contrastFactor,0f,0f,0f},
                              new float[]{0f,0f,_contrastFactor,0f,0f},
                              new float[]{0f,0f,0f,1f,0f},
                              //including the BLATANT FUDGE
                              new float[]{0.001f,0.001f,0.001f,0f,1f}});
            //use it in the image attributes
            ia.SetColorMatrix(cm);
            //draw the original to the temporary using the matrix
            g.DrawImage(_image, new Rectangle(0, 0, _image.Width, _image.Height), 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, ia);
            g.Dispose(); //Don't need this anymore;
            ia.Dispose(); // or this

            return bm;
        }
#endregion
#region SaturationImage
        private Bitmap SaturationImage(Image _image)
        {
            if (_image == null)
                return null;

            Bitmap bm = new Bitmap(_image.Width, _image.Height);
            Graphics g = Graphics.FromImage(bm);

            float SaturationComplement = 1.0f - _saturation;
            float SaturationComplementR = 0.3086f * SaturationComplement;
            float SaturationComplementG = 0.6094f * SaturationComplement;
            float SaturationComplementB = 0.0820f * SaturationComplement;

            ColorMatrix cm = new ColorMatrix(new float[][]{
                               new float[]{SaturationComplementR + _saturation,  SaturationComplementR,  SaturationComplementR,  0.0f, 0.0f},
                               new float[]{SaturationComplementG,  SaturationComplementG + _saturation,  SaturationComplementG,  0.0f, 0.0f},
                               new float[]{SaturationComplementB,  SaturationComplementB,  SaturationComplementB + _saturation,  0.0f, 0.0f},
                               new float[]{0.0f,  0.0f,  0.0f,  1.0f,  0.0f},
                               new float[]{0.0f,  0.0f,  0.0f,  0.0f,  1.0f}});

            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm);

            g.DrawImage(_image, new Rectangle(0, 0, bm.Width, bm.Height), 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, ia);
            return bm;
        }
#endregion
#region BrightnessImage
        private Bitmap BrightnessImage(Image _image)
        {
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = _image.Width;
            int height = _image.Height;

            float[][] colorMatrixElements = {
                                                new float[] {_brightness, 0, 0, 0, 0},
                                                new float[] {0, _brightness, 0, 0, 0},
                                                new float[] {0, 0, _brightness, 0, 0},
                                                new float[] {0, 0, 0, 1, 0},
                                                new float[] {0, 0, 0, 0, 1}
                                            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix,ColorMatrixFlag.Default,ColorAdjustType.Bitmap);
            Graphics graphics = Graphics.FromImage(_image);
            graphics.DrawImage(_image, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, imageAttributes);
            return (Bitmap)_image;
        }
#endregion
#region DataOnImage
        private Bitmap DataOnImage(Image _image)
        {
            Graphics g = Graphics.FromImage(_image);
            Font drawFont = new Font("Arial", 8);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            g.DrawString(_min_pix.ToString() + "/" + _max_pix.ToString(), drawFont, drawBrush, new PointF(5.0F, 5.0F));
            g.DrawString(frame_counter.ToString(), drawFont, drawBrush, new PointF(5.0F, 15.0F));
            g.DrawString(counter.ToString(), drawFont, drawBrush, new PointF(5.0F, 25.0F));
            return (Bitmap)_image;
        }
#endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
			fs.Seek(OLSFH.Size * frame_counter, SeekOrigin.Begin);

            OLSFH = ReadStruct<OLSFileHeader>(fs);
            OLSIH = ReadStruct<OLSInfoHeader>(fs);
            toolStripStatusLabel2.Text = "Кадр " + Convert.ToString(frame_counter++);
            toolStripStatusLabel4.Text = "Контраст " + this._contrastFactor.ToString("0.00");
			if(frame_counter < tbNavigation.Maximum) 
				tbNavigation.Value = frame_counter;
            if (OLSFH.Type == 0)
            {
                State = PlayState.stop;
                timer1.Enabled = false;
                _play.Enabled = true;
                toolStripStatusLabel3.Text = "Stop";
                fs.Close();
				tbNavigation.Value = 0;
				frame_counter = 0;
				return;
            }
            if (OLSFH.Type != 0x4D42 /* BM in ASCII */)
                throw new IOException("Прочитан неверный заголовок файла.");

            long totalHeaderSize = Marshal.SizeOf(typeof(OLSFileHeader)) + Marshal.SizeOf(typeof(OLSInfoHeader));
            if (OLSFH.OffBits < totalHeaderSize)
                throw new IOException("Прочитаны противоречивые данные.");

            UInt32 nBytesPass = OLSFH.OffBits - (UInt32)totalHeaderSize;
            fs.Seek(nBytesPass, SeekOrigin.Current);

            Int32 cx = (Int32)OLSIH.Width;
            Int32 cy = (Int32)OLSIH.Height;

            UInt32 nImageBytes = OLSFH.Size - OLSFH.OffBits;
            //			Int32 dataSize;
            if (nImageBytes <= Int32.MaxValue)
                dataSize = Convert.ToInt32(nImageBytes);
            else 
                throw new NotImplementedException();

            ImageData = new Byte[dataSize];
            fs.Read(ImageData, 0, dataSize);
            
            if (frame_counter % 20 == 0)
                CalcImage();

            if (checkBox3.Checked)
                Contrast2();

            if(comboBox1.SelectedIndex == 0)
                Image24bpp();
            if (comboBox1.SelectedIndex == 1)
                Image48bpp();
        }

        private void _stop_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = "Stop";
            timer1.Enabled = false;
            if (State != PlayState.stop)
            {
                _play.Enabled = true;
                State = PlayState.stop;
				frame_counter = 0;
				tbNavigation.Value = 0;
                fs.Close();
            }
        }

        private void _play_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName != "")
            {
                fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
				if(frame_counter != 0)
					fs.Seek(OLSFH.Size * tbNavigation.Value, SeekOrigin.Begin);
                State = PlayState.play;
                toolStripStatusLabel3.Text = "Play";
                timer1.Enabled = true;
                _play.Enabled = false;
            }
        }

        private void _pause_Click(object sender, EventArgs e)
        {
            if (State == PlayState.play)
            {
                State = PlayState.pause;
                timer1.Enabled = false;
                toolStripStatusLabel3.Text = "Pause";
            }
            else if(State == PlayState.pause)
            {
                State = PlayState.play;
                timer1.Enabled = true;
                toolStripStatusLabel3.Text = "Play";
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _saturation = 1.0f / 255 * this.trackBar2.Value;
        //    CalcImage();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            _contrastFactor = 0.04f * this.trackBar2.Value;
            toolStripStatusLabel4.Text = "Контраст " + _contrastFactor.ToString("0.00");
            //UpdateImage();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            _brightness = 0.01f * this.trackBar3.Value;
            //toolStripStatusLabel4.Text = "Контраст " + _contrastFactor.ToString("0.00");
        }

		private void tbNavigation_Scroll(object sender, EventArgs e)
		{
			//if (State == PlayState.play)
			//{
			//	State = PlayState.pause;
			//	timer1.Enabled = false;
			//	toolStripStatusLabel3.Text = "Pause";
			//	fs.Seek(OLSFH.Size * tbNavigation.Value, SeekOrigin.Begin);
			//}
			//else if (State == PlayState.stop)
			//{
			//	State = PlayState.stop;
			//	timer1.Enabled = false;
			//	toolStripStatusLabel3.Text = "Stop";
			//}
			frame_counter = tbNavigation.Value;
			toolStripStatusLabel2.Text = "Кадр " + Convert.ToString(tbNavigation.Value);
		}
	}
}

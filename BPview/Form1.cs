using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using BPview.Resources;

namespace BPview
{
	public partial class Form1 : Form
	{
		public BPM[] pic;
		public bool pic1loaded = false;
		public bool pic2loaded = false;
		public Bitmap BlackPic = new Bitmap(Const.IM_WIDTH, Const.IM_HEIGHT, PixelFormat.Format24bppRgb);
		public int count_minus = 0;
		public int count_plus = 0;
		public int count_eqv = 0;
		public bool st = false;
		public String[] filenames = new String[2];

		public Form1()
		{
			InitializeComponent();
		}

		private void открыть1_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
				return;
			pic[0] = new BPM(0, openFileDialog1.FileName, Const.clGreen);
			pic1loaded = true;
			
			pictureBox1.Image = pic[0].GetBitmap;
			pictureBox1.Refresh();
			st = false;
		}

		private void открыть2_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
				return;
			pic[1] = new BPM(1, openFileDialog1.FileName, Const.clRed);
			pic2loaded = true;
			pictureBox1.Image = pic[1].GetBitmap;
			pictureBox1.Refresh();
			st = false;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.Multiselect = true;
			if (openFileDialog1.ShowDialog() != DialogResult.OK)
				return;
			if (openFileDialog1.FileNames.Length == 0)
				return;
			else if (openFileDialog1.FileNames.Length == 1)
			{
				if (filenames[0] == null)
				{
					filenames[0] = openFileDialog1.FileNames[0];
					pic[0] = new BPM(0, filenames[0], Const.clGreen);
					pic1loaded = true;
					pic2loaded = false;
					pictureBox1.Image = pic[0].GetBitmap;
					pictureBox1.Refresh();
					st = false;
				}
				else
				{
					filenames[1] = openFileDialog1.FileNames[0];
					pic[1] = new BPM(1, filenames[1], Const.clRed);
					pic1loaded = true;
					pic2loaded = true;
					pictureBox1.Image = pic[1].GetBitmap;
					pictureBox1.Refresh();
					st = false;
				}
			}
			else
			{
				filenames[0] = openFileDialog1.FileNames[0];
				filenames[1] = openFileDialog1.FileNames[1];
				pic[0] = new BPM(0, filenames[0], Const.clGreen);
				pic1loaded = true;
				pictureBox1.Image = pic[0].GetBitmap;
				pictureBox1.Refresh();
				st = false;
				pic[1] = new BPM(1, filenames[1], Const.clRed);
				pic2loaded = true;
				pictureBox1.Image = pic[1].GetBitmap;
				pictureBox1.Refresh();
				st = false;
			}
			//String ss = String.Empty;
			//foreach (String sss in openFileDialog1.FileNames)
			//{
			//	ss += sss + "\r\n";
			//}
			//MessageBox.Show(ss);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			pic = new BPM[2];

			toolItem1.Enabled = false;
			toolItem1.Text = "";
			toolItem2.Enabled = false;
			toolItem2.Text = "";
			toolCompare.Enabled = false;

			openFileDialog1.FileName = "";
			openFileDialog1.DefaultExt = "map";
			openFileDialog1.Filter = "Карты битых пикселов (*.map)|*.map|Все файлы (*.*)|*.*";
		}

		private void выходToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void toolItem1_Click(object sender, EventArgs e)
		{
			pictureBox1.Image = pic[0].GetBitmap;
			pictureBox1.Refresh();
		}

		private void toolItem2_Click(object sender, EventArgs e)
		{
			pictureBox1.Image = pic[1].GetBitmap;
			pictureBox1.Refresh();
		}

		private void сервисToolStripMenuItem_Click(object sender, EventArgs e)
		{
			servicemenu();
		}

		private void toolCompare_Click(object sender, EventArgs e)
		{
			Bitmap bm = new Bitmap(256, 320, PixelFormat.Format24bppRgb);
			BitmapData bmpData = bm.LockBits(new Rectangle(0, 0, 256, 320), ImageLockMode.ReadWrite, bm.PixelFormat);

			count_minus = 0;
			count_plus = 0;
			count_eqv = 0;

			int numBytes = bm.Width * bm.Height * 3;
			byte[] rgbValues = new byte[numBytes];
			
			for (int i = 0; i < pic[0].GetRGB.Length; i += 3)
			{
				int f0 = pic[0].GetRGB[i + 0] + pic[0].GetRGB[i + 1] + pic[0].GetRGB[i + 2];
				int f1 = pic[1].GetRGB[i + 0] + pic[1].GetRGB[i + 1] + pic[1].GetRGB[i + 2];
				if (f0 != 0 && f1 != 0)
				{
					//rgbValues[i + 0] = col[2][2]; // B
					//rgbValues[i + 1] = col[2][1]; // G
					//rgbValues[i + 2] = col[2][0]; // R
					rgbValues[i + 0] = Const.clBlue[2]; // B
					rgbValues[i + 1] = Const.clBlue[1]; // G
					rgbValues[i + 2] = Const.clBlue[0]; // R
					count_eqv++;
				}
				if (f0 != 0 && f1 == 0)
				{
					rgbValues[i + 0] = pic[0].GetRGB[i + 0]; // B
					rgbValues[i + 1] = pic[0].GetRGB[i + 1]; // G
					rgbValues[i + 2] = pic[0].GetRGB[i + 2]; // R
					count_minus++;
				}
				if (f0 == 0 && f1 != 0)
				{
					rgbValues[i + 0] = pic[1].GetRGB[i + 0]; // B
					rgbValues[i + 1] = pic[1].GetRGB[i + 1]; // G
					rgbValues[i + 2] = pic[1].GetRGB[i + 2]; // R
					count_plus++;
				}
			}

			Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);
			bm.UnlockBits(bmpData);

			Graphics g = Graphics.FromImage(bm);
			Rectangle dest = new Rectangle(0, 0, bm.Width, bm.Height);
			Rectangle src = new Rectangle(0, bm.Height, bm.Width, -bm.Height);
			g.DrawImage(bm, dest, src, GraphicsUnit.Pixel);

			pictureBox1.Image = bm;
			pictureBox1.Refresh();
			st = true;

			MessageBox.Show("Файл 1: " + pic[0].Filename 
							+ "\r\nФайл 2: " + pic[1].Filename + "\r\n\r\n"
							+ "Пикселов совпадает     : " + count_eqv.ToString() + "\r\n"
							+ "Пикселов исчезло        : " + count_minus.ToString() + "\r\n"
							+ "Пикселов добавилось : " + count_plus.ToString() + "\r\n\r\n",
							//+ "Битых пикселов всего:  " + (count_eqv + count_plus - count_minus).ToString(), 
							"Результат сравнения",MessageBoxButtons.OK,MessageBoxIcon.Information);


		}

		private void qqToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Файл 1: " + pic[0].Filename
							+ "\r\nФайл 2: " + pic[1].Filename + "\r\n\r\n"
							+ "Пикселов совпадает     : " + count_eqv.ToString() + "\r\n"
							+ "Пикселов исчезло        : " + count_minus.ToString() + "\r\n"
							+ "Пикселов добавилось : " + count_plus.ToString() + "\r\n\r\n",
				//+ "Битых пикселов всего:  " + (count_eqv + count_plus - count_minus).ToString(),
							"Результат сравнения", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void сервисToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			servicemenu();
		}

		private void servicemenu()
		{
			if (!pic1loaded && !pic2loaded)
			{
				toolItem1.Enabled = false;
				toolItem1.Visible = false;
				toolItem1.Text = "";
				toolItem2.Enabled = false;
				toolItem2.Visible = false;
				toolItem2.Text = "";
				toolCompare.Enabled = false;
				Stat.Enabled = false;
				toolExchange.Enabled = false;
			}
			if (pic1loaded && !pic2loaded)
			{
				toolItem1.Enabled = true;
				toolItem1.Visible = true;
				toolItem1.Text = pic[0].Filename;
				toolItem2.Enabled = false;
				toolItem2.Visible = false;
				toolItem2.Text = "";
				toolCompare.Enabled = false;
				toolExchange.Enabled = false;
			}
			if (!pic1loaded && pic2loaded)
			{
				toolItem1.Enabled = false;
				toolItem1.Visible = false;
				toolItem1.Text = "";
				toolItem2.Enabled = true;
				toolItem2.Visible = true;
				toolItem2.Text = pic[1].Filename;
				toolCompare.Enabled = false;
				toolExchange.Enabled = false;
			}
			if (pic1loaded && pic2loaded)
			{
				toolItem1.Enabled = true;
				toolItem1.Visible = true;
				toolItem1.Text = pic[0].Filename;
				toolItem2.Enabled = true;
				toolItem2.Visible = true;
				toolItem2.Text = pic[1].Filename;
				toolCompare.Enabled = true;
				toolExchange.Enabled = true;
				Stat.Enabled = true;
			}
			if (st)
				Stat.Enabled = true;
			else
				Stat.Enabled = false;
		}

		private void toolExchange_Click(object sender, EventArgs e)
		{
			String sss = String.Empty;
			sss = filenames[0];
			filenames[0] = filenames[1];
			filenames[1] = sss;

			pic[0] = new BPM(0, filenames[0], Const.clGreen);
			pic1loaded = true;
			pictureBox1.Image = pic[0].GetBitmap;
			pictureBox1.Refresh();
			st = false;
			pic[1] = new BPM(1, filenames[1], Const.clRed);
			pic2loaded = true;
			pictureBox1.Image = pic[1].GetBitmap;
			pictureBox1.Refresh();
			st = false;
			String ss = String.Empty;
			//foreach (String ssss in filenames)
			//{
			//	ss += ssss + "\r\n";
			//}
			//MessageBox.Show(ss);
		}

		private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
		{
		}

		private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form ab = new AboutBox1();
			ab.ShowDialog();
		}

	}

	public class Const
	{
		public const int SIZE_BPTABLE = 10240;
		public const int IM_WIDTH = 256;
		public const int IM_HEIGHT = 320;

		public static readonly Byte[] clBlack = new Byte[] { 0, 0, 0 };
		public static readonly Byte[] clWhite = new Byte[] { 255, 255, 255 };
		public static readonly Byte[] clBlue = new Byte[] { 0, 0, 255 };
		public static readonly Byte[] clRed = new Byte[] { 255, 0, 0 };
		public static readonly Byte[] clGreen = new Byte[] { 0, 255, 0 };
		public static readonly Byte[] clYellow = new Byte[] { 255, 255, 0 };
		public static readonly Byte[] clDarkOrange = new Byte[] { 255, 140, 0 };
		public static readonly Byte[] clMagenta = new Byte[] { 255, 0, 255 };
		public static readonly Byte[] clCyan1 = new Byte[] { 0, 255, 255 };
		public static readonly Byte[] clRosyBrown1 = new Byte[] { 255, 193, 193 };
		public static readonly Byte[] clBurlywood1 = new Byte[] { 255, 211, 155 };
	}

	public class BPM
	{
		//		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x27ff)]
		private Byte[] data_arr = new Byte[Const.SIZE_BPTABLE];
		private Bitmap newbm;
		private byte[] rgbValues;
		private String filename;
		private string fullfilename;
		private int num;

		public BPM(int index, String fn, Byte[] color)
		{
			FileInfo fi = new FileInfo(fn);
			fullfilename = fn;
			filename = fi.Name;
			num = index;
			FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
			fs.Read(data_arr, 0, 10240);
			fs.Close();

			newbm = new Bitmap(Const.IM_WIDTH, Const.IM_HEIGHT, PixelFormat.Format24bppRgb);
			BitmapData bmpData = newbm.LockBits(new Rectangle(0, 0, Const.IM_WIDTH, Const.IM_HEIGHT), ImageLockMode.ReadWrite, newbm.PixelFormat);

			int numBytes = newbm.Width * newbm.Height * 3;
			rgbValues = new byte[numBytes];

			for (int i = 0; i < data_arr.Length; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if ((data_arr[i] & (1 << j)) > 0)
					{
						rgbValues[i * 24 + j * 3 + 0] = color[2]; // B
						rgbValues[i * 24 + j * 3 + 1] = color[1]; // G
						rgbValues[i * 24 + j * 3 + 2] = color[0]; // R
					}
					else
					{
						rgbValues[i * 24 + j * 3 + 0] = 0; // B
						rgbValues[i * 24 + j * 3 + 1] = 0; // G
						rgbValues[i * 24 + j * 3 + 2] = 0; // R
					}
				}
			}

			Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);
			newbm.UnlockBits(bmpData);

			Graphics g = Graphics.FromImage(newbm);
			Rectangle dest = new Rectangle(0, 0, newbm.Width, newbm.Height);
			Rectangle src = new Rectangle(0, newbm.Height, newbm.Width, -newbm.Height);
			g.DrawImage(newbm, dest, src, GraphicsUnit.Pixel);
		}

		public string FFilename
		{
			get	{return fullfilename;}
		}

		public string Filename
		{
			get { return filename; }
		}

		public Bitmap GetBitmap
		{
			get {return newbm;}
		}

		public Byte[] GetRGB
		{
			get { return rgbValues; }
		}
		public int Index
		{
			get { return num; }
			set { num = value; }
		}

		~BPM()
		{
			newbm.Dispose();
		}
	}
}


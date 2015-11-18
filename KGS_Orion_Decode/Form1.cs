using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iofile = System.IO.File;
using iopath = System.IO.Path;

namespace KGS_Orion_Decode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rb_kgs.Checked)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = "kgs";
                ofd.Filter = "KGS files (*.kgs)|*.kgs|All files (*.*)|*.*";
                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                textBox1.Text = ofd.FileName;
                button1.Enabled = false;
                textBox1.Enabled = false;
                rb_bin.Enabled = false;
                rb_kgs.Enabled = false;
                FileInfo fi = new FileInfo(ofd.FileName);
                long len = (new FileInfo(ofd.FileName)).Length / 8;

                progressBar1.Maximum = (int)len;

                UInt16[] K = new UInt16[len];
                Int16[] b = new Int16[len];
                Int16[] It = new Int16[len];
                UInt16[] TBP = new UInt16[len];

                using (BinaryReader br = new BinaryReader(File.Open(ofd.FileName, FileMode.Open)))
                {
                    label1.Text = "Чтение файла...";
                    for (int i = 0; i < len; i++)
                    {
                        b[i] = br.ReadInt16();
                        K[i] = br.ReadUInt16();
                        It[i] = br.ReadInt16();
                        TBP[i] = br.ReadUInt16();
                        progressBar1.Value = i;
                        Application.DoEvents();
                    }
                }
                label1.Text = "Чтение файла... ОК";
                progressBar1.Value = 0;

                using (StreamWriter sw = new StreamWriter(File.Create(ofd.FileName.Replace("kgs", "csv")), System.Text.Encoding.Default))
                {
                    label1.Text = "Запись файла...";
                    sw.WriteLine("b;K;It;ТБП;");
                    for (int i = 0; i < len; i++)
                    {
                        sw.WriteLine(b[i].ToString() + ";" + K[i].ToString() + ";" + It[i].ToString() + ";" + TBP[i].ToString() + ";");
                        progressBar1.Value = i;
                        Application.DoEvents();
                    }
                }
                progressBar1.Value = 0;
                label1.Text = "Файл сохранен под именем '" + ofd.FileName.Replace("kgs", "csv") + "'";
                textBox1.Text = "";
                button1.Enabled = true;
                textBox1.Enabled = true;
                rb_bin.Enabled = true;
                rb_kgs.Enabled = true;
            }
            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = "bin";
                ofd.Filter = "BIN files (*.bin)|*.bin|All files (*.*)|*.*";
                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                textBox1.Text = ofd.FileName;
                button1.Enabled = false;
                textBox1.Enabled = false;
                rb_bin.Enabled = false;
                rb_kgs.Enabled = false;
                FileInfo fi = new FileInfo(ofd.FileName);
                long len = (new FileInfo(ofd.FileName)).Length / 2;
                UInt16[] K = new UInt16[len];

                progressBar1.Maximum = (int)len;

                using (BinaryReader br = new BinaryReader(File.Open(ofd.FileName, FileMode.Open)))
                {
                    label1.Text = "Чтение файла...";
                    for (int i = 0; i < len; i++)
                    {
                        K[i] = br.ReadUInt16();
                        progressBar1.Value = i;
                        Application.DoEvents();
                    }
                }
                label1.Text = "Чтение файла... ОК";
                progressBar1.Value = 0;

                using (StreamWriter sw = new StreamWriter(File.Create(ofd.FileName.Replace("bin", "csv")), System.Text.Encoding.Default))
                {
                    label1.Text = "Запись файла...";
                    for (int i = 0; i < len; i++)
                    {
                        sw.WriteLine(K[i].ToString() + ";");
                        progressBar1.Value = i;
                        Application.DoEvents();
                    }
                }
                progressBar1.Value = 0;
                label1.Text = "Файл сохранен под именем '" + ofd.FileName.Replace("bin", "csv") + "'";
                textBox1.Text = "";
                button1.Enabled = true;
                textBox1.Enabled = true;
                rb_bin.Enabled = true;
                rb_kgs.Enabled = true;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMPReader3
{
    public partial class marker : Form
    {
        public marker()
        {
            InitializeComponent();
           
        }

        private void marker_Load(object sender, EventArgs e)
        {
            //pictureBox1.Image = Form1
        }

        private void marker_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            
            this.Hide();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

using System;
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
	public partial class ra : Form
	{
		public ra()
		{
			InitializeComponent();
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			label2.Text = trackBar1.Value.ToString() + "%";
		}

		private void ra_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}

	}
}

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
    public partial class histo2 : Form
    {
        private Form1 m_parent;
        public histo2()
        {
            InitializeComponent();
            m_parent = this.Owner as Form1;
        }

        private void histo2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}

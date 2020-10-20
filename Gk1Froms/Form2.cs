using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gk1Froms
{
    public partial class Form2 : Form
    {
        public Form2(double angle)
        {
            InitializeComponent();
            textBox1.Text = Math.Round(angle,2).ToString();
        }

        public string GetText()
        {
            return textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

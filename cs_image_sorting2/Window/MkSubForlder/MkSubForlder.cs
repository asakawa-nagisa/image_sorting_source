using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_image_sorting2
{
    public partial class MkSubForlder : Form
    {
        string main_path;
        public MkSubForlder(string main_path)
        {
            this.main_path = main_path;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mkDir();
        }

        private void mkDir()
        {
            try
            {
                Directory.CreateDirectory(String.Format(@"{0}\{1}", this.main_path, this.textBox1.Text));
                this.Close();
            }
            catch
            {
                MessageBox.Show("フォルダを作成できませんでした。");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                mkDir();
            }
        }
    }
}

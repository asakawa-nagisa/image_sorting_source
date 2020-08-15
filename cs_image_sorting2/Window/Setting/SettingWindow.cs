using System;
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
    public partial class SettingWindow : Form
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

        private void SettingWindow_Load(object sender, EventArgs e)
        {
            System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.label3.Text = ver.FileVersion;
            this.checkBox1.Checked = Program.setting.preview;
            this.trackBar1.Value = Program.setting.size;
            this.numericUpDown1.Value = Program.setting.load_num;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.setting.size = this.trackBar1.Value;
            Program.setting.preview = this.checkBox1.Checked;
            Program.setting.load_num = (int)this.numericUpDown1.Value;
            NagisaLibrary.BinaryIO.Save((string)Properties.Settings.Default["SettingFile"], Program.setting);
            this.Close();
        }
    }
}

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
    public partial class Diff : Form
    {
        string A_path;
        string B_path;
        public Diff(string A_path, string B_path)
        {
            this.A_path = A_path;
            this.B_path = B_path;
            InitializeComponent();
        }

        /// <summary>
        /// 指定したファイルをロックせずに、System.Drawing.Imageを作成する。
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image。</returns>
        public static System.Drawing.Image CreateImage(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return img;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            File.Delete(this.A_path);
            this.Close();
        }

        private void Diff_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = null;
            this.pictureBox1.Image = CreateImage(this.A_path);
            this.label1.Text = String.Format("幅:{0}", this.pictureBox1.Image.Width);
            this.label2.Text = String.Format("高:{0}", this.pictureBox1.Image.Height);
            this.pictureBox2.Image = null;
            this.pictureBox2.Image = CreateImage(this.B_path);
            this.label3.Text = String.Format("幅:{0}", this.pictureBox2.Image.Width);
            this.label4.Text = String.Format("高:{0}", this.pictureBox2.Image.Height);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.Delete(this.B_path);
            System.IO.File.Move(this.A_path, this.B_path);
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string new_path = Path.GetDirectoryName(this.B_path);
            string extension = Path.GetExtension(this.B_path);

            System.IO.File.Move(
                this.A_path,
                String.Format(@"{0}\{1}{2}", new_path, this.textBox1.Text, extension)
                );
            this.Close();
        }

        private void Diff_FontChanged(object sender, EventArgs e)
        {
            this.pictureBox1.Image.Dispose();
            this.pictureBox2.Image.Dispose();
        }
    }
}

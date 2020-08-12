using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cs_image_sorting2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = this.textBox1.Text;
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                this.textBox1.Text = fbd.SelectedPath;
                SetImages(fbd.SelectedPath);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            this.textBox2.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            SetImages(this.textBox1.Text);
            getSubDir(this.textBox2.Text);
            this.KeyPreview = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                //画像リストの取得
                SetImages(this.textBox1.Text);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                //サブディレクトリの取得
                getSubDir(this.textBox2.Text);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count != 0)
            {
                try
                {
                    ListViewItem itemx = listView1.SelectedItems[0];
                    this.pictureBox1.Image = CreateImage(fileList[itemx.ImageIndex]);
                    this.toolStripStatusLabel2.Text = String.Format("高さ:{0}", this.pictureBox1.Image.Height);
                    this.toolStripStatusLabel3.Text = String.Format("幅:{0}", this.pictureBox1.Image.Width);
                }
                catch
                {
                    SetImages(this.textBox1.Text);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath =this.textBox2.Text;
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                this.textBox2.Text = fbd.SelectedPath;
                getSubDir(this.textBox2.Text);
            }
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.toolStripStatusLabel4.Text = String.Format("選択画像数:{0}", this.listView1.SelectedItems.Count);
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            this.toolStripStatusLabel4.Text = String.Format("選択画像数:{0}", this.listView1.CheckedItems.Count);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.moveFiles();
            this.pictureBox1.Image = null;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int index = 0;
            if (this.listView1.FocusedItem != null)
            {
                index = this.listView1.FocusedItem.Index;
                if (e.Button == MouseButtons.XButton1)
                {
                    UpIndex(index);
                }
                else if (e.Button == MouseButtons.XButton2)
                {
                    DownIndex(index);
                }
            }
        }

        private void 新規フォルダ作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MkSubForlder msf = new MkSubForlder(this.textBox2.Text);
            msf.ShowDialog(this);
            getSubDir(this.textBox2.Text);
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.listView1.FocusedItem.Checked = !this.listView1.FocusedItem.Checked;
        }

        private void Main_FontChanged(object sender, EventArgs e)
        {
            thread.Abort();
            thread.Join();
            Console.WriteLine("End");
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            int index = 0;
            if (this.listView1.FocusedItem != null)
            {
                index = this.listView1.FocusedItem.Index;
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                if (this.listView1.Focused)
                {
                    foreach (ListViewItem itm in this.listView1.Items)
                    {
                        itm.Checked = true;
                    }
                }
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                if (this.listView1.Focused)
                {
                    foreach (ListViewItem itm in this.listView1.Items)
                    {
                        itm.Checked = false;
                    }
                }
            }

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    try
                    {
                        this.listView1.FocusedItem.Checked = !this.listView1.FocusedItem.Checked;
                    }
                    catch
                    { }
                    break;

                case Keys.Down:
                    if (thread == null)
                    {
                        UpIndex(index);
                    }
                    else
                    {
                        MessageBox.Show("読み込み中は実行できません。");
                    }
                    e.Handled = true;
                    break;

                case Keys.Right:
                    if (thread == null)
                    {
                        UpIndex(index);
                    }
                    else
                    {
                        MessageBox.Show("読み込み中は実行できません。");
                    }
                    break;

                case Keys.Up:
                    if(thread == null)
                    {
                        DownIndex(index);
                    }
                    else
                    {
                        MessageBox.Show("読み込み中は実行できません。");
                    }
                    e.Handled = true;
                    break;

                case Keys.Left:
                    if (thread == null)
                    {
                        DownIndex(index);
                    }
                    else
                    {
                        MessageBox.Show("読み込み中は実行できません。");
                    }
                    break;
                default:
                    break;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            //サブディレクトリの取得
            getSubDir(this.textBox2.Text);
        }

        private void 全選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in this.listView1.Items)
            {
                itm.Checked = true;
            }
        }

        private void 全選択解除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in this.listView1.Items)
            {
                itm.Checked = false;
            }
        }

        private void 移動ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = this.textBox1.Text;
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                this.textBox1.Text = fbd.SelectedPath;
                SetImages(fbd.SelectedPath);
            }
        }
    }
}

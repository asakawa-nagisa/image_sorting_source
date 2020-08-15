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

using NagisaLibrary;

namespace cs_image_sorting2
{
    public partial class Main : Form
    {
        private string text1;
        private string text2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロード時のイベント（初期化として利用）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            System.Reflection.AssemblyTitleAttribute asmttl = (System.Reflection.AssemblyTitleAttribute)Attribute.GetCustomAttribute(System.Reflection.Assembly.GetExecutingAssembly(), typeof(System.Reflection.AssemblyTitleAttribute));
            System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.Text = String.Format("{0} [{1}]",
                asmttl.Title, ver.FileVersion);
            //フォームのアイコンを設定する
            this.Icon = Properties.Resources.icon;
            // 全部品でMain_KeyDownを許可する。
            this.KeyPreview = true;
            // 画像リスト読み込みディレクトリの初期値指定
            this.textBox1.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            // ディレクトリリスト読み込みの初期値指定
            this.textBox2.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            // 画像リスト読み込み
            SetImages(this.textBox1.Text);
            // ディレクトリリスト読み込み
            getSubDir(this.textBox2.Text);
            this.text1 = this.textBox1.Text;
            this.text2 = this.textBox2.Text;
            ReflectionSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SetImageListDir();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            SetDirectoryListDir();
        }

        /// <summary>
        /// 画像リスト読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                //画像リストの取得
                SetImages(this.textBox1.Text);
            }
        }

        /// <summary>
        /// ディレクトリリスト読み込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                //サブディレクトリの取得
                getSubDir(this.textBox2.Text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count != 0)
            {
                try
                {
                    ListViewItem itemx = listView1.SelectedItems[0];
                    this.pictureBox1.Image = CreateImage(fileList[itemx.ImageIndex]);
                    this.toolTip1.SetToolTip(this.pictureBox1, String.Format("{0}×{1}", this.pictureBox1.Image.Width, this.pictureBox1.Image.Height));
                }
                catch(Exception ex)
                {
                    // エラー発生時画像表示領域の初期化
                    this.pictureBox1.Image = null;
                    this.toolTip1.SetToolTip(this.pictureBox1, String.Format(""));
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.label3.Text = String.Format("選択画像数:{0}", this.listView1.SelectedItems.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            this.label3.Text = String.Format("選択画像数:{0}", this.listView1.CheckedItems.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (image_load_thread != null)
            {
                image_load_thread.Abort();
                image_load_thread.Join();
                image_load_thread = null;
            }
            this.moveFiles();
            this.pictureBox1.Image = null;
            this.toolTip1.SetToolTip(this.pictureBox1, String.Format(""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int index = 0;
            if (this.listView1.FocusedItem != null)
            {
                index = this.listView1.FocusedItem.Index;
                switch(e.Button)
                {
                    case MouseButtons.XButton1:
                        UpIndex(index);
                        break;

                    case MouseButtons.XButton2:
                        DownIndex(index);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新規フォルダ作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MkSubForlder msf = new MkSubForlder(this.textBox2.Text);
            msf.ShowDialog(this);
            getSubDir(this.textBox2.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.listView1.FocusedItem.Checked = !this.listView1.FocusedItem.Checked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FontChanged(object sender, EventArgs e)
        {
            image_load_thread.Abort();
            image_load_thread.Join();
            // Console.WriteLine("End");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_Leave(object sender, EventArgs e)
        {
            //サブディレクトリの取得
            getSubDir(this.textBox2.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全選択ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in this.listView1.Items)
            {
                itm.Checked = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全選択解除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itm in this.listView1.Items)
            {
                itm.Checked = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            int index = 0;
            if (this.listView1.FocusedItem != null)
            {
                index = this.listView1.FocusedItem.Index;
            }

            switch (e.KeyCode)
            {
                case Keys.A:
                    if((e.Control) && (this.listView1.Focused) && (image_load_thread == null))
                    {
                        foreach (ListViewItem itm in this.listView1.Items)
                        {
                            itm.Checked = true;
                        }
                    }
                    break;
                case Keys.C:
                    if((e.Control) && (this.listView1.Focused) && (image_load_thread == null))
                    {
                        foreach (ListViewItem itm in this.listView1.Items)
                        {
                            itm.Checked = false;
                        }
                    }
                    break;

                case Keys.Enter:
                    try
                    {
                        this.listView1.FocusedItem.Checked = !this.listView1.FocusedItem.Checked;
                    }
                    catch
                    {
                    }
                    break;

                case Keys.Down:
                    if (this.listView1.Focused)
                    {
                        e.Handled = true;
                    }
                    this.UpIndex(index);
                    break;

                case Keys.Right:
                    this.UpIndex(index);
                    break;

                case Keys.Up:
                    if (this.listView1.Focused)
                    {
                        e.Handled = true;
                    }
                    this.DownIndex(index);
                    break;

                case Keys.Left:
                    this.DownIndex(index);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text == string.Empty)
            {
                this.textBox1.Text = this.text1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool beforpreview = Program.setting.preview;
            int beforsize = Program.setting.size;

            SettingWindow setting = new SettingWindow();
            setting.ShowDialog(this);
            if (beforpreview != Program.setting.preview)
            {
                this.pictureBox1.Visible = Program.setting.preview;
                this.splitter1.Visible = Program.setting.preview;
            }
            if (beforsize != Program.setting.size)
            {
                SetImages(this.textBox1.Text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((image_move_thread != null) && (!image_move_thread.IsAlive)) image_move_thread = null;
            if ((image_load_thread != null) && (!image_load_thread.IsAlive)) image_load_thread = null;
        }
    }
}

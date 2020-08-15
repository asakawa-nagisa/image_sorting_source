using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cs_image_sorting2
{
    public partial class Main
    {
        private string[] fileList = new string[0];
        Thread image_load_thread = null;
        Thread image_move_thread = null;

        private void ReflectionSettings()
        {
            this.pictureBox1.Visible = Program.setting.preview;
            this.splitter1.Visible = Program.setting.preview;
            SetImages(this.textBox1.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void SetImages(string path)
        {
            Console.WriteLine("Do Set Images.");
            if (image_load_thread != null)
            {
                image_load_thread.Abort();
                image_load_thread.Join();
                image_load_thread = null;
            }

            // 画像読み込みスレッドの開始
            ImageListGetThread ilgt = new ImageListGetThread(this, path);
            image_load_thread = new Thread(new ThreadStart(ilgt.Worker));
            image_load_thread.Start();
        }

        /// <summary>
        /// ディレクトリリストの取得
        /// </summary>
        /// <param name="path"></param>
        private void getSubDir(string path)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            System.IO.DirectoryInfo[] subFolders = di.GetDirectories("*");

            this.toolStripProgressBar2.Value = 0;
            this.toolStripProgressBar2.Maximum = subFolders.Count();
            this.toolStripProgressBar2.Visible = true;
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add("デフォルト");
            this.comboBox1.Text = "デフォルト";
            //ListBox1に結果を表示する
            foreach (System.IO.DirectoryInfo subFolder in subFolders)
            {
                this.comboBox1.Items.Add(subFolder.FullName.Replace(this.textBox2.Text,"").Replace("\\",""));
            }
            this.toolStripProgressBar2.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void moveFiles()
        {
            ListView.CheckedListViewItemCollection cc = listView1.CheckedItems;
            int[] list = new int[0];
            string base_path = this.textBox2.Text;

            // ベースパスの変更
            if (this.comboBox1.SelectedIndex != 0)
            {
                base_path = String.Format(@"{0}\{1}", this.textBox2.Text, this.comboBox1.Text);
            }

            if ((this.textBox1.Text != base_path))
            {
                // ターゲットリストの取得
                foreach (ListViewItem itemx in cc)
                {
                    Array.Resize(ref list, list.Count() + 1);
                    list[list.Count() - 1] = itemx.ImageIndex;
                }
                Array.Reverse(list);

                if (list.Count() > 0)
                {
                    this.imageList1.Images.Clear();
                    this.listView1.Items.Clear();

                    while (this.imageList1.Images.Count != 0) Console.WriteLine("Wateing...");
                    while (this.listView1.Items.Count != 0) Console.WriteLine("Wateing...");

                    Console.WriteLine("Do Move Images.");
                    if (image_move_thread != null)
                    {
                        image_move_thread.Abort();
                        image_move_thread.Join();
                        image_move_thread = null;
                    }

                    // 画像読み込みスレッドの開始
                    ImageMoveThread imt = new ImageMoveThread(this, this.textBox1.Text,base_path,list,fileList);
                    image_move_thread = new Thread(new ThreadStart(imt.Worker));
                    image_move_thread.Start();
                }
            }
            else if (this.textBox1.Text == base_path)
            {
                MessageBox.Show("同一ディレクトリへの転送はできません");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string[] removeList(int index)
        {
            var foos = new List<String>(fileList);
            foos.RemoveAt(index);
            return foos.ToArray(); 
        }

        /// <summary>
        /// 指定したファイルをロックせずに、System.Drawing.Imageを作成する。
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image。</returns>
        public static System.Drawing.Image CreateImage(string filename)
        {
            System.Drawing.Image img = null;
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                img = System.Drawing.Image.FromStream(fs);
            }
            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void UpIndex(int index)
        {
            
            if (index < this.listView1.Items.Count)    index++;
            MoveIndex(index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void DownIndex(int index)
        {
            if (index >= 0) index--;
            MoveIndex(index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void MoveIndex(int index)
        {
            if (listView1.Items.Count > 0)
            {
                try
                {
                    this.listView1.SelectedItems.Clear();
                    this.listView1.Items[index].Focused = true;
                    this.listView1.Items[index].Selected = true;
                    this.listView1.EnsureVisible(index);
                }
                catch
                {
                    SetImages(this.textBox1.Text);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetImageListDir()
        {
            this.textBox1.Text = this.DirDialog(this.textBox1.Text);
            if (this.textBox1.Text != string.Empty)
            {
                SetImages(this.textBox1.Text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDirectoryListDir()
        {
            this.textBox2.Text = this.DirDialog(this.textBox2.Text);
            if (this.textBox2.Text != string.Empty)
            {
                getSubDir(this.textBox2.Text);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private string DirDialog(string path, string text = "フォルダを指定してください。")
        {
            string ret = string.Empty;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = text;
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = path;
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                //選択されたフォルダを表示する
                ret = fbd.SelectedPath;
            }
            return ret;
        }
    }
}

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
        private int width = 100;
        private int height = 100;
        private string[] fileList = new string[0];
        Thread thread = null;

        private void SetImages(string path)
        {
            if (thread != null)
            {
                thread.Abort();
                thread.Join();
                thread = null;
            }

            ImageListGetThread ilgt = new ImageListGetThread(this, path);
            thread = new Thread(new ThreadStart(ilgt.Worker));
            thread.Start();
        }

        private void getSubDir(string path)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            System.IO.DirectoryInfo[] subFolders = di.GetDirectories("*");

            this.comboBox1.Items.Clear();
            this.comboBox1.Items.Add("デフォルト");
            this.comboBox1.Text = "デフォルト";
            //ListBox1に結果を表示する
            foreach (System.IO.DirectoryInfo subFolder in subFolders)
            {
                this.comboBox1.Items.Add(subFolder.FullName.Replace(this.textBox2.Text,"").Replace("\\",""));
            }
        }

        private void moveFiles()
        {
            ListView.CheckedListViewItemCollection cc = listView1.CheckedItems;
            int[] list = new int[0];
            foreach (ListViewItem itemx in cc)
            {
                Array.Resize(ref list, list.Count() + 1);
                list[list.Count() - 1] = itemx.ImageIndex;
            }

            Array.Reverse(list);

            if ((list.Count() > 0) &&(this.textBox1.Text != this.textBox2.Text || this.comboBox1.Text != "デフォルト"))
            {
                foreach (ListViewItem itm in this.listView1.Items)
                {
                    itm.Checked = false;
                }
                this.imageList1.Images.Clear();
                this.listView1.Items.Clear();


                while (this.imageList1.Images.Count!= 0) ;
                while (this.listView1.Items.Count != 0) ;

                foreach (int index in list)
                {
                    string fileName = fileList[index].Replace(this.textBox1.Text, "").Replace("\\", "");
                    string moveAfterPath = String.Empty;
                    if (this.comboBox1.SelectedIndex == 0)
                    {
                        moveAfterPath = String.Format(@"{0}\{1}", this.textBox2.Text, fileName);
                    }
                    else
                    {
                        moveAfterPath = String.Format(@"{0}\{1}\{2}", this.textBox2.Text, this.comboBox1.Text, fileName);
                    }
                    try
                    {
                        System.IO.File.Move(fileList[index], moveAfterPath);
                    }
                    catch(IOException ioec)
                    {
                        Console.WriteLine(ioec.ToString());
                        if (ioec.ToString().IndexOf("既に存在するファイルを作成することはできません。") >= 0)
                        {
                            Diff diff = new Diff(fileList[index], moveAfterPath);
                            diff.StartPosition = FormStartPosition.CenterScreen;
                            diff.ShowDialog(this);
                        }
                    }
                    fileList = removeList(index);
                }
                this.SetImages(this.textBox1.Text);
            }
            else if ((this.textBox1.Text == this.textBox2.Text && this.comboBox1.Text == "デフォルト"))
            {
                MessageBox.Show("同一ディレクトリへの転送はできません");
            }
        }

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
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return img;
        }

        private void UpIndex(int index)
        {
            index++;
            if (index >= this.listView1.Items.Count)
            {
                index--;
            }
            MoveIndex(index);
        }

        private void DownIndex(int index)
        {
            index--;
            if (index < 0)
            {
                index++;
            }
            MoveIndex(index);
        }

        private void MoveIndex(int index)
        {
            Console.WriteLine(index);
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
    }
}

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
    public partial class Main
    {

        private int max_count = 0;
        private int max_images = 0;
        public delegate void SetDelegate1(int max_count, int max_images);
        public delegate bool SetDelegate2(Image thumbnail, string[] imgFiles, int i);
        public delegate void SetDelegate3();
        public delegate void NumIncrement();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="max_count"></param>
        /// <param name="max_images"></param>
        public void Set1(int max_count, int max_images)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetDelegate1(Set1), max_count, max_images);
                return;
            }
            else
            {
                this.max_count = max_count;
                this.max_images = max_images;
                this.toolStripProgressBar1.Value = 0;
                this.toolStripProgressBar1.Maximum = max_count;
                this.toolStripProgressBar1.Visible = true;
                this.toolStripStatusLabel1.Visible = true;
                this.toolStripStatusLabel1.Text = "いめ～じろ～どちゆ：";
                this.imageList1.Images.Clear();
                this.listView1.Items.Clear();
                fileList = new string[0];
                this.imageList1.ImageSize = new Size(Program.setting.size, Program.setting.size);
                listView1.LargeImageList = imageList1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void NumberIncrement()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new NumIncrement(NumberIncrement));
                return;
            }
            else
            {
                this.toolStripProgressBar1.Value++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thumbnail"></param>
        /// <param name="imgFiles"></param>
        /// <param name="i"></param>
        public bool Set2(Image thumbnail, string[] imgFiles, int i)
        {
            bool ret = true;
            if (this.InvokeRequired)
            {
                this.Invoke(new SetDelegate2(Set2), thumbnail, imgFiles, i);
            }
            else
            {
                try
                {
                    this.imageList1.Images.AddStrip(thumbnail);
                    thumbnail.Dispose();
                    this.listView1.Items.Add("", i);
                    Array.Resize(ref fileList, fileList.Count() + 1);
                    fileList[fileList.Count() - 1] = imgFiles[i];
                    this.toolStripStatusLabel1.Text = String.Format("いめ～じ読み込みちゆ～:");
                    this.label1.Text = String.Format("読み込んだ画像数:{0}/{1}", this.imageList1.Images.Count, this.max_count);
                    this.label2.Text = String.Format("フォルダ内の画像数:{0}", max_images);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    ret = false;
                }
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Set3()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetDelegate3(Set3));
                return;
            }
            else
            {
                this.toolStripProgressBar1.Visible = false;
                this.toolStripStatusLabel1.Text = "いめ～じろ～どかんりよ";
            }
        }
    }
}

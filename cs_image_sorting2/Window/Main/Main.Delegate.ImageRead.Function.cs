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
        public delegate void SetDelegate1(int max_count);
        public delegate void SetDelegate2(Image thumbnail, string[] imgFiles, int i);
        public delegate void SetDelegate3();
        public delegate void NumIncrement();

        public void Set1(int max_count)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetDelegate1(Set1), max_count);
                return;
            }
            else
            {
                this.toolStripProgressBar1.Value = 0;
                this.toolStripProgressBar1.Maximum = max_count;
                this.imageList1.Images.Clear();
                this.listView1.Items.Clear();
                fileList = new string[0];
                this.imageList1.ImageSize = new Size(width, height);
                listView1.LargeImageList = imageList1;
            }
        }

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

        public void Set2(Image thumbnail, string[] imgFiles, int i)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetDelegate2(Set2), thumbnail, imgFiles, i);
                return;
            }
            else
            {
                this.imageList1.Images.Add(thumbnail);
                this.listView1.Items.Add("", i);
                Array.Resize(ref fileList, fileList.Count() + 1);
                fileList[fileList.Count() - 1] = imgFiles[i];
                this.toolStripStatusLabel1.Text = String.Format("読み込み:{0}/{1}", this.listView1.Items.Count, imgFiles.Count());
            }
        }

        public void Set3()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetDelegate3(Set3));
                return;
            }
            else
            {
                this.toolStripStatusLabel1.Text = String.Format("フォルダ内総数:{0}", this.listView1.Items.Count);
            }
        }
    }
}

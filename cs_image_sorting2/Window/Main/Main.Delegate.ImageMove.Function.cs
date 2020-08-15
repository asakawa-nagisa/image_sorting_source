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
        public delegate int[] MoveDelegateInit();
        public delegate void MoveDelegate1();
        public delegate void ShowDiffDelegate(int index, string moveAfterPath);
        public delegate void MoveEndDelegate();

        public int[] MoveInit()
        {
            int[] ret = new int[0];
            if (this.InvokeRequired)
            {
                ret = (int[])this.Invoke(new MoveDelegateInit(MoveInit));
            }
            else
            {
                ListView.CheckedListViewItemCollection cc = listView1.CheckedItems;
                foreach (ListViewItem itemx in cc)
                {
                    Array.Resize(ref ret, ret.Count() + 1);
                    ret[ret.Count() - 1] = itemx.ImageIndex;
                }

                Array.Reverse(ret);
            }
            return ret;
        }

        // 移動処理をスレッド化する。
        public void MoveSet1()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MoveDelegate1(MoveSet1));
                return;
            }
            else
            {
            }
        }

        public void ShowDiffWondow(int index, string moveAfterPath)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowDiffDelegate(ShowDiffWondow), index, moveAfterPath);
                return;
            }
            else
            {
                Diff diff = new Diff(fileList[index], moveAfterPath);
                diff.StartPosition = FormStartPosition.CenterScreen;
                diff.ShowDialog(this);
            }
        }

        public void MoveEnd()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MoveEndDelegate(MoveEnd));
                return;
            }
            else
            {
                this.SetImages(this.textBox1.Text);
            }
        }
    }
}

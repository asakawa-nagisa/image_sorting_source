using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cs_image_sorting2
{
    class ImageMoveThread
    {
        string base_path;
        string befor_path;
        int[] list;
        string[] fileList;
        Main main;
        public ImageMoveThread(Main main, string befor_path, string base_path, int[] list, string[] fileList)
        {
            this.base_path = base_path;
            this.befor_path = befor_path;
            this.list = list;
            this.fileList = fileList;
            this.main = main;
        }

        public void Worker()
        {
            foreach (int index in list)
            {
                string fileName = fileList[index].Replace(this.befor_path, "").Replace("\\", "");
                string moveAfterPath = String.Empty;
                moveAfterPath = String.Format(@"{0}\{1}", base_path, fileName);
                try
                {
                    System.IO.File.Move(fileList[index], moveAfterPath);
                }
                catch (IOException ioec)
                {
                    //Console.WriteLine(ioec.ToString());
                    if (ioec.ToString().IndexOf("既に存在するファイルを作成することはできません。") >= 0)
                    {
                        this.main.ShowDiffWondow(index, moveAfterPath);
                    }
                }
                fileList = removeList(index);
            }
            this.main.MoveEnd();
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
    }
}

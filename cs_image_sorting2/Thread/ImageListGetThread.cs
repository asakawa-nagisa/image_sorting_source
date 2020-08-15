using System;
using System.IO;
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
    class ImageListGetThread
    {
        string path;
        Main main;
        public ImageListGetThread(Main main, string path)
        {
            this.path = path;
            this.main = main;
        }

        public void Worker()
        {
            string[] patterns = { 
                ".jpg",
                ".jpeg",
                ".png",
                ".gif",
                ".bmp",
            };
            string[] files = Directory.GetFiles(path, "*.*");
            string[] imgFiles = files.Where(file => patterns.Any(pattern => file.ToLower().EndsWith(pattern))).ToArray();

            int max = imgFiles.Count();
            if (imgFiles.Count() > Program.setting.load_num)
            {
                max = Program.setting.load_num;
                System.Windows.Forms.MessageBox.Show(String.Format("{0}件以上は読み込めません。", Program.setting.load_num));
            }
            this.main.Set1(max, imgFiles.Count());
            for (int i = 0; i < max; i++)
            {
                try
                {
                    Image original = Bitmap.FromFile(imgFiles[i]);
                    Image thumbnail = createThumbnail(original, Program.setting.size, Program.setting.size);
                    bool do_next = this.main.Set2(thumbnail, imgFiles, i);
                    original.Dispose();
                    thumbnail.Dispose();
                    this.main.NumberIncrement();
                    if (!do_next)
                    {
                        MessageBox.Show("読み込み中にエラーが発生しました。このエラーは最大読み込み枚数を下げることで回避できる可能性があります。");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (
                        ex is InvalidAsynchronousStateException ||
                        ex is ObjectDisposedException ||
                        ex is ThreadAbortException ||
                        ex is NullReferenceException)
                    {
                        return;
                    }
                    else if (ex is IOException || ex is OutOfMemoryException)
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            try
            {
                this.main.Set3();
            }
            catch
            { 
            }
            Console.WriteLine("スレッド終了");
        }

        // 幅w、高さhのImageオブジェクトを作成
        Image createThumbnail(Image image, int w, int h)
        {
            Bitmap canvas = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);

            float fw = (float)w / (float)image.Width;
            float fh = (float)h / (float)image.Height;

            float scale = Math.Min(fw, fh);
            fw = image.Width * scale;
            fh = image.Height * scale;

            g.DrawImage(image, (w - fw) / 2, (h - fh) / 2, fw, fh);
            g.Dispose();
            g = null;

            return canvas;
        }
    }
}

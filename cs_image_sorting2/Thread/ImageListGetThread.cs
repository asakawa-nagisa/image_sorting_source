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
    class ImageListGetThread
    {
        string path;
        Main main;
        private int width = 100;
        private int height = 100;
        public ImageListGetThread(Main main, string path)
        {
            this.path = path;
            this.main = main;
        }

        public void Worker()
        {
            string[] patterns = { 
                ".jpg",
                ".JPG",
                ".jpeg",
                ".png",
                ".gif",
                ".bmp",
            };
            string[] files = Directory.GetFiles(path, "*.*");
            string[] imgFiles = files.Where(file => patterns.Any(pattern => file.ToLower().EndsWith(pattern))).ToArray();

            this.main.Set1(imgFiles.Count());

            for (int i = 0; i < imgFiles.Length; i++)
            {
                try
                {
                    Image original = Bitmap.FromFile(imgFiles[i]);
                    Image thumbnail = createThumbnail(original, width, height);
                    this.main.Set2(thumbnail, imgFiles, i);
                    original.Dispose();
                    thumbnail.Dispose();
                    original = null;
                    thumbnail = null;
                    this.main.NumberIncrement();
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    if (ex is InvalidAsynchronousStateException || ex is ObjectDisposedException || ex is ThreadAbortException)
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

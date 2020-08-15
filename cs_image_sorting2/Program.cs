using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_image_sorting2
{
    static class Program
    {
        public static cs_image_sorting2.Setting.Setting setting;
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

            setting = new cs_image_sorting2.Setting.Setting();
            setting.preview = true;
            setting.size = 100;
            setting.load_num = 2000;
            setting = (cs_image_sorting2.Setting.Setting)NagisaLibrary.BinaryIO.FerstLoad((string)Properties.Settings.Default["SettingFile"], setting);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}

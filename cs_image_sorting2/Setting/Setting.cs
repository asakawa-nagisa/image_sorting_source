using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace cs_image_sorting2.Setting
{
    [Serializable()]
    public class Setting
    {
        /// <summary>
        /// 
        /// </summary>
        public bool preview { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int load_num { get; set; }
    }
}

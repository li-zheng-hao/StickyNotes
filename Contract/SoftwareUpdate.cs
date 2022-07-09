using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class HistoryData
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string history_text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int major_version_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int minor_version_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int revision_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string software_name { get; set; }
    }

    public class SoftwareUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string software_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string update_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int download_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int major_version_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int minor_version_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int revision_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string patch_file_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string download_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<HistoryData> history_text { get; set; }
    }

  
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTool
{
    public class Author
    {
        /// <summary>
        /// 
        /// </summary>
        public string login { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string node_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avatar_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gravatar_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string html_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string followers_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string following_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gists_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string starred_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subscriptions_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string organizations_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string repos_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string events_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string received_events_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string site_admin { get; set; }
    }

    public class Uploader
    {
        /// <summary>
        /// 
        /// </summary>
        public string login { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string node_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avatar_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gravatar_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string html_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string followers_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string following_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gists_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string starred_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subscriptions_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string organizations_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string repos_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string events_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string received_events_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string site_admin { get; set; }
    }

    public class AssetsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string node_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string label { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Uploader uploader { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int download_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string created_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updated_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string browser_download_url { get; set; }
    }

    public class GithubReleaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string assets_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string upload_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string html_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Author author { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string node_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tag_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string target_commitish { get; set; }
        /// <summary>
        /// 问题修复+功能更新
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string draft { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prerelease { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string created_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string published_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AssetsItem> assets { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tarball_url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string zipball_url { get; set; }
    
        public string body { get; set; }
        }

}

using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class HttpHelper
    {
        public static string BaseUrl ;
        public static ResponseResult HttpGet(string url, string[] paramsName=null, object[]paramsVal=null)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest(url);
            if (paramsName.Length != paramsVal.Length)
                throw new Exception("参数个数不一致");
            if (paramsName != null)
            {
                for (int i = 0; i < paramsName.Length; i++)
                {
                    request.AddParameter(paramsName[i], paramsVal[i]);
                }
            }
            var response = client.Get<ResponseResult>(request);
            return response.Data;
        }
        // http download file from url
        public static void HttpDownloadFile(string url, string fileName,bool useBaseUrl=false)
        {
            RestClient client;
            if (useBaseUrl == false)
            {
                client = new RestClient();
            }
            else
            {
                client = new RestClient(BaseUrl);
            }
            var request = new RestRequest(url);
            var response = client.Get(request);
            var file = new System.IO.FileInfo(fileName);
            file.Directory.Create();
            System.IO.File.WriteAllBytes(fileName, response.RawBytes);
        }

        // dynamic to class object
        public static T DynamicToObject<T>(dynamic dynamicObject)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dynamicObject));
        }
    }

    public class ResponseResult
    {
        public bool success { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }
        public int code { get; set; }
    }

    
}

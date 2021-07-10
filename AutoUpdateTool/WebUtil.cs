using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoUpdateTool
{
    public class WebUtil
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.

        public static async Task<string> HttpGet(string url)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return "";
            }
        }

        public static async Task<bool> HttpDownLoadFile(string url,string fileName)
        {
            var uri = new Uri(url);
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri);
            File.Delete(Directory.GetCurrentDirectory()+"\\"+fileName);
            using (var fs = new FileStream(
               Directory.GetCurrentDirectory()+"\\"+fileName,
                FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }

            return true;
        }
    }
}

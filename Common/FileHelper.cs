using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileHelper
    {
        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        /// <summary>
        /// 解压缩zip文件 然后解压缩到对应的
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="outputFolder"></param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void Decompress(string zipFile, string outputFolder)
        {
            if (!File.Exists(zipFile))
            {
                throw new FileNotFoundException("The zip file was not found.", zipFile);
            }
            if (!Directory.Exists(outputFolder))
            {
                
                Directory.CreateDirectory(outputFolder);
            }
            using (ZipArchive archive = ZipFile.OpenRead(zipFile))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name == "") continue;
                    entry.ExtractToFile(Path.Combine(outputFolder, entry.Name), true);
                }
            }
        }
    }
}

using StickyNotes.Utils;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StickyNotes
{
    public static class XMLHelper
    {
        /// <summary>
        /// 将对象序列化为指定的文件名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool SaveObjAsXml<T>(T obj, string fileName)
        {

            //var dir = Application.StartupPath;
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                if (File.Exists(dir + "/" + fileName))
                {
                    File.Copy(dir + "/" + fileName, dir + "/" + "temp.xml", true);

                }
                FileStream fs = new FileStream(dir + "/" + fileName, FileMode.Create);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(fs, obj);
                fs.Flush();
                fs.Close();
                if (File.Exists(dir + "/" + fileName))
                {
                    File.Delete(dir + "/" + "temp.xml");
                }
                return true;
            }
            catch (Exception e)
            {
                if (File.Exists(dir + "/" + "temp.xml"))
                {
                    File.Copy(dir + "/" + "temp.xml", dir + "/" + fileName, true);
                    File.Delete(dir + "/" + "temp.xml");
                }
                string errStr = "定时存储数据时发生异常,异常内容为:" + e.Message;
                Logger.Log().Error(errStr);
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// 将xml文件进行反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DecodeXML<T>(string fileName)
        {
            var dir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location);
            fileName = dir  +"/"+ fileName;
            try
            {
                if (File.Exists(fileName) == false)
                    return default(T);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                T obj = (T)xs.Deserialize(fs);
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }

        }
    }
}

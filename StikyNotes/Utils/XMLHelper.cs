using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using StikyNotes.Utils;

namespace StikyNotes
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
        public static bool SaveObjAsXml<T>(T obj,string fileName)
        {

            var dir = Application.StartupPath;
            try
            {
                FileStream fs = new FileStream(dir+"/"+fileName, FileMode.Create);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(fs, obj);
                fs.Flush();
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
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
            var dir = Application.StartupPath;
            fileName = dir + "/" + fileName;
            try
            {
                if (File.Exists(fileName)==false)
                    return default(T);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                T obj = (T)xs.Deserialize(fs);
                return obj;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
            
        }
    }
}

using DB;
using Newtonsoft.Json;
using StickyNotes.Utils;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StickyNotes
{
    public static class DataHelper
    {
        /// <summary>
        /// 序列化数据并存储到数据库中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool SaveData<T>(T obj)
        {
            
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                

                var str=JsonConvert.SerializeObject(obj);
                var service=new ProgramDataService();
                var db=new ProgramDB();
                db.CreateTime = DateTime.Now;
                db.Data = str;
                service.Insert(db);
                return true;
            }
            catch (Exception e)
            {
                string errStr = "定时存储数据时发生异常,异常内容为:" + e.Message;
                Logger.Log().Error(errStr);
                return false;
            }
        }

        /// <summary>
        /// 从数据库读取数据并反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T RestoreData<T>()
        {
            try
            {
                var service = new ProgramDataService();
                var db=service.GetNewestDb();
                if (db == null)
                    return default(T);
                var obj=JsonConvert.DeserializeObject<T>(db.Data);
                return obj;
            }
            catch (Exception ex)
            {
                Logger.Log().Error(ex.Message);
                return default(T);
            }

        }
    }
}

using DB;
using Newtonsoft.Json;
using StickyNotes.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
        public static async Task<bool> SaveDataAsync(ProgramData obj)
        {
            
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                List<WindowsDataDB> windowsDataDBs = new List<WindowsDataDB>();
                foreach (var item in obj.Datas)
                {
                    windowsDataDBs.Add(new WindowsDataDB() { Data = item.RichTextBoxContent,WindowId=item.WindowID });
                    item.RichTextBoxContent = null;
                }
                var str =JsonConvert.SerializeObject(obj);
                var service=new ProgramDataService();
                var db=new ProgramDB();
                db.CreateTime = DateTime.Now;
                db.Data = str;
                var programDataId=await service.InsertReturnIdentityAsync(db);
                windowsDataDBs.ForEach(it => it.ProgramDataId = programDataId);
                await new WindowsDataService().InsertRangeAsync(windowsDataDBs);
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
        public static async Task<ProgramData> RestoreDataAsync()
        {
            try
            {
                var service = new ProgramDataService();
                var db=service.GetNewestDb();
                var windowsDataDBs=await new WindowsDataService().GetWindowsDataByProgramDataId(db.Id);
               
                if (db == null)
                    return default(ProgramData);
                var programData=JsonConvert.DeserializeObject<ProgramData>(db.Data);
                foreach(var item in programData.Datas)
                {
                    var data = windowsDataDBs.Find(it => it.WindowId == item.WindowID);
                    if(data != null)
                        item.RichTextBoxContent = data.Data;
                }
                return programData;
            }
            catch (Exception ex)
            {
                Logger.Log().Error(ex.Message);
                return default(ProgramData);
            }

        }

        
    }
}

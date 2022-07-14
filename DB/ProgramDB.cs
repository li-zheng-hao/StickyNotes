using log4net;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class ProgramDataService : Repository<ProgramDB>
    {

        /// <summary>
        /// 获取最新的数据
        /// </summary>
        /// <returns></returns>
        public ProgramDB GetNewestDb()
        {
            try
            {
                var db = base.AsSugarClient().Queryable<ProgramDB>().OrderByDescending(d => d.CreateTime).First();
                return db;

            }
            catch
            {
                return null;
            }
        }

        // delete data yestaday
        public bool DeleteByDate(DateTime time)
        {
            try
            {
                //var db = base.AsSugarClient().Deleteable<ProgramDB>().Where(it => it.CreateTime < time).ExecuteCommand();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("DB").Error(ex.Message);
                return false;
            }
          
        }
       
    }
    [SugarIndex("index_programdb_createtime", nameof(ProgramDB.CreateTime), OrderByType.Asc)]
    public class ProgramDB
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// ProgramData序列化之后的值 
        /// </summary>
        public string Data { get; set; }


        public DateTime CreateTime { get; set; }
    }

   
}

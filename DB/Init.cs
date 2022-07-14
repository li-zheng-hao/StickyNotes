using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public  class DBInit
    {
        public static void InitDB()
        {
            var programDataService = new ProgramDataService();
            //建库
            programDataService.AsSugarClient().DbMaintenance.CreateDatabase();
            //建表
            programDataService.AsSugarClient().CodeFirst.InitTables<ProgramDB>();
            programDataService.AsSugarClient().CodeFirst.InitTables<WindowsDataDB>();
            programDataService.AsSugarClient().CodeFirst.InitTables<Versions>();            
        }
    }
}

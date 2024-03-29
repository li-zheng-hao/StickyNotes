﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class WindowsDataService:Repository<WindowsDataDB>
    {
        public async Task<List<WindowsDataDB>> GetWindowsDataByProgramDataId(int programDataId)
        {
            return await base.AsSugarClient().Queryable<WindowsDataDB>().Where(it => it.ProgramDataId == programDataId).ToListAsync();
        }
    }
    public class WindowsDataDB
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// ProgramData序列化之后的值 
        /// </summary>
        [SqlSugar.SugarColumn(ColumnDataType ="blob")]
        public byte[] Data { get; set; }

        public int ProgramDataId { get; set; }

        public string WindowId { get; set; }
    }
}

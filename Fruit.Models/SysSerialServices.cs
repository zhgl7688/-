using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Models
{
    public class SysSerialServices
    {

        /// <summary>
        /// 为指定表请求新的序列号
        /// </summary>
        /// <param name="db">数据上下文</param>
        /// <param name="table">应用表名</param>
        /// <param name="cycle">周期值，当此值不为空同时与当前最后周期值不同时序列号重新从1开始</param>
        /// <param name="updateSerial">是否保存序列号的更新</param>
        /// <returns>新的序列号</returns>
        public int GetNewSerial(SysContext db, string table, int? cycle = null, bool updateSerial = true)
        {
            var serial = db.sys_serial.Find(table);
            if (serial == null)
            {
                if (updateSerial)
                {
                    serial = new sys_serial { Table = table, Cycle = cycle, Serial = 1 };
                    db.sys_serial.Add(serial);
                    db.SaveChanges();
                }
                return 1;
            }
            if (serial.Cycle != cycle)
            {
                if (updateSerial)
                {
                    serial.Cycle = cycle;
                    serial.Serial = 1;
                    db.SaveChanges();
                }
                return 1;
            }
            serial.Serial++;
            if (updateSerial)
            {
                db.SaveChanges();
            }
            return serial.Serial;
        }


        /// <summary>
        /// 为指定表请求新的序列号
        /// </summary>
        /// <param name="table">应用表名</param>
        /// <param name="cycle">周期值，当此值不为空同时与当前最后周期值不同时序列号重新从1开始</param>
        /// <param name="updateSerial">是否保存序列号的更新</param>
        /// <returns>新的序列号</returns>
        public int GetNewSerial(string table, int? cycle = null, bool updateSerial = true)
        {
            using (var db = new SysContext())
            {
                return GetNewSerial(db, table, cycle, updateSerial);
            }
        }
    }
}

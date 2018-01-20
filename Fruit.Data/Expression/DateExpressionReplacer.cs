using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Data
{
    public class DateExpressionReplacer : IFruitExpressionReplacer
    {
        public bool CanHandle(string name, object context)
        {
            switch(name)
            {
                case "年":
                case "年份":
                case "月":
                case "月份":
                case "日":
                case "日期":
                case "月度":
                    return true;
                default:
                    return false;
            }
        }

        public string Replace(string name, object context)
        {
            switch (name)
            {
                case "年":
                case "年份":
                    return DateTime.Now.Year.ToString();
                case "月":
                case "月份":
                    return DateTime.Now.Month.ToString("D2");
                case "日":
                case "日期":
                    return DateTime.Now.Day.ToString("D2");
                case "月度":
                    return DateTime.Now.ToString("yyyyMM");
                default:
                    throw new Exception(name + " 不是已知的可识别变量名！");
            }
        }
    }
}

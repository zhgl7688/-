using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fruit.Web.Models
{
    /// <summary>
    /// 表示 attendTrack_v 视图的查询返回数据模型
    /// </summary>
    public class AttendTrack
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string OrganizeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal TckLong { get; set; }
        public decimal TckLat { get; set; }
        public string Address { get; set; }
        public string Tp { get; set; }
    }
}
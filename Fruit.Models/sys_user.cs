using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fruit.Models
{
    [Serializable]
    public partial class sys_user : ISerializable
    {
        protected sys_user(SerializationInfo info, StreamingContext context)
        {
            UserCode = info.GetString("UserCode");
            UserName = info.GetString("UserName");
            Description = info.GetString("Description");
            try
            {
                UserSeq = info.GetString("UserSeq");
                CompCode = info.GetString("CompCode");
                IsEnable = info.GetBoolean("IsEnable");
                RoleName = info.GetString("RoleName");
                Mobile = info.GetString("Mobile");
                OrganizeName = info.GetString("OrganizeName");
                ConfigJSON = info.GetString("ConfigJSON");
                LoginCount = info.GetInt32("LoginCount");
                LastLoginDate = info.GetDateTime("LastLoginDate");
            }
            catch { }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UserCode", UserCode);
            info.AddValue("UserSeq", UserSeq);
            info.AddValue("CompCode", UserSeq);
            info.AddValue("UserName", UserName);
            info.AddValue("IsEnable", IsEnable);
            info.AddValue("Description", Description);
            info.AddValue("RoleName", RoleName);
            info.AddValue("Mobile", Mobile);
            info.AddValue("OrganizeName", OrganizeName);
            info.AddValue("ConfigJSON", ConfigJSON);
            info.AddValue("LoginCount", LoginCount);
            info.AddValue("LastLoginDate", LastLoginDate);
        }
    }
}

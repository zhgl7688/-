using Fruit.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Fruit.Models
{
    public class SysUserService
    {
        private static readonly object _lockObj = new object();
        private static IReadOnlyList<sys_session> _cacheSysSessions = null;
        private static ConnectionStringSettings _defaultConnection = null;

        private static IReadOnlyList<sys_session> SysSessions
        {
            get
            {
                if (_cacheSysSessions == null)
                {
                    lock (_lockObj)
                    {
                        if (_cacheSysSessions == null)
                        {
                            using (var db = new SysContext())
                            {
                                _cacheSysSessions = db.sys_session.AsNoTracking().OrderBy(s => s.Sort).ToList();
                            }
                        }
                    }
                }
                return _cacheSysSessions;
            }
        }

        private static ConnectionStringSettings DefaultConnection
        {
            get
            {
                if (_defaultConnection == null)
                {
                    lock (_lockObj)
                    {
                        if (_defaultConnection == null)
                        {
                            foreach (ConnectionStringSettings connStr in ConfigurationManager.ConnectionStrings)
                            {
                                if (connStr.Name != "SysContext" && connStr.Name != "MmsContext" && connStr.Name.EndsWith("Context"))
                                {
                                    _defaultConnection = connStr;
                                    break;
                                }
                            }
                        }
                    }
                }
                return _defaultConnection;
            }
        }

        public sys_user Login(string user, string pass)
        {
            sys_user u = null;
            using (var db = new SysContext())
            {
                u = db.sys_user.Find(user);
                if (u != null)
                {
                    if (!u.Password.Equals(pass))
                    {
                        // 密码错误
                        u = null;
                    }
                    //else if (u.IsEnable.HasValue && !u.IsEnable.Value)
                    else if (!u.IsEnable)
                    {
                        // 帐户未启用
                        u = null;
                    }
                }

                if (u != null)
                {
                    u.LoginCount++;
                    u.LastLoginDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return u;
        }

        public void OnLogined(sys_user user)
        {
            // 记录登录日志
            using (var db = new SysContext())
            {
                var hostIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string clientName = null;
                try
                {
                    System.Net.IPAddress address = System.Net.IPAddress.Parse(hostIp);
                    System.Net.IPHostEntry ipInfor = System.Net.Dns.GetHostEntry(address);
                    clientName = ipInfor.HostName;
                }
                catch { }
                db.sys_loginHistory.Add(new sys_loginHistory
                {
                    UserCode = user.UserCode,
                    UserName = user.UserName,
                    HostIP = hostIp,
                    HostName = clientName,
                    LoginDate = DateTime.Now
                });
                db.SaveChanges();
            }

            // 执行 Session 初始化
            Dictionary<string, OneConnection> openConnections = new Dictionary<string, OneConnection>();
            foreach (var s in SysSessions)
            {
                try
                {
                    // 准备连接
                    var connName = string.IsNullOrEmpty(s.Connection) ? DefaultConnection.Name : s.Connection;
                    OneConnection conn = null;
                    if (openConnections.ContainsKey(connName))
                    {
                        conn = openConnections[connName];
                    }
                    else
                    {
                        conn = openConnections[connName] = CreateOneConnection(connName);
                    }

                    if (!string.IsNullOrEmpty(s.Condition))
                    {
                        // 进行条件测试
                        object val = conn.ExecuteScalar(FruitExpression.Replace(s.Condition));
                        if (val is bool && (bool)val == false)
                        {
                            continue;
                        }
                    }

                    if(!string.IsNullOrEmpty(s.T_SQL))
                    {
                        // 运行查询
                        object val = conn.ExecuteScalar(FruitExpression.Replace(s.T_SQL));
                        if (val != DBNull.Value && val != null)
                        {
                            HttpContext.Current.Session[s.SessionName] = val.ToString();
                            continue;
                        }
                    }

                    if(!string.IsNullOrEmpty(s.DefaultValue))
                    {
                        // 默认值
                        HttpContext.Current.Session[s.SessionName] = FruitExpression.Replace(s.DefaultValue);
                    }
                }
                catch (Exception e) {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
            // 关闭所有打开过的连接
            foreach(var conn in openConnections.Values){
                conn.Dispose();
            }
        }

        private OneConnection CreateOneConnection(string connName)
        {
            foreach (ConnectionStringSettings connStr in ConfigurationManager.ConnectionStrings)
            {
                if(connStr.Name == connName) {
                    return OneConnectionFactories.GetConnection(connStr.ProviderName, connStr.ConnectionString);
                }
            }
            throw new Exception(connName + " 不是已知的连接字符串名称！");
        }

        /// <summary>
        /// 测试指定用户是否有特定的菜单权限
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public bool MenuVisible(string userCode, string menuCode)
        {
            var db = new SysContext();
            return db.sys_roleMenuMap.Any(rm => rm.MenuCode == menuCode && rm.sys_role.sys_userRoleMap.Any(ur => ur.UserCode == userCode));
        }
    }
}

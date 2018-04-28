using Fruit.Models;
using Fruit.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fruit.Web.Controllers
{
    public partial class PluginsController
    {

        private static void RegisterLookupConfig()
        {
            LookupConfigs = new Dictionary<string, LookupConfig>();
            // POPUP 用户选择 START
            LookupConfigs.Add("用户选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_name"">业务员编号:</label><input id=""form_name"" class=""easyui-textbox"" data-bind=""value:'form.name'"" /><label for=""form_psncode"">业务员名称:</label><input id=""form_psncode"" class=""easyui-textbox"" data-bind=""value:'form.psncode'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/用户选择'"" style=""height:300px;"" data-value-fields=""psncode"" data-list-fields=""name,psncode"" data-return-fields=""""><thead><tr><th field=""name"">业务员编号</th><th field=""psncode"">业务员名称</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new LUOLAI1401Context())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        sbCondition.Append(string.Format("{0}{1}", "CompCode=", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode));
                        sbCondition.Append(" AND ");
                        SerachCondition.TextBox(sbCondition, "name", "name", "varchar");
                        SerachCondition.TextBox(sbCondition, "psncode", "psncode", "varchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<PersonInfo>(db.Database, "*", "PersonInfo", sbCondition.ToString(), "name", "desc");
                    }
                }
            });
            // POPUP 用户选择 END
            // POPUP 选择产品 START
            LookupConfigs.Add("选择产品", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_PdtCode"">产品编码:</label><input id=""form_PdtCode"" class=""easyui-textbox"" data-bind=""value:'form.PdtCode'"" /><label for=""form_PdtName"">产品名称:</label><input id=""form_PdtName"" class=""easyui-textbox"" data-bind=""value:'form.PdtName'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/选择产品'"" style=""height:300px;"" data-value-fields=""PdtCode,PdtName,RetailPrc"" data-list-fields=""PdtCode,PdtName,RetailPrc,TradPrc,BaseUnits"" data-return-fields=""PdtCode,PdtName,OrdPrice""><thead><tr><th field=""PdtCode"">产品编码</th><th field=""PdtName"">产品名称</th><th field=""RetailPrc"">零售价</th><th field=""TradPrc"">批发价</th><th field=""BaseUnits"">规格</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new LUOLAI1401Context())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "PdtCode", "PdtCode", "varchar");
                        SerachCondition.TextBox(sbCondition, "PdtName", "PdtName", "varchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<base_productInfo>(db.Database, "*", "base_productInfo", sbCondition.ToString(), "PdtCode", "desc");
                    }
                }
            });
            // POPUP 选择产品 END
            // POPUP 客户类型 START
            LookupConfigs.Add("客户类型", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Text"">Text:</label><input id=""form_Text"" class=""easyui-textbox"" data-bind=""value:'form.Text'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/客户类型'"" style=""height:300px;"" data-value-fields=""Value"" data-list-fields=""Text"" data-return-fields=""""><thead><tr><th field=""Text"">Text</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SysContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        sbCondition.Append(string.Format("{0}", "CodeType='QualityRequire'"));
                        sbCondition.Append(" AND ");
                        SerachCondition.TextBox(sbCondition, "Text", "Text", "varchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<sys_code>(db.Database, "*", "sys_code", sbCondition.ToString(), "Text", "desc");
                    }
                }
            });
            // POPUP 客户类型 END
            // POPUP 所属角色 START
            LookupConfigs.Add("所属角色", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_RoleCode"">角色编号:</label><input id=""form_RoleCode"" class=""easyui-textbox"" data-bind=""value:'form.RoleCode'"" /><label for=""form_RoleName"">角色名称:</label><input id=""form_RoleName"" class=""easyui-textbox"" data-bind=""value:'form.RoleName'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/所属角色'"" style=""height:300px;"" data-value-fields=""RoleCode"" data-list-fields=""RoleCode,RoleName"" data-return-fields=""""><thead><tr><th field=""RoleCode"">角色编号</th><th field=""RoleName"">角色名称</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SysContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        sbCondition.Append(string.Format("{0}{1}{2}", "CompCode='", System.Web.HttpContext.Current.Session["CompCode"], "'"));
                        sbCondition.Append(" AND ");
                        SerachCondition.TextBox(sbCondition, "RoleCode", "RoleCode", "varchar");
                        SerachCondition.TextBox(sbCondition, "RoleName", "RoleName", "varchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<sys_role>(db.Database, "*", "sys_role", sbCondition.ToString(), "RoleCode", "desc");
                    }
                }
            });
            // POPUP 所属角色 END
            // POPUP GUID START
            LookupConfigs.Add("GUID", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_name"">名称:</label><input id=""form_name"" class=""easyui-textbox"" data-bind=""value:'form.name'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/GUID'"" style=""height:300px;"" data-value-fields=""id"" data-list-fields=""id,name"" data-return-fields=""id""><thead><tr><th field=""id"">编号</th><th field=""name"">名称</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new LUOLAI1401Context())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "name", "name", "nvarchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<a_guid>(db.Database, "*", "a_guid", sbCondition.ToString(), "id", "desc");
                    }
                }
            });
            // POPUP GUID END
            // POPUP 商品分类选择 START
            LookupConfigs.Add("商品分类选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Name"">商品分类:</label><input id=""form_Name"" class=""easyui-textbox"" data-bind=""value:'form.Name'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/商品分类选择'"" style=""height:300px;"" data-value-fields=""Code"" data-list-fields=""Name"" data-return-fields=""ProductCategory""><thead><tr><th field=""Name"">商品分类</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SMTERPContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "Name", "Name", "nvarchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<tbBdProductCategory>(db.Database, "*", "tbBdProductCategory", sbCondition.ToString(), "Name", "desc");
                    }
                }
            });
            // POPUP 商品分类选择 END
            // POPUP 仓库-专卖店选择 START
            LookupConfigs.Add("仓库-专卖店选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Name"">仓库-专卖店选择:</label><input id=""form_Name"" class=""easyui-textbox"" data-bind=""value:'form.Name'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/仓库-专卖店选择'"" style=""height:300px;"" data-value-fields=""Code"" data-list-fields=""Name"" data-return-fields=""Warehouse,ParentCode""><thead><tr><th field=""Name"">仓库-专卖店选择</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SMTERPContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "Name", "Name", "nvarchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<tbBdWarehouse>(db.Database, "*", "tbBdWarehouse", sbCondition.ToString(), "Name", "desc");
                    }
                }
            });
            // POPUP 仓库-专卖店选择 END
            // POPUP 供应商选择 START
            LookupConfigs.Add("供应商选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Name"">供应商:</label><input id=""form_Name"" class=""easyui-textbox"" data-bind=""value:'form.Name'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/供应商选择'"" style=""height:300px;"" data-value-fields=""Code"" data-list-fields=""Name"" data-return-fields=""Supplier""><thead><tr><th field=""Name"">供应商</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SMTERPContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "Name", "Name", "nvarchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<tbBdSupplier>(db.Database, "*", "tbBdSupplier", sbCondition.ToString(), "Name", "desc");
                    }
                }
            });
            // POPUP 供应商选择 END
            // POPUP 单位选择 START
            LookupConfigs.Add("单位选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Name"">单位选择:</label><input id=""form_Name"" class=""easyui-textbox"" data-bind=""value:'form.Name'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/单位选择'"" style=""height:300px;"" data-value-fields=""Code"" data-list-fields=""Name"" data-return-fields=""Unit""><thead><tr><th field=""Name"">单位选择</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SMTERPContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "Name", "Name", "nvarchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<tbBdUnit>(db.Database, "*", "tbBdUnit", sbCondition.ToString(), "Name", "desc");
                    }
                }
            });
            // POPUP 单位选择 END
            // POPUP 采购员选择 START
            LookupConfigs.Add("采购员选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Name"">采购员:</label><input id=""form_Name"" class=""easyui-textbox"" data-bind=""value:'form.Name'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/采购员选择'"" style=""height:300px;"" data-value-fields=""PeopleCode"" data-list-fields=""Name"" data-return-fields=""Litigant""><thead><tr><th field=""Name"">采购员</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SMTERPContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "Name", "Name", "nvarchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<tbBdLitigant>(db.Database, "*", "tbBdLitigant", sbCondition.ToString(), "Name", "desc");
                    }
                }
            });
            // POPUP 采购员选择 END
            // POPUP 仓库 START
            LookupConfigs.Add("仓库", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Name"">仓库名称:</label><input id=""form_Name"" class=""easyui-textbox"" data-bind=""value:'form.Name'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/仓库'"" style=""height:300px;"" data-value-fields=""Code,Name"" data-list-fields=""Name"" data-return-fields=""DrawingPath,DrawingPath_RefText""><thead><tr><th field=""Name"">仓库名称</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SMTERPContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "Name", "Name", "nvarchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<tbBdWarehouse>(db.Database, "*", "tbBdWarehouse", sbCondition.ToString(), "Name", "desc");
                    }
                }
            });
            // POPUP 仓库 END
            // POPUP 选择客户 START
            LookupConfigs.Add("选择客户", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_PopCode"">客户名称:</label><input id=""form_PopCode"" class=""easyui-textbox"" data-bind=""value:'form.PopCode'"" /><label for=""form_PopName"">客户编号:</label><input id=""form_PopName"" class=""easyui-textbox"" data-bind=""value:'form.PopName'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/选择客户'"" style=""height:300px;"" data-value-fields=""PopCode"" data-list-fields=""PopCode,PopName"" data-return-fields=""""><thead><tr><th field=""PopCode"">客户名称</th><th field=""PopName"">客户编号</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new LUOLAI1401Context())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        sbCondition.Append(string.Format("{0}{1}", "CompCode=", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode));
                        sbCondition.Append(" AND ");
                        SerachCondition.TextBox(sbCondition, "PopCode", "PopCode", "varchar");
                        SerachCondition.TextBox(sbCondition, "PopName", "PopName", "varchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<wq_termPop>(db.Database, "*", "wq_termPop", sbCondition.ToString(), "PopCode", "desc");
                    }
                }
            });
            // POPUP 选择客户 END
            // POPUP 所属组织 START
            LookupConfigs.Add("所属组织", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_OrganizeCode"">组织编号:</label><input id=""form_OrganizeCode"" class=""easyui-textbox"" data-bind=""value:'form.OrganizeCode'"" /><label for=""form_OrganizeName"">组织名称:</label><input id=""form_OrganizeName"" class=""easyui-textbox"" data-bind=""value:'form.OrganizeName'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/所属组织'"" style=""height:300px;"" data-value-fields=""OrganizeCode"" data-list-fields=""OrganizeCode,OrganizeName"" data-return-fields=""""><thead><tr><th field=""OrganizeCode"">组织编号</th><th field=""OrganizeName"">组织名称</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SysContext())
                    {
                        var _input = System.Web.HttpContext.Current.Request["_input"];
                        var sbCondition = new System.Text.StringBuilder();
                        sbCondition.Append(string.Format("{0}{1}", "CompCode=", (System.Web.HttpContext.Current.Session["sys_user"] as sys_user).CompCode));
                        sbCondition.Append(" AND ");
                        if(string.IsNullOrEmpty(_input))
                        {
                            SerachCondition.TextBox(sbCondition, "OrganizeCode", "OrganizeCode", "varchar");
                            SerachCondition.TextBox(sbCondition, "OrganizeName", "OrganizeName", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "OrganizeCode", "varchar", false, true);
                            SerachCondition.TextBox(sbCondition, "_input", "OrganizeName", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<sys_organize>(db.Database, "*", "sys_organize", sbCondition.ToString(), "OrganizeCode", "desc");
                    }
                }
            });
            // POPUP 所属组织 END
            // POPUP 性别 START
            LookupConfigs.Add("性别", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Text"">Text:</label><input id=""form_Text"" class=""easyui-textbox"" data-bind=""value:'form.Text'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/性别'"" style=""height:300px;"" data-value-fields=""Value"" data-list-fields=""Text"" data-return-fields=""""><thead><tr><th field=""Text"">Text</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SysContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        sbCondition.Append(string.Format("{0}", "CodeType='Sex'"));
                        sbCondition.Append(" AND ");
                        SerachCondition.TextBox(sbCondition, "Text", "Text", "varchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<sys_code>(db.Database, "*", "sys_code", sbCondition.ToString(), "Text", "desc");
                    }
                }
            });
            // POPUP 性别 END
            // POPUP 产品选择 START
            LookupConfigs.Add("产品选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Name"">产品编号:</label><input id=""form_Name"" class=""easyui-textbox"" data-bind=""value:'form.Name'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/产品选择'"" style=""height:300px;"" data-value-fields=""Guid"" data-list-fields=""Name"" data-return-fields=""""><thead><tr><th field=""Name"">产品编号</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SMTERPContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        SerachCondition.TextBox(sbCondition, "Name", "Name", "nvarchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<tbBdProduct>(db.Database, "*", "tbBdProduct", sbCondition.ToString(), "Name", "desc");
                    }
                }
            });
            // POPUP 产品选择 END
            // POPUP 终端类型 START
            LookupConfigs.Add("终端类型", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Text"">Text:</label><input id=""form_Text"" class=""easyui-textbox"" data-bind=""value:'form.Text'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/终端类型'"" style=""height:300px;"" data-value-fields=""Text"" data-list-fields=""Text"" data-return-fields=""""><thead><tr><th field=""Text"">Text</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new SysContext())
                    {
                        var sbCondition = new System.Text.StringBuilder();
                        sbCondition.Append(string.Format("{0}", "CodeType='Kz_zd'"));
                        sbCondition.Append(" AND ");
                        SerachCondition.TextBox(sbCondition, "Text", "Text", "varchar");
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<sys_code>(db.Database, "*", "sys_code", sbCondition.ToString(), "Text", "desc");
                    }
                }
            });
            
             
            
            // POPUP 订单选择 START
            LookupConfigs.Add("订单选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_selleruserid"">卖家会员:</label><input id=""form_selleruserid"" class=""easyui-textbox"" data-bind=""value:'form.selleruserid'"" /><label for=""form_ordid"">订单号:</label><input id=""form_ordid"" class=""easyui-textbox"" data-bind=""value:'form.ordid'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/订单选择'"" style=""height:300px;"" data-value-fields=""ordid"" data-list-fields=""selleruserid,ordid,buyeruserid"" data-return-fields=""""><thead><tr><th field=""selleruserid"">卖家会员</th><th field=""ordid"">订单号</th><th field=""buyeruserid"">买家会员</th></tr></thead></table></div>
<script>
var db = new fruit.databind($('#popupgridbox'));
</script>";
                },
                GetData = (pageReq) =>
                {
                    using(var db = new LUOLAI1401Context())
                    {
                        var _input = System.Web.HttpContext.Current.Request["_input"];
                        var sbCondition = new System.Text.StringBuilder();
                        if(string.IsNullOrEmpty(_input))
                        {
                            SerachCondition.TextBox(sbCondition, "selleruserid", "selleruserid", "char");
                            SerachCondition.TextBox(sbCondition, "ordid", "ordid", "char");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "selleruserid", "char", false, true);
                            SerachCondition.TextBox(sbCondition, "_input", "ordid", "char", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<fw_orderinfo>(db.Database, "*", "fw_orderinfo", sbCondition.ToString(), "selleruserid", "desc");
                    }
                }
            });
            // POPUP 订单选择 END
            // -- INSERT POINT --
        }
    }
}
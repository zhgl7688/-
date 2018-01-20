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
            // POPUP 终端类型 END
            // POPUP 员工 START
            LookupConfigs.Add("员工", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_FID"">编号:</label><input id=""form_FID"" class=""easyui-textbox"" data-bind=""value:'form.FID'"" /><label for=""form_empID"">姓名:</label><input id=""form_empID"" class=""easyui-textbox"" data-bind=""value:'form.empID'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/员工'"" style=""height:300px;"" data-value-fields=""FID"" data-list-fields=""FID,empID"" data-return-fields=""""><thead><tr><th field=""FID"">编号</th><th field=""empID"">姓名</th></tr></thead></table></div>
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
                            SerachCondition.TextBox(sbCondition, "FID", "FID", "int");
                            SerachCondition.TextBox(sbCondition, "empID", "empID", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "FID", "int", false, true);
                            SerachCondition.TextBox(sbCondition, "_input", "empID", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<HR_PractiseCerts>(db.Database, "*", "HR_PractiseCerts", sbCondition.ToString(), "FID", "desc");
                    }
                }
            });
            // POPUP 员工 END
            // POPUP 合同选择 START
            LookupConfigs.Add("合同选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Code"">Code:</label><input id=""form_Code"" class=""easyui-textbox"" data-bind=""value:'form.Code'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/合同选择'"" style=""height:300px;"" data-value-fields=""Code"" data-list-fields=""Code"" data-return-fields=""""><thead><tr><th field=""Code"">Code</th></tr></thead></table></div>
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
                            SerachCondition.TextBox(sbCondition, "Code", "Code", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "Code", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<PM_Contracts>(db.Database, "*", "PM_Contracts", sbCondition.ToString(), "Code", "desc");
                    }
                }
            });
            // POPUP 合同选择 END
            // POPUP 合作单位选择 START
            LookupConfigs.Add("合作单位选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_FID"">编号:</label><input id=""form_FID"" class=""easyui-textbox"" data-bind=""value:'form.FID'"" /><label for=""form_Contact"">联系人:</label><input id=""form_Contact"" class=""easyui-textbox"" data-bind=""value:'form.Contact'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/合作单位选择'"" style=""height:300px;"" data-value-fields=""FID"" data-list-fields=""FID,Contact,Address"" data-return-fields=""""><thead><tr><th field=""FID"">编号</th><th field=""Contact"">联系人</th><th field=""Address"">地址</th></tr></thead></table></div>
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
                            SerachCondition.TextBox(sbCondition, "FID", "FID", "bigint");
                            SerachCondition.TextBox(sbCondition, "Contact", "Contact", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "FID", "bigint", false, true);
                            SerachCondition.TextBox(sbCondition, "_input", "Contact", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<BD_Customers>(db.Database, "*", "BD_Customers", sbCondition.ToString(), "FID", "desc");
                    }
                }
            });
            // POPUP 合作单位选择 END
            // POPUP 项目选择 START
            LookupConfigs.Add("项目选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_ProjName"">工程名称:</label><input id=""form_ProjName"" class=""easyui-textbox"" data-bind=""value:'form.ProjName'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/项目选择'"" style=""height:300px;"" data-value-fields=""Code"" data-list-fields=""ProjName"" data-return-fields=""""><thead><tr><th field=""ProjName"">工程名称</th></tr></thead></table></div>
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
                            SerachCondition.TextBox(sbCondition, "ProjName", "ProjName", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "ProjName", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<PM_ProjectInfos>(db.Database, "*", "PM_ProjectInfos", sbCondition.ToString(), "ProjName", "desc");
                    }
                }
            });
            // POPUP 项目选择 END
            // POPUP 证书选择 START
            LookupConfigs.Add("证书选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_certNo"">证书编号:</label><input id=""form_certNo"" class=""easyui-textbox"" data-bind=""value:'form.certNo'"" /><label for=""form_certOrgan"">发证机关:</label><input id=""form_certOrgan"" class=""easyui-textbox"" data-bind=""value:'form.certOrgan'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/证书选择'"" style=""height:300px;"" data-value-fields=""certNo"" data-list-fields=""certNo,certOrgan"" data-return-fields=""certNO""><thead><tr><th field=""certNo"">证书编号</th><th field=""certOrgan"">发证机关</th></tr></thead></table></div>
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
                            SerachCondition.TextBox(sbCondition, "certNo", "certNo", "varchar");
                            SerachCondition.TextBox(sbCondition, "certOrgan", "certOrgan", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "certNo", "varchar", false, true);
                            SerachCondition.TextBox(sbCondition, "_input", "certOrgan", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<HR_PractiseCerts>(db.Database, "*", "HR_PractiseCerts", sbCondition.ToString(), "certNo", "desc");
                    }
                }
            });
            // POPUP 证书选择 END
            // POPUP 承包合同 START
            LookupConfigs.Add("承包合同", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Code"">合同号:</label><input id=""form_Code"" class=""easyui-textbox"" data-bind=""value:'form.Code'"" /><label for=""form_Corp"">单位:</label><input id=""form_Corp"" class=""easyui-textbox"" data-bind=""value:'form.Corp'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/承包合同'"" style=""height:300px;"" data-value-fields=""Code"" data-list-fields=""Code,Corp,Amt"" data-return-fields=""""><thead><tr><th field=""Code"">合同号</th><th field=""Corp"">单位</th><th field=""Amt"">造价</th></tr></thead></table></div>
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
                            SerachCondition.TextBox(sbCondition, "Code", "Code", "varchar");
                            SerachCondition.TextBox(sbCondition, "Corp", "Corp", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "Code", "varchar", false, true);
                            SerachCondition.TextBox(sbCondition, "_input", "Corp", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<PM_Contracts>(db.Database, "*", "PM_Contracts", sbCondition.ToString(), "Code", "desc");
                    }
                }
            });
            // POPUP 承包合同 END
            // POPUP 合作单位 START
            LookupConfigs.Add("合作单位", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_Contact"">Contact:</label><input id=""form_Contact"" class=""easyui-textbox"" data-bind=""value:'form.Contact'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/合作单位'"" style=""height:300px;"" data-value-fields=""FID"" data-list-fields=""Contact"" data-return-fields=""""><thead><tr><th field=""Contact"">Contact</th></tr></thead></table></div>
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
                            SerachCondition.TextBox(sbCondition, "Contact", "Contact", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "Contact", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<BD_Customers>(db.Database, "*", "BD_Customers", sbCondition.ToString(), "Contact", "desc");
                    }
                }
            });
            // POPUP 合作单位 END
            // POPUP 发票选择 START
            LookupConfigs.Add("发票选择", new LookupConfig
            {
                GetContext = () =>
                {
                    return @"<div id=""popupgridbox""><div class=""z-search""><label for=""form_invoiceTitle"">发票抬头:</label><input id=""form_invoiceTitle"" class=""easyui-textbox"" data-bind=""value:'form.invoiceTitle'"" /><label for=""form_invoiceCode"">发票号:</label><input id=""form_invoiceCode"" class=""easyui-textbox"" data-bind=""value:'form.invoiceCode'"" /><a href=""#"" class=""easyui-linkbutton"" iconCls=""icon-search"" plain=""true"" data-bind=""click:'popupSearch'"">搜索</a></div><table data-bind=""datagrid"" data-options=""border:false, rownumbers:true, pagination:true, pageSize:20, url:'/Plugins/GetLookupData/发票选择'"" style=""height:300px;"" data-value-fields=""invoiceCode"" data-list-fields=""invoiceTitle,invoiceCode,Amt,invoiceContent"" data-return-fields=""invoiceCode""><thead><tr><th field=""invoiceTitle"">发票抬头</th><th field=""invoiceCode"">发票号</th><th field=""Amt"">开票金额</th><th field=""invoiceContent"">开票内容</th></tr></thead></table></div>
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
                            SerachCondition.TextBox(sbCondition, "invoiceTitle", "invoiceTitle", "varchar");
                            SerachCondition.TextBox(sbCondition, "invoiceCode", "invoiceCode", "varchar");
                        }
                        else
                        {
                            sbCondition.Append(" ( ");
                            SerachCondition.TextBox(sbCondition, "_input", "invoiceTitle", "varchar", false, true);
                            SerachCondition.TextBox(sbCondition, "_input", "invoiceCode", "varchar", false, true);
                            sbCondition.Length-=5;
                            sbCondition.Append(" )     ");
                        }
                        if(sbCondition.Length > 5)
                        {
                            sbCondition.Length-=5;
                        }
                        return pageReq.ToPageList<FA_Invoices>(db.Database, "*", "FA_Invoices", sbCondition.ToString(), "invoiceTitle", "desc");
                    }
                }
            });
            // POPUP 发票选择 END
            // -- INSERT POINT --
        }
    }
}
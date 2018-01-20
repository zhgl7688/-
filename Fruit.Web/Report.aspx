<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Fruit.Web.Report" %>
<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="frp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        html,body{margin:0;padding:0;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <frp:WebReport ID="WebReport1" runat="server" AutoWidth="true" AutoHeight="true" Height="100%" Width="100%" LocalizationFile="~/Localization/Chinese (Simplified).frl" OnStartReport="WebReport1_StartReport" />
    </div>
    </form>
</body>
</html>

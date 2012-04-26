<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Survey.WebPages.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            font-family: Arial;
            font-size: large;
        }
    
        .style2
        {
            width: 203px;
            height: 183px;
            margin-left: 0px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        <span class="notranslate"><span class="style1">There was a problem processing you request.</span>
        </span>
    </p>
    <p>
        <span class="notranslate">&nbsp;</span></p>
    <p>
        <img alt="Parks and Recreations" class="style2" 
            src="../Content/Images/logo.png" /></p>
    <div>
    
    </div>
    <span class="notranslate">
        <input type="button" 
        onclick="window.opener=null; window.close(); return false;" align="middle" 
        value="Close this window" />
    </span>
    </form>
</body>
</html>


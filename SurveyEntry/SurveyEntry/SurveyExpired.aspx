﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyExpired.aspx.cs" Inherits="SurveyEntry.SurveyExpired" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 220px;
        }
        .style2
        {
            width: 220px;
            height: 176px;
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
        <span class="notranslate"><span class="style1"> The time allotted to respond to this survey has expired.</span>
        </span>
    </p>
    <p>
        <span class="notranslate">&nbsp;</span></p>
    <p>
        <img alt="Parks and Recreation" class="style2" 
            longdesc="Parks and Recreation Logo" src="Styles/Images/logo.png" /></p>
    <div>
    
    </div>
    <h1 style="margin-left: 520px">
        <span class="notranslate">
        <input type="button" 
            onclick="window.opener=null; window.close(); return false;" 
            value="Close this window" />
        </span>
    </h1>
    </form>
</body>

</html>

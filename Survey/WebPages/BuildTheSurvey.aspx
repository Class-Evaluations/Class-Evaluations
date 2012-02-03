<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuildTheSurvey.aspx.cs" Inherits="Survey.WebPages.BuildTheSurvey" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   
    <style type="text/css">
        .style1
        {
            text-align: center;
        }
        .style2
        {
            width: 139px;
            height: 123px;
            margin-left: 0px;
            margin-top: 0px;
        }
        .style3
        {
            font-size: x-large;
        }
        .style4
        {
            font-size: medium;
        }
        .style5
        {
            text-align: center;
            font-size: small;
        }
        .style6
        {
            font-size: large;
        }
        .style7
        {
            font-weight: bold;
            font-size: medium;
        }
        .style8
        {
            text-align: center;
            font-size: medium;
        }
    </style>
    </head>
 <body>
    <form id="frmSurvey" runat="server">
        <div id = "grid"  align="center" style="font-family: Arial; font-size: medium">
            <asp:Label ID="LabelTitle" runat="server" Text="SurveyHeader"></asp:Label>        
        </div>
        <p></p>
        <p></p>
        <div id = "grid" align="left" style="font-family: Arial; font-size: medium">
            <asp:Label ID="lbprogramName" runat="server" Text="Name of the Program:"></asp:Label>
            <asp:TextBox ID="txtprogramName" runat="server" Width="338px" 
                BorderStyle="Solid" BorderWidth="1px">
                </asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lbfacilityUsed" runat="server" Text="Facility Used:"></asp:Label>
            <asp:TextBox ID="txtfacilityUsed" runat="server" Width="338px" 
                BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
            <br />
           <asp:Label ID="lbprogramDates" runat="server" Text="Dates of Program(mm/dd/yyyy):"></asp:Label>
           <asp:TextBox ID="txtprogramDates" runat="server" Width="338px" 
                BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
            <br />
            <hr style="height: -15px" />
        </div>
        <p></p>
        <p></p>
       <div id = "grid" align="left" style="font-family: Arial; font-size: small">
            <asp:Label ID="LabelIntroduction" runat="server" Text="SurveyIntroduction"></asp:Label>               
            <p>&nbsp;</div>
            <div>
                <br />

                <asp:Panel ID="panelContent" runat="server"></asp:Panel>
                <asp:PlaceHolder
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                <br />
        </div>

    <p>
        <span class="notranslate">&nbsp;</span></p>
    <p>
        <img alt="Parks and Recreations" class="style2" 
            src="../Content/Images/logo.png" /></p>


        <hr style="height: 2px; margin-top: 8px" />
        <p class="style1" style="font-weight: 700">
            <asp:Button ID="Button2" runat="server" Text="Submit Survey" 
                onclick="Button2_Click"></asp:Button>
        </p>
    <p class="style1" style="font-weight: 700">
        Thank you for helping Raleigh Parks and Recreation Server You Better!</p>
    <p class="style8" style="font-style: italic">
        If you would like us to respond to any concern, please provide your name and 
        phone number.</p>
    <p class="style1" style="font-style: italic">
        <span class="style4">We will be happy to follow up with you!&nbsp;&nbsp;</span><asp:TextBox 
            ID="txtpersonInformation" runat="server" Width="338px" 
                BorderStyle="Solid" BorderWidth="1px" CssClass="style7"></asp:TextBox></p>
    <p class="style5" 
        style="font-family: Arial; ">
        2401 WADE AVENUE&nbsp;&nbsp; *&nbsp;&nbsp;&nbsp; RALEIGH,&nbsp; NORTH CAROLINA&nbsp; 27607&nbsp;&nbsp;&nbsp; 
        *&nbsp;&nbsp;&nbsp; (919)831-6640&nbsp;&nbsp;&nbsp; *&nbsp;&nbsp;&nbsp; WEBSITE: PARKS.RALEIGHNC.GOV</p>
</body>
</form>
<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation = "false" CodeBehind="BuildTheSurvey.aspx.cs" Inherits="SurveyEntry.BuildTheSurvey" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html>
    <head id="Head1" runat="server">
   
    <style type="text/css">
        .style1 
        {
            text-align: center;
        }
        .style5
        {
            text-align: center;
            font-size: small;
        }
        .style9
        {
            width: 220px;
            height: 176px;
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
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                <br />
        </div>

    <p>
        <span class="notranslate">&nbsp;</span></p>
    <p align="center">
        <img alt="Parks and Recreation" class="style9" src="../Styles/Images/logo.png" /></p>


        <p align="center"> 
           &nbsp;<asp:Button ID="Button2" runat="server" Text="Submit Survey" onclick="Button2_Click"></asp:Button>
        </p>
    <p class="style1" style="font-weight: 700" align="center">
        Thank you for helping Raleigh Parks and Recreation Server You Better!</p>
        <p class="style1" style="font-weight: 700">
            &nbsp;</p>
        <p class="style1" style="font-style: italic">
            &nbsp;</p>
        <p class="style1" style="font-style: italic">
            &nbsp;</p>
    <p class="style5" 
        style="font-family: Arial; ">
        2401 WADE AVENUE&nbsp;&nbsp; *&nbsp;&nbsp;&nbsp; RALEIGH,&nbsp; NORTH CAROLINA&nbsp; 27607&nbsp;&nbsp;&nbsp; 
        *&nbsp;&nbsp;&nbsp; (919)831-6640&nbsp;&nbsp;&nbsp; *&nbsp;&nbsp;&nbsp; WEBSITE: PARKS.RALEIGHNC.GOV</p>
    </body>
</form>
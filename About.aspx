<%@ Page Title="WeR Support Tool" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="WebApplication1.About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1>        
    </hgroup>

    <br />

    <asp:Label runat="server" ID="lblCaption" Font-Bold="true" ForeColor="DarkBlue">Enter WeR Site URL:</asp:Label>                 
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:TextBox ID="txtBoxServerURL" runat="server"></asp:TextBox>
    

    <asp:Button runat="server" ID="Button1" Font-Bold="true" Text="Start Test" OnClick="cmdCHeck_Click"
                 style="top: 0px; left: 47px; position: relative; height: 34px; width: 120px;"/>    

    <br />
    <br />
    <br />
    <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true"></asp:Label>
    <br />
            <asp:Label runat="server" ID="TestResults" Font-Bold="true" Font-Underline="true" Font-Size="X-Large" ForeColor="DarkBlue">Test Results</asp:Label>                 
    <p>&nbsp;</p>
            <asp:Label runat="server" ID="ConnectionStatus" Font-Bold="true" Font-Size="Large" ForeColor="DarkBlue">Ping Status</asp:Label>                 
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;                 
            <asp:Image runat="server" src="" id="imgConnectionStatus" style="width:30px; height:30px"  alt="Connection Result" width="130"/>
            <br />
    <br />
            <asp:Label runat="server" ID="PortStatus" Font-Bold="true" Font-Size="Large" ForeColor="DarkBlue">Port Status</asp:Label>                 
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;                 
            <asp:Image runat="server" src="" id="imgPortStatus" style="width:30px; height:30px"  alt="Connection Result" width="130"/>
            <br />
    <br />
            <asp:Label runat="server" ID="LocalFirewall" Font-Bold="true" Font-Size="Large" ForeColor="DarkBlue">Firewall Enabled</asp:Label>                 
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;                 
            <asp:Image runat="server" src="" id="imgFirewallStatus" style="width:35px; height:35px"  alt="Connection Result" width="130"/>


    <br />
    <br />
    <br />
    <br />


</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .auto-style1 {
            text-decoration: underline;
        }
    </style>
</asp:Content>


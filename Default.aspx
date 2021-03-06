﻿<%@ Page Title="Essence Support Tools" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2>Welcome to Essence Group Support Center</h2>
            </hgroup>
            <p>
                To learn more about company solutions, visit <a href="http://www.essence-grp.com/home" title="Essence Group Website">Essence Group Website</a>.
                <br><br>
                If you have any questions about our solutions, send us <a href="mailto:ProfessionalServices@essence-grp.com" title="Email">Email</a>.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>Please choose one of the following options to troobleshoot Essence solution:</h3>
    <ol class="round">
        <li class="one">
            <h5>Download client for troubleshooting</h5>
            Click the following link to download Essence Troobleshooting tool to local PC
            <a href="Client/EssenceSupportTool.exe">Download</a>
            <br><br><br>
        </li>
        <li class="two">
            <h5>Open Essence Troobleshooting page to check online</h5>
            Open Essence Troobleshooting page to check online.
            <a href="SupportTools.aspx">Continue</a>            
             <br><br><br>
        </li>        
        <li class="three">
            <h5>Download ECOP Real Time Parser</h5>
            Click the following link to download ECOP Real Time Parser tool to local PC
            <a href="Client/EcopRealTimeParser.exe">Download</a>
            <br><br><br>            
        </li>        
    </ol>
</asp:Content>

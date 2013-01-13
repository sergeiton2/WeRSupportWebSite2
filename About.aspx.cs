using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Net.Sockets;
using NetFwTypeLib;
using NATUPNPLib;
using NETCONLib;
using System.Diagnostics;


namespace WebApplication1
{    
    public partial class About : System.Web.UI.Page
    {
        private const string CLSID_FIREWALL_MANAGER = "{304CE942-6E39-40D8-943A-B913C40C9CD4}";
        private const string PROGID_AUTHORIZED_APPLICATION = "HNetCfg.FwAuthorizedApplication";
        private const string PROGID_OPEN_PORT = "HNetCfg.FWOpenPort";

        public int portno = 43001;

        protected void Page_Load(object sender, EventArgs e)
        {
            TestResults.Visible = false;
            ConnectionStatus.Visible = false;
            PortStatus.Visible = false;
            imgConnectionStatus.Visible = false;
            imgPortStatus.Visible = false;
            LocalFirewall.Visible = false;
            imgFirewallStatus.Visible = false;

        }
        protected void cmdCHeck_Click(object sender, EventArgs e)
        {
            string serverURL = txtBoxServerURL.Text;
            lblMessage.Text = "";

            TestResults.Visible = false;
            ConnectionStatus.Visible = false;
            PortStatus.Visible = false;
            imgConnectionStatus.Visible = false;
            imgPortStatus.Visible = false;
            LocalFirewall.Visible = false;
            imgFirewallStatus.Visible = false;

            if (serverURL.Length > 1)
            {
                lblMessage.Text = "Please wait. Testing connectivity to WeR Server\n";


                bool isPingable = isSitePingable(addHttp(serverURL));

                lblMessage.Text =  lblMessage.Text + "Testing port connectivity to WeR Server\n";

                bool isPortOpen = checkPortConnectivity(removeHttp(serverURL));

                lblMessage.Text = "";

                bool isFWEnabled = checkFirewall();

                TestResults.Visible = true;
                ConnectionStatus.Visible = true;
                PortStatus.Visible = true;
                imgConnectionStatus.Visible = true;
                imgPortStatus.Visible = true;

                if (isPingable)
                    imgConnectionStatus.Attributes["src"] = "Images/status_ok1.png";
                else
                    imgConnectionStatus.Attributes["src"] = "Images/status_error1.jpg";

                if (isPortOpen)
                    imgPortStatus.Attributes["src"] = "Images/status_ok1.png";
                else
                    imgPortStatus.Attributes["src"] = "Images/status_error1.jpg";

                if (!isPortOpen)
                {
                    LocalFirewall.Visible = true;
                    imgFirewallStatus.Visible = true;

                    if (isFWEnabled)
                        imgFirewallStatus.Attributes["src"] = "Images/firewallEnabled.png";
                    else
                        imgFirewallStatus.Attributes["src"] = "Images/firewallDisabled.png";
                }


            }
            else
                lblMessage.Text = "Server URL is too short!";
        }

        
        
        
        public bool isSitePingable(string serverURL)
        {
            WebClient workstation = new WebClient();

            byte[] data = null;            
            try
            {
                data = workstation.DownloadData(serverURL);
            }
            catch (Exception ex)
            {
                return false;
            }

            if (data != null && data.Length > 0)
                return true;
            else
                return false;            
        }


        public bool checkPortConnectivity(string serverURL)
        {                        
            try
            {
                // Short version - was not tested
                //TcpClient tcpClient = new TcpClient();
                //tcpClient.Connect(serverURL, 43001);

                IPAddress ipa = (IPAddress)Dns.GetHostAddresses(serverURL)[0];

                System.Net.Sockets.Socket sock =
                    new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                  System.Net.Sockets.SocketType.Stream,
                                                  System.Net.Sockets.ProtocolType.Tcp);
                sock.Connect(ipa, portno);
                if (sock.Connected == true) // Port is in use and connection is successful
                    return true;
                sock.Close();

            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (ex.ErrorCode == 10061) // Port is unused and could not establish connection 
                    return true;
            }            

            return false;

        }

        public string removeHttp(string url) 
        {
            string returnURL = url;

            if (url.Contains("http://"))
                returnURL = returnURL.Replace("http://", "");
            if (url.Contains("https://"))
                returnURL = returnURL.Replace("http://", "");

            return returnURL;
        }

        public string addHttp(string url)
        {
            string returnURL = url;

            if (url.Contains("http://") || url.Contains("https://"))
                return returnURL;
            else
                returnURL = "http://" + url;
            
            return returnURL;
        }

        public bool checkFirewall()
        {
            INetFwMgr manager = GetFirewallManager();
            bool isFirewallEnabled = manager.LocalPolicy.CurrentProfile.FirewallEnabled;
            if (isFirewallEnabled == true)
            {
                return true;                
            }
            return false;
        }
                       
        private static NetFwTypeLib.INetFwMgr GetFirewallManager()
        {
            Type objectType = Type.GetTypeFromCLSID(new Guid(CLSID_FIREWALL_MANAGER));
            return Activator.CreateInstance(objectType) as NetFwTypeLib.INetFwMgr;
        }
               
        public bool AuthorizeApplication(string title, string applicationPath, NET_FW_SCOPE_ scope, NET_FW_IP_VERSION_ ipVersion)
        {      
            // Create the type from prog id  
            Type type = Type.GetTypeFromProgID(PROGID_AUTHORIZED_APPLICATION);  
            INetFwAuthorizedApplication auth = Activator.CreateInstance(type) as INetFwAuthorizedApplication;  
            auth.Name  = title;  
            auth.ProcessImageFileName = applicationPath;  
            auth.Scope = scope;  
            auth.IpVersion = ipVersion;  
            auth.Enabled = true;

            INetFwMgr manager = GetFirewallManager(); 
            try 
            { 
                manager.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(auth); 
            }
            catch (Exception ex) 
            { 
                return false; 
            } 
            
            return true;
        }

        public bool OpenInboundPort()
        {  
            Type type = Type.GetTypeFromProgID(PROGID_OPEN_PORT);  
            INetFwOpenPort port = Activator.CreateInstance(type) as INetFwOpenPort;  
            port.Name = "WeRServer";
            port.Port = portno;  
            //port.Scope = scope;  
            port.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            //port.IpVersion = ipVersion;  
            
            INetFwMgr manager = GetFirewallManager();  
            
            try  
            {    
                manager.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(port);  
            }  
            catch (Exception ex)  
            {    
                return false;  
            }

            return true;
        }

        public bool OpenOutboundRule()
        {
            bool isInvoked = false;            

            Process netshProgram = new Process();
            netshProgram.StartInfo.FileName = "netsh";
            netshProgram.StartInfo.Arguments = " advfirewall firewall add rule name=\"WeR-Port\" protocol=TCP dir=out localport=43001 action=allow";
            netshProgram.Start();

            return isInvoked;
        }        
    }
}
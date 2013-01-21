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
            lbl_WebServiceResponse.Visible = false;
            imgWebServiceStatus.Visible = false;

        }
        protected void cmdCHeck_Click(object sender, EventArgs e)
        {

            btn_startTest.Text = "Test Running";
            btn_startTest.Enabled = false;
            string serverURL = txtBoxServerURL.Text;
            lblMessage.Text = "";
            bool isPortOpen, isWebServiceAck, isFWEnabled = false;

            TestResults.Visible = false;
            ConnectionStatus.Visible = false;
            PortStatus.Visible = false;
            imgConnectionStatus.Visible = false;
            imgPortStatus.Visible = false;
            LocalFirewall.Visible = false;
            imgFirewallStatus.Visible = false;
            lbl_WebServiceResponse.Visible = false;
            imgWebServiceStatus.Visible = false;

            if (serverURL.Length > 1)
            {
                lblMessage.Text = "Please wait. Testing connectivity to Essence Server\n";


                bool isPingable = isSitePingable(removeHttp(serverURL));

                lblMessage.Text = lblMessage.Text + "Testing port connectivity to Essence Server\n";

                if (isPingable)
                {
                    // Not relevant - We have a test for port response
                    isWebServiceAck = isWebServiceResponses(addHttp(serverURL));
                    lblMessage.Text = lblMessage.Text + "Testing Essence web service response \n";

                    isPortOpen = checkPortConnectivity(removeHttp(serverURL));

                    lblMessage.Text = "";

                    isFWEnabled = checkFirewall();                    
                }
                else
                {
                    isWebServiceAck = false;
                    isPortOpen = false;

                }
                

                TestResults.Visible = true;
                ConnectionStatus.Visible = true;
                PortStatus.Visible = true;
                imgConnectionStatus.Visible = true;
                imgPortStatus.Visible = true;
                lbl_WebServiceResponse.Visible = true;
                imgWebServiceStatus.Visible = true;

                if (isPingable)
                    imgConnectionStatus.Attributes["src"] = "Images/status_ok1.png";
                else
                    imgConnectionStatus.Attributes["src"] = "Images/status_error1.jpg";

                if (isWebServiceAck)
                    imgWebServiceStatus.Attributes["src"] = "Images/status_ok1.png";
                else
                    imgWebServiceStatus.Attributes["src"] = "Images/status_error1.jpg";

                if (isPortOpen)
                    imgPortStatus.Attributes["src"] = "Images/status_ok1.png";
                else
                    imgPortStatus.Attributes["src"] = "Images/status_error1.jpg";

                if ( isPingable && (!isPortOpen))
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

            btn_startTest.Text = "Start Test";
            btn_startTest.Enabled = true;
        }

        
        
        
        public bool isSitePingable(string serverURL)
        {
            var ping = new System.Net.NetworkInformation.Ping();

            var result = ping.Send(serverURL);

            if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                return false;
            else
                return true;
        }



        public bool isWebServiceResponses(string serverURL)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(serverURL);
            request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
            request.Timeout = 2000;
            request.Method = "HEAD";
            try
            {
                var response = request.GetResponse();
                return true;
                // do something with response.Headers to find out information about the request
            }
            catch (WebException wex)
            {
                //set flag if there was a timeout or some other issues
                return false;
            }
        }

        public bool checkPortConnectivity(string serverURL)
        {                        
            try
            {
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
            port.Name = "EssenceServer";
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
            netshProgram.StartInfo.Arguments = " advfirewall firewall add rule name=\"Essence-Port\" protocol=TCP dir=out localport=43001 action=allow";
            netshProgram.Start();

            return isInvoked;
        }        
    }
}
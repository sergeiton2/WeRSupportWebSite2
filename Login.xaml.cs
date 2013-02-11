using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Windows; 
using System.Windows.Controls; 
using System.Windows.Data; 
using System.Windows.Documents; 
using System.Windows.Input; 
using System.Windows.Media; 
using System.Windows.Media.Imaging; 
using System.Windows.Navigation; 
using System.Windows.Shapes; 
using System.Data; 
using System.Data.SqlClient; 
using System.Text.RegularExpressions; 

namespace Netorious.AppWindows
{ 
    /// <summary> 
    /// Interaction logic for MainWindow.xaml 
    /// </summary> 
  
    public partial class Login : Window 
    { 

        public Login() 
        { 
            InitializeComponent();            
        } 

        Registration registration = new Registration();

        private void button1_Click(object sender, RoutedEventArgs e) 
        { 
            if (textBoxEmail.Text.Length == 0) 
            { 
                errormessage.Text = "Enter an email."; 
                textBoxEmail.Focus(); 
            } 
            else if (!textBoxEmail.Text.Equals("1") && (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
            { 
                errormessage.Text = "Enter a valid email."; 
                textBoxEmail.Select(0, textBoxEmail.Text.Length); 
                textBoxEmail.Focus(); 

            }
            else if ((comboBoxDatabase.SelectedValue as ListBoxItem) == null)
            {
                errormessage.Text = "Select database from provided list"; 
            }
            else 
            {
                // Save global variables with connection string to database selected from combo for future nhibernate use when initiation session
                if ((comboBoxDatabase.SelectedValue as ListBoxItem).Content.ToString().Equals("mydb"))
                {
                    Application.Current.Properties["HOSTNAME"] = "localhost";
                    Application.Current.Properties["DBSID"] = "mydb";
                    Application.Current.Properties["DBUSER"] = "root";
                    Application.Current.Properties["DBPASSWORD"] = "varase";
                }
                else if ((comboBoxDatabase.SelectedValue as ListBoxItem).Content.ToString().Equals("mydbTest"))
                {
                    Application.Current.Properties["HOSTNAME"] = "localhost";
                    Application.Current.Properties["DBSID"] = "mydbTest";
                    Application.Current.Properties["DBUSER"] = "root";
                    Application.Current.Properties["DBPASSWORD"] = "varase";
                }
                
                string email = textBoxEmail.Text; 
                string password = passwordBox1.Password;
                UserIdentity userInstance = new UserIdentity();

                bool isUserAuthenticated = nhibernategateway.ValidateUserCredentials(email,password, out userInstance);

                if (isUserAuthenticated) 
                {
                    Application.Current.Properties["LoggedUserID"] = userInstance.id.ToString();
                    Application.Current.Properties["LoggedUserEmail"] = userInstance.email.ToString();
                                        
                    MainConsole myMainWindow = new MainConsole();
                    System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                    System.Windows.Application.Current.MainWindow = myMainWindow;
                    
                    myMainWindow.Show(); 
                    Close(); 
                } 
                else 
                { 
                    errormessage.Text = "Please enter existing Email/Password."; 
                }                 
            } 
        } 

        private void buttonRegister_Click(object sender, RoutedEventArgs e) 
        { 
            registration.Show(); 
            Close(); 
        }

        
        private void OpenNewDBConnectionWindow(object sender, SelectionChangedEventArgs e)
        {
            if ((comboBoxDatabase.SelectedValue as ListBoxItem).Content.ToString().Contains("Create new connection"))
            {

                var dialog = new MyDialog();
                if (dialog.ShowDialog() == true)
                {                    
                    string newHostname = dialog.txtBoxNewDBHostname.Text;
                    string newSID = dialog.txtBoxNewDBSID.Text;
                    string newUsername = dialog.txtBoxNewDBUsername.Text;
                    string newPassword = dialog.txtBoxNewDBPassword.Text;

                    // Shoud save to xml file containing all connections. Later!!!
                } 
            }
        } 
    } 
}


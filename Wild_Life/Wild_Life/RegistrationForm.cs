using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;

namespace Wild_Life
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
            passwordtxtB.PasswordChar = '*';
            confPWtxtB.PasswordChar = '*';
        }

        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "TNUkyLlzJLn3xoiJxjrRTWLrCKtqwa4LzItRC1Tb",
            BasePath = "https://wildlife1-29704-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch (Exception)
            {

                MessageBox.Show("Currently you are facing a connection issue");
            }
        }

        private void Register_Click(object sender, EventArgs e)
        {
            #region Condition
            if (string.IsNullOrWhiteSpace(usernametxtB.Text) && string.IsNullOrWhiteSpace(passwordtxtB.Text) && string.IsNullOrWhiteSpace(confPWtxtB.Text))
            {
                MessageBox.Show("Please fill All fields");
                return;
            }
            
            else if (passwordtxtB.Text != confPWtxtB.Text)
            {
                MessageBox.Show("Password Didnt Match");
                return;
            }
            #endregion

            MyUser user = new MyUser()
            {
                UserName = usernametxtB.Text,
                Password = passwordtxtB.Text,
            };       
            SetResponse set = client.Set(@"Admin_Users/" + usernametxtB.Text, user);
            MessageBox.Show("Successfully Registerd");
            usernametxtB.Clear();
            passwordtxtB.Clear();
            confPWtxtB.Clear();
            Login loginWindow = new Login();
            loginWindow.ShowDialog();
            this.Close();
        }
    }
}

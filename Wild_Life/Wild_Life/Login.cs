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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            passwordtxtB.PasswordChar = '*';
        }

        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "TNUkyLlzJLn3xoiJxjrRTWLrCKtqwa4LzItRC1Tb",
            BasePath = "https://wildlife1-29704-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        private void Login_Load(object sender, EventArgs e)
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

        private void BtnSignIn_Click(object sender, EventArgs e)
        {
            #region Condition
            if (string.IsNullOrWhiteSpace(usernametxtB.Text) && string.IsNullOrWhiteSpace(passwordtxtB.Text))
            {
                MessageBox.Show("Please fill All fields");
                return;
            }
            #endregion

            FirebaseResponse res = client.Get(@"Admin_Users/" + usernametxtB.Text);
            MyUser ResUser = res.ResultAs<MyUser>();
            MyUser CurUser = new MyUser()
            {
                UserName = usernametxtB.Text,
                Password = passwordtxtB.Text
            };
            if (MyUser.IsEqual(ResUser, CurUser))
            {
                usernametxtB.Clear();
                passwordtxtB.Clear();
                this.Hide();
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Credentials you have inserted is invalied!");
                return;
            }
        }

        private void Register_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegistrationForm reg = new RegistrationForm();
            reg.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PasswordtxtB_TextChanged(object sender, EventArgs e)
        {

        }

        private void Password_Click(object sender, EventArgs e)
        {

        }

        private void UserNametxtB_TextChanged(object sender, EventArgs e)
        {

        }

        private void User_Name_Click(object sender, EventArgs e)
        {

        }
    }
}

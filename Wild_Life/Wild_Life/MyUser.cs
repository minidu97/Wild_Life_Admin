using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wild_Life
{
    class MyUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        private static string error = "Communication Error";
        public static void ShowError()
        {
            System.Windows.Forms.MessageBox.Show(error);
        }
        public static bool IsEqual(MyUser user1, MyUser user2)
        {
            if (user1 == null || user2 == null)
            {
                return false;
            }
            if (user1.UserName != user2.UserName)
            {
                error = "Username Doesnt Exisist";
                return false;
            }
            else if (user1.Password != user2.Password)
            {
                error = "Username or Password Didnt match";
                return false;
            }
            return true;
        }
    }
}

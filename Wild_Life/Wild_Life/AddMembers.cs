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
    public partial class AddMembers : Form
    {
        public AddMembers()
        {
            InitializeComponent();
        }

        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "TNUkyLlzJLn3xoiJxjrRTWLrCKtqwa4LzItRC1Tb",
            BasePath = "https://wildlife1-29704-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        DataTable dt = new DataTable();

        private void AddMembers_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch (Exception)
            {

                MessageBox.Show("Currently you are facing a connection issue");
            }

            dt.Columns.Add("Employee ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Address");
            dt.Columns.Add("Email Address");
            dt.Columns.Add("Job Position");
            dt.Columns.Add("Contact Information");

            dataGridView1.DataSource = dt;
            dataimport();

        }

        private async void btnCreateUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBName.Text) && string.IsNullOrWhiteSpace(TBAddress.Text) && string.IsNullOrWhiteSpace(TBEmail.Text) && string.IsNullOrWhiteSpace(TBId.Text) && string.IsNullOrWhiteSpace(TBJob_P.Text) && string.IsNullOrWhiteSpace(TBContact.Text))
            {
                MessageBox.Show("Please fill All fields");
                return;
            }

            var data = new Data
            {
                Name = TBName.Text,
                Address = TBAddress.Text,
                Email = TBEmail.Text,
                EmpId = TBId.Text,
                Position = TBJob_P.Text,
                ContactInfo = TBContact.Text
            };

            SetResponse response = await client.SetAsync(@"Employees/" + TBId.Text, data);
            Data result = response.ResultAs<Data>();
            MessageBox.Show("Member Added Successfully");
            TBName.Clear();
            TBAddress.Clear();
            TBEmail.Clear();
            TBId.Clear();
            TBJob_P.Clear();
            TBContact.Clear();
            dataimport();
        }

        private async void btnCheckUsr_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetAsync(@"Employees/" + TBId.Text);
            Data obj = response.ResultAs<Data>();
            TBName.Text = obj.Name;
            TBAddress.Text = obj.Address;
            TBEmail.Text = obj.Email;
            TBId.Text = obj.EmpId;
            TBJob_P.Text = obj.Position;
            TBContact.Text = obj.ContactInfo;

            MessageBox.Show("Member Information retrived Successfully");
        }

        private async void btnUpdate_user_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBName.Text) && string.IsNullOrWhiteSpace(TBAddress.Text) && string.IsNullOrWhiteSpace(TBEmail.Text) && string.IsNullOrWhiteSpace(TBId.Text) && string.IsNullOrWhiteSpace(TBJob_P.Text) && string.IsNullOrWhiteSpace(TBContact.Text))
            {
                MessageBox.Show("Please fill All fields");
                return;
            }
            var data = new Data
            {
                Name = TBName.Text,
                Address = TBAddress.Text,
                Email = TBEmail.Text,
                EmpId = TBId.Text,
                Position = TBJob_P.Text,
                ContactInfo = TBContact.Text
            };

            FirebaseResponse response = await client.UpdateAsync(@"Employees/" + TBId.Text, data);
            Data result = response.ResultAs<Data>();
            MessageBox.Show("Member Updated Successfully");
            TBName.Clear();
            TBAddress.Clear();
            TBEmail.Clear();
            TBId.Clear();
            TBJob_P.Clear();
            TBContact.Clear();
            dataimport();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteAsync(@"Employees/" + TBId.Text);
            MessageBox.Show("Member Id "+ TBId.Text+" Deleted Successfully");
            TBName.Clear();
            TBAddress.Clear();
            TBEmail.Clear();
            TBId.Clear();
            TBJob_P.Clear();
            TBContact.Clear();
            dataimport();
        }

        private async void dataimport()
        {
            dt.Rows.Clear();
            FirebaseResponse resp1 = await client.GetAsync(@"Employees");
            Console.WriteLine(resp1.Body);
            Dictionary<string, Data> obj1 = resp1.ResultAs<Dictionary<string, Data>>();
            foreach (var item in obj1.Values)
            {
                    DataRow row = dt.NewRow();
                    row["Employee ID"] = item.EmpId;
                    row["Name"] = item.Name;
                    row["Address"] = item.Address;
                    row["Email Address"] = item.Email;
                    row["Job Position"] = item.Position;
                    row["Contact Information"] = item.ContactInfo;
                    dt.Rows.Add(row);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataimport();
        }
    }
}

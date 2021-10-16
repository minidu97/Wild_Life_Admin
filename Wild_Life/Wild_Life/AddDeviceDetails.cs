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
    public partial class AddDeviceDetails : Form
    {
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "TNUkyLlzJLn3xoiJxjrRTWLrCKtqwa4LzItRC1Tb",
            BasePath = "https://wildlife1-29704-default-rtdb.firebaseio.com/"
        };

        public AddDeviceDetails()
        {
            InitializeComponent();
        }

        IFirebaseClient client;
        DataTable dt = new DataTable();


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AddDeviceDetails_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch (Exception)
            {

                MessageBox.Show("Currently you are facing a connection issue");
            }

            dt.Columns.Add("Device Id");
            dt.Columns.Add("Product No");
            dt.Columns.Add("Installation date");
            dt.Columns.Add("Installed Employee");
            dt.Columns.Add("Installed Block");
            
            dataGridView1.DataSource = dt;
            import();

        }

        private async void btnAddDevice_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBDeviceId.Text) && string.IsNullOrWhiteSpace(TBProductNo.Text)  && string.IsNullOrWhiteSpace(TBInstallerEmpId.Text) && string.IsNullOrWhiteSpace(TBInstallationBlock.Text))
            {
                MessageBox.Show("Please fill All fields");
                return;
            }
            var devices = new Devices
            {
                DeviceId = TBDeviceId.Text,
                ProductNo = TBProductNo.Text,
                Installationdate = dateTimePicker1.Text,
                InstallerEmpId = TBInstallerEmpId.Text,
                InstallationBlock = TBInstallationBlock.Text
            };
            
            SetResponse response = await client.SetAsync(@"Devices/" + TBDeviceId.Text, devices);
            Devices result = response.ResultAs<Devices>();
            MessageBox.Show("Device Added Successfully");
            TBDeviceId.Clear();
            TBProductNo.Clear();
            TBInstallerEmpId.Clear();
            TBInstallationBlock.Clear();
            import();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetAsync(@"Devices/" + TBDeviceId.Text);
            Devices obj = response.ResultAs<Devices>();

            TBDeviceId.Text = obj.DeviceId;
            TBProductNo.Text = obj.ProductNo;
            dateTimePicker1.Text = obj.Installationdate;
            TBInstallerEmpId.Text = obj.InstallerEmpId;
            TBInstallationBlock.Text = obj.InstallationBlock;
            

            MessageBox.Show("Device Information retrived Successfully");
            TBDeviceId.Clear();
        }

        private async void btnUpdate_device_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBDeviceId.Text) && string.IsNullOrWhiteSpace(TBProductNo.Text) && string.IsNullOrWhiteSpace(dateTimePicker1.Text) && string.IsNullOrWhiteSpace(TBInstallerEmpId.Text) && string.IsNullOrWhiteSpace(TBInstallationBlock.Text))
            {
                MessageBox.Show("Please fill All fields");
                return;
            }
            var devices = new Devices
            {
                ProductNo = TBProductNo.Text,
                DeviceId = TBDeviceId.Text,
                Installationdate = dateTimePicker1.Text,
                InstallerEmpId = TBInstallerEmpId.Text,
                InstallationBlock = TBInstallationBlock.Text
            };

            FirebaseResponse response = await client.UpdateAsync(@"Devices/" + TBDeviceId.Text, devices);
            Devices result = response.ResultAs<Devices>();
            MessageBox.Show("Device Updated Successfully");
            TBDeviceId.Clear();
            TBProductNo.Clear();
            TBInstallerEmpId.Clear();
            TBInstallationBlock.Clear();
            import();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteAsync(@"Devices/" + TBDeviceId.Text);
            MessageBox.Show(TBDeviceId.Text+" Deleted Successfully");
            TBDeviceId.Clear();
            import();
        }

        private async void import()
        {
            dt.Rows.Clear();
            FirebaseResponse resp1 = await client.GetAsync(@"Devices/");
            Console.WriteLine(resp1.Body);
            Dictionary<string, Devices> obj1 = resp1.ResultAs<Dictionary<string, Devices>>();
            
            foreach (var item in obj1.Values)
            {
                DataRow row = dt.NewRow();
                row["Device Id"] = item.DeviceId;
                row["Product No"] = item.ProductNo;
                row["Installation date"] = item.Installationdate;
                row["Installed Employee"] = item.InstallerEmpId;
                row["Installed Block"] = item.InstallationBlock;
                dt.Rows.Add(row);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            import();
        }
    }
}

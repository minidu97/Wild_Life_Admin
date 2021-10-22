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
    public partial class ViewHistory : Form
    {
        public ViewHistory()
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

        private void ViewHistory_Load(object sender, EventArgs e)
        {

            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch (Exception)
            {

                MessageBox.Show("Currently you are facing a connection issue");
            }

            comboBox1.SelectedIndex = 2;


            dt.Columns.Add("Time");
            dt.Columns.Add("Animal Name and ID");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");

            dataGridView1.DataSource = dt;
        }

        private async void btnView_Click(object sender, EventArgs e)
        {
            int curYear = DateTime.Now.Year;
            int Year = dateTimePicker1.Value.Date.Year;
            int Month = dateTimePicker1.Value.Date.Month;
            int date = dateTimePicker1.Value.Date.Day;
            if (curYear >= Year )
            {
                try
                {
                    dt.Rows.Clear();
                    FirebaseResponse resp1 = await client.GetAsync(@"animals/" + Year + "/" + Month + "/" + date);
                    //Console.WriteLine(curYear);
                    //Console.WriteLine(Month);
                    //Console.WriteLine(date);
                    //Console.WriteLine(resp1.Body);
                    Dictionary<string, Animals> obj1 = resp1.ResultAs<Dictionary<string, Animals>>();

                    if (comboBox1.SelectedIndex == 2)
                    {
                        foreach (var item in obj1.Values)
                        {
                            DataRow row = dt.NewRow();
                            row["Time"] = item.Time;
                            row["Animal Name and ID"] = item.Animal;
                            row["Latitude"] = item.Latitude;
                            row["Longitude"] = item.Longitude;
                            dt.Rows.Add(row);
                        }
                    }
                    else
                    {

                        foreach (var item in obj1.Values)
                        {
                            if (comboBox1.Text.Equals(item.Animal))
                            {
                                DataRow row = dt.NewRow();
                                row["Time"] = item.Time;
                                row["Animal Name and ID"] = item.Animal;
                                row["Latitude"] = item.Latitude;
                                row["Longitude"] = item.Longitude;
                                dt.Rows.Add(row);
                            }


                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("No Records Available");
                }
            }

            else
            {
                MessageBox.Show("Invallied Year");
            }

        }

        private void btn_excel_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application apps = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = apps.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            apps.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Exported from gird view";
            for(int i = 1; i<dataGridView1.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            }
            for(int i = 0; i<dataGridView1.Rows.Count - 1;i++)
            {
                for(int j = 0; j<dataGridView1.Columns.Count ; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }
            workbook.SaveAs2("D:\\Dataset.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            apps.Quit();
        }
    }
}

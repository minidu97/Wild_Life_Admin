using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Device.Location;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using Newtonsoft.Json;

namespace Wild_Life
{
    public partial class MainWindow : Form
    {
        private String latitude;
        private String longitude;
        private GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

        

        public MainWindow()
        {
            InitializeComponent();
        }

        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "TNUkyLlzJLn3xoiJxjrRTWLrCKtqwa4LzItRC1Tb",
            BasePath = "https://wildlife1-29704-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;


        private void btnAddMembers_Click(object sender, EventArgs e)
        {
            AddMembers AddM = new AddMembers();
            AddM.ShowDialog();
        }

        private async void btn_load_map_Click(object sender, EventArgs e)
        {
            
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            int date = DateTime.Now.Day;
            string time = DateTime.Now.ToString("HH:mm:ss");

            Console.WriteLine(Year);
            Console.WriteLine(Month);
            Console.WriteLine(date);
            Console.WriteLine(time);

            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            
            

           
            

            FirebaseResponse resp1 = await client.GetAsync(@"animals/" + Year + "/" + Month + "/12" );
            Console.WriteLine();

            List<Animals> lstAnimals = new List<Animals>();


            TimeSpan NowTimeSMin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(-30).Minute, DateTime.Now.Second);
            
            TimeSpan NowTimeSMax = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            Dictionary<string, Animals> obj1 = resp1.ResultAs<Dictionary<string, Animals>>();

            var FilteredList = obj1.Values.Where(x=> NowTimeSMin <  TimeSpan.Parse(x.Time) && NowTimeSMax > TimeSpan.Parse(x.Time)).ToList();
            

            if (FilteredList != null)
            {
                double[] lat = new double[FilteredList.Count];
                double[] longt = new double[FilteredList.Count];
                string[] names = new string[FilteredList.Count];

                for (int i = 0; i < FilteredList.Count; i++)
                {
                    lat[i] = Convert.ToDouble(FilteredList[i].Latitude);
                    longt[i] = Convert.ToDouble(FilteredList[i].Longitude);
                    names[i] = FilteredList[i].Animal;
                }

                for (int i = 0; i < FilteredList.Count; i++)
                {
                    map.Position = new PointLatLng(lat[i], longt[i]);
                    map.MinZoom = 5;
                    map.MaxZoom = 1000;
                    map.Zoom = 10;

                    PointLatLng point = new PointLatLng(lat[i], longt[i]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);

                    GMapOverlay markers = new GMapOverlay("markers");
                    marker.ToolTipText = names[i];
                    markers.Markers.Add(marker);
                    map.Overlays.Add(markers);
                }
                locate_me();
            }
            else
            {
                MessageBox.Show("No data Found within the time");
            }


     

            //Dictionary<string, Animals> obj1 = resp1.ResultAs<Dictionary<string, Animals>>();

            //foreach (var item in obj1.Values)

            //{

            //   var time1 = TimeSpan.Parse(item.Time);
            //    int H = DateTime.Now.Add(time1).Hour;
            //    int M = DateTime.Now.Add(time1).Minute;
            //    if ()
            //    {

            //    }


            //}


            
        }

        private void TBLong_TextChanged(object sender, EventArgs e)
        {

        }

        private void TBLat_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void MainWindow_Load(object sender, EventArgs e)
        {
            watcher.StatusChanged += watcher_StatusChanged;
            watcher.Start();

            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch (Exception)
            {

                MessageBox.Show("Currently you are facing a connection issue");
            }

        }
        

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            try
            {
                if (e.Status == GeoPositionStatus.Ready)
                {
                    if (watcher.Position.Location.IsUnknown)
                    {
                        latitude = "0";
                        longitude = "0";
                    }
                    else
                    {
                        latitude = watcher.Position.Location.Latitude.ToString();
                        longitude = watcher.Position.Location.Longitude.ToString();
                    }
                }
                else
                {
                    latitude = "0";
                    longitude = "0";
                }
            }
            catch (Exception)
            {

                latitude = "0";
                longitude = "0";
            }
        }

        
        public void locate_me()
        {

            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            map.MinZoom = 5;
            map.MaxZoom = 1000;
            map.Zoom = 10;

            PointLatLng point = new PointLatLng(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.yellow_dot);

            GMapOverlay markers = new GMapOverlay("markers");
            marker.ToolTipText = "My Current Location";
            markers.Markers.Add(marker);
            map.Overlays.Add(markers);
       }

        private void btnDeviceDetails_Click(object sender, EventArgs e)
        {
            AddDeviceDetails reg = new AddDeviceDetails();
            reg.ShowDialog();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            ViewHistory his = new ViewHistory();
            his.ShowDialog();
        }
    }
}

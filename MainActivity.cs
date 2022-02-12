using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Plugin.BLE;
using Android.Widget;
using Plugin.BLE.Abstractions.Exceptions;
using Android.Content;
using System.Threading;

namespace FitnessApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private static string address = "7D:59:73:FE:18:CD";
        private static Guid guid = new Guid("00000000-0000-0000-0000-7d5973fe18cd");
        private static Guid service = new Guid("00001826-0000-1000-8000-00805f9b34fb");
        private static Guid characteristic = new Guid("00002ad3-0000-1000-8000-00805f9b34fb");
        private static Guid descriptor = new Guid("00002902-0000-1000-8000-00805f9b34fb");

        

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var ble = CrossBluetoothLE.Current;
            Class1.getInstance().adapter = CrossBluetoothLE.Current.Adapter;

            Button кнопкаНачатьТренировку = FindViewById<Button>(Resource.Id.appCompatButton1);
            кнопкаНачатьТренировку.Text = "Подключаемся...";
            кнопкаНачатьТренировку.Click += КнопкаНачатьТренировку_Click;

            try
            {
                var q = await Class1.getInstance().adapter.ConnectToKnownDeviceAsync(guid);
                Console.WriteLine(" --------- Successfully connected: " + q.State);

                var _service = await q.GetServiceAsync(service);
                Class1.getInstance().deviceCharacteristic = await _service.GetCharacteristicAsync(characteristic);
                var _descriptor = await Class1.getInstance().deviceCharacteristic.GetDescriptorAsync(descriptor);
                await _descriptor.WriteAsync(new byte[] { 0x01, 0x00 });

                кнопкаНачатьТренировку.Text = "Начать тренировку";
                кнопкаНачатьТренировку.Enabled = true;
            }
            catch (DeviceConnectionException ex)
            {
                Console.WriteLine(" --------- Error while connecting: " + ex.ToString());
                кнопкаНачатьТренировку.Text = "Ошибка соединения";
                return;
            }
        }

        private void КнопкаНачатьТренировку_Click(object sender, EventArgs e)
        {
            Class1.getInstance().счётчикБицепс =
            Class1.getInstance().рукиВСторону =
            Class1.getInstance().жимНадГоловой = 0;
            Class1.getInstance().таймер = 0;

            StartActivity(typeof(Screen2Activity));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}

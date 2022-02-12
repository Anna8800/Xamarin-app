using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace FitnessApp
{
    [Activity(Label = "Screen2Activity")]
    public class Screen2Activity : Activity
    {
        TextView счётчикБицепс;
        TextView рукиВСторону;
        TextView жимНадГоловой;
        TextView таймер;

        Timer timer;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.screen2);

            счётчикБицепс = FindViewById<TextView>(Resource.Id.textView2);
            рукиВСторону = FindViewById<TextView>(Resource.Id.textView4);
            жимНадГоловой = FindViewById<TextView>(Resource.Id.textView6);
            таймер = FindViewById<TextView>(Resource.Id.textView8);
            счётчикБицепс.Text = "0";
            рукиВСторону.Text = "0";
            жимНадГоловой.Text = "0";

            Button сброситьРезультаты = FindViewById<Button>(Resource.Id.appCompatButton3);
            сброситьРезультаты.Click += delegate (object o, EventArgs e) {
                timer.Stop();
                счётчикБицепс.Text = "0";
                рукиВСторону.Text = "0";
                жимНадГоловой.Text = "0";
                таймер.Text = "0";

            };

            Button закончитьТренировку = FindViewById<Button>(Resource.Id.appCompatButton4);
            закончитьТренировку.Click += delegate (object o, EventArgs e) {
                OnBackPressed();
            };

            Class1.getInstance().deviceCharacteristic.ValueUpdated += (o, args) =>
            {
                var bytes = args.Characteristic.Value;

                switch (bytes[0])
                {
                    case 1:
                        Class1.getInstance().счётчикБицепс++;
                        счётчикБицепс.Text = Class1.getInstance().счётчикБицепс.ToString();
                        break;

                    case 2:
                        Class1.getInstance().рукиВСторону++;
                        рукиВСторону.Text = Class1.getInstance().рукиВСторону.ToString();
                        break;

                    case 3:
                        Class1.getInstance().жимНадГоловой++;
                        жимНадГоловой.Text = Class1.getInstance().жимНадГоловой.ToString();
                        break;

                    default:
                        break;
                }

                Console.WriteLine("Получили данные: " + bytes[0].ToString());
            };

            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            

            await Class1.getInstance().deviceCharacteristic.StartUpdatesAsync();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Class1.getInstance().таймер++;
            таймер.Text = Class1.getInstance().таймер.ToString();
        }
    }
}
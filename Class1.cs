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
using Plugin.BLE;

namespace FitnessApp
{
    public class Class1
    {

        private static Class1 INSTANCE = new Class1();
        public Plugin.BLE.Abstractions.Contracts.IAdapter adapter { get; set; }
        public Plugin.BLE.Abstractions.Contracts.ICharacteristic deviceCharacteristic { get; set; }
        public int счётчикБицепс { get; set; }
        public int рукиВСторону { get; set; }
        public int жимНадГоловой { get; set; }
        public long таймер { get; set; }

        private Class1() { }


        public static Class1 getInstance()
        {
            return INSTANCE;
        }
    }
}
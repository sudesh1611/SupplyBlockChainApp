using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
//using Plugin.CurrentActivity;
using Android.Views;
using Android.Widget;

namespace SupplyBlockChainApp.Droid
{
#if DEBUG
    [Application(Debuggable = true)]
#else
	[Application(Debuggable = false)]
#endif
    public class MainApplication:Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            //CrossCurrentActivity.Current.Init(this);
            //UserDialogs.Init(this);
        }
    }
}
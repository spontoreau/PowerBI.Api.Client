using Android.App;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace XamarinClient.Android
{
	[Activity(Label = "XamarinClient.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : FormsApplicationActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication (new App ());
		}
	}
}



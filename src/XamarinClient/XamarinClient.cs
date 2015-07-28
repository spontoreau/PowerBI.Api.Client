using System;

using Xamarin.Forms;
using PowerBI.Api.Client;
using PowerBI.Api.Client.Configuration;

namespace XamarinClient
{
	public class App : Application
	{
		public App()
		{
			PowerBIClient.Initialize(
					"https://api.powerbi.com/beta/myorg/datasets",
					"https://login.windows.net/common/oauth2/authorize",
					"https://analysis.windows.net/powerbi/api",
					"MyClientId",
					"MyUser",
					"MyPassword"
			);

			MainPage = new RootPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}


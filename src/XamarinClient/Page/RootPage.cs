using System;
using Xamarin.Forms;

namespace XamarinClient
{
	/// <summary>
	/// Root page.
	/// </summary>
	public class RootPage : ContentPage
	{
		public RootPage()
		{
			InitializeComponent();
			Bind();
		}

		void InitializeComponent()
		{
			var layout = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center
			};
			var button = new Button
			{
				VerticalOptions = LayoutOptions.Center,
				Text = "Insert",
				IsEnabled = false
			};
			button.SetBinding<MainViewModel>(Button.CommandProperty, vm => vm.InsertCommand);

			layout.Children.Add(button);
			Content = layout;
		}

		void Bind()
		{
			var viewModel = new MainViewModel();
			viewModel.Insert += async (sender, e) => await DisplayAlert("Insertion", "Données bien insérée !", "OK");
			BindingContext = viewModel;
		}
	}
}


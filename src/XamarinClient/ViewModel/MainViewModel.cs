using System;
using System.Windows.Input;
using PowerBI.Api.Client;
using Xamarin.Forms;

namespace XamarinClient
{
	/// <summary>
	/// Main view model.
	/// </summary>
	public class MainViewModel : ViewModel
	{
		/// <summary>
		/// Occurs when insert.
		/// </summary>
		public event EventHandler Insert;

		/// <summary>
		/// The identifier.
		/// </summary>
		static int Id = 1;

		/// <summary>
		/// The name of the dataset.
		/// </summary>
		static readonly string DatasetName = "MyProductCatalog";

		/// <summary>
		/// Gets the insert command.
		/// </summary>
		/// <value>The insert command.</value>
		public ICommand InsertCommand { get; private set; }

		public MainViewModel()
		{
			InsertCommand = new Command(x => {
				var isInsert = PowerBIClient.Do<bool>(api => {
					var isDatasetExist = api.IsDatasetExist(DatasetName);
					if(isDatasetExist)
					{
						var datasetId = api.GetDatasetId(DatasetName);

						var isObjectInsert = api.Insert(datasetId, new Product
						{
							CreationDate = DateTime.Now,
							Id = Id,
							IsAvaible = true,
							Name = "Computer" + Id,
							Price = 500.00
						});

						if(isObjectInsert)
							Id++;

						return isObjectInsert;
					}
					else
					{
						return false;
					}
				});

				if(isInsert)
					OnInsert();
			});
		}

		/// <summary>
		/// Raises the connected event.
		/// </summary>
		protected void OnInsert()
		{
			var tmp = Insert;
			if(tmp != null)
				tmp(this, EventArgs.Empty);
		}
	}
}


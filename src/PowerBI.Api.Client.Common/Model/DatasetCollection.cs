using System.Collections.Generic;

namespace PowerBI.Api.Client.Model
{
	/// <summary>
	/// Dataset collection.
	/// </summary>
	public sealed class DatasetCollection
	{
		/// <summary>
		/// Gets or sets the datasets.
		/// </summary>
		/// <value>The datasets.</value>
		public List<Dataset> Datasets { get; set; }
	}
}


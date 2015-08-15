using System.Collections.Generic;

namespace PowerBI.Api.Client.Model
{
	/// <summary>
	/// Table collection.
	/// </summary>
	public sealed class TableCollection
	{
		/// <summary>
		/// Gets or sets the datasets.
		/// </summary>
		/// <value>The datasets.</value>
		public List<Table> Tables { get; set; }
	}
}


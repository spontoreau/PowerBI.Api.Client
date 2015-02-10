using System.Collections.Generic;

namespace PowerBI.Api.Client.Schema
{
	/// <summary>
	/// Dataset shema.
	/// </summary>
	public class DatasetSchema
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the tables.
		/// </summary>
		/// <value>The tables.</value>
		public IList<TableSchema> Tables { get; set; }
	}
}


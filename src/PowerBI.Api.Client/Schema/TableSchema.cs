using System.Collections.Generic;

namespace PowerBI.Api.Client.Schema
{
	/// <summary>
	/// Table schema.
	/// </summary>
	public class TableSchema
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the columns.
		/// </summary>
		/// <value>The columns.</value>
		public IList<ColumnSchema> Columns { get; set; } 
	}
}


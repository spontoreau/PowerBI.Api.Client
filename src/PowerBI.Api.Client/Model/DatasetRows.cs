using System.Collections.Generic;

namespace PowerBI.Api.Client.Model
{
	/// <summary>
	/// Dataset rows.
	/// </summary>
	public sealed class DatasetRows
	{
		/// <summary>
		/// Gets or sets the rows.
		/// </summary>
		/// <value>The rows.</value>
		public IList<object> Rows { get; set; }
	}
}


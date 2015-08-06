namespace PowerBI.Api.Client.Model
{
	/// <summary>
	/// Dataset.
	/// </summary>
	public sealed class Dataset
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the default retention policy.
		/// </summary>
		/// <value>The default retention policy.</value>
		public string DefaultRetentionPolicy { get; set; }
	}
}


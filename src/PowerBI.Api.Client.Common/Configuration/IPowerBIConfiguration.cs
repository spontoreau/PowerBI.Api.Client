namespace PowerBI.Api.Client.Configuration
{
	/// <summary>
	/// PowerBI configuration.
	/// </summary>
	public interface IPowerBIConfiguration
	{
		/// <summary>
		/// PowerBI Api url
		/// </summary>
		string Url { get; }

		/// <summary>
		/// OAuth authority
		/// </summary>
		string Authority { get; }

		/// <summary>
		/// Resource for the token
		/// </summary>
		string Resource { get; }

		/// <summary>
		/// ClientId for the token
		/// </summary>
		string Client { get; }

		/// <summary>
		/// User
		/// </summary>
		string User { get; }

		/// <summary>
		/// Password
		/// </summary>
		string Password { get; }
	}
}


using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
	/// <summary>
	/// OAuth configuration element
	/// </summary>
	public sealed class OAuth : ConfigurationElement
	{
		/// <summary>
		/// OAuth authority
		/// </summary>
		[ConfigurationProperty("Authority", IsRequired = true)]
		public string Authority
		{
			get { return (string)this["Authority"]; }
		}

		/// <summary>
		/// Resource for the token
		/// </summary>
		[ConfigurationProperty("Resource", IsRequired = true)]
		public string Resource
		{
			get { return (string)this["Resource"]; }
		}

		/// <summary>
		/// ClientId for the token
		/// </summary>
		[ConfigurationProperty("Client", IsRequired = true)]
		public string Client
		{
			get { return (string)this["Client"]; }
		}

		/// <summary>
		/// User
		/// </summary>
		[ConfigurationProperty("User", IsRequired = true)]
		public string User
		{
			get { return (string)this["User"]; }
		}

		/// <summary>
		/// Password
		/// </summary>
		[ConfigurationProperty("Password", IsRequired = true)]
		public string Password
		{
			get { return (string)this["Password"]; }
		}
	}
}


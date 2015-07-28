
#if !PCL
using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
	/// <summary>
	/// PowerBI configuration.
	/// </summary>
	public sealed class PowerBIConfiguration : ConfigurationSection, IPowerBIConfiguration
	{
		/// <summary>
		/// PowerBI Api url
		/// </summary>
		public string Url 
		{ 
			get
			{ 
				return Api.Url;
			} 
		}

		/// <summary>
		/// OAuth authority
		/// </summary>
		public string Authority
		{ 
			get
			{ 
				return OAuth.Authority;
			} 
		}

		/// <summary>
		/// Resource for the token
		/// </summary>
		public string Resource
		{ 
			get
			{ 
				return OAuth.Resource;
			} 
		}

		/// <summary>
		/// ClientId for the token
		/// </summary>
		public string Client
		{ 
			get
			{ 
				return OAuth.Client;
			} 
		}

		/// <summary>
		/// User
		/// </summary>
		public string User		
		{ 
			get
			{ 
				return OAuth.User;
			} 
		}

		/// <summary>
		/// Password
		/// </summary>
		public string Password		
		{ 
			get
			{ 
				return OAuth.Password;
			} 
		}

		/// <summary>
		/// Gets the API configuration.
		/// </summary>
		/// <value>The API.</value>
		[ConfigurationProperty("Api", IsRequired = true)]
		public Api Api
		{
			get { return (Api)this["Api"]; }
		}

		/// <summary>
		/// Gets the OAuth configuration.
		/// </summary>
		/// <value>The OAuth.</value>
		[ConfigurationProperty("OAuth", IsRequired = true)]
		public OAuth OAuth
		{
			get { return (OAuth)this["OAuth"]; }
		}
	}
}
#endif
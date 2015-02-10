using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
	/// <summary>
	/// PowerBI configuration.
	/// </summary>
	public sealed class PowerBIConfiguration : ConfigurationSection
	{
		/// <summary>
		/// Gets the API configuration.
		/// </summary>
		/// <value>The API.</value>
		[ConfigurationProperty("Api", IsRequired = true)]
		public Api Api
		{
			get
			{
				return (Api)this["Api"];
			}
		}

		/// <summary>
		/// Gets the OAuth configuration.
		/// </summary>
		/// <value>The OAuth.</value>
		[ConfigurationProperty("OAuth", IsRequired = true)]
		public OAuth OAuth
		{
			get
			{
				return (OAuth)this["OAuth"];
			}
		}
	}
}


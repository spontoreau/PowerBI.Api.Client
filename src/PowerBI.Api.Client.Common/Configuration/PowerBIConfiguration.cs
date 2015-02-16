#if !PCL
using System.Configuration;
#endif

namespace PowerBI.Api.Client.Configuration
{
	/// <summary>
	/// PowerBI configuration.
	/// </summary>
	#if !PCL
	public sealed class PowerBIConfiguration : ConfigurationSection
	#else
	public sealed class PowerBIConfiguration
	#endif
	{
		/// <summary>
		/// Gets the API configuration.
		/// </summary>
		/// <value>The API.</value>
		#if !PCL
		[ConfigurationProperty("Api", IsRequired = true)]
		#endif
		public Api Api
		{
			#if !PCL
			get { return (Api)this["Api"]; }
			#else
			get; set;
			#endif
		}

		/// <summary>
		/// Gets the OAuth configuration.
		/// </summary>
		/// <value>The OAuth.</value>
		#if !PCL
		[ConfigurationProperty("OAuth", IsRequired = true)]
		#endif
		public OAuth OAuth
		{
			#if !PCL
			get { return (OAuth)this["OAuth"]; }
			#else
			get; set;
			#endif
		}
	}
}


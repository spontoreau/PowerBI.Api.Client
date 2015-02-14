using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
	/// <summary>
	/// Api configuration element
	/// </summary>
	#if !PCL
	public sealed class Api : ConfigurationElement
	#else
	public sealed class Api
	#endif
	{
		/// <summary>
		/// PowerBI Api url
		/// </summary>
		#if !PCL
		[ConfigurationProperty("Url", IsRequired = true)]
		#endif
		public string Url
		{
			#if !PCL
			get { return (string)this["Url"]; }
			#else
			get; set;
			#endif
		}
	}
}


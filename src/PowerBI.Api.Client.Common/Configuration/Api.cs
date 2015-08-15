#if !PCL
using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
	/// <summary>
	/// Api configuration element
	/// </summary>
	public sealed class Api : ConfigurationElement
	{
		/// <summary>
		/// PowerBI Api url
		/// </summary>
		[ConfigurationProperty("Url", IsRequired = true)]
		public string Url
		{
			get { return (string)this["Url"]; }
		}
	}
}
#endif


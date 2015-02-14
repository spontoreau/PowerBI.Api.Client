using System.Configuration;

namespace PowerBI.Api.Client.Configuration
{
	/// <summary>
	/// OAuth configuration element
	/// </summary>
	#if !PCL
	public sealed class OAuth : ConfigurationElement
	#else
	public sealed class OAuth
	#endif
	{
		/// <summary>
		/// OAuth authority
		/// </summary>
		#if !PCL
		[ConfigurationProperty("Authority", IsRequired = true)]
		#endif
		public string Authority
		{
			#if !PCL
			get { return (string)this["Authority"]; }
			#else
			get; set;
			#endif
		}

		/// <summary>
		/// Resource for the token
		/// </summary>
		#if !PCL
		[ConfigurationProperty("Resource", IsRequired = true)]
		#endif
		public string Resource
		{
			#if !PCL
			get { return (string)this["Resource"]; }
			#else
			get; set;
			#endif
		}

		/// <summary>
		/// ClientId for the token
		/// </summary>
		#if !PCL
		[ConfigurationProperty("Client", IsRequired = true)]
		#endif
		public string Client
		{
			#if !PCL
			get { return (string)this["Client"]; }
			#else
			get; set;
			#endif
		}

		/// <summary>
		/// User
		/// </summary>
		#if !PCL
		[ConfigurationProperty("User", IsRequired = true)]
		#endif
		public string User
		{
			#if !PCL
			get { return (string)this["User"]; }
			#else
			get; set;
			#endif
		}

		/// <summary>
		/// Password
		/// </summary>
		#if !PCL
		[ConfigurationProperty("Password", IsRequired = true)]
		#endif
		public string Password
		{
			#if !PCL
			get { return (string)this["Password"]; }
			#else
			get; set;
			#endif
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using PowerBI.Api.Client.Configuration;
using PowerBI.Api.Client.Http;
using PowerBI.Api.Client.Model;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

#if !PCL
using PowerBI.Api.Client.Schema;
using System.Configuration;
#else
using System.Threading.Tasks;
#endif

namespace PowerBI.Api.Client
{
	/// <summary>
	/// PowerBI.
	/// </summary>
	public sealed class PowerBIClient
	{
		/// <summary>
		/// The Synchronize object.
		/// </summary>
		static readonly object SyncRoot = new object();

		/// <summary>
		/// The root configuration.
		/// </summary>
		static IPowerBIConfiguration RootConfiguration;

		/// <summary>
		/// Gets or sets the configuration.
		/// </summary>
		/// <value>The configuration.</value>
		IPowerBIConfiguration Configuration { get; set; }

		/// <summary>
		/// Gets or sets the authentication context.
		/// </summary>
		/// <value>The authentication context.</value>
		AuthenticationContext AuthenticationContext { get; set; }

		/// <summary>
		/// Gets or sets the access token.
		/// </summary>
		/// <value>The access token.</value>
		string AccessToken { get; set; }

		/// <summary>
		/// Initializes a new instance of the PowerBI class.
		/// </summary>
		/// <param name="configuration">Configuration.</param>
		PowerBIClient(IPowerBIConfiguration configuration)
		{
			Configuration = configuration;
		}
			
		/// <summary>
		/// Initialize the api.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="authority">Authority.</param>
		/// <param name="resource">Resource.</param>
		/// <param name="client">Client.</param>
		/// <param name="user">User.</param>
		/// <param name="password">Password.</param>
		public static void Initialize(string url, string authority, string resource, string client, string user, string password)
		{
			if(string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
			if(string.IsNullOrEmpty(authority)) throw new ArgumentNullException("authority");
			if(string.IsNullOrEmpty(resource)) throw new ArgumentNullException("resource");
			if(string.IsNullOrEmpty(client)) throw new ArgumentNullException("client");
			if(string.IsNullOrEmpty(user)) throw new ArgumentNullException("user");
			if(string.IsNullOrEmpty(user)) throw new ArgumentNullException("password");

			RootConfiguration = new PowerBISimpleConfiguration 
			{
				Url = url,
				Authority = authority,
				Client = client,
				Resource = resource,
				User = user,
				Password = password
			};
		}

		/// <summary>
		/// Do the specified action.
		/// </summary>
		/// <param name="action">Action.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Do(Action<PowerBIClient> action)
		{
			var api = Get();
			api.Authenticate();
			action(api);
		}

		/// <summary>
		/// Do the specified function.
		/// </summary>
		/// <param name="function">Function.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Do<T>(Func<PowerBIClient, T> function)
		{
			var api = Get();
			api.Authenticate();
			return function(api);

		}

		/// <summary>
		/// Get a PowerBI instance.
		/// </summary>
		/// 
		static PowerBIClient Get()
		{
			lock(SyncRoot)
			{
				return new PowerBIClient(RootConfiguration);
			}
		}

		/// <summary>
		/// Authenticate.
		/// </summary>
		void Authenticate()
		{
			#if !PCL
			if(RootConfiguration == null)
				RootConfiguration = (PowerBIConfiguration)ConfigurationManager.GetSection(typeof(PowerBIConfiguration).Name);
			#endif

			if (AuthenticationContext == null)
			{
				var tokenCache = new TokenCache();
				AuthenticationContext = new AuthenticationContext(Configuration.Authority, tokenCache);
			}

			//For PCL we need to use the ADAL 3.O alpha. Because this version isn't a release we use compilation 
			//condition to not use it in the classic version (PCL version of the PowerBI.Api.Client is also a Pre-Release)
			//We use synchronous call (We be change in the next version of the library)
			#if !PCL
			var authResult = string.IsNullOrEmpty(AccessToken) 
				? AuthenticationContext.AcquireToken(Configuration.Resource,Configuration.Client, new UserCredential(Configuration.User, Configuration.Password))
				: AuthenticationContext.AcquireTokenSilent(Configuration.Resource, Configuration.Client);

			AccessToken = authResult.AccessToken;
			#else
			var task = string.IsNullOrEmpty(AccessToken) 
				? AuthenticationContext.AcquireTokenAsync(Configuration.Resource,Configuration.Client, new UserCredential(Configuration.User, Configuration.Password))	
				: AuthenticationContext.AcquireTokenSilentAsync(Configuration.Resource, Configuration.Client);	
			Task.WaitAll(task);
			AccessToken = task.Result.AccessToken;

			#endif
		}

		/// <summary>
		/// Gets all dataset.
		/// </summary>
		/// <returns>The all dataset.</returns>
		public IList<Dataset> GetDatasets()
		{
			return new WebApiClient(AccessToken)
				.Get<DatasetCollection>(GetDatasetUrl()).Value;
		}

		/// <summary>
		/// Gets the tables.
		/// </summary>
		/// <returns>The tables.</returns>
		/// <param name="datasetId">Dataset identifier.</param>
		public IList<Table> GetTables(string datasetId)
		{
			return new WebApiClient(AccessToken)
				.Get<TableCollection>(GetTableUrl(datasetId)).Tables;
		}

		/// <summary>
		/// Get Dataset identifier by name.
		/// </summary>
		/// <returns>The dataset identifier.</returns>
		/// <param name="datasetName">Dataset name.</param>
		public string GetDatasetId(string datasetName)
		{
			return GetDatasets()
				.First(x => x.Name == datasetName).Id;
		}

		/// <summary>
		/// Check if a name matches with a registered Dataset
		/// </summary>
		/// <returns><c>true</c> if this instance if the dataset exist; otherwise, <c>false</c>.</returns>
		/// <param name="datasetName">Dataset name.</param>
		public bool IsDatasetExist(string datasetName)
		{
			return GetDatasets()
				.Any(x => x.Name == datasetName);
		}

		/// <summary>
		/// Check if an identifier matches with a registered Dataset
		/// </summary>
		/// <returns><c>true</c> if this instance if the dataset exist; otherwise, <c>false</c>.</returns>
		/// <param name="datasetId">Dataset Identifier.</param>
		public bool IsDatasetIdExist(string datasetId)
		{
			return GetDatasets()
				.Any(x => x.Id == datasetId);
		}

		/// <summary>
		/// Create a Dataset and its related tables
		/// </summary>
		/// <returns><c>true</c>, if dataset was created, <c>false</c> otherwise.</returns>
		/// <param name="datasetName">Dataset name.</param>
		/// <param name="useRetentionPolicy">Optional, </param> 
		/// <param name="types">Types.</param>
		public bool CreateDataset(string datasetName, bool useRetentionPolicy, params Type[] types)
		{
			#if !PCL

			return new WebApiClient(AccessToken)
				.Post(string.Format("{0}/dataset?defaultRetentionPolicy={1}", Configuration.Url, useRetentionPolicy ? "basicFIFO" : "none"), SchemaBuilder.GetDataset(datasetName, ref types));
			#else
			throw new NotImplementedException("Dataset creation isn't implement in PCL version of PowerBI.Api.Client");
			#endif
		}

		public bool UpdateTable(string datasetId, string tableName, Type type)
		{
			#if !PCL
			return new WebApiClient(AccessToken)
				.Put(GetTableUpdateUrl(datasetId, tableName), SchemaBuilder.GetTable(type));
			#else
			throw new NotImplementedException("Table update isn't implement in PCL version of PowerBI.Api.Client");
			#endif
		}

		/// <summary>
		/// Insert a data into a table
		/// </summary>
		/// <param name="datasetId">Dataset identifier.</param>
		/// <param name="obj">Object.</param>
		public bool Insert(string datasetId, object obj)
		{
			return new WebApiClient(AccessToken)
				.Post(GetTableRowsUrl(datasetId, obj.GetType()), new DatasetRows { Rows = new List<object> { obj } });
		}

		/// <summary>
		/// Insert a list of data into a table
		/// </summary>
		/// <returns><c>true</c>, if all was inserted, <c>false</c> otherwise.</returns>
		/// <param name="datasetId">Dataset identifier.</param>
		/// <param name="objs">Objects.</param>
		public bool InsertAll(string datasetId, IList<object> objs)
		{
			if(objs.Count == 0)
				return false;
				
			return new WebApiClient(AccessToken)
				.Post(GetTableRowsUrl(datasetId, objs[0].GetType()), new DatasetRows { Rows = objs });
		}

		/// <summary>
		/// Clear a table
		/// </summary>
		/// <param name="datasetId">Dataset identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public bool Delete<T>(string datasetId)
		{
			return new WebApiClient(AccessToken)
				.Delete(GetTableRowsUrl(datasetId, typeof(T)));
		}

		/// <summary>
		/// Gets the URL.
		/// </summary>
		/// <returns>The insert URL.</returns>
		/// <param name="datasetId">Dataset identifier.</param>
		/// <param name="type">Type.</param>
		string GetTableRowsUrl(string datasetId, Type type)
		{
			return string.Format("{0}/datasets/{1}/tables/{2}/rows", Configuration.Url, datasetId, type.Name);
		}

		/// <summary>
		/// Gets the table URL.
		/// </summary>
		/// <returns>The table URL.</returns>
		/// <param name="datasetId">Dataset identifier.</param>
		string GetTableUrl(string datasetId)
		{
			return string.Format("{0}/datasets/{1}/tables", Configuration.Url, datasetId);
		}

		/// <summary>
		/// Gets the table URL.
		/// </summary>
		/// <returns>The table URL.</returns>
		/// <param name="datasetId">Dataset identifier.</param>
		string GetTableUpdateUrl(string datasetId, string tableName)
		{
			return string.Format("{0}/datasets/{1}/tables/{3}", Configuration.Url, datasetId, tableName);
		}

		/// <summary>
		/// Gets the dataset URL.
		/// </summary>
		/// <returns>The dataset URL.</returns>
		string GetDatasetUrl()
		{
			return string.Format("{0}/datasets", Configuration.Url);
		}
	}
}


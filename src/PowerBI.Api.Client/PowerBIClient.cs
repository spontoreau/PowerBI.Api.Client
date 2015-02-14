using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PowerBI.Api.Client.Configuration;
using PowerBI.Api.Client.Http;
using PowerBI.Api.Client.Model;
using PowerBI.Api.Client.Schema;

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
		static readonly PowerBIConfiguration RootConfiguration;

		/// <summary>
		/// Gets or sets the configuration.
		/// </summary>
		/// <value>The configuration.</value>
		PowerBIConfiguration Configuration { get; set; }

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
		/// Initializes the <see cref="PowerBI.Api.Client.PowerBIClient"/> class.
		/// </summary>
		static PowerBIClient(){
			#if !PCL
			RootConfiguration = (PowerBIConfiguration)ConfigurationManager.GetSection(typeof(PowerBIConfiguration).Name);
			#endif
		}

		/// <summary>
		/// Initializes a new instance of the PowerBI class.
		/// </summary>
		/// <param name="configuration">Configuration.</param>
		PowerBIClient(PowerBIConfiguration configuration)
		{
			Configuration = configuration;
		}

		#if PCL
		/// <summary>
		/// Intialize the specified api and oAuth configuration.
		/// </summary>
		/// <param name="api">API.</param>
		/// <param name="oAuth">O auth.</param>
		public static void Intialize(PowerBI.Api.Client.Configuration.Api api, OAuth oAuth)
		{
			RootConfiguration = new PowerBIConfiguration { Api = api, OAuth = oAuth };
		}
		#endif

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
			if (AuthenticationContext == null)
			{
				var tokenCache = new TokenCache();
				AuthenticationContext = new AuthenticationContext(Configuration.OAuth.Authority, tokenCache);
			}

			var authResult = string.IsNullOrEmpty(AccessToken) 
				? AuthenticationContext.AcquireToken(Configuration.OAuth.Resource,Configuration.OAuth.Client, new UserCredential(Configuration.OAuth.User, Configuration.OAuth.Password))
				: AuthenticationContext.AcquireTokenSilent(Configuration.OAuth.Resource, Configuration.OAuth.Client);

			AccessToken = authResult.AccessToken;
		}

		/// <summary>
		/// Gets all dataset.
		/// </summary>
		/// <returns>The all dataset.</returns>
		public IList<Dataset> GetDatasets()
		{
			return new WebApiClient(AccessToken)
				.Get<DatasetCollection>(Configuration.Api.Url).Datasets;
		}

		/// <summary>
		/// Gets the dataset identifier.
		/// </summary>
		/// <returns>The dataset identifier.</returns>
		/// <param name="datasetName">Dataset name.</param>
		public string GetDatasetId(string datasetName)
		{
			return GetDatasets()
				.First(x => x.Name == datasetName).Id;
		}

		/// <summary>
		/// Determines if a dataset exist
		/// </summary>
		/// <returns><c>true</c> if this instance if the dataset exist; otherwise, <c>false</c>.</returns>
		/// <param name="datasetName">Dataset name.</param>
		public bool IsDatasetExist(string datasetName)
		{
			return GetDatasets()
				.Any(x => x.Name == datasetName);
		}

		/// <summary>
		/// Determines if a dataset exist
		/// </summary>
		/// <returns><c>true</c> if this instance if the dataset exist; otherwise, <c>false</c>.</returns>
		/// <param name="datasetId">Dataset Identifier.</param>
		public bool IsDatasetIdExist(string datasetId)
		{
			return GetDatasets()
				.Any(x => x.Id == datasetId);
		}

		/// <summary>
		/// Creates the dataset.
		/// </summary>
		/// <returns><c>true</c>, if dataset was created, <c>false</c> otherwise.</returns>
		/// <param name="datasetName">Dataset name.</param>
		/// <param name="types">Types.</param>
		public bool CreateDataset(string datasetName, params Type[] types)
		{
			return new WebApiClient(AccessToken)
				.Post(Configuration.Api.Url, SchemaBuilder.GetDataset(datasetName, ref types));
		}

		/// <summary>
		/// Insert the specified obj in the corresponding dataset.
		/// </summary>
		/// <param name="datasetId">Dataset identifier.</param>
		/// <param name="obj">Object.</param>
		public bool Insert(string datasetId, object obj)
		{
			return new WebApiClient(AccessToken)
				.Post(GetUrl(datasetId, obj.GetType()), new DatasetRows { Rows = new List<object> { obj } });
		}

		/// <summary>
		/// Insert all the specified obj in the corresponding dataset.
		/// </summary>
		/// <returns><c>true</c>, if all was inserted, <c>false</c> otherwise.</returns>
		/// <param name="datasetId">Dataset identifier.</param>
		/// <param name="objs">Objects.</param>
		public bool InsertAll(string datasetId, IList<object> objs)
		{
			if(objs.Count == 0)
				return false;
				
			return new WebApiClient(AccessToken)
				.Post(GetUrl(datasetId, objs[0].GetType()), new DatasetRows { Rows = objs });
		}

		/// <summary>
		/// Delete all the rows corresponding to a T type and a datasetId.
		/// </summary>
		/// <param name="datasetId">Dataset identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public bool Delete<T>(string datasetId)
		{
			return new WebApiClient(AccessToken)
				.Delete(GetUrl(datasetId, typeof(T)));
		}

		/// <summary>
		/// Gets the URL.
		/// </summary>
		/// <returns>The insert URL.</returns>
		/// <param name="datasetId">Dataset identifier.</param>
		/// <param name="type">Type.</param>
		string GetUrl(string datasetId, Type type)
		{
			return string.Format("{0}/{1}/tables/{2}/rows", Configuration.Api.Url, datasetId, type.Name);
		}
	}
}


using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PowerBI.Api.Client.Http
{
	/// <summary>
	/// Web API client.
	/// </summary>
	class WebApiClient
	{
		/// <summary>
		/// Access token
		/// </summary>
		string AccessToken { get; set; }

		/// <summary>
		/// Create a new instance of WebApiClient
		/// </summary>
		/// <param name="accessToken">Access token</param>
		public WebApiClient(string accessToken)
		{
			AccessToken = accessToken;
		}

		/// <summary>
		/// Get.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Get<T>(string url)
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", AccessToken));
				var responseTask = httpClient.GetAsync(url);
				responseTask.Wait();
				var jsonTask = responseTask.Result.Content.ReadAsStringAsync();
				jsonTask.Wait();

				return JsonConvert.DeserializeObject<T>(jsonTask.Result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
			}
		}

		/// <summary>
		/// Post the specified obj.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="obj">Object.</param>
		public bool Post(string url, object obj)
		{
			using (var httpClient = new HttpClient())
			{
				var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
				httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", AccessToken));
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				var responseTask = httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
				responseTask.Wait();
				return responseTask.Result.EnsureSuccessStatusCode().IsSuccessStatusCode;
			}
		}

		/// <summary>
		/// Delete.
		/// </summary>
		/// <param name="url">URL.</param>
		public bool Delete(string url)
		{
			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", AccessToken));
				var responseTask = httpClient.DeleteAsync(url);
				responseTask.Wait();
				return responseTask.Result.EnsureSuccessStatusCode().IsSuccessStatusCode;
			}
		}
	}
}


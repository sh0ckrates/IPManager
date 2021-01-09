using IPManager.Library.Integration.WebApi.Abstractions.RequestProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IPManager.Library.Integration.ExternalApi.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {   
        public class JsonContent : StringContent
        {  
            public JsonContent(object obj) :   
                base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            {
            }
        }

        public async Task<T> PostRequest<T, TK>(string apiUrl, TK postObject)
        {
            T result = default(T);

            using (var client = new HttpClient())
            {
                var postContent = JsonConvert.SerializeObject(postObject);
                var response = await client
                    .PostAsync(apiUrl, new StringContent(postContent, Encoding.UTF8, "text/json"))
                    .ConfigureAwait(false);

                await HandleResponse(response);
                string responseData = await response.Content.ReadAsStringAsync();

                result = await Task.Run(() => JsonConvert.DeserializeObject<T>(responseData));
            }

            return result;
        }

        public async Task<List<T>> PostItemWithMultipleResultRequest<T, TK>(string apiUrl, TK postObject)
        {
            List<T> result = null;

            using (var client = new HttpClient())
            {
                var postContent = JsonConvert.SerializeObject(postObject);
                var response = await client
                    .PostAsync(apiUrl, new StringContent(postContent, Encoding.UTF8, "text/json"))
                    .ConfigureAwait(false);
                await HandleResponse(response);
                string responseData = await response.Content.ReadAsStringAsync();
                result = await Task.Run(() => JsonConvert.DeserializeObject<List<T>>(responseData));
            }

            return result;
        }

        public async Task<List<T>> PostItemsWithMultipleResultRequest<T, TK>(string apiUrl, List<TK> postObject)
        {
            List<T> result = null;

            using (var client = new HttpClient())
            {
                var postContent = JsonConvert.SerializeObject(postObject);
                var response = await client
                    .PostAsync(apiUrl, new StringContent(postContent, Encoding.UTF8, "text/json"))
                    .ConfigureAwait(false);
                await HandleResponse(response);
                string responseData = await response.Content.ReadAsStringAsync();
                result = await Task.Run(() => JsonConvert.DeserializeObject<List<T>>(responseData));
            }

            return result;
        }

        public async Task<TK> PutRequest<T, TK>(string apiUrl, T postObject)
        {
            TK result = default(TK);

            using (var client = new HttpClient())
            {
                var postcontent = JsonConvert.SerializeObject(postObject);
                var response = await client.PutAsync(apiUrl, new StringContent(postcontent, Encoding.UTF8, "text/json"))
                    .ConfigureAwait(false);
                await HandleResponse(response);
                string responseData = await response.Content.ReadAsStringAsync();
                result = await Task.Run(() => JsonConvert.DeserializeObject<TK>(responseData));
            }

            return result;
        }

        public async Task<T> GetSingleItemRequest<T>(string apiUrl)
        {
            T result = default(T);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);
                await HandleResponse(response);
                string responseData = await response.Content.ReadAsStringAsync();
                result = await Task.Run(() => JsonConvert.DeserializeObject<T>(responseData));
            }

            return result;
        }

        public async Task<List<T>> GetMultipleItemsRequest<T>(string apiUrl)
        {
            List<T> result = null;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);
                await HandleResponse(response);
                string responseData = await response.Content.ReadAsStringAsync();
                result = await Task.Run(() => JsonConvert.DeserializeObject<List<T>>(responseData));
            }

            return result;
        }

        public async Task DeleteRequest(string apiUrl)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(apiUrl).ConfigureAwait(false);
                await HandleResponse(response);
            }
        }

        public async Task<List<JObject>> GetRawJsonObjects(string apiUrl)
        {
            List<JObject> items = null;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl).ConfigureAwait(false);
                await HandleResponse(response);
                var stream = await response.Content.ReadAsStreamAsync();

                using (var streamReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    items = await Task.Run(() => serializer.Deserialize<List<JObject>>(jsonReader));
                }
            }

            return items;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(content);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace IPManager.Library.Integration.WebApi.Abstractions.RequestProvider
{
    public interface IRequestProvider
    {
        Task<T> PostRequest<T, TK>(string apiUrl, TK postObject); 
        Task<TK> PutRequest<T, TK>(string apiUrl, T postObject);
        Task<T> GetSingleItemRequest<T>(string apiUrl);
        Task<List<T>> GetMultipleItemsRequest<T>(string apiUrl);
        Task DeleteRequest(string apiUrl);
        Task<List<JObject>> GetRawJsonObjects(string apiUrl);
        Task<List<T>> PostItemsWithMultipleResultRequest<T, TK>(string apiUrl, List<TK> postObject); 
    }

}
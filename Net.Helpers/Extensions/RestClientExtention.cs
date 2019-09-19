using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
namespace Net.Helpers.Extensions {
    public static class RestClientExtension {
        public static async Task<RestResponse> ExecuteAsync (this RestClient client, RestRequest request) {
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse> ();
            RestRequestAsyncHandle handle = client.ExecuteAsync (request, r => taskCompletion.SetResult (r));
            return (RestResponse) (await taskCompletion.Task);
        }
        public static async Task<T> ExecuteAsync<T> (this RestClient client, RestRequest request) {
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse> ();
            RestRequestAsyncHandle handle = client.ExecuteAsync (request, r => taskCompletion.SetResult (r));
            var response = (RestResponse) (await taskCompletion.Task);
            return JsonConvert.DeserializeObject<T> (response.Content);
        }
    }
}
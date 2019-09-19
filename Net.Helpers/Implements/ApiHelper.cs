using System;
using System.Threading.Tasks;
using Net.Helpers.Extensions;
using Net.Helpers.Interfaces;
using RestSharp;


namespace Net.Helpers.Implements {
    public class ApiHelper : IApiHelper {
        private readonly ILogHelper _logHelper;
        private readonly IStringHelper _stringHelper;
        public ApiHelper (ILogHelper logHelper, IStringHelper stringHelper) {
            _logHelper = logHelper;
            _stringHelper = stringHelper;
        }

        public async Task<T> CallApi<T> (string url, RestRequest request = null) {
            var client = new RestClient {
            BaseUrl = new Uri (url)
            };
            if (request == null) {
                request = new RestRequest (Method.GET);
            }
            var response = await client.ExecuteAsync<T> (request);
            _logHelper.Log (new {
                url = url,
                resource = request.Resource,
                method = request.Method.ToString()
            }, "call-api");
            return response;
        }
    }
}
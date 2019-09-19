using System;
using System.Threading.Tasks;
using RestSharp;

namespace Net.Helpers.Interfaces {
    public interface IApiHelper {
        Task<T> CallApi<T> (string url, RestRequest request = null);
    }
}
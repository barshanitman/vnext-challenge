using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using System.Net;

namespace Vnext.Function
{

    public interface IDeviceAsset
    {
        public Task<object> GetAssetId(string deviceId);
        public Task<object> GetMultipleAssetIdsAsync(IEnumerable<string> deviceId);


    }

    public class DeviceAsset : IDeviceAsset
    {
        private readonly HttpClient _httpClient;

        public DeviceAsset(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<object> GetAssetId(string deviceId)
        {

            // HttpClient client = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("x-functions-key", "yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw==");
            var response = await _httpClient.GetAsync($"http://tech-assessment.vnext.com.au/api/devices/assetId/{deviceId}");

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<object>(content);


        }

        public async Task<object> GetMultipleAssetIdsAsync(IEnumerable<string> devices)
        {



            List<ResponseBody> finalResults = new List<ResponseBody>();
            _httpClient.DefaultRequestHeaders.Add("x-functions-key", "yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw==");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            List<string> urls = new List<string>();
            foreach (string device in devices)
            {
                urls.Add("http://tech-assessment.vnext.com.au/api/devices/assetId/" + device);

            }

            var requests = urls.Select(url => _httpClient.GetAsync(url)).ToList();




            var responses = requests.Select(task => task.Result);
            foreach (var response in responses)
            {

                var result = await response.Content.ReadAsAsync<ResponseBody>();


                finalResults.Add(result);

            }
            return JsonConvert.SerializeObject(finalResults);



        }


    }


}
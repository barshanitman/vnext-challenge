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
        public Task<object> GetMultipleAssetIdsAsync(IEnumerable<Devices> devices);

        public IEnumerable<Devices> AssignAssetIdToDevices(IEnumerable<Devices> listOfDevices, IEnumerable<ResponseBody> assetids);

        public IEnumerable<Devices> AddToDatabase(IEnumerable<Devices> devices);



    }

    public class DeviceAsset : IDeviceAsset
    {
        private readonly HttpClient _httpClient;

        private readonly IGenericRepo<Devices> _devices;

        public DeviceAsset(HttpClient httpClient, IGenericRepo<Devices> devices)
        {
            _httpClient = httpClient;
            _devices = devices;

        }

        public IEnumerable<Devices> AddToDatabase(IEnumerable<Devices> devices)
        {
            // foreach (Devices device in devices)
            // {
            //     _devices.Add(device);


            // }
            _devices.AddMultiple(devices);
            _devices.SaveChanges();

            return devices;

        }

        public IEnumerable<Devices> AssignAssetIdToDevices(IEnumerable<Devices> listOfDevices, IEnumerable<ResponseBody> assetids)
        {
            foreach (Devices device in listOfDevices)
            {

                device.AssetId = assetids.Where(item => item.deviceId == device.Id).First().assetId;


            }

            return listOfDevices;

        }

        public async Task<object> GetAssetId(string deviceId)
        {


            _httpClient.DefaultRequestHeaders.Add("x-functions-key", "yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw==");
            var response = await _httpClient.GetAsync($"http://tech-assessment.vnext.com.au/api/devices/assetId/{deviceId}");

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<object>(content);


        }

        public async Task<object> GetMultipleAssetIdsAsync(IEnumerable<Devices> devices)
        {

            List<ResponseBody> finalResults = new List<ResponseBody>();
            _httpClient.DefaultRequestHeaders.Add("x-functions-key", "yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw==");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            List<string> urls = new List<string>();
            foreach (Devices device in devices)
            {
                urls.Add("http://tech-assessment.vnext.com.au/api/devices/assetId/" + device.Id);

            }

            var requests = urls.Select(url => _httpClient.GetAsync(url)).ToList();



            var responses = requests.Select(task => task.Result);
            foreach (var response in responses)
            {

                var result = await response.Content.ReadAsAsync<ResponseBody>();


                finalResults.Add(result);

            }

            IEnumerable<Devices> assignedDeviceAssetIds = AssignAssetIdToDevices(devices, finalResults);

            IEnumerable<Devices> results = AddToDatabase(assignedDeviceAssetIds);

            return results;


        }

    }


}
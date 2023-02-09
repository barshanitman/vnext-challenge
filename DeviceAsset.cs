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
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Vnext.Function
{

    public interface IDeviceAsset
    {
        public Task<object> GetAssetId(IEnumerable<string> devices);
        public Task<object> GetMultipleAssetIdsAsync(IEnumerable<Devices> devices);

        public IEnumerable<Devices> AssignAssetIdToDevices(IEnumerable<Devices> listOfDevices, IEnumerable<ResponseBody> assetids);

        public IEnumerable<Devices> AddToDatabase(IEnumerable<Devices> devices);

        public IEnumerable<List<T>> splitList<T>(List<T> locations, int size);

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
        public IEnumerable<List<T>> splitList<T>(List<T> locations, int size)
        {
            for (int i = 0; i < locations.Count; i += size)
            {
                yield return locations.GetRange(i, Math.Min(size, locations.Count - i));
            }
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

        public async Task<object> GetAssetId(IEnumerable<string> devices)
        {


            _httpClient.DefaultRequestHeaders.Add("x-functions-key", "yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw==");
            // var response = await _httpClient.GetAsync($"http://tech-assessment.vnext.com.au/api/devices/assetId/{deviceId}");
            List<string> urls = new List<string>();
            foreach (string device in devices)
            {
                urls.Add("http://tech-assessment.vnext.com.au/api/devices/assetId/" + device);

            }

            List<string> finalResults = new List<string>();

            IEnumerable<List<string>> batches = splitList<string>((List<string>)urls, 400);
            int numberOfRequests = urls.Count;
            int maxParallelRequests = 100;
            var semaphoreSlim = new SemaphoreSlim(maxParallelRequests, maxParallelRequests);

            var tasks = new List<Task<string>>();

            for (int i = 0; i < numberOfRequests; ++i)
            {
                tasks.Add(MakeRequest(urls[i], semaphoreSlim, _httpClient));
            }
            // foreach (List<string> batch in batches)
            // {
            //     var requestNew = urls.Select(url => _httpClient.GetStringAsync(url));
            //     var responseNew = await Task.WhenAll(requestNew.ToList());
            //     finalResults.AddRange(responseNew);
            //     await Task.Delay(3000);


            // }


            // var requests = urls.Select(url => _httpClient.GetStringAsync(url));

            // var responses = requests.Select(task => task.Result);
            // var responses = await Task.WhenAll(requests.ToList());
            var responses = await Task.WhenAll(tasks);


            var finalResponses = responses.Select(res =>

            JsonConvert.DeserializeObject(res, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })
             );
            return finalResponses;




            // var content = await response.Content.ReadAsStringAsync();
            // return JsonConvert.DeserializeObject<object>(content);


        }

        public async Task<object> GetMultipleAssetIdsAsync(IEnumerable<Devices> devices)
        {

            var semaphore = new SemaphoreSlim(200);

            List<ResponseBody> finalResults = new List<ResponseBody>();
            _httpClient.DefaultRequestHeaders.Add("x-functions-key", "yeK7CM/Pj2vA3MFpuBxIFX7QIl1cKFOiviZaOjtVCrTq0VUzKeQjfw==");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            List<string> urls = new List<string>();
            foreach (Devices device in devices)
            {
                urls.Add("http://tech-assessment.vnext.com.au/api/devices/assetId/" + device.Id);

            }

            IEnumerable<List<string>> batches = splitList<string>((List<string>)urls, 10);

            List<Task<HttpResponseMessage>> masterTask = new List<Task<HttpResponseMessage>>();


            // int count = 0;





            // foreach (List<string> batch in batches)
            // {
            //     count += 1;

            //     if (count == 400)
            //     {
            //         await Task.Delay(120000);
            //         count = 0;

            //     }

            //     Console.WriteLine(batch.Count);
            //     var batchRequest = batch.Select(url => _httpClient.GetAsync(url)).ToList();
            //     masterTask.AddRange(batchRequest);

            //     await Task.Delay(1000);

            // }


            // var requests = urls.Select(url => _httpClient.GetAsync(url)).ToList();
            // var requests = urls.Select(url => MakeRequest(url, semaphore, _httpClient)).ToList();


            // var responses = requests.Select(task => task.Result);
            // var responses = await Task.WhenAll(requests);


            // var responses = masterTask.Select(task => task.Result);
            // foreach (var response in responses)
            // {



            //     var result = await response.Content.ReadAsAsync<ResponseBody>();
            //     Console.WriteLine(response.StatusCode);


            //     finalResults.Add(result);

            // }

            // IEnumerable<Devices> assignedDeviceAssetIds = AssignAssetIdToDevices(devices, finalResults);
            // IEnumerable<Devices> assignedDeviceAssetIds = AssignAssetIdToDevices(devices, responses);

            // IEnumerable<Devices> results = AddToDatabase(assignedDeviceAssetIds);
            return "Hello";

            // return results;

        }
        public async Task<string> MakeRequest(string url, SemaphoreSlim semaphore, HttpClient httpClient)
        {
            try
            {
                await semaphore.WaitAsync();
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
                var response = await httpClient.SendAsync(request);

                // Add an optional delay for further throttling:
                await Task.Delay(TimeSpan.FromMilliseconds(3000));

                return await response.Content.ReadAsStringAsync();
            }
            finally
            {
                semaphore.Release();
            }
        }





    }


}
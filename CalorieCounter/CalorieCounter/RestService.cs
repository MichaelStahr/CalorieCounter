using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CalorieCounter
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient(GetInsecureHandler());
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }



        public HttpClientHandler GetInsecureHandler()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public async Task<FoodItem> GetFoodCaloriesAsync(string uri)
        {
            List<FoodItem> foodItems = null;
            FoodItem item = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    string c = await response.Content.ReadAsStringAsync();

                    foodItems = JsonConvert.DeserializeObject<List<FoodItem>>(c);

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            if (foodItems == null)
            {
                return item;
            }
            else
            {
                return foodItems[0];
            }

        }

        public async Task<List<FoodItem>> GetFoodDataAsync(string uri)
        {
            List<FoodItem> listFood = null;
            FoodItem foodItem = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;
                
                if (response.IsSuccessStatusCode)
                {

                    string c = await response.Content.ReadAsStringAsync();

                    listFood = JsonConvert.DeserializeObject<List<FoodItem>>(c);

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            
             return listFood;
            
        }

        // not complete yet but does access Miami API
        public async Task<string> GetMiamiFoodDataAsync(string uri)
        {
            string foods = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    
                    string c = await response.Content.ReadAsStringAsync();
                    string file = "<items>\n" + c + "\n</items>";
                    string newFile = file.Replace("&", "and");
                    XElement items = XElement.Parse(newFile);
                    List<XElement> itemNodes = items.Elements("item").ToList();
                    var s = itemNodes[0].Element("formal_name");

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }

            return foods;

        }

        public async Task<string> InsertFoodIntoLogForUser(string uri, FoodEaten data)
        {
            string inserted = "false";
            try
            {
                var json = JsonConvert.SerializeObject(data);
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await _client.SendAsync(message))
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            inserted = "success";
                        }
                    }
                }
                //var content = new StringContent(data, Encoding.UTF8, "application/json");
                //HttpResponseMessage response = await _client.PostAsync(uri, content);
                //HttpStatusCode i = response.StatusCode;
                //string result = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    inserted = "success";
                //}
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return inserted;
        }

        public async Task<List<UserLogData>> GetDailyValuesForUser(string uri)
        {
            List<UserLogData> logData = null;
            UserLogData data = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {

                    string content = await response.Content.ReadAsStringAsync();

                    logData = JsonConvert.DeserializeObject<List<UserLogData>>(content);

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }

            return logData;

        }
    }
}

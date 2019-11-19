using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
                    //FoodItem item = new FoodItem
                    //{
                    //    Calories = 8,
                    //};
                    //string c = JsonConvert.SerializeObject(item);

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
                    //FoodItem item = new FoodItem
                    //{
                    //    Calories = 8,
                    //};
                    //string c = JsonConvert.SerializeObject(item);

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

        public async Task<string> GetMiamiFoodDataAsync(string uri)
        {
            string foods = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    //FoodItem item = new FoodItem
                    //{
                    //    Calories = 8,
                    //};
                    //string c = JsonConvert.SerializeObject(item);
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
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CalorieCounter
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient(GetInsecureHandler());
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

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

        

        /// <summary>
        /// Get foods in our database
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>
        /// List of food items
        /// </returns>
        public async Task<List<MiamiItem>> GetFoodDataAsync(string uri)
        {
            //List<FoodItem> listFood = null;
            List<MiamiItem> miamiFoodList = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;
                
                if (response.IsSuccessStatusCode)
                {

                    string c = await response.Content.ReadAsStringAsync();

                    //listFood = JsonConvert.DeserializeObject<List<FoodItem>>(c);
                    miamiFoodList = JsonConvert.DeserializeObject<List<MiamiItem>>(c);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
             return miamiFoodList; 
        }

        /// <summary>
        /// Get Miami foods by calling their API
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>
        /// List a string representation of Miami items in xml
        /// </returns>
        public async Task<String> GetMiamiFoodDataAsync(string uri)
        {
            string file = null;
            XElement items = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    
                    string c = await response.Content.ReadAsStringAsync();
                    c = c.Replace("\n", String.Empty);
                    // need to start and end file with same word
                    file = "<items>" + c + "</items>";
                    
                    // XElement parse does not like '&'
                    file = file.Replace("&", "and");
                    file = file.Replace(";", String.Empty);
                    items = XElement.Parse(file);


                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }

            // return list of foods to be placed into our db
            return items.ToString();

        }

        /// <summary>
        /// Insert Miami items into our database
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="xmlContent"></param>
        public async Task InsertMiamiFoodDataAsync(string uri, string xmlContent)
        {
            try
            {
                
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    xmlContent = "xml=" + xmlContent;
                    xmlContent = xmlContent.Replace("&", String.Empty);
                    xmlContent = xmlContent.Replace(";", String.Empty);
                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(xmlContent, Encoding.UTF8, "application/x-www-form-urlencoded");
                    
                    using (var response = await _client.SendAsync(message))
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            // sucssessful insert into DB
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }

        /// <summary>
        /// Insert food into a user eats into the UserEats table
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        public async Task InsertFoodIntoUserEats(string uri, string data)
        {
            try
            {
                //var json = JsonConvert.SerializeObject(data);
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

                    using (var response = await _client.SendAsync(message))
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            // success
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }

        /// <summary>
        /// Get the daily nutritional values of a day for a user
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>
        /// A list of nutrional values
        /// </returns>
        public async Task<List<DailyValues>> DisplayDailyValuesByUserDayAsync(string uri)
        {
            List<DailyValues> listFood = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {

                   
                    string c = await response.Content.ReadAsStringAsync();

                   
                    listFood = JsonConvert.DeserializeObject<List<DailyValues>>(c);


                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }


            return listFood;

        }

        /// <summary>
        /// Get food items and their calories
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>
        /// A list of foods with their calories
        /// </returns>
        public async Task<List<SimpleFood>> GetSimpleFoodItemForUserAsync(string uri)
        {
            List<SimpleFood> simpleFoodList = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {

                    string c = await response.Content.ReadAsStringAsync();

                    simpleFoodList = JsonConvert.DeserializeObject<List<SimpleFood>>(c);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return simpleFoodList;
        }

        /// <summary>
        /// Get a token authenticating a user
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns>
        /// A token's info
        /// </returns>
        public async Task<TokenResponse> ObtainAccessToken(string uri, string content)
        {

            string result = "";
            TokenResponse tokenData = null;
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    
                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

                    using (var response = await _client.SendAsync(message))
                    {
                        result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            tokenData = JsonConvert.DeserializeObject<TokenResponse>(result);
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return tokenData;
        }

        /// <summary>
        /// Checks if a user exists
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>
        /// True if the user exists, false otherwise
        /// </returns>
        public async Task<bool> GetUser(string uri)
        {
            List<User> users = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {

                    string c = await response.Content.ReadAsStringAsync();

                    users = JsonConvert.DeserializeObject<List<User>>(c);
                    if (users.Count == 1)
                    {
                        return true;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return false;
        }

        /// <summary>
        /// Get a user's info
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>
        /// A list containing one user
        /// </returns>
        public async Task<List<User>> GetUserInfo(string uri)
        {
            List<User> users = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {

                    string c = await response.Content.ReadAsStringAsync();

                    users = JsonConvert.DeserializeObject<List<User>>(c);
                   
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return users;
        }

        /// <summary>
        /// Insert a user into the Person table
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        public async Task InsertNewsUser(string uri, string content)
        {
            string result = "";
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {

                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

                    using (var response = await _client.SendAsync(message))
                    {
                        result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            // user inserted successfully
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }

        /// <summary>
        /// Update the user weight and height
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        public async Task UpdateWeightAndHeightForUser(string uri, string content)
        {
            string result = "";
            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {

                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

                    using (var response = await _client.SendAsync(message))
                    {
                        result = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            // user inserted successfully
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }

        /// <summary>
        /// Get locations that are active/open
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>
        /// A list of locations 
        /// </returns>
        public async Task<List<Location>> GetActiveLocations(string uri)
        {
            List<Location> locations = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                HttpStatusCode i = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {

                    string c = await response.Content.ReadAsStringAsync();

                    locations = JsonConvert.DeserializeObject<List<Location>>(c);

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return locations;
        }

    }
}

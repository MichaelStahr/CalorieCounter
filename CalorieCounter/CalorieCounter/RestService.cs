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
        public async Task<String> GetMiamiFoodDataAsync(string uri)
        {
            string newFile = null;
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
                    //c= c.Replace(" ", String.Empty);
                    // need to start and end file with same word
                    //file ="<?xml version='1.0' encoding='utf-8'?>"
                    //    + "<items>"
                    //    + "< item >  < service_unit > ASC Toasted Bagel</ service_unit >  < meal > Served All Day</ meal >  < meal_number > 13 </ meal_number >  < course > 12 - Proteins </ course >  < key_name > Chickuscan </ key_name >  < formal_name > Tuscan Chicken Strip</ formal_name >  < portion_size > 3 oz serving</ portion_size >  < cost ></ cost >  < sort > 1700 </ sort >  < itemevent_intid > 3240729 </ itemevent_intid >  < eventdate > Feb 27 2020 12:00:00:000AM </ eventdate >  < gram_weight > 85.04850006103516 </ gram_weight >  < PRO > 16.15921501159668 </ PRO >  < FAT > 1.700970001220703 </ FAT >  < CHO > 3.401940002441406 </ CHO >  < KCAL > 100.3572300720215 </ KCAL >  < CA > 20.41164001464844 </ CA >  < FE > 0.8504850006103516 </ FE >  < P > 0 </ P >  < K > 0 </ K >  < NA > 440.5512303161621 </ NA >  < VTAIU > 0 </ VTAIU >  < VITC > 0 </ VITC >  < B1 > 0 </ B1 >  < B2 > 0 </ B2 >  < NIA > 0 </ NIA >  < B6 > 0 </ B6 >  < CHOL > 45.07570503234864 </ CHOL >  < SFA > 0.8504850006103516 </ SFA >  < PUFA > 0 </ PUFA >  < TDFB > 0.8504850006103516 </ TDFB >  < SUGR > 0 </ SUGR >  < FATRN > 0 </ FATRN >  < course_sort > 12 </ course_sort >  < menuplangrp ></ menuplangrp > </ item >" + 
                    //      "</items>";
                    file = "<items>" + c + "</items>";
                    //file =
                       //"<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                       //"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>" +
                       //"<soap:Body>" +
                       //"<InsertXml xmlns='http://tempuri.org/'>" +
                       //file =
                       //"<items>" +
                       //"<item><service_unit>Test</service_unit><meal>Served All Day</meal><meal_number>13</meal_number><course>12 - Proteins</course><key_name>Chickuscan</key_name><formal_name>Tuscan Chicken Strip</formal_name><portion_size>3 oz serving</portion_size><cost></cost><sort>1700</sort><itemevent_intid>3240729</itemevent_intid><eventdate>Feb 27 2020 12:00:00:000AM</eventdate><gram_weight>85.04850006103516</gram_weight><PRO>16.15921501159668</PRO><FAT>1.700970001220703</FAT><CHO>3.401940002441406</CHO><KCAL>100.3572300720215</KCAL><CA>20.41164001464844</CA><FE>0.8504850006103516</FE><P>0</P><K>0</K><NA>440.5512303161621</NA><VTAIU>0</VTAIU><VITC>0</VITC><B1>0</B1><B2>0</B2><NIA>0</NIA><B6>0</B6><CHOL>45.07570503234864</CHOL><SFA>0.8504850006103516</SFA><PUFA>0</PUFA><TDFB>0.8504850006103516</TDFB><SUGR>0</SUGR><FATRN>0</FATRN><course_sort>12</course_sort><menuplangrp></menuplangrp></item>" +
                       //"</items>";
                       //"</InsertXml>" +
                       //"</soap:Body>" +
                       //"</soap:Envelope>";

                    //<xml>c</xml> returns 400 error but <items>c</items> gives 200 but error is parameter expected 

                    // XElement parse does not like '&'
                    file = file.Replace("&", "and");
                    file = file.Replace(";", String.Empty);
                    items = XElement.Parse(file);
                    // all items start and end with '<item>' so put them in a list
                    //List<XElement> itemNodes = items.Elements("item").ToList();
                    // get first item from list (for testing purposes)
                    //var s = itemNodes[0].Element("formal_name");

                    // return list of foods to be placed into our db

                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }

            return items.ToString();

        }

        public async Task InsertMiamiFoodDataAsync(string uri, string xmlContent)
        {
            try
            {
                //xmlContent = xmlContent.Replace("\r\n", String.Empty);

                //var json = JsonConvert.SerializeObject(xmlContent);
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
                            // yay
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }

        public async Task InsertMiamiFoodDataAsync2(string uri, string xmlContent)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlContent);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)await request.GetResponseAsync();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                }
            } catch(WebException e)
            {
                Console.WriteLine(e.Message);
            }
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
        
        public async Task UpdateDailyLogForUser(string uri, UserLogData data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                using (var message = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    message.Version = HttpVersion.Version10;
                    message.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _client.SendAsync(message);
                    
                    string result = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                         //yay   
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }

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
    }
}

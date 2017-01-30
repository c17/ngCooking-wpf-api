using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace apis.Client
{
    public class ApiClient
    {
        String _baseUrl;

        public ApiClient(String baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<T> Get<T>(String relativeUrl) where T: class
        {
            String url = String.Format("{0}/{1}", _baseUrl, relativeUrl);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    // ... Read the string.
                    string result = await content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(result);
                }
            }
        }

        // TODO
        // Ajouter la possibilité d'intégrer un fichier au body de la requête + des cookies (auth)
        public async Task<Boolean> Post<T>(
            String relativeUrl,
            T data,
            Dictionary<String, String> cookies = null)
            where T: class
        {
            String url = String.Format("{0}/{1}", _baseUrl, relativeUrl);

            CookieContainer cookieContainer = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookieContainer;
            var baseAddress = new Uri(_baseUrl);

            if (cookies != null)
                foreach (KeyValuePair<String, String> cookie in cookies)
                    cookieContainer.Add(baseAddress, new Cookie(cookie.Key, cookie.Value));

            using (HttpClient client = new HttpClient(handler))
            {
                String json = JsonConvert.SerializeObject(data);

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(url, jsonContent))
                {
                    return response.IsSuccessStatusCode;
                }
            }
        }

        public async Task<Boolean> PostImage(
            String relativeUrl,
            Dictionary<String, String> parameters,
            byte[] fileData,
            String fileName,
            Dictionary<String, String> cookies = null)
        {
            String url = String.Format("{0}/{1}", _baseUrl, relativeUrl);

            CookieContainer cookieContainer = new CookieContainer();
            HttpClientHandler clientHandler = new HttpClientHandler();
            //var handler = new LoggingHandler(clientHandler);
            clientHandler.CookieContainer = cookieContainer;
            var baseAddress = new Uri(_baseUrl);

            if (cookies != null)
                foreach (KeyValuePair<String, String> cookie in cookies)
                    cookieContainer.Add(baseAddress, new Cookie(cookie.Key, cookie.Value));

            using (HttpClient client = new HttpClient(clientHandler))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));    
                client.DefaultRequestHeaders.Connection.Add("keep-alive");
                //client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36"));

                //La requête est multipart/form-data
                System.Net.Http.MultipartFormDataContent multiDataContent = new System.Net.Http.MultipartFormDataContent();
                
                //On ajoute l'image avec son media type image/jpeg
                var memStream = new System.IO.MemoryStream(fileData);
                var imageContent = new System.Net.Http.ByteArrayContent(fileData);
                imageContent.Headers.ContentType = 
                    MediaTypeHeaderValue.Parse("image/jpeg");
                multiDataContent.Add(imageContent, fileName);

                //On ajoute les champs supplémentaires
                foreach (KeyValuePair<String, String> parameter in parameters) {
                    multiDataContent.Add(new System.Net.Http.StringContent(parameter.Value, Encoding.UTF8, "text/plain"), parameter.Key);
                }

                //Bug: boundary est entouré de "", et ne devrait pas l'être
                foreach (NameValueHeaderValue pValue in multiDataContent.Headers.ContentType.Parameters)
                    pValue.Value = pValue.Value.Replace("\"", String.Empty);

                using (HttpResponseMessage response = await client.PostAsync(url, multiDataContent))
                {
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }
}
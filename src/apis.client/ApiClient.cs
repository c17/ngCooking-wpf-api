using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

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
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string result = await content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(result);
            }
        }        
    }
}
using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;

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

        public async Task<Boolean> Post<T>(String relativeUrl, T data) where T: class
        {
            String url = String.Format("{0}/{1}", _baseUrl, relativeUrl);

            using (HttpClient client = new HttpClient())
            {
                String json = JsonConvert.SerializeObject(data);

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpContent = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(url, httpContent))
                {
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }
}
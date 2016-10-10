using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TamigoServices
{
    internal static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string uri, T obj)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            return client.PostAsync(uri, content);
        }

        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            var stringContent = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringContent);
        }
    }
}
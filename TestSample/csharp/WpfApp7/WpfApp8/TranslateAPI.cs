namespace WpfApp8
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    internal class TranslateAPI
    {
        private static readonly HttpClient Client = new HttpClient();
        private readonly string _apiURL = "https://openapi.naver.com/v1/papago/n2mt";

        public async Task<string> APIRequest(string body)
        {
            string url = $"{_apiURL}";
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
                requestMessage.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

                requestMessage.Headers.Add("X-Naver-Client-Id", "RgwPbpGHJHuCR0xJTCa9");
                requestMessage.Headers.Add("X-Naver-Client-Secret", "ssrdMuvhP_");

                HttpResponseMessage responseMessage = await Client.SendAsync(requestMessage);
                return await responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

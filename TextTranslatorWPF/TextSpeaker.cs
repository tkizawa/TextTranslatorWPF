using System;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace TextTranslatorWPF
{
    class TextSpeaker
    {
        private const string subscriptionKey = "6f6a4a6d7ba644f493f71272ea6a30b6";

        public static async Task Run(string text)
        {
            var authTokenSource = new AzureAuthToken(subscriptionKey.Trim());
            string authToken = string.Empty;
            try
            {
                authToken = await authTokenSource.GetAccessTokenAsync();
            }
            catch (HttpRequestException)
            {
                if (authTokenSource.RequestStatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Request to token service is not authorized (401). Check that the Azure subscription key is valid.");
                    return;
                }
                if (authTokenSource.RequestStatusCode == HttpStatusCode.Forbidden)
                {
                    Console.WriteLine("Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.");
                    return;
                }
                throw;
            }

            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Speak?text="+text+"&language=ja-jp&format=" +  WebUtility.HtmlEncode("audio/wav") + "&options=MaxQuality";
            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.Headers.Add("Authorization", authToken);
            using (WebResponse response = webRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                using (SoundPlayer player = new SoundPlayer(stream))
                {
                    player.PlaySync();
                }
            }
        }
    }
}

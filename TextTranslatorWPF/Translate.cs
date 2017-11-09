using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace TextTranslatorWPF
{
    class Translate
    {
        private const string subscriptionKey = "6f6a4a6d7ba644f493f71272ea6a30b6";

        public static string TextOut;

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

            //            string text = "Good morning.";
            string from = "ja-jp";
            string to = "en";
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + WebUtility.HtmlEncode(text) + "&from=" + from + "&to=" + to;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            using (WebResponse response = httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                string translation = (string)dcs.ReadObject(stream);
                //                Console.WriteLine("Translation for source text '{0}' from {1} to {2} is", text, "en", "ja-jp");
                //txtText2.text=translation;
                TextOut=translation;
            }
        }
    }
}

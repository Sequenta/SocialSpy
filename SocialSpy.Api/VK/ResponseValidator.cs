using Newtonsoft.Json.Linq;

namespace SocialSpy.Api.VK
{
    public class ResponseValidator
    {
        public JObject GetResponseData(string responseJson)
        {
            if (responseJson == string.Empty)
            {
                throw new EmptyResponseException();
            }
            var responseData = string.Empty;
            if (responseJson.Contains("response"))
            {
                responseData = RemoveResponseTag(responseJson);
            }
            return JObject.Parse(responseData);
        }

        public static string RemoveResponseTag(string responseJson)
        {
            var responseData = responseJson.Replace(@"{""response"":[", "");
            responseData = responseData.Remove(responseData.Length - 2);
            return responseData;
        }
    }
}
using Newtonsoft.Json.Linq;
using SocialSpy.Api.VK;

namespace SocialSpy.Service
{
    public class ResponseValidator
    {
        public JObject GetResponseData(string responseJson)
        {
            if (responseJson == string.Empty)
            {
                throw new EmptyResponseException();
            }
            var responseBody = GetResponseBody(responseJson);
            return JObject.Parse(responseBody);
        }

        public static string GetResponseBody(string responseJson)
        {
            var response = JObject.Parse(responseJson);
            var responseValues = (JArray) response["response"];
            return responseValues.First.ToString();
        }
    }
}
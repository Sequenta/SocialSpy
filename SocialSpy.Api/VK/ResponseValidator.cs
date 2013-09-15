﻿using Newtonsoft.Json.Linq;

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
            var responseBody = GetResponseBody(responseJson);
            return JObject.Parse(responseBody);
        }

        public string GetResponseBody(string responseJson)
        {
            var response = JObject.Parse(responseJson);
            var responseValues = (JArray) response["response"];
            return responseValues.First.ToString();
        }
    }
}
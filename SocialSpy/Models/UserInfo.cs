using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Json;

namespace SocialSpy.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }

        public UserInfo(string jsonParameters)
        {
            jsonParameters = jsonParameters.Replace(@"{""response"":[", "");
            jsonParameters = jsonParameters.Remove(jsonParameters.Length - 2);
            var serializer = new JsonNetSerializer();
            var values = serializer.Parse<Dictionary<string, string>>(jsonParameters);
            UserId = values["uid"];
            FirstName = values["first_name"];
            LastName = values["last_name"];
            Sex = TryGetValue("sex", values);
            City = TryGetValue("city", values);
            Country = TryGetValue("country", values);
            Image = TryGetValue("photo_200_orig", values);
        }

        private string TryGetValue(string valueName, Dictionary<string, string> values)
        {
            string outValue;
            values.TryGetValue(valueName, out outValue);
            return outValue;
        }
    }
}
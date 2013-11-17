using Newtonsoft.Json;

namespace SocialSpy.Domain
{
    public class UserInfo
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("photo_200_orig")]
        public string PictureUrl { get; set; }
        [JsonProperty("sex")]
        public string Sex { get; set; }
        [JsonProperty("online")]
        public string IsOnline { get; set; }
    }
}
using System.IO;
using System.Text;
using Microsoft.AspNet.SignalR.Client.Http;
using RabbitMQ.Client;

namespace SocialSpy.Api.VK
{
    public class VkInformer
    {
        public string GetUserInfo(string id)
        {
            var http = new DefaultHttpClient();
            var requestUrl = string.Format("https://api.vk.com/method/getProfiles?uids={0}&fields=sex,city,country,photo_200_orig", id);
            string userInfo;
            using (var result = http.Post(requestUrl, request => { }).Result)
            {
                using (var resultStream = new StreamReader(result.GetStream()))
                {
                    userInfo = resultStream.ReadToEnd();
                }
            }
            return userInfo;
        }

        public void GetFriendsInfo(string id)
        {
            PublishUserId(id);
        }

        private void PublishUserId(string id)
        {
            var connectionFactory = new ConnectionFactory { HostName = "localhost" };
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(id);
                    channel.QueueDeclare("idQueue", false, false, false, null);
                    channel.BasicPublish("", "idQueue", null, body);
                    channel.QueueDeclare("statisticQueue", false, false, false, null);
                    channel.BasicPublish("", "statisticQueue", null, body);
                }
            }
        }
    }
}
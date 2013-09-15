using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SocialSpy.Service.Properties;

namespace SocialSpy.Service
{
    public class StatisticSender
    {
        private readonly ConnectionFactory connectionFactory;
        private readonly IHubProxy clientsideHubProxy;

        public StatisticSender()
        {
            var clientsideHubConnection = new HubConnection(Settings.Default.HubConnectionString);
            clientsideHubProxy = clientsideHubConnection.CreateHubProxy(Settings.Default.HubName);
            clientsideHubConnection.Start().Wait();
            connectionFactory = new ConnectionFactory { HostName = Settings.Default.HostName };
        }

        public void SendStatistic()
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("statisticQueue", false, false, false, null);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("statisticQueue", true, consumer);
                    while (true)
                    {
                        var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var body = eventArgs.Body;
                        var message = Encoding.UTF8.GetString(body);
                        if (message != string.Empty)
                        {
                            SendStatistic(message);
                        }
                    }
                }
            }
        }

        private void SendStatistic(string userId)
        {
            var jsonFriendsInfo = GetFriendsInfo(userId);
            var statistic = FormUpStatistic(jsonFriendsInfo);
            Console.WriteLine(statistic);
            clientsideHubProxy.Invoke("ShowUserStatistic", statistic).Wait();
        }

        private string FormUpStatistic(string jsonFriendsInfo)
        {
            var response = JObject.Parse(jsonFriendsInfo);
            var friendsInfo = (JArray)response["response"];
            var sexStatistic = GetSexStatistic(friendsInfo) + ",";
            var onlineStatistic = GetOnlineStatistic(friendsInfo);
            return string.Format("{0}{1}{2}{3}","{",sexStatistic,onlineStatistic,"}");
        }

        private string GetSexStatistic(JArray friendsInfo)
        {
            var male = friendsInfo.Select(friendInfo => friendInfo["sex"].ToString()).Count(sex => sex == "2");
            var unknown = friendsInfo.Select(friendInfo => friendInfo["sex"].ToString()).Count(sex => sex == "0");
            var female = friendsInfo.Select(friendInfo => friendInfo["sex"].ToString()).Count(sex => sex == "1");
            var sexStatistic = string.Format("Male:{0},Female:{1},Unknown:{2}",male, female, unknown);
            return sexStatistic;
        }

        private string GetOnlineStatistic(JArray friendsInfo)
        {
            var online = friendsInfo.Select(friendInfo => friendInfo["online"].ToString()).Count(isOnline => isOnline=="1");
            var offline = friendsInfo.Select(friendInfo => friendInfo["online"].ToString()).Count(isOnline => isOnline == "0");
            var onlineStatistic = string.Format("Online:{0},Offline:{1}", online, offline);
            return onlineStatistic;
        }

        private string GetFriendsInfo(string userId)
        {
            string json;
            var urlString = String.Format("https://api.vk.com/method/friends.get?user_id={0}&count=100&fields=sex,online", userId);
            var request = WebRequest.Create(new Uri(urlString)) as HttpWebRequest;
            var response = (HttpWebResponse)request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    json = sr.ReadToEnd();
                }
            }
            return string.IsNullOrEmpty(json) ? "" : json;
        }
    }
}
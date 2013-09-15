using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNet.SignalR.Client.Hubs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SocialSpy.Service.Properties;

namespace SocialSpy.Service
{
    public class InfoSender
    {
        private ConnectionFactory connectionFactory;
        private HubConnection clientsideHubConnection;
        private IHubProxy clientsideHubProxy;

        public InfoSender()
        {
            clientsideHubConnection = new HubConnection(Settings.Default.HubConnectionString);
            clientsideHubProxy = clientsideHubConnection.CreateHubProxy(Settings.Default.HubName);
            clientsideHubConnection.Start().Wait();
            connectionFactory = new ConnectionFactory { HostName = Settings.Default.HostName };
        }

        public void SendFriendInfo()
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("friendsIdQueue", false, false, false, null);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("friendsIdQueue", true, consumer);
                    while (true)
                    {
                        var eventArgs = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var body = eventArgs.Body;
                        var message = Encoding.UTF8.GetString(body);
                        if (message != string.Empty)
                        {
                            PublishFriendInfo(message);
                        }
                    }
                }
            }
        }

        private void PublishFriendInfo(string message)
        {
            var friendInfo = GetFriendInfo(message);
            Console.WriteLine(friendInfo);
            clientsideHubProxy.Invoke("ShowFriendInfo", friendInfo).Wait();
        }

        private string GetFriendInfo(string id)
        {
            string json = String.Empty;
            string urlString = String.Format("https://api.vk.com/method/getProfiles?uids={0}&fields=photo_50", id);
            var request = WebRequest.Create(new Uri(urlString)) as HttpWebRequest;
            var response = (HttpWebResponse)request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    json = sr.ReadToEnd();
                }
            }
            json = ResponseValidator.GetResponseBody(json);
            return string.IsNullOrEmpty(json) ? "" : json;
        }
    }
}
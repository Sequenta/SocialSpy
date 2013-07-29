using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SocialSpy.Service.Properties;

namespace SocialSpy.Service
{
    public class InfoSender
    {
        private ConnectionFactory connectionFactory;
        public delegate void MessageReceived(object sender, string message);
        public event MessageReceived OnMessageReceived = delegate { };

        public InfoSender()
        {
            connectionFactory = new ConnectionFactory { HostName = Settings.Default.HostName };
            OnMessageReceived += GetFriendInfo;
        }

        public void SendFriendId()
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
                            OnMessageReceived(this, message);
                        }
                    }
                }
            }
        }

        private void GetFriendInfo(object sender, string message)
        {
            var friendInfo = GetFriendsList(message);
            PublishFriendInfo(friendInfo);
        }

        private string GetFriendsList(string id)
        {
            string json = String.Empty;
            string urlString = String.Format("https://api.vk.com/method/getProfiles?uids={0}", id);
            var request = WebRequest.Create(new Uri(urlString)) as HttpWebRequest;
            var response = (HttpWebResponse)request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(stream))
                {
                    json = sr.ReadToEnd();
                }
            }

            json = json.Replace(@"{""response"":[", "");
            json = json.Remove(json.Length - 2);

            if (string.IsNullOrEmpty(json))
            {
                return "";
            }

            return json;
        }

        private void PublishFriendInfo(string friendInfo)
        {
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    var body = Encoding.UTF8.GetBytes(friendInfo);
                    channel.QueueDeclare("friendsInfoQueue", false, false, false, null);
                    channel.BasicPublish("", "friendsInfoQueue", null, body);
                }
            }
        }
    }
}
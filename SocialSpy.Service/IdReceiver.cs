using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SocialSpy.Service.Properties;

namespace SocialSpy.Service
{
    public class IdReceiver
    {
        private ConnectionFactory connectionFactory;
        public delegate void MessageReceived(object sender, string message);
        public event MessageReceived OnMessageReceived = delegate { };

        public IdReceiver()
        {
            connectionFactory = new ConnectionFactory{HostName = Settings.Default.HostName};
            OnMessageReceived += GetUserFriends;
        }

        public void ReceiveUserId()
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("idQueue", false, false, false, null);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("idQueue", true, consumer);
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

        private void GetUserFriends(object sender, string message)
        {
            var friendsList = GetFriendsList(message);
            PublishFriendsId(friendsList);
        }

        private List<string> GetFriendsList(string id)
        {
            string json = String.Empty;
            string urlString = String.Format("https://api.vk.com/method/friends.get?user_id={0}&count=10", id);
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
                return new List<string>();
            }

            return json.Split(new []{','}).ToList();
        }

        private void PublishFriendsId(List<string> friendsList)
        {
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    foreach (var friend in friendsList)
                    {
                        var body = Encoding.UTF8.GetBytes(friend);
                        channel.QueueDeclare("friendsIdQueue", false, false, false, null);
                        channel.BasicPublish("", "friendsIdQueue", null, body);
                        Thread.Sleep(2000);
                    }
                }
            }
        }
    }
}
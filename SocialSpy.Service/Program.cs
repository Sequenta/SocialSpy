using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialSpy.Service.Properties;

namespace SocialSpy.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var mode = args[0];
            Console.WriteLine("Mode = {0}",mode);
            var hostName = Settings.Default.HostName;
            IdReceiver receiver;
            InfoSender sender;
            switch (mode)
            {
                case "idreceiver":
                    receiver = new IdReceiver();
                    receiver.ReceiveUserId();
                    break;
                case "infosender":
                    sender = new InfoSender();
                    sender.SendFriendId();
                    break;
                default:
                    Console.WriteLine("Unknown mode! Is="+mode);
                    throw new ArgumentException("Unknown mode! Is=" + mode);
            }
        }
    }
}

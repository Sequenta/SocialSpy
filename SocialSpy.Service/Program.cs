using System;

namespace SocialSpy.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var mode = args[0];
            Console.WriteLine("Mode = {0}",mode);
            switch (mode)
            {
                case "idreceiver":
                    var receiver = new IdReceiver();
                    receiver.ReceiveUserId();
                    break;
                case "infosender":
                    var infoSender = new InfoSender();
                    infoSender.SendFriendInfo();
                    break;
                case "statisticsender":
                    var statisticSender = new StatisticSender();
                    statisticSender.SendStatistic();
                    break;
                default:
                    Console.WriteLine("Unknown mode! Is="+mode);
                    throw new ArgumentException("Unknown mode! Is=" + mode);
            }
        }
    }
}

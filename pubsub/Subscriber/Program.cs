using System;
using EasyNetQ;

namespace Subscriber {
    class Program {
        const string AMQP = "amqps://gsfihevq:eFtfoAM2d-JvSPxFBMu8te_VYbO91cBN@bunny.rmq.cloudamqp.com/gsfihevq";

        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(AMQP);
            var subscriberId = "itkonekt";
            bus.PubSub.Subscribe<string>(subscriberId, message => Console.WriteLine(message));
            Console.WriteLine("Subscribed! Listening for messages...");
            Console.ReadLine();
        }
    }
}

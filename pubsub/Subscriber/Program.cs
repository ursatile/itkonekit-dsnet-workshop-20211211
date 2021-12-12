using System;
using EasyNetQ;
using Messages;
using System.Threading;

namespace Subscriber {
    class Program {
        const string AMQP = "amqps://gsfihevq:eFtfoAM2d-JvSPxFBMu8te_VYbO91cBN@bunny.rmq.cloudamqp.com/gsfihevq";

        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(AMQP);
            var subscriberId = $"subscriber@{Environment.MachineName}";
            bus.PubSub.Subscribe<string>(subscriberId, message => {
                Console.WriteLine(message);
                Thread.Sleep(TimeSpan.FromSeconds(5));
            });
            bus.PubSub.Subscribe<Greeting>(subscriberId, HandleGreeting);
            Console.WriteLine("Subscribed! Listening for messages...");
            Console.ReadLine();
        }

        static void HandleGreeting(Greeting g) {
            Console.WriteLine($"{g.From} says (in {g.Language}:");
            Console.WriteLine($"  {g.Text}");
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}

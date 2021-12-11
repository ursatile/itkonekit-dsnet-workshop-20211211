using System;
using EasyNetQ;

namespace Publisher {
    class Program {
        const string AMQP = "amqps://gsfihevq:eFtfoAM2d-JvSPxFBMu8te_VYbO91cBN@bunny.rmq.cloudamqp.com/gsfihevq";

        static void Main(string[] args) {
            using var bus = RabbitHutch.CreateBus(AMQP);
            var count = 1;
            while (true) {
                Console.WriteLine("Press a key to publish a message...");
                Console.ReadKey(false);
                var message = $"Message {count++} from {Environment.MachineName} at {DateTime.UtcNow:O}";
                bus.PubSub.Publish(message);
                Console.WriteLine($"Published message: {message}");
            }
        }
    }
}

using System;
using System.IO;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Autobarn.AuditLog {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        static void Main(string[] args) {
            var amqp = config.GetConnectionString("AutobarnRabbitMQConnectionString");
            using var bus = RabbitHutch.CreateBus(amqp);
            var subscriberId = $"autobarn.auditlog@{Environment.MachineName}";
            bus.PubSub.Subscribe<NewVehicleMessage>(subscriberId, HandleNewVehicleMessage);
            Console.WriteLine("Running Autobarn.AuditLog. Listening for messages...");
            Console.ReadLine();
        }

        private static void HandleNewVehicleMessage(NewVehicleMessage nvm) {
            var csv = $"{nvm.Registration},{nvm.Manufacturer},{nvm.ModelName},{nvm.Year},{nvm.Color},{nvm.ListedAt:O}";
            File.AppendAllText("vehicles.log", csv);
            Console.WriteLine(csv);
        }


        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}

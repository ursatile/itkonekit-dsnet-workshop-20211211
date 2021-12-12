using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Autobarn.Notifier {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static HubConnection hub;
        private static IBus bus;
        private const string SUBSCRIBER_ID = "Autobarn.Notifier";
        static async Task Main(string[] args) {
            JsonConvert.DefaultSettings = JsonSettings;
            Thread.Sleep(TimeSpan.FromSeconds(5));
            bus = RabbitHutch.CreateBus(config.GetConnectionString("AutobarnRabbitMQConnectionString"));
            hub = new HubConnectionBuilder().WithUrl(config["AutobarnSignalRHubUrl"]).Build();
            await hub.StartAsync();
            Console.WriteLine("Hub started and connected!");
            bus.PubSub.Subscribe<NewVehiclePriceMessage>(SUBSCRIBER_ID, HandleNewVehiclePriceMessage);
            Console.WriteLine("Subscribed to NewVehiclePriceMessages");
            Console.ReadLine();
        }

        private static async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage arg) {
            var json = JsonConvert.SerializeObject(arg);
            await hub.SendAsync("SendAMessageToAllThePeopleOnOurWebsite", "Autobarn.Notifier",
                json);
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        private static JsonSerializerSettings JsonSettings() =>
            new JsonSerializerSettings {
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
    }
}

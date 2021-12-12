using System;
using System.IO;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Autobarn.PricingClient {
    class Program {
        private const string SUBSCRIBER_ID = "Autobarn.PricingClient";
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static IBus bus;
        private static Pricer.PricerClient grpcClient;

        static async Task Main(string[] args) {
            Console.WriteLine("Running Autobarn.PricingClient.");
            bus = RabbitHutch.CreateBus(config.GetConnectionString("AutobarnRabbitMQConnectionString"));
            using var channel = GrpcChannel.ForAddress(config["AutobarnPricingServerEndpoint"]);
            grpcClient = new Pricer.PricerClient(channel);
            Console.WriteLine("Connected to gRPC! Subscribing to messages...");
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
            Console.WriteLine("Subscribed to NewVehicleMessages");
            Console.ReadLine();
        }

        private static async Task HandleNewVehicleMessage(NewVehicleMessage arg) {
            var request = new PriceRequest() {
                ManufacturerName = arg.Manufacturer,
                ModelName = arg.ModelName,
                Color = arg.Color,
                Year = arg.Year
            };
            var price = await grpcClient.GetPriceAsync(request);
            Console.WriteLine(
                $"Got a price: {arg.Manufacturer} {arg.ModelName} ({arg.Color}, {arg.Year}) : {price.Price} {price.CurrencyCode}");
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

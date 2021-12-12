using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace Autobarn.Notifier {
    class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        private static HubConnection hub;

        static async Task Main(string[] args) {
            hub = new HubConnectionBuilder().WithUrl(config["AutobarnSignalRHubUrl"]).Build();
            await hub.StartAsync();
            Console.WriteLine("Hub started and connected!");
            Console.WriteLine("Press any key to send a message");
            while (true) {
                Console.ReadKey();
                var message = $"Hello from the Notifier at {DateTime.UtcNow}";
                await hub.SendAsync("SendAMessageToAllThePeopleOnOurWebsite",
                    "Autobarn.Notifier", message);
                Console.WriteLine($"Sent message: {message}");
            }
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

using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Autobarn.AuditLog {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        static void Main(string[] args) {
            var amqp = config.GetConnectionString("AutobarnRabbitMQConnectionString");
            Console.WriteLine(amqp);
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

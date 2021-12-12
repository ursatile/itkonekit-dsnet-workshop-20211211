using System;
using Grpc.Net.Client;
using GrpcGreeting;
using System.Threading.Tasks;

namespace GrpcClient {
    class Program {
        static async Task Main(string[] args) {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            Console.WriteLine("Connected to GRPC endpoint. Press 1,2 or 3 to send a request:");
            while (true) {
                var friendliness = Int32.Parse(Console.ReadKey().KeyChar.ToString());
                Console.WriteLine();
                var request = new HelloRequest {
                    Name = "Dylan",
                    Friendliness = friendliness,
                    Language = "en"
                };
                var reply = await client.SayHelloAsync(request);
                Console.WriteLine(reply.Message);
            }

        }
    }
}

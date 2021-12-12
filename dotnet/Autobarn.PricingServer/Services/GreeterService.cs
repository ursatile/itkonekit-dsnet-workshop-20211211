using System;
using System.Threading.Tasks;
using Autobarn.PricingEngine;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer.Services {
    public class PricerService : Pricer.PricerBase {
        private readonly ILogger<PricerService> logger;
        public PricerService(ILogger<PricerService> logger) {
            this.logger = logger;
        }

        public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
            var (price, currencyCode) = CalculatePrice(request);

            return Task.FromResult(new PriceReply {
                Price = price,
                CurrencyCode = currencyCode
            });
        }

        public (int, string) CalculatePrice(PriceRequest priceRequest) {
            if (priceRequest.ManufacturerName == "DMC") return (100000, "USD");
            return priceRequest.Color.Equals("orange", StringComparison.InvariantCultureIgnoreCase) ? (500, "GBP") : (2000, "EUR");
        }
    }
}

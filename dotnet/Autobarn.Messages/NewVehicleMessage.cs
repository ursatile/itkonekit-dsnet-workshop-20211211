using System;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Registration { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public DateTimeOffset ListedAt { get; set; }

        public NewVehiclePriceMessage WithPrice(int price, string currency) {
            return new NewVehiclePriceMessage() {
                Registration = this.Registration,
                Color = this.Color,
                Year = this.Year,
                Manufacturer = this.Manufacturer,
                ModelName = this.ModelName,
                Price = price,
                Currency = currency
            };
        }
    }

    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string Currency { get; set; }
    }
}

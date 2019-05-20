using System;

namespace ShippingCosts
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public PackageSize PackageSize { get; set; }
        public CarrierCode CarrierCode { get; set; }

        public decimal Price { get; set; } = 0.0m;
        public decimal Discount { get; set; } = 0.0m;

        public Transaction(CarrierCode carrierCode, PackageSize packageSize, DateTime date)
        {
            CarrierCode = carrierCode;
            PackageSize = packageSize;
            Date = date;
            Price = CarrierData.GetPrice(carrierCode, packageSize);
        }

        public Transaction(string row)
        {
            var items = row.Split(' ');

            //Transaction format validation
            if (items.Length == 3 &&
                DateTime.TryParse(items[0], out var date) &&
                Enum.TryParse(items[1], out PackageSize size) &&
                Enum.TryParse(items[2], out CarrierCode code))
            {
                Date = date;
                PackageSize = size;
                CarrierCode = code;
                Price = CarrierData.GetPrice(code, size);
            }
        }

        public bool IsValid()
        {
            return Date != DateTime.MinValue &&
                PackageSize != PackageSize.None &&
                CarrierCode != CarrierCode.None;
        }
    }
}
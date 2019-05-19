using System;
using System.Collections.Generic;

namespace ShippingCosts
{
    public class Carrier
    {
        public CarrierCode CarrierCode { get; set; }
        public Dictionary<PackageSize, double> ShippingPrices { get; set; }

        public Carrier(CarrierCode carrierCode, Dictionary<PackageSize, double> shippingPrices)
        {
            CarrierCode = carrierCode;
            ShippingPrices = shippingPrices;
        }
    }
}

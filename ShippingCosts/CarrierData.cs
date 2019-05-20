using System;
using System.Collections.Generic;
using System.Linq;

namespace ShippingCosts
{
    public class CarrierData
    {
        private static readonly Dictionary<CarrierCode, Dictionary<PackageSize, decimal>> CarrierPricing = GetCarrierPricing();

        private static Dictionary<CarrierCode, Dictionary<PackageSize, decimal>> GetCarrierPricing()
        {
            var LPPricing = new Dictionary<PackageSize, decimal>() {
                { PackageSize.S, 1.5m }, { PackageSize.M, 4.9m }, { PackageSize.L, 6.9m }
            };
            var MRPricing = new Dictionary<PackageSize, decimal>() {
                { PackageSize.S, 2m }, { PackageSize.M, 3m }, { PackageSize.L, 4m }
            };
            //var TestPricing = new Dictionary<PackageSize, decimal>() {
            //    { PackageSize.S, 0.5m }, { PackageSize.M, 3m }, { PackageSize.L, 4m }
            //};

            return new Dictionary<CarrierCode, Dictionary<PackageSize, decimal>>()
            {
                { CarrierCode.LP, LPPricing },
                { CarrierCode.MR, MRPricing },
                //{ CarrierCode.TEST, TestPricing }
            };
        }

        /// <summary>
        /// Gets the price of the transaction based on transaction
        /// </summary>
        /// <returns>Price of the package to be sent via specified carrier</returns>
        /// <param name="transaction">Transaction to get the price for</param>
        public static decimal GetPrice(Transaction transaction)
        {
            return CarrierPricing[transaction.CarrierCode][transaction.PackageSize];
        }

        /// <summary>
        /// Gets the price of the transaction based on package size and carrier
        /// </summary>
        /// <returns>Price of the specified package size to be sent via specified carrier</returns>
        /// <param name="carrierCode">Valid carrier code</param>
        /// <param name="packageSize">Valid package size</param>
        public static decimal GetPrice(CarrierCode carrierCode, PackageSize packageSize)
        {
            return CarrierPricing[carrierCode][packageSize];
        }

        /// <summary>
        /// Retrieves the lowest price of a <see cref="PackageSize"/> package of all the carriers
        /// </summary>
        /// <param name="packageSize">Lowest package size to look for</param>
        /// <returns>Lowest package price</returns>
        public static decimal GetLowestPrice(PackageSize packageSize)
        {
            return CarrierPricing
                                //Only carriers with pricing for packageSize
                                .Where(carrier => carrier.Value.ContainsKey(packageSize))
                                .Min(carrier => carrier.Value[packageSize]);
        }
    }
}

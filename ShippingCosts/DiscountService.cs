using System;
using System.Collections.Generic;
using System.Linq;

namespace ShippingCosts
{
    public class DiscountService
    {
        private int LargePackageCount;
        private decimal DiscountBalance = 10;

        /// <summary>
        /// Calculates discounts for all <paramref name="transactions"/> provided
        /// </summary>
        /// <returns>Collection of discounts</returns>
        /// <param name="transactions">Transactions to calculate the discount amounts for</param>
        public void ApplyDiscounts(Transaction[] transactions)
        {
            var monthGroups = transactions
                .Where(t => t.IsValid())
                .GroupBy(transaction => transaction.Date.Month);

            foreach (var group in monthGroups)
            {
                ResetLimits();
                foreach (var transaction in group)
                {
                    ApplyDiscount(transaction);
                }
            }
        }

        /// <summary>
        /// Calculates the discount amount for <paramref name="transaction"/>
        /// </summary>
        /// <returns>Discount amount</returns>
        /// <param name="transaction">Transaction to calculate the discount amount for</param>
        public void ApplyDiscount(Transaction transaction)
        {
            switch (transaction.PackageSize)
            {
                case PackageSize.S:
                    transaction.Discount = CarrierData.GetPrice(transaction) - CarrierData.GetLowestPrice(transaction.PackageSize);
                    break;
                case PackageSize.L:
                    //please note that LargePackageCount++ happens after the conditional check
                    if (transaction.CarrierCode == CarrierCode.LP && LargePackageCount++ == 2)
                        transaction.Discount = CarrierData.GetPrice(transaction);
                    break;
            }

            DeductFromDiscountBalance(transaction);
        }

        private void DeductFromDiscountBalance(Transaction transaction)
        {
            if (DiscountBalanceSpent(transaction))
            {
                transaction.Discount = DiscountBalance;
            }

            //deduct the discount from discount balance
            DiscountBalance -= transaction.Discount;
        }

        private bool DiscountBalanceSpent(Transaction transaction)
        {
            return DiscountBalance - transaction.Discount <= 0;
        }

        private void ResetLimits()
        {
            LargePackageCount = 0;
            DiscountBalance = 10;
        }
    }
}
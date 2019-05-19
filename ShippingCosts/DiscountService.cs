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
        public IEnumerable<Transaction> ApplyDiscounts(IEnumerable<Transaction> transactions)
        {
            foreach (var group in transactions.GroupBy(transaction => transaction.Date.Month))
            {
                ResetLimits();
                foreach (var transaction in group)
                {
                    yield return ApplyDiscount(transaction);
                }
            }
        }

        /// <summary>
        /// Calculates the discount amount for <paramref name="transaction"/>
        /// </summary>
        /// <returns>Discount amount</returns>
        /// <param name="transaction">Transaction to calculate the discount amount for</param>
        public Transaction ApplyDiscount(Transaction transaction)
        {
            switch (transaction.PackageSize)
            {
                case PackageSize.S:
                    transaction.Discount = CarrierData.GetPrice(transaction) - CarrierData.GetLowestPrice(transaction.PackageSize);
                    break;
                case PackageSize.L:
                    if (transaction.CarrierCode == CarrierCode.LP && LargePackageCount++ == 2)
                        transaction.Discount = CarrierData.GetPrice(transaction);
                    break;
            }

            return ApplyDiscountLimit(transaction);
        }

        private Transaction ApplyDiscountLimit(Transaction transaction)
        {
            if (EmptyingDiscountBalance(transaction))
            {
                transaction.Discount = DiscountBalance;
            }

            DiscountBalance -= transaction.Discount;
            return transaction;
        }

        private bool EmptyingDiscountBalance(Transaction transaction)
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
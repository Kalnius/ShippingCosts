using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShippingCosts
{
    class Program
    {
        static void Main(string[] args)
        {
            var discountService = new DiscountService();
            var file = File.ReadAllLines("input.txt");

            var discountedTransactions = discountService.ApplyDiscounts(file.Select(line => new Transaction(line)));
            var zippedData = file.Zip(discountedTransactions, (line, discount) => (line,discount));

            foreach ((var line, var transaction) in zippedData)
            {
                if (transaction.IsValid())
                {
                    var updatedPrice = (CarrierData.GetPrice(transaction) - transaction.Discount).ToString("#,##0.00");
                    var discount = transaction.Discount > 0 ? transaction.Discount.ToString("#,##0.00") : "-";

                    Console.WriteLine($"{line} {updatedPrice} {discount}");
                }
                else Console.WriteLine($"{line} Ignored");
            }
            
            Console.ReadKey();
        }
    }
}

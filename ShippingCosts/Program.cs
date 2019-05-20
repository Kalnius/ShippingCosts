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
            var file = File.ReadAllLines("input.txt");
            var transactions = file.Select(line => new Transaction(line)).ToArray();

            new DiscountService().ApplyDiscounts(transactions);

            //zipping here only to be able to simultaneously iterate over both the line from input.txt and the transaction
            foreach ((var line, var transaction) in file.Zip(transactions, (line, transaction) => (line, transaction)))
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

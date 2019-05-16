using System;
using System.IO;
using System.Linq;

namespace ShippingCosts
{
    class Program
    {
        static void Main(string[] args)
        {
            var transactions = File.ReadAllLines("input.txt")
                .Select(row => new Transaction(row.Split(' ')));

            foreach (var transaction in transactions)
                Console.WriteLine($"{transaction.Date.ToString("yyyy-MM-dd")} {transaction.PackageSize} {transaction.CarrierCode} {transaction.Valid}");
            Console.ReadKey();
        }
    }
}

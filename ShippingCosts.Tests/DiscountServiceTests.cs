using System;
using System.Linq;
using NUnit.Framework;
using ShippingCosts;

namespace Tests
{
    public class DiscountServiceTests
    {
        string TransactionsText =
            "2015-02-01 S MR\n" +
            "2015-02-02 S MR\n" +
            "2015-02-02 S MR\n" +
            "2015-02-02 S MR\n" +
            "2015-02-03 L LP\n" +
            "2015-02-05 S LP\n" +
            "2015-02-06 S MR\n" +
            "2015-02-06 L LP\n" +
            "2015-02-07 L MR\n" +
            "2015-02-08 M MR\n" +
            "2015-02-09 L LP\n" +
            "2015-02-10 L LP\n" +
            "2015-02-10 S MR\n" +
            "2015-02-10 S MR\n" +
            "2015-02-11 L LP\n" +
            "2015-02-12 M MR\n" +
            "2015-02-13 M LP\n" +
            "2015-02-15 S MR\n" +
            "2015-02-17 L LP\n" +
            "2015-02-17 S MR\n" +
            "2015-02-24 L LP\n" +
            "2015-02-29 CUSPS\n" +
            "2015-03-01 S MR\n";

        Transaction[] Transactions;

        [SetUp]
        public void Setup()
        {
            var transactionsStrings = TransactionsText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var transactions = transactionsStrings.Select(str => new Transaction(str)).ToArray();
            new DiscountService().ApplyDiscounts(transactions);
            Transactions = transactions;
        }

        [Test]
        public void ValidTransactionString_ConstructsValidTransaction()
        {
            var transaction = new Transaction("2015-02-07 L MR");

            Assert.AreEqual(transaction.Date.ToString("yyyy-MM-dd"), "2015-02-07");
            Assert.AreEqual(transaction.PackageSize, PackageSize.L);
            Assert.AreEqual(transaction.CarrierCode, CarrierCode.MR);
            Assert.IsTrue(transaction.IsValid());
        }

        [Test]
        public void NonValidTransactionString_ConstructsNonValidTransaction()
        {
            var transaction = new Transaction("2015-02-29 CUSPS");

            Assert.AreEqual(transaction.Date.ToString("yyyy-MM-dd"), "0001-01-01");
            Assert.AreEqual(transaction.PackageSize, PackageSize.None);
            Assert.AreEqual(transaction.CarrierCode, CarrierCode.None);
            Assert.IsFalse(transaction.IsValid());
        }

        //TESTING "ANY S PACKAGE SHOULD HAVE LOWEST PRICE AMONG ALL CARRIERS" RULE
        [Test]
        public void WhenSendingSmallPackage_LowestPriceIsApplied()
        {
            var transaction = new Transaction(CarrierCode.MR, PackageSize.S, DateTime.Now);

            new DiscountService().ApplyDiscount(transaction);
            Assert.AreEqual(CarrierData.GetLowestPrice(PackageSize.S), transaction.Price - transaction.Discount);
        }

        //TESTING "THIRD L PACKAGE A CALENDAR MONTH IS FREE" RULE
        [Test]
        public void WhenSendingThirdLargePackageThisMonth_ClientGetsFullDiscount()
        {
            //Tenth transaction is the third Large package this month
            Assert.AreEqual(6.9, Transactions[10].Discount);
        }

        //TESTING "THERE IS A 10 EURO DISCOUNT LIMIT PER MONTH" RULE
        [Test]
        public void DiscountsCannotExceedLimitPerMonth()
        {
            //Seventeenth transaction is Small package for MR carrier
            //It should have a 0.5 discount, but does not - discount balance is 0
            Assert.AreEqual(0, Transactions[17].Discount);
        }
    }
}
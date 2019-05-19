namespace ShippingCosts
{
    public class Discount
    {
        public Transaction Transaction { get; set; }
        public double InitialPrice { get; set; }
        public double Amount { get; set; }
    }
}
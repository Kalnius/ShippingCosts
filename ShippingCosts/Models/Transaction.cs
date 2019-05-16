using System;
using System.Runtime.Serialization;

namespace ShippingCosts
{
    [Serializable]
    public class Transaction
    {
        public bool Valid = true;
        public DateTime Date { get; set; }
        public PackageSize PackageSize { get; set; }
        public CarrierCode CarrierCode { get; set; }

        public Transaction(string[] transactionRow)
        {
            if (transactionRow.Length == 3 && 
                DateTime.TryParse(transactionRow[0], out var date) && 
                Enum.TryParse(transactionRow[1], out PackageSize packageSize) && 
                Enum.TryParse(transactionRow[2], out CarrierCode carrierCode))
            {
                Date = date;
                PackageSize = packageSize;
                CarrierCode = carrierCode;
            }
            else Valid = false;
        }
    }
}

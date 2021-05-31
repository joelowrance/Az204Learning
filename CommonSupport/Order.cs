using System.Text.Json;

namespace CommonSupport
{
    public class Order
    {
        public string Orderid  { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice  { get; set; }

        public override string ToString()
        {
            //return $"OrderId: {Orderid}, Qty: {Quantity}, Price {UnitPrice:C}";
            return JsonSerializer.Serialize(this);
        }
    }

}
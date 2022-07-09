namespace DemoWebshopApi.Data.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public bool Paid { get; set; }
    }
}

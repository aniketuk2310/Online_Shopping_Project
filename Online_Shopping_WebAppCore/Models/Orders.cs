using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace Online_Shopping_WebAPICore.Models
{
    public class Orders
    {
        [Key]
        public int OId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public int Quantity { get; set; }
        public Product Product { get; set; }
        public User Customer { get; set; }

        public string PaymentMethod { get; set; }

        public OStatus status { get; set; }
    }
    public enum OStatus
    {
        Delivered,Undelivered
    }
}

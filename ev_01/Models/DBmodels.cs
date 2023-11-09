using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ev_01.Models
{
    public class Food
    {
        public Food()
        {
            this.Receipts = new List<Receipt>();
        }
        public int FoodId { get; set; }
        public string FoodName { get; set; }

        //
        public virtual ICollection<Receipt> Receipts { get; set; }

    }
    public class Order
    {
        public Order()
        {
            this.Receipts = new List<Receipt>();
        }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string? Picture { get; set; }
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public bool AmountPaid { get; set; }

        //
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
    public class Receipt
    {
        public int ReceiptId { get; set; }

        [ForeignKey("Food")]
        public int FoodId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        //
        public virtual Food? Food { get; set; }
        public virtual Order? Order { get; set; }
    }

    public class ResturantDBcontext : DbContext
    {
        public ResturantDBcontext(DbContextOptions<ResturantDBcontext> options):base(options)
        {
            
        }

        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

    }

}

using System.ComponentModel.DataAnnotations;

namespace ev_01.Models.ViewModels
{
    public class OrderVM
    {
        public OrderVM()
        {
            this.FoodList = new List<int>();
        }

        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string? Picture { get; set; }

        [Display(Name = "Image")]
        public IFormFile? PictureFile { get; set; }
        public DateTime OrderDate { get; set; }
        public int TableNumber { get; set; }
        public bool AmountPaid { get; set; }

        public List<int> FoodList { get; set; }
    }
}

using SalesApp.API.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApp.API.Data.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public PaymentType PaymentType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public DateOnly SaleDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public IEnumerable<SaleDetail> Details { get; set; } = new List<SaleDetail>();
    }
}
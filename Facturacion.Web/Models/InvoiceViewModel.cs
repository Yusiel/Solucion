using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturacion.Web.Models
{
    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }

        public bool Paid { get; set; }

        public DateTime IssueDate { get; set; }

        public List<InvoiceDetailViewModel> InvoiceDetail { get; set; }
    }

    public class InvoiceDetailViewModel
    {
        public int InvoiceDetailId { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; }

        public decimal SellPrice { get; set; }

        public decimal? Total { get; set; }

        public ProductViewModel Product { get; set; }
    }

    public class ProductViewModel
    {

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class ClientsViewModels
    {
        public int ClientId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        [Display(Name = "Born date")]
        public DateTime BornDate { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public virtual List<Invoice> Invoice { get; set; }

        public decimal Profit { get; set; }

        [Display(Name = "Is Profit?")]
        public bool IsProfit 
        { get
            {
                if (Profit>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        
        }

        [Display(Name ="Discount 10%")]
        public decimal Discount
        {
            get
            {
                if (Profit > 0)
                {
                    return (Profit*Convert.ToDecimal(0.1));
                }
                else
                {
                    return 0;
                }
            }

        }
    }
}
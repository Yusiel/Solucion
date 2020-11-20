using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Facturacion.Web.Models;

namespace Facturacion.Web.Controllers
{
    public class ClientsController : Controller
    {
        readonly FacturacionContext db = new FacturacionContext();

        // GET: Clients
        public ActionResult Index()
        {
            var clients = db.Client.ToList();
            List<ClientsViewModels> clientsViewModel = clients.Select(cli =>
             new ClientsViewModels()
             {
                 Address =cli.Address,
                 BornDate=cli.BornDate,
                 ClientId=cli.ClientId,
                 Email=cli.Email,
                 Invoice=cli.Invoice.ToList(),
                 Name=cli.Name,
                 PhoneNumber=cli.PhoneNumber,
                 Profit=GetProfit(cli.Invoice.ToList())
             }).ToList();

            return View(clientsViewModel);
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client cli = db.Client.Find(id);
            if (cli == null)
            {
                return HttpNotFound();
            }

            var clientViewModel = new ClientsViewModels()
            {
                Address = cli.Address,
                BornDate = cli.BornDate,
                ClientId = cli.ClientId,
                Email = cli.Email,
                Invoice = cli.Invoice.ToList(),
                Name = cli.Name,
                PhoneNumber = cli.PhoneNumber,
                Profit = GetProfit(cli.Invoice.ToList())
            };

            return View(clientViewModel);
        }

        [HttpPost]
        public JsonResult GetInvoicesByClient(string textJson)
        {
            var clientId = Convert.ToInt32(textJson);
            var invoices = this.db.Invoice.Where(x=>x.ClientId==clientId).ToList();
            List<InvoiceViewModel> dato = invoices.Select(inv =>
                new InvoiceViewModel
                {
                    InvoiceId = inv.InvoiceId,
                    Paid = inv.Paid,
                    IssueDate = inv.IssueDate,
                    InvoiceDetail = inv.InvoiceDetail.ToList().Select(det =>
                      new InvoiceDetailViewModel
                      {
                          Amount = det.Amount,
                          InvoiceDetailId = det.InvoiceDetailId,
                          ProductId = det.ProductId,
                          SellPrice = det.SellPrice,
                          Total = det.Total,
                          Product = new ProductViewModel
                          {
                              Name = det.Product.Name,
                              UnitPrice = det.Product.UnitPrice,
                          }
                      }).ToList(),
                }
            ).ToList();
            return Json(new { data = dato });
        }

        public static decimal GetProfit(List<Invoice> invoices)
        {
            var sellsByMonth = invoices.GroupBy(mo =>  mo.IssueDate.Year*100+ mo.IssueDate.Month);
            decimal SumProfit=0;
            foreach (var item in sellsByMonth)
            {
                for (int i = 0; i < item.ToList().Count; i++)
                {
                    var sumExpenses= item.ToList()[i].InvoiceDetail.Sum(x => x.Amount * x.Product.UnitPrice);
                    var sumSells = item.ToList()[i].InvoiceDetail.Sum(x => x.Amount * x.SellPrice);
                    SumProfit += (sumSells - sumExpenses);
                }
            }

            return SumProfit;
        }
    }
}

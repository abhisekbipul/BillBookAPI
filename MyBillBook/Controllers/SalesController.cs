using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBillBook.Data;
using MyBillBook.Models;
using System.Reflection.Metadata;

namespace MyBillBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public SalesController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [Route("GetSales")]
        [HttpGet]
        public IActionResult GetSales()
        {
            var sales = db.sales.ToList();
            return Ok(sales);
        }
        [Route("GetSalesById/{invoiceId}")]
        [HttpGet]
        public IActionResult GetSalesById(int invoiceId)
        {
            var sale = db.sales.FirstOrDefault(s => s.InvoiceId == invoiceId);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [Route("CreateSales")]
        [HttpPost]
        public IActionResult CreateSale(Sales sales)
        {
            sales.Status = "unpaid";
            if (ModelState.IsValid)
            {
                db.sales.Add(sales);
                db.SaveChanges();
                return CreatedAtAction(nameof(GetSalesById), new { invoiceId = sales.InvoiceId }, sales);
            }
            return BadRequest(ModelState);
        }

        [Route("DownloadInvoice/{invoiceId}")]
        [HttpGet]
        public IActionResult DownloadInvoice(int invoiceId)
        {
            var sale = db.sales.FirstOrDefault(s => s.InvoiceId == invoiceId);
            if (sale == null)
            {
                return NotFound();
            }

            using (var memoryStream = new MemoryStream())
            {

                var document = new iTextSharp.text.Document();
                PdfWriter.GetInstance(document, memoryStream).CloseStream = false;
                document.Open();


                document.Add(new Paragraph("Invoice"));
                document.Add(new Paragraph("Invoice ID: " + sale.InvoiceId));
                document.Add(new Paragraph("Customer Name: " + sale.CustomerName));
                document.Add(new Paragraph("Date: " + sale.Date.ToShortDateString()));
                document.Add(new Paragraph("HSN: " + sale.HSN));
                document.Add(new Paragraph("Particulars: " + sale.Particulars));
                document.Add(new Paragraph("Quantity: " + sale.Quantity));
                document.Add(new Paragraph("Rate: " + sale.Rate));
                document.Add(new Paragraph("Value: " + sale.Value));

                document.Close();


                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                return File(bytes, "application/pdf", $"Invoice_{sale.InvoiceId}.pdf");
            }
        }
    }
}

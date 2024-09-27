namespace MyBillBook.Users
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string HSN { get; set; }
        public string Particulars { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }
        public Invoice Invoice { get; set; }
    }
}

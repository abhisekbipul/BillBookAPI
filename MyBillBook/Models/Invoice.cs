namespace MyBillBook.Users
{
    public class Invoice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<InvoiceItem> Items { get; set; }
        public User User { get; set; }
    }
}

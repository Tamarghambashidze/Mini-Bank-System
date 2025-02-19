namespace FinalProject.Models
{
    internal class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int CurrencyId { get; set; } //foreign key
        public CurrencyType Currency { get; set; }
        public int AccountTypeId { get; set; } //foreign key
        public AccountType Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime OpenDate { get; set; }

        //Many to many relationship
        public ICollection<Client> Clients { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}

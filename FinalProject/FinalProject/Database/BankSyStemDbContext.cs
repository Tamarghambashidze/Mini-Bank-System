using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Database
{
    internal class BankSyStemDbContext : DbContext    
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientDetails> ClientDetailsTable { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<ClientAccount> ClientToAccountTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-Q2OUL1L;Initial Catalog=BankSyStemDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //one-to-one
            modelBuilder.Entity<Client>()
                .HasOne(c => c.ClientDetails)
                .WithOne(cd => cd.Client)
                .HasForeignKey<ClientDetails>(cd => cd.ClientId);

            //one-to-many
            modelBuilder.Entity<AccountType>()
                .HasMany(at => at.Accounts)
                .WithOne(a => a.Type)
                .HasForeignKey(a => a.AccountTypeId);
            modelBuilder.Entity<CurrencyType>()
                .HasMany(ct => ct.Accounts)
                .WithOne(a => a.Currency)
                .HasForeignKey(a => a.CurrencyId);
            modelBuilder.Entity<TransactionStatus>()
                .HasMany(ts => ts.Transactions)
                .WithOne(t => t.Status)
                .HasForeignKey(t => t.StatusId);
            modelBuilder.Entity<TransactionType>()
                .HasMany(tt => tt.Transactions)
                .WithOne(t => t.Type)
                .HasForeignKey(t => t.TransactionTypeId);
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);

            //Many-to-many
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Accounts)
                .WithMany(a => a.Clients)
                .UsingEntity<ClientAccount>();

            modelBuilder.Entity<ClientAccount>()
                .HasKey(ca => new { ca.AccountId, ca.ClientId });
        }
    }
}

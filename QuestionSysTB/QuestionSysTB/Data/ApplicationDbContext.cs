using Microsoft.EntityFrameworkCore;
using Qiwi.BillPayments.Model;
using QuestionSysTB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionSysTB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ApplicationBill> Bills { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
    }
}

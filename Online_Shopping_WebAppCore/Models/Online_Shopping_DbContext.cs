using Microsoft.EntityFrameworkCore;
using Online_Shopping_WebAPICore.ViewModels;
using System;

namespace Online_Shopping_WebAPICore.Models
{
    public partial class Online_Shopping_DbContext : DbContext
    {
        public Online_Shopping_DbContext() { }
        public Online_Shopping_DbContext(DbContextOptions<Online_Shopping_DbContext> options) : base(options) { }

        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Orders> Orders { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasIndex(p => new { p.Email, p.Phone }).IsUnique(true);
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB; database=ProjectDb;integrated security=true;");
            }
        }
    }
}

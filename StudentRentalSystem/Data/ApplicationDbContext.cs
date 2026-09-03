using Microsoft.EntityFrameworkCore;
using StudentRentalSystem.Models;


namespace StudentRentalSystem.Data
{

    public class ApplicationDbContext : DbContext
    {


        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
            )
            : base(options)
        {

        }







        public DbSet<Student> Students { get; set; }



        public DbSet<Item> Items { get; set; }



        public DbSet<Rental> Rentals { get; set; }



        public DbSet<Payment> Payments { get; set; }



        public DbSet<ExtraCharge> ExtraCharges { get; set; }



        public DbSet<Admin> Admins { get; set; }









        protected override void OnModelCreating(
            ModelBuilder modelBuilder
        )
        {


            base.OnModelCreating(modelBuilder);







            // Item Price Precision


            modelBuilder.Entity<Item>()
                .Property(x => x.PricePerDay)
                .HasPrecision(18, 2);







            // Rental Total Amount Precision


            modelBuilder.Entity<Rental>()
                .Property(x => x.TotalAmount)
                .HasPrecision(18, 2);







            // Payment Amount Precision


            modelBuilder.Entity<Payment>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);







            // Extra Charge Amount Precision


            modelBuilder.Entity<ExtraCharge>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);



        }





    }

}
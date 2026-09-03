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








            // =========================
            // DECIMAL PRECISION CONFIG
            // =========================



            modelBuilder.Entity<Item>()
                .Property(x => x.PricePerDay)
                .HasPrecision(18, 2);






            modelBuilder.Entity<Rental>()
                .Property(x => x.TotalAmount)
                .HasPrecision(18, 2);






            modelBuilder.Entity<Payment>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);






            modelBuilder.Entity<ExtraCharge>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);









            // =========================
            // DEFAULT ADMIN ACCOUNT
            // =========================



            modelBuilder.Entity<Admin>()
            .HasData(
                new Admin
                {

                    AdminId = 1,

                    Email = "admin@gmail.com",

                    Password = "Admin@123"

                }
            );






        }





    }

}
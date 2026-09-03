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


    }

}
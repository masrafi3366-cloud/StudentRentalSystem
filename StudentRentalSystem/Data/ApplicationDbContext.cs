using Microsoft.EntityFrameworkCore;


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

    }
}
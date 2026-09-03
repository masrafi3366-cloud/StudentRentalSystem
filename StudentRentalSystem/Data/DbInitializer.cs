using StudentRentalSystem.Models;


namespace StudentRentalSystem.Data
{

    public static class DbInitializer
    {


        public static void Initialize(
            ApplicationDbContext context
        )
        {


            if (!context.Admins.Any())
            {


                Admin admin = new Admin();


                admin.Email = "admin@gmail.com";


                admin.Password = "Admin@123";



                context.Admins.Add(admin);


                context.SaveChanges();

            }


        }


    }

}
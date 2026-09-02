using System.Security.Cryptography;
using System.Text;


namespace StudentRentalSystem.Helpers
{

    public static class PasswordHelper
    {


        public static string HashPassword(string password)
        {

            using SHA256 sha256 =
            SHA256.Create();


            byte[] bytes =
            Encoding.UTF8.GetBytes(password);



            byte[] hash =
            sha256.ComputeHash(bytes);



            return Convert.ToBase64String(hash);

        }







        public static bool VerifyPassword(
            string password,
            string hashedPassword
        )
        {

            string hash =
            HashPassword(password);



            return hash == hashedPassword;

        }



    }

}
using Microsoft.AspNetCore.Http;
using StudentRentalSystem.Models;
using System.Collections.Generic;


namespace StudentRentalSystem.Models.ViewModels
{

    public class ItemCreateViewModel
    {

        public Item Item { get; set; }



        public IFormFile? Image { get; set; }



        public List<PaymentMethodInput> PaymentMethods { get; set; }
        = new List<PaymentMethodInput>();

    }





    public class PaymentMethodInput
    {


        public string MethodName { get; set; }


        public string AccountNumber { get; set; }


    }

}
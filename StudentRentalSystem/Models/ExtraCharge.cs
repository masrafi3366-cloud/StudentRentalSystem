using System.ComponentModel.DataAnnotations;


namespace StudentRentalSystem.Models
{

    public class ExtraCharge
    {


        [Key]
        public int ChargeId { get; set; }


        public int RentalId { get; set; }


        public int LateDays { get; set; }


        public decimal Amount { get; set; }


        public bool PaidStatus { get; set; }


    }

}
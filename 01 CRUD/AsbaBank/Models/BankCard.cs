using System.ComponentModel.DataAnnotations;

namespace AsbaBank.Models
{
    public class BankCard
    {
        [Key]
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Please provide a account id.")]
        [Display(Name = "Account Id")] 
        public virtual int AccountId { get; set; }

        [Required(ErrorMessage = "Please provide a client id.")]
        [Display(Name = "Client Id")] 
        public virtual int ClientId { get; set; }

        public virtual bool Disabled { get; set; }
    }
}
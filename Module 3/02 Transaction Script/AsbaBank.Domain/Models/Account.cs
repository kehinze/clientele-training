using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace AsbaBank.Domain.Models
{
    public class Account
    {
        [Key]
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Please provide a client id.")]
        [Display(Name = "Client Id")] 
        public virtual int ClientId { get; set; }

        [Required(ErrorMessage = "Please provide an account number.")]
        [Display(Name = "Account Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "An account number must be 10 digits long.")]
        [Digits(ErrorMessage = "An account number may only have digits.")]
        public virtual string AccountNumber { get; set; }

        [Required(ErrorMessage = "Please provide an account balance id.")]
        [Digits(ErrorMessage = "An account balance may only have digits.")]
        public virtual decimal Balance { get; set; }

        public virtual bool Closed { get; set; }
    }
}
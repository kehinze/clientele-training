using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace AsbaBank.Domain.Models
{
    public class Transaction
    {
        [Key]
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Please provide an account id.")]
        [Display(Name = "Account Id")]
        public virtual int AccountId { get; set; }

        [Required(ErrorMessage = "Please provide a transaction amount.")]
        [Digits(ErrorMessage = "A transaction amount may only have digits.")]
        [Display(Name = "Transaction Amount")]
        public virtual decimal TransactionAmount { get; set; }

        [Required(ErrorMessage = "Please provide a transaction trasaction date.")]
        [Display(Name = "Date")]
        public virtual DateTime TransactionDate { get; set; }
    }
}
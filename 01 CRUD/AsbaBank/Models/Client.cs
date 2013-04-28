using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using DataAnnotationsExtensions;

namespace AsbaBank.Models
{
    public class Client
    {
        [Key]
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Please provide a client name.")]
        [Display(Name = "Client Name")] 
        [MinLength(3, ErrorMessage = "A client name must be at leats than 3 characters")]
        public virtual string ClientName { get; set; }

        [Required(ErrorMessage = "Please provide a postal code")]
        [Display(Name = "Street Name")]
        [StringLength(512, MinimumLength = 4, ErrorMessage = "A street name must be at least 4 characters long.")]
        public virtual string Street { get; set; }

        [Digits(ErrorMessage = "A street number may only have digits.")]
        [Required(ErrorMessage = "Please provide a phone number.")]
        [Display(Name = "Street Number")]
        public virtual string StreetNumber { get; set; }
        
        [Digits(ErrorMessage = "A post code may only have digits.")]
        [Required(ErrorMessage = "Please provide a postal code")]
        [Display(Name = "Postal Code")]
        [StringLength(512, MinimumLength = 4, ErrorMessage = "A postal code must be 4 digits long.")]
        public virtual string PostalCode { get; set; }

        [Required(ErrorMessage = "Please provide a city name.")]
        [StringLength(512, MinimumLength = 4, ErrorMessage = "A city name must be at least 4 characters long.")]
        public virtual string City { get; set; }

        [Digits(ErrorMessage = "A phone number may only have digits.")]
        [Required(ErrorMessage = "Please provide a phone number.")]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "A phone number must be 10 digits long.")]
        public virtual string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [Email(ErrorMessage = "Please provide a valid email address.")]
        public virtual string Email { get; set; }
    }
}
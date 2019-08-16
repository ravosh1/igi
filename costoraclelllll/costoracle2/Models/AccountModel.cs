using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace costoracle2.Models
{
    public class AccountModel
    {
    }
    public class RegisterModel
    {
       // public string RegistrationId { get; set; }
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }

        [Required]
        [Display(Name = "Email-id")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
             ErrorMessage = "Enter a valid email address")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must contain atleast 6 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must contain atleast 6 characters!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not match")]
        public string ConfirmPassword { get; set; }

        //[Required]
        //[Display(Name = "AgeGroup")]
        //public string AgeGroup { get; set; }

     //   public string UserType { get; set; }
        
        //public string UserName { get; set; }      
        public string EmailVerified { get; set; }
        public string CreateDate { get; set; }
        public string Status { get; set; }
       
    }

    public class ForgetPasswordModel
    {
        [Required]
        [Display(Name = "Email-id")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
            ErrorMessage = "Enter a valid email address")]
        public string EmailId { get; set; }
        public string RegistrationId { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        
    }
    }
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace costoracle2.Models
{
    public class ServicesModel
    {

    }

    //public class InsuranceModel
    //{
    //    public string VehicleId { get; set; }

    //    public string UserId { get; set; }

    //    public string Id { get; set; }

    //    [Required]
    //    [Display(Name = "Product Name")]
    //    public string ProductName { get; set; }


    //    public string vehicle { get; set; }

    //    [Required]
    //    [Display(Name = "VehicleRegNo")]
    //    public string VehicleRegNo { get; set; }

    //    [Required]
    //    [Display(Name = "Make")]
    //    public string Make { get; set; }

    //    [Required]
    //    [Display(Name = "Vehicle Age")]
    //    public string VehicleAge { get; set; }
    //    public string ageother { get; set; }
    //    [Required]
    //    [Display(Name = "Model")]
    //    public string Models { get; set; }
    //    [Required]
    //    [Display(Name = "Transmission")]
    //    public string Transmission { get; set; }
    //    [Required]
    //    [Display(Name = "Fuel Type")]
    //    public string FuelType { get; set; }

    //    public string Knw_Reg_No { get; set; }
    //}

    public class AddvehicleinfoModel
    {

        public string VehicleId { get; set; }

        public string UserId { get; set; }

        public string Id { get; set; }

        [Required]
        [Display(Name = "Car Kept")]
        public string carkept { get; set; }

        [Required]
        [Display(Name = "LGA_Address")]
        public string LGA_Address { get; set; }

        [Required]
        [Display(Name = "LGA_AddressState")]
        public string LGA_AddressState { get; set; }

        [Required]
        [Display(Name = "Mileage")]
        public string Mileage { get; set; }

        [Required]
        [Display(Name = "Mileage Unit")]
        public string Mileunit { get; set; }

    }

    public class yourinfoModel
    {

        public string VehicleId { get; set; }

        public string UserId { get; set; }

        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string fname { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lname { get; set; }

        [Required]
        [Display(Name = "Driver Age Group")]
        public string agedriver { get; set; }
    }

    public class CoverinfoModel
    {
        public string VehicleId { get; set; }

        public string UserId { get; set; }

        public string Id { get; set; }

        [Required]
        [Display(Name = "Cover Level")]
        public string coverlevel { get; set; }

        [Required]
        [Display(Name = "No claim Year")]
        public string noclaimyr { get; set; }

        [Required]
        [Display(Name = "Voluntary Excess")]
        public string voluntaryexcess { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public string startdate { get; set; }
        [Required]
        [Display(Name = "Insurance Duration")]
        public string insuranceduration { get; set; }
    }

    public class ListinfoModel
    {
        [Required]
        [Display(Name = "Start Date")]
        public string startdate { get; set; }
        [Required]
        [Display(Name = "Insurance Duration")]
        public string insuranceduration { get; set; }
        public string condition1 { get; set; }
        public string condition2 { get; set; }
        public string condition3 { get; set; }
        public string ontable { get; set; }
        public string VehicleId { get; set; }
        public string Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public string VehicleRegNo { get; set; }

        public string vehicle { get; set; }


        [Required]
        [Display(Name = "Make")]
        public string Make { get; set; }

        [Required]
        [Display(Name = "Vehicle Age")]
        public string VehicleAge { get; set; }
        [Required]
        [Display(Name = "Model")]
        public string Models { get; set; }
        [Required]
        [Display(Name = "Transmission")]
        public string Transmission { get; set; }
        [Required]
        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }


      //  public string VehicleId { get; set; }
     //   public string Id { get; set; }

        [Required]
        [Display(Name = "Car Kept")]
        public string carkept { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string LGA_AddressState { get; set; }


        [Required]
        [Display(Name = "Address")]
        public string LGA_Address { get; set; }

        [Required]
        [Display(Name = "Mileage")]
        public string Mileage { get; set; }

        [Required]
        [Display(Name = "Mileage Unit")]
        public string Mileunit { get; set; }

       // public string VehicleId { get; set; }
      //  public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string fname { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lname { get; set; }

        [Required]
        [Display(Name = "Driver Age Group")]
        public string agedriver { get; set; }

      //  public string VehicleId { get; set; }
     //   public string Id { get; set; }

        [Required]
        [Display(Name = "Cover Level")]
        public string coverlevel { get; set; }

        [Required]
        [Display(Name = "No claim Year")]
        public string noclaimyr { get; set; }

        [Required]
        [Display(Name = "Voluntary Excess")]
        public string voluntaryexcess { get; set; }

        public string Knw_Reg_No { get; set; }

    }


    public class ServiceprovideREGrModel
    {
     

        public string Id { get; set; }

        [Required]
        [Display(Name = "Business Name")]
        public string businessname { get; set; }

        [Required]
        [Display(Name = "Main Contact Name")]
        public string maincontactname { get; set; }

        [Required]
        [Display(Name = "Contact No")]
        public string contactno { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required]
        [Display(Name = "Website")]
        public string website { get; set; }

        [Required]
        [Display(Name = "Division")]
        public string division { get; set; }

        [Required]
        [Display(Name = "RC No.")]
        public string Rcno { get; set; }

        [Required]
        [Display(Name = "Sub-Category Tag")]
        public string subcattag { get; set; }


        [Required]
        [Display(Name = "Email-id")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
             ErrorMessage = "Enter a valid email address")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must countain atleast 6 characters!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must countain atleast 6 characters!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not match")]
        public string ConfirmPassword { get; set; }
        public string InsuranceId { get; set; }
        public string UserId { get; set; }
        public string ServiceProviderId { get; set; }
        public string price { get; set; }
    }

    public class getlga
    {
        public string lgaId { get; set; }
        public string stateId { get; set; }
    }

    public class getstate
    {
        public string countryid { get; set; }
        public string stateId { get; set; }
    }

    public class getVehicleType
    {
        public string VehicalTypeId { get; set; }        
    }

    public class getParking
    {
        public string ParkingId { get; set; }
    }

    public class getCoverLevel
    {
        public string CoverLevelId { get; set; }
    }

    //public class getinsurancequotelist
    //{
    //    public string InsuranceId { get; set; }
    //    public string UserId { get; set; }
    //    public string ServiceProviderId { get; set; }
    //}
    public class InsuranceRequestModel
    {       

        public string UserId { get; set; }

        public string VehicleType { get; set; }
        public string VehicleTypeId { get; set; }

        [Required]
        //[Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }             

        [Required]
        [Display(Name = "Make")]
        public string MakeId { get; set; }
        public string Make { get; set; }
        
        [Required]
        [Display(Name = "Model")]
        public string Models { get; set; }
        public string ModelId { get; set; }

        [Required]
        [Display(Name = "Vehicle Age")]
        public string VehicleAge { get; set; }

        public string ageother { get; set; }

        [Required]
        [Display(Name = "Transmission Type")]
        public string TransmissionType { get; set; }

        [Required]
        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }

        [Required]
        public string ParkingId { get; set; }
        public string Parking { get; set; }

        [Required]
        public string StateId { get; set; }
        public string State { get; set; }

        [Required]
        public string LGAId { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string LGAaddress { get; set; }

        
         [Required]
        [Display(Name = "Car Value")]
        public string CarValue { get; set; }
        [Required]
        [Display(Name = "Mileage")]
        public string Mileage { get; set; }

        [Required]
        [Display(Name = "Mileage")]
        public string Mileunit { get; set; }
        

        [Required]
        [Display(Name = "Driver FirstName")]
        public string DriverFirstName { get; set; }

        [Required]
        [Display(Name = "Driver LastName")]
        public string DriverLastName { get; set; }

        [Required]
        [Display(Name = "Driver DOB")]
        public string DriverDOB { get; set; }

        [Required]
        [Display(Name = "Cover StartDate")]
        public string CoverStartDate { get; set; }

        [Required]
        public string CoverLableId { get; set; }

        public string CoverLable { get; set; }


        [Required]
        [Display(Name = "Duration")]
        public string Duration { get; set; }

        [Required]
        [Display(Name = "NoClaimYearId")]
        public string NoClaimYearId { get; set; }

        [Required]
        [Display(Name = "VoluntaryExcess")]
        public string VoluntaryExcess { get; set; }

        public string InsuranceId { get; set; }

    }
}
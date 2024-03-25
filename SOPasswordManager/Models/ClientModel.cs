using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SOPasswordManager.Models
{
    //public class CountryModel
    //{
    //    public int CountryId { get; set; }
    //    [Required]
    //    public string CountryName { get; set; }
    //}
    public class ClientContactsModel
    {
        public int ContactId { get; set; }
        [Required]
        public string Name { get; set; }        
        public string Surname { get; set; }       
        [EmailAddress]
        public string Email1 { get; set; }
        [EmailAddress]
        public string Email2 { get; set; }         
        public string PhoneNumber1 { get; set; }    
        public string PhoneNumber2 { get; set; }
        [Required]
        public int? clientId { get; set; }
        public string Notes { get; set; }
    }

    public class ProviderContactsModel
    {
        public int ProviderContactDetailId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email1 { get; set; }
        [EmailAddress]
        public string Email2 { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        [Required]
        public int? providerId { get; set; }
        public string Notes { get; set; }
    }

    public class ClientModel
    {
        //public int ClientId { get; set; }
        public string contactId { get; set; }
        //public int contactId { get; set; }
        public int Client_ID { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        [Required]
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        [Required]
        public int? countryId { get; set; }
        [Required]
        public int? cityId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class ProviderModel
    {
        //public int ClientId { get; set; }
        public string ProviderContact_ID { get; set; }
        //public int contactId { get; set; }
        public int Provider_ID { get; set; }
        [Required]
        public string ProviderName { get; set; }
        [Required]
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        [Required]
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        [Required]
        public int? countryId { get; set; }
        [Required]
        public int? cityId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public string BillingFullName { get; set; }
        public string IdCard { get; set; }
        public string FullAddress { get; set; }
        public string PostalCode { get; set; }
    }

    public class ClientUserModel
    {      
        public int ClientUserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int? ClientId { get; set; }
        [Required]
        public int? RoleId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
    }
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        [Required (ErrorMessage ="Required")]
        public string ProjectName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int? ClientId { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class AddProjectUser
    {      
        public int projectId { get; set; }
        [Required]
        public int[] SytemUserId { get; set; }       
    }
    public class ProjectUserModel
    {
        public int ProjectUserId { get; set; }
        [Required]
        public int? projectId { get; set; }
     
        public string Name { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        public string Description { get; set; }
       
        public string Url { get; set; }
        
        public string UserName { get; set; }
       
        public string Password { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        //[Compare("NewPassword")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password doesn't match.")]
        public string ConfirmPassword { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SOPasswordManager.Repo.DTO
{
    public class TwoFactorModel
    {
        [Required]
        public int OTP { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class CredentialsDataViewModel
    {
        public string URL { get; set; }

        public string UserName { get; set; }

        public string Description { get; set; }

        public string Password { get; set; }
    }

    public class CountryModel
    {
        public int CountryId { get; set; }
        [Required]
        public string CountryName { get; set; }
    }

    public class CityModel
    {
        public int CityId { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public string CityName { get; set; }
    }

    public class SystemUserModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Remote("ValidateEmail", "User",
        ErrorMessage = "Email already exist.", AdditionalFields = "SytemUserId")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please select role")]
        public int? RoleId { get; set; }
        public int SytemUserId { get; set; }
        public bool Status { get; set; }
    }
}

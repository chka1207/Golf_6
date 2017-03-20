using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace Golf_6.Models
{

    //Hela lösningen för login och att tilldela en medlem ett lösenord har gjorts i samarbete med Nicklas Persson i grupp 4

    public class AccountModels
    {
        public class LoginViewModel
        {
            [Required]
            [Display(Name = "Email")]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Lösenord")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public class NewUserViewModel
        {
            [Required]
            [Display(Name = "UserID")]
            public int UserID { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]                        
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public Tuple<byte[],byte[]> GeneratePass(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, 20))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(20);

                return Tuple.Create(salt, key);
            }
        }

        public bool AuthenticationUser (string password, string userid)
        {
            byte[] salt = null, key = null;
            Postgres x = new Postgres();

            var tabell = x.SqlFrågaParameters("select salt, key from loginkonto where agare = @par1", Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", Convert.ToInt16(userid))
            });
            foreach(DataRow dr in tabell.Rows)
            {
                salt = (byte[])dr["salt"];
                key = (byte[])dr["key"];
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt))
            {
                byte[] newKey = deriveBytes.GetBytes(20);
                if(!newKey.SequenceEqual(key))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Förnamn")]
        public string Fornamn { get; set; }
        [Required]
        [Display(Name = "Efternamn")]
        public string Efternamn { get; set; }
        [Required]
        [Display(Name = "Gatuadress")]
        public string Adress { get; set; }
        [Required]
        [Display(Name = "Postnummer")]
        public string Postnummer { get; set; }
        [Required]
        [Display(Name = "Ort")]
        public string Ort { get; set; }
        [Required]
        [Display(Name = "Kön")]
        public string Kon { get; set; }
        //[Required]
        //[Display(Name = "Handikapp")]
        //public double Hcp { get; set; }
        [Required]
        [Display(Name = "Medlemskategori")]
        public int MedlemsKategori { get; set; }
    }

    //public class RegistreraNyMedlem
    //{
    //    [Required]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }
    //    [Required]
    //    [Display(Name = "Förnamn")]
    //    public string Fornamn { get; set; }
    //    [Required]
    //    [Display(Name = "Efternamn")]
    //    public string Efternamn { get; set; }
    //    [Required]
    //    [Display(Name = "Gatuadress")]
    //    public string Adress { get; set; }
    //    [Required]
    //    [Display(Name = "Postnummer")]
    //    public string Postnummer { get; set; }
    //    [Required]
    //    [Display(Name = "Ort")]
    //    public string Ort { get; set; }
    //    [Required]
    //    [Display(Name = "Kön")]
    //    public string Kon { get; set; }
    //    //[Required]
    //    //[Display(Name = "Handikapp")]
    //    //public double Hcp { get; set; }
    //    [Required]
    //    [Display(Name = "Medlemskategori")]
    //    public int MedlemsKategori { get; set; }
    //}

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }



    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

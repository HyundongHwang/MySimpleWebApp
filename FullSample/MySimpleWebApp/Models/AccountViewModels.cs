using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySimpleWebApp.Models
{
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

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
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
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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




















    public class AccountLoginOrSignInModel
    {
        public string ReturnUrl { get; set; }
    }

    public class AccountSignInViewModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "이메일")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "'{0}'는 {2}글자 이상 되야 합니다.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "비밀번호확인")]
        [Compare("Password", ErrorMessage = "비밀번호와 비밀번호확인이 서로 다릅니다.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "이름")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "주소")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "전화번호")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "생년월일")]
        public DateTime BirthDate { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }
    }


    public class AccountEditViewModel
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "이메일")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "'{0}'는 {2}글자 이상 되야 합니다.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "구 비밀번호")]
        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "'{0}'는 {2}글자 이상 되야 합니다.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "새 비밀번호")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "새 비밀번호확인")]
        [Compare("NewPassword", ErrorMessage = "비밀번호와 비밀번호확인이 서로 다릅니다.")]
        public string NewPasswordConfirm { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "이름")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "주소")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "전화번호")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "생년월일")]
        public DateTime BirthDate { get; set; }
    }

    public class AccountLoginWithPasswordViewModel
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "이메일")]
        public string Email { get; set; }


        [StringLength(100, ErrorMessage = "'{0}'는 {2}글자 이상 되야 합니다.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }






















}
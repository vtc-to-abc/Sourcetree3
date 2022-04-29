using System.ComponentModel.DataAnnotations;

namespace BlazorServer.Data.Models
{
    public class AccountModel
    {
        [Required]
        public string? account_username { get; set; }
        [Required]
        public string? account_password { get; set; }
        public int account_role_id { get; set; }

        public string? Token { get; set; }
    }
}

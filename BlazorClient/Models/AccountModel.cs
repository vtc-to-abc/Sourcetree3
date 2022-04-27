namespace BlazorClient.Models
{
    public class AccountModel
    {
        public string account_username { get; set; }
        public string account_password { get; set; }
        public int account_role_id { get; set; }

        public string Token { get; set; }
    }
}

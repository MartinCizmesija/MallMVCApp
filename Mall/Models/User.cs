namespace Mall.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public bool IsAdmin { get; set; }
        public bool ConfirmedByAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public virtual MallCenter MallNavigation { get; set; }
        public virtual Store StoreIdNavigation { get; set; }
    }
}

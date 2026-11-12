namespace SimpleAdsAuth.Data
{
    public class SimpleAd
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }        
    }
}

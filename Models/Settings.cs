namespace TimeApp.Models
{
    public class Settings
    {
        public Api Api { get; set; }
        public string Iso2Country { get; set; }
        public Photo Photo { get; set; }
    }

    public class Api
    {
        public BaseUrl BaseUrl { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class BaseUrl
    {
        public string Android { get; set; }
        public string Ios { get; set; }
    }

    public class Photo
    {
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
    }
}

namespace TimeApp.Models
{
#pragma warning disable IDE1006 // Estilos de nombres
    public class AuthenticatedUser
    {
        public int workerId { get; set; }
        public string workerSsn { get; set; }
        public Profile profile { get; set; }

        public Stream GetAvatarStream()
        {
            try
            {
                if (string.IsNullOrEmpty(profile.avatar)) return null;

                return new MemoryStream(Convert.FromBase64String(profile.avatar));
            }
            catch
            {
                return null;
            }
        }
    }

    public class Profile
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string[] companies { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
    }
#pragma warning restore IDE1006 // Estilos de nombres
}

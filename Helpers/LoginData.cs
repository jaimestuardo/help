using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeApp.Helpers
{
    public class LoginData
    {
        public static async Task<bool> IsAuthenticated()
        {
            var hasAuth = await SecureStorage.GetAsync("hasAuth");
            return !(hasAuth == null) && hasAuth.Equals("true");
        }

        public static async Task<int> GetWorkerId()
        {
            if (int.TryParse(await SecureStorage.GetAsync("workerId"), out int id))
                return id;

            return 0;
        }

        public static async Task<string> GetLogin()
        {
            var login = await SecureStorage.GetAsync("userLogin");
            return login;
        }

        public static async Task<string> GetFirstName()
        {
            var firstName = await SecureStorage.GetAsync("firstName");
            return firstName;
        }

        public static async Task<string> GetFullName()
        {
            var login = await SecureStorage.GetAsync("userFullname");
            return login;
        }

        public static async Task<string> GetEmail()
        {
            var email = await SecureStorage.GetAsync("email");
            return email;
        }

        public static async Task<string> GetAvatar()
        {
            var avatar = await SecureStorage.GetAsync("avatar");
            return avatar;
        }

        public static async Task<string> GetCompany()
        {
            var company = await SecureStorage.GetAsync("company");
            return company;
        }

        public static async Task<string> GetSSN()
        {
            var avatar = await SecureStorage.GetAsync("ssn");
            return avatar;
        }

        public static void Clear()
        {
            SecureStorage.RemoveAll();
        }

        public static async Task Set(bool authenticated, int workerId, string userLogin, string firstName, string lastName, string email, string avatar, string[] companies, string ssn)
        { 
            await SecureStorage.SetAsync("hasAuth", authenticated ? "true" : "false" );
            await SecureStorage.SetAsync("workerId", workerId.ToString());
            await SecureStorage.SetAsync("userLogin", userLogin);
            await SecureStorage.SetAsync("firstName", firstName);
            await SecureStorage.SetAsync("userFullname", $"{firstName} {lastName}");
            await SecureStorage.SetAsync("email", email);
            await SecureStorage.SetAsync("avatar", avatar);
            await SecureStorage.SetAsync("company", companies == null || companies.Length == 0 ? string.Empty : companies[0]);
            await SecureStorage.SetAsync("ssn", ssn);
        }
    }
}

using System.Linq;
using BusinessObjects;

namespace DataAccessLayer
{
    public class AccountDAO
    {
        public static AccountMember? GetAccountById(string accountID)
        {
            using var db = new MyStoreContext();
            return db.AccountMembers.FirstOrDefault(c => c.MemberId.Equals(accountID));
        }

        // Helper method to support email login if needed
        public static AccountMember? GetAccountByEmail(string email)
        {
            using var db = new MyStoreContext();
            return db.AccountMembers.FirstOrDefault(c => c.EmailAddress != null && c.EmailAddress.Equals(email));
        }
    }
}

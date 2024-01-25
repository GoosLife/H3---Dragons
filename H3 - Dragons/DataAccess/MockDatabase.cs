using H3___Dragons.Models;

namespace H3___Dragons.DataAccess
{
    public class MockDatabase
    {
        public static List<User> Users;

        static MockDatabase()
        {
            Users = new List<User>();
        }

        public static void TryAddUser(User user, string role = "listener")
        {
            if (Users.Exists(existingUser => existingUser.DragonName == user.DragonName))
            {
                throw new InvalidOperationException("User already exists.");
            }
            else
            {
                Users.Add(user);
            }
        }
    }
}

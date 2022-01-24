using ISSProjectFINAL.Models;

namespace ISSProjectFINAL.Repos
{
    class UserRepo
    {
        public List<User> Users = new()
        {
            new() { Username = "admin", Password = "admin123", AccessLevel = "TopSecret" },
            new() { Username = "restricted", Password = "user123", AccessLevel = "Restricted" },
            new() { Username = "confidential", Password = "user123", AccessLevel = "Confidential" },
            new() { Username = "unclassified", Password = "user123", AccessLevel = "Unclassified" },
        };

        public User Get(string username, string password)
        {
            User user = Users.FirstOrDefault(x => x.Username.ToLower() == username && x.Password == password);
            return user;
        }
    }
}

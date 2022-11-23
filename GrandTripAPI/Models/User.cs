using System.Collections.Generic;

namespace GrandTripAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public IEnumerable<Route> Routes { get; set; }
        
        #region Methods

        public static User CreateNew(string username, string password)
        {
            return new User
            {
                Username = username,
                Password = password
            };
        }
        #endregion
    }
}
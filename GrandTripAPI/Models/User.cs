using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace GrandTripAPI.Models
{
    public class User
    {
        public static readonly Dictionary<string, string> Roles = new()
        {
            { "Default", "Обычный пользователь" },
            { "Editor", "Редактор" },
            { "Admin", "Администратор" }
        };
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Salt { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public IEnumerable<Route> CreatedRoutes { get; set; } = new List<Route>();
        //public IEnumerable<Route> FavouriteRoutes { get; set; }

        #region Methods

        public static User CreateNew(string username, string password)
        {
            var salt = new byte[128 / 8];
            using var rngcsp = new RNGCryptoServiceProvider();
            rngcsp.GetNonZeroBytes(salt);

            var hashedPassword = Hash(password, salt);
            
            return new User
            {
                Username = username,
                Salt = salt,
                Password = hashedPassword,
                Role = "Default",
                CreatedRoutes = new List<Route>(),
                //FavouriteRoutes = new List<Route>()
            };
        }

        
        #nullable enable
        public UserInfo GetInfo()
        {
            int[]? Projecter(IEnumerable<Route>? routes) => routes?.Select(r => r.RouteId).ToArray();

            return new UserInfo(Id, Username, Role, 
                Projecter(CreatedRoutes)
                //,Projecter(FavouriteRoutes)
            );
        }

        private static string Hash(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password, salt, KeyDerivationPrf.HMACSHA256, 100000, 256 / 8
                ));
        }

        public bool ValidatePassword(string password) => Hash(password, Salt) == Password;
        #endregion
    }

    public record UserInfo(int Id, string Username, string Role,
        int[]? CreatedRoutesIds); //, int[] FavouriteRoutesIds);
    #nullable disable
}
using Microsoft.EntityFrameworkCore;
using restauracja_wpf.Data;
using restauracja_wpf.Models;
using restauracja_wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace restauracja_wpf.Interfaces
{
    public class UserManagement
    {
        private readonly GenericDataService<User> _userService;
        private User _user = new();

        public UserManagement(GenericDataService<User> userService)
        {
            _userService = userService;
        }

        public UserManagement(GenericDataService<User> userService, User user) : this(userService)
        {
            _user = user;
        }

        public async void AddUser(string firstname, string lastname, string login, string password, string restaurant, string role)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                var foundRestaurant = await context.Restaurants
                    .FirstOrDefaultAsync(r => r.Name == restaurant);
                var foundRole = await context.Roles
                    .FirstOrDefaultAsync(r => r.Name == role);

                if (foundRestaurant == null)
                    throw new Exception("Restaurant not found");
                else if (foundRole == null)
                    throw new Exception("Role not found");

                await _userService.Create(new User()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    Login = login,
                    PasswordHash = password,
                    RoleId = foundRole.Id,
                    RestaurantId = foundRestaurant.Id
                });
            }
        }

        public async Task<IEnumerable<User>> GetMatchingUsers(string lastname)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<User> foundUsers = await context.Users
                    .Where(u => u.LastName.Contains(lastname))
                    .ToListAsync();

                return foundUsers;
            }
        }

        public async void UpdatePassword(int id, User entity)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                var existing = await context.Set<User>().FindAsync(id);
                if (existing == null)
                    throw new Exception("Entity not found");

                if (existing is User user && entity is User updatedUser)
                {
                    user.PasswordHash = updatedUser.PasswordHash;
                }

                await context.SaveChangesAsync();
            }
        }   
    }
}

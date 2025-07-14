using Microsoft.EntityFrameworkCore;
using restauracja_wpf.Data;
using restauracja_wpf.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace restauracja_wpf.Services
{
    public class UserDataService(GenericDataService<User> userService) : GenericDataService<User>(new RestaurantContextFactory())
    {
        public async Task<User> Login(string login, string password)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                var user = await context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Login == login && u.PasswordHash == password);
                if (user == null)
                    throw new Exception("Invalid login or password");
                return user;
            }
        }

        public async void CreateUser(string firstname, string lastname, string login, string password, string restaurant, string role, bool status)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                //var foundRestaurant = await context.Restaurants
                //    .FirstOrDefaultAsync(r => r.Name == restaurant);
                var foundRole = await context.Roles
                    .FirstOrDefaultAsync(r => r.Name == role);

                //if (foundRestaurant == null)
                //    throw new Exception("Restaurant not found");
                if (foundRole == null)
                    throw new Exception("Role not found");

                await userService.Create(new User()
                {
                    FirstName = firstname,
                    LastName = lastname,
                    Login = login,
                    PasswordHash = password,
                    RoleId = foundRole.Id,
                    RestaurantId = null!,
                    //RestaurantId = foundRestaurant.Id,
                    Status = status
                });
            }
        }

        public new async Task<User> Get(int id)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                User foundUser = await context.Users
                    .Include(u => u.Role)
                    .Include(u => u.Restaurant)
                    .FirstOrDefaultAsync(u => u.Id == id);
                return foundUser;
            }
        }

        public async Task<IEnumerable<User>> GetUserByLastname(string lastname)
        {
            using (var context = new RestaurantContextFactory().CreateDbContext())
            {
                IEnumerable<User> foundUsers = await context.Users
                    .Include(u => u.Role)
                    .Include(u => u.Restaurant)
                    .Where(u => u.LastName.Contains(lastname))
                    .ToListAsync();

                return foundUsers;
            }
        }

        public async void UpdateUserPassword(int id, User entity)
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

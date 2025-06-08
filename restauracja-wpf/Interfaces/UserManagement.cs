using restauracja_wpf.Data;
using restauracja_wpf.Models;
using restauracja_wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restauracja_wpf.Interfaces
{
    public class UserManagement
    {
        public async void AddUser(string firstname, string lastname, string login, string password, string restaurant, string role)
        {
            GenericDataService<User> userService = new GenericDataService<User>(new RestaurantContextFactory());
            GenericDataService<Role> roleService = new GenericDataService<Role>(new RestaurantContextFactory());
            GenericDataService<Restaurant> restaurantService = new GenericDataService<Restaurant>(new RestaurantContextFactory());

            var roles = await roleService.GetAll();
            var foundRole = roles.Select(r => r).Where(r => r.Name == role);

            var restaurants = await restaurantService.GetAll();     // wip
            var foundRestaurant = restaurants.Select(r => r).Where(r => r.Name == restaurant);

            await userService.Create(new User()
            {
                FirstName = firstname,
                LastName = lastname,
                Login = login,
                PasswordHash = password,
                RoleId = foundRole.First().Id,
                RestaurantId = foundRestaurant.First().Id
            });
        }
    }
}

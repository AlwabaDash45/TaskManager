using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    /// <summary>
    /// Controller responsible for managing user accounts and assigning roles.
    /// Accessible only to users with the SuperAdmin role.
    /// Provides functionality for listing users with pagination and search,
    /// and assigning roles dynamically.
    /// </summary>
    [Authorize(Roles = "SuperAdmin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const int PageSize = 5;

        public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Displays a paginated and optionally searchable list of users.
        /// </summary>
        /// <param name="page">Current page number.</param>
        /// <param name="search">Optional search keyword for name or email.</param>
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            var usersQuery = _userManager.Users
                .Where(u => string.IsNullOrEmpty(search)
                    || u.Email.Contains(search)
                    || u.FullName.Contains(search));

            var totalUsers = await usersQuery.CountAsync();

            var users = await usersQuery
                .OrderBy(u => u.Email)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var userList = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    CellPhone = user.CellPhone,
                    Roles = roles
                });
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);
            ViewBag.Search = search;

            return View(userList);
        }

        /// <summary>
        /// Assigns a new role to a specified user. Removes any existing roles before assignment.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="role">The new role to assign.</param>
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _roleManager.RoleExistsAsync(role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction("Index");
        }
    }
}

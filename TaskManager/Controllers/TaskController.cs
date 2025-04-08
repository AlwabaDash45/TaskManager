using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repository;

namespace TaskManager.Controllers
{
    /// <summary>
    /// Controller for managing task CRUD operations and user-specific task views.
    /// Supports role-based access for Admins, SuperAdmins, and Users.
    /// </summary>
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays tasks based on user role. Admins and SuperAdmins see all tasks, Users see only their assigned ones.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "SuperAdmin");

            var tasks = isAdmin
                ? await _taskRepository.GetAllTasksAsync()
                : await _taskRepository.GetTasksByUserIdAsync(userId);

            return View(tasks);
        }

        /// <summary>
        /// Displays the task creation form. Admins and SuperAdmins only.
        /// </summary>
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = _userManager.Users.ToList();
            return View();
        }

        /// <summary>
        /// Handles task creation. Assigns task to selected user.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create(TaskItem task, string AssignedToUserId)
        {
            if (ModelState.IsValid)
            {
                task.AssignedToUserId = AssignedToUserId;
                await _taskRepository.AddTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = _userManager.Users.ToList();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays task edit form for Admins and SuperAdmins.
        /// </summary>
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Users = _userManager.Users.ToList();
            return View(task);
        }

        /// <summary>
        /// Handles task update after editing.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                await _taskRepository.UpdateTaskAsync(task);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = _userManager.Users.ToList();
            return View(task);
        }

        /// <summary>
        /// Displays confirmation view for task deletion.
        /// </summary>
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        /// <summary>
        /// Deletes the specified task from the system.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays tasks assigned to the currently logged-in user.
        /// </summary>
        public async Task<IActionResult> MyTasks()
        {
            var userId = _userManager.GetUserId(User);
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            return View(tasks);
        }

        /// <summary>
        /// Marks a specific task as completed. Available only to users.
        /// </summary>
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MarkComplete(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task != null)
            {
                task.IsCompleted = true;
                await _taskRepository.UpdateTaskAsync(task);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

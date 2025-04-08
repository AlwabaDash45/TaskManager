# Task Manager

A robust ASP.NET Core MVC web application that features role-based authentication and task assignment logic. The app includes Admin, User, and SuperAdmin roles with customized dashboards and identity extensions (Full Name and Cell Phone).

---

## Features

- **User Authentication** using ASP.NET Identity
- **Role-Based Authorization**: Admin, User, SuperAdmin
- **Task Management**: Create, edit, assign, delete tasks
- **SuperAdmin Panel**: Manage users, assign roles, view contact info
- **Search & Pagination** for user management
- **Bootstrap UI** for a clean and responsive layout

---

## Tech Stack

- ASP.NET Core MVC (.NET 7+ recommended)
- Entity Framework Core
- SQL Server (LocalDb or SQL Express)
- Identity for authentication & authorization
- Bootstrap 5

---

## Prerequisites

- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with ASP.NET & web development workload
- [.NET SDK 7.0 or later](https://dotnet.microsoft.com/)
- SQL Server Express or LocalDB
- Git (optional if not using Visual Studio Git integration)

---

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone https://github.com/YOUR_USERNAME/TaskManager.git
   cd TaskManager
   ```

2. **Update database connection:**
   Open `appsettings.json` and configure your SQL Server connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=TaskManager;Trusted_Connection=True;"
   }
   ```

3. **Run EF migrations:**
   ```bash
   Add-Migration InitialCreate
   Update-Database
   ```

4. **Run the app:**
   ```bash
   dotnet run
   ```

---

##  Default Users

The app seeds two users for demonstration:

| Role       | Email                    | Password         |
|------------|--------------------------|------------------|
| SuperAdmin | superadmin@taskmanager.com | SuperAdmin@123   |
| Admin      | admin@taskmanager.com      | Admin@123        |

---

##  Tips

- SuperAdmins can access **User Management** from the navbar.
- Users can only **view and complete their assigned tasks**.
- Admins and SuperAdmins can **create/edit/delete** tasks.

---

## Author

Created by [Alwaba Loyola Dlanga] with help from ChatGPT 🤖

---

## License

This project is open-source and available under the [MIT License](LICENSE).

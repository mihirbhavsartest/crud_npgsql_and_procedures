using CRUD_WO_ORM.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace CRUD_WO_ORM.Controllers
{
    public class AuthController : Controller
    {
        private DataSource.DataSource ds;
        public AuthController() 
        { 
            ds = new DataSource.DataSource();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("EmployeeId") != null)
            {
                return RedirectToAction("Index", "Employee");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Employee e)
        {
            NpgsqlCommand command = ds.source.CreateCommand($"Select * from CheckUser('{e.Username}','{e.Password}')");
            NpgsqlDataReader reader = command.ExecuteReader();
            Employee emp;
            if (reader.Read())
            {
                emp = new Employee();
                emp.EmployeeId = (int)reader["EmployeeId"];
                emp.EmployeeRole= (string)reader["EmployeeRole"];
                HttpContext.Session.SetString("EmployeeId",emp.EmployeeId.ToString());
                HttpContext.Session.SetString("EmployeeRole", emp.EmployeeRole.ToString());
                return RedirectToAction("Index","Employee");    
            }
            ModelState.AddModelError("Password", "Invalid Username Or Password");
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("EmployeeId");
            HttpContext.Session.Remove("EmployeeRole");
            return RedirectToAction("Index","Employee");
        }
        public IActionResult Signup()
        {
            return View();
        }
    }
}

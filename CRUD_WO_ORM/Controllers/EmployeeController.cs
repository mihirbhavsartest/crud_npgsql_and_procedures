using CRUD_WO_ORM.DataSource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using CRUD_WO_ORM.Models;

namespace CRUD_WO_ORM.Controllers
{
    public class EmployeeController : Controller
    {
        private DataSource.DataSource ds;
        public EmployeeController() 
        { 
            ds = new DataSource.DataSource();
        }
        // GET: EmployeeController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("EmployeeId") == null || HttpContext.Session.GetString("EmployeeRole")==null)
                return RedirectToAction("Login", "Auth");

            
            if (HttpContext.Session.GetString("EmployeeRole") == "Admin")
            {
            NpgsqlCommand command = ds.source.CreateCommand("Select * from GetAllEmployees();");
            List<Employee> list= new List<Employee>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Employee emp = new Employee();
                emp.EmployeeId = reader.GetInt32("EmployeeId");
                emp.EmployeeName = reader.GetString("EmployeeName");
                emp.EmployeeDOB = reader.GetString("EmployeeDOB");
                emp.EmployeeFunction = reader.GetString("EmployeeFunction");
                emp.EmployeeLocation = reader.GetString("EmployeeLocation");
                emp.EmployeeRole = reader.GetString("EmployeeRole");
                emp.Username = reader.GetString("Username");
                emp.Password = reader.GetString("Password");
                list.Add(emp);
            }
                
            return View(list);
            }
            else
            {
                int id = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                NpgsqlCommand command = ds.source.CreateCommand($"Select * from GetEmployee({id});");
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    List<Employee> list= new List<Employee>();
                    Employee emp = new Employee();
                    emp.EmployeeId = reader.GetInt32("EmployeeId");
                    emp.EmployeeName = reader.GetString("EmployeeName");
                    emp.EmployeeDOB = reader.GetString("EmployeeDOB");
                    emp.EmployeeFunction = reader.GetString("EmployeeFunction");
                    emp.EmployeeLocation = reader.GetString("EmployeeLocation");
                    emp.EmployeeRole = reader.GetString("EmployeeRole");
                    emp.Username = reader.GetString("Username");
                    emp.Password = reader.GetString("Password");
                    list.Add(emp);
                    
                    return View(list);
                }
                return RedirectToAction("Login", "Auth");
            }
        }


        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetString("EmployeeId") == null || HttpContext.Session.GetString("EmployeeRole")!="Admin")
                return Unauthorized();
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                NpgsqlCommand command = ds.source.CreateCommand($"Call AddEmployee('{emp.EmployeeName}','{emp.EmployeeDOB}','{emp.EmployeeFunction}','{emp.EmployeeLocation}','{emp.EmployeeRole}','{emp.Username}','{emp.Password}')");
                command.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        public ActionResult ReturnEmployeeIfAvailable(NpgsqlDataReader reader)
        {
            Employee e;
            if (reader.Read())
            {
                e = new Employee();
                e.EmployeeId = reader.GetInt32("EmployeeId");
                e.EmployeeName = reader.GetString("EmployeeName");
                e.EmployeeDOB = reader.GetString("EmployeeDOB");
                e.EmployeeFunction = reader.GetString("EmployeeFunction");
                e.EmployeeLocation = reader.GetString("EmployeeLocation");
                e.EmployeeRole = reader.GetString("EmployeeRole");
                e.Username = reader.GetString("Username");
                e.Password = reader.GetString("Password");
                if (HttpContext.Session.GetString("EmployeeRole")!="Admin" && HttpContext.Session.GetString("EmployeeId") != Convert.ToString(e.EmployeeId)) 
                    return Unauthorized();
                return View(e);
            }
            return NotFound();
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("EmployeeId") == null)
                return Unauthorized();

            NpgsqlCommand command = ds.source.CreateCommand($"Select * from GetEmployee({id})");
            var reader = command.ExecuteReader();
            return ReturnEmployeeIfAvailable(reader);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {

            NpgsqlCommand command = ds.source.CreateCommand($"Call UpdateEmployee('{emp.EmployeeId}','{emp.EmployeeName}','{emp.EmployeeDOB}','{emp.EmployeeFunction}','{emp.EmployeeLocation}','{emp.EmployeeRole}','{emp.Username}','{emp.Password}')");
            command.ExecuteNonQuery();
            return RedirectToAction("Index");
            }
            return View(emp);
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("EmployeeId") == null)
                return Unauthorized();
            NpgsqlCommand command = ds.source.CreateCommand($"Select * from GetEmployee({id})");
            var reader = command.ExecuteReader();
            return ReturnEmployeeIfAvailable(reader);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmp(int id)
        {
            NpgsqlCommand command = ds.source.CreateCommand($"Select * from GetEmployee({id})");
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
            NpgsqlCommand commanddel = ds.source.CreateCommand($"Call DeleteEmployee('{id}')");
            commanddel.ExecuteNonQuery();
            return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}

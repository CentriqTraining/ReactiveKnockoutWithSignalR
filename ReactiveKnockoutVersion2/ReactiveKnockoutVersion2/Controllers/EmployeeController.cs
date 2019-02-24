using ReactiveKnockoutVersion2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ReactiveKnockoutVersion2.Controllers
{
    public class EmployeeController : ApiController
    {
        private MVC_ReactiveContext db = new MVC_ReactiveContext();

        // GET api/Employee
        [Route("employees")]
        public IEnumerable<Employee> GetEmployees()
        {
            var emps = db.Employees.ToList();
            return emps;
        }

        // GET api/Employee/5
        public Employee GetEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return employee;
        }

        // PUT api/Employee/5
        [Route("employees/save")]
        public HttpResponseMessage PutEmployee(EmployeeChange_DTO changes)
        {
            
            var empToEdit = db.Employees.Find(changes.Id);
            if (empToEdit != null)
            {
                db.Entry(empToEdit).Property(changes.PropertyName).CurrentValue = changes.PropertyValue;
                try
                {
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                    throw;
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

        }

        // POST api/Employee
        public HttpResponseMessage PostEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, employee);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = employee.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Employee/5
        public HttpResponseMessage DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Employees.Remove(employee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
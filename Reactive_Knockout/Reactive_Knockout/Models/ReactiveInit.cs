using MVC_Reactive.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Reactive_Knockout.Models
{
    public class ReactiveInit : DropCreateDatabaseIfModelChanges<MVC_ReactiveContext>
    {
        protected override void Seed(MVC_ReactiveContext context)
        {
            context.Employees.Add(new Employee() { FirstName = "Michael", LastName = "Tucker", Salary = 18.00M, Position = "Manager" });
            context.Employees.Add(new Employee() { FirstName = "Morgan", LastName = "Grimes", Salary = 12.00M, Position = "Asst. Manager" });
            context.Employees.Add(new Employee() { FirstName = "Emmett", LastName = "Milbarge", Salary = 14.00M, Position = "Asst. Manager", TerminationDate = DateTime.Parse("1/31/2012") });
            context.Employees.Add(new Employee() { FirstName = "Harry", LastName = "Tang", Salary = 12.00M, Position = "Asst. Manager", TerminationDate = DateTime.Parse("1/31/2011") });
            context.Employees.Add(new Employee() { FirstName = "Diane", LastName = "Beckman", Salary = 22.00M, Position = "Manager" });
            context.Employees.Add(new Employee() { FirstName = "John", LastName = "Casey", Salary = 10.00M, Position = "Green Shirt" });
            context.Employees.Add(new Employee() { FirstName = "Fernando", Salary = 9.00M, Position = "Green Shirt" });
            context.Employees.Add(new Employee() { FirstName = "Bunny", Salary = 9.00M, Position = "Green Shirt" });
            context.Employees.Add(new Employee() { FirstName = "Chuck", LastName = "Bartowski", Salary = 13.00M, Position = "Nerd Herd" });
            context.Employees.Add(new Employee() { FirstName = "Jeffrey", LastName = "Barnes", Salary = 11.25M, Position = "Nerd Herd, Apple" });
            context.Employees.Add(new Employee() { FirstName = "Lester", LastName = "Patel", Salary = 11.25M, Position = "Nerd Herd, Apple" });
            context.Employees.Add(new Employee() { FirstName = "Skip", LastName = "Johnson", Salary = 12.00M, Position = "Nerd Herd" });
            context.Employees.Add(new Employee() { FirstName = "Anna", LastName = "Wu", Salary = 9.00M, Position = "Nerd Herd", TerminationDate = DateTime.Parse("1/31/2012") });
            context.Employees.Add(new Employee() { FirstName = "Hannah", LastName = "", Salary = 8.00M, Position = "Nerd Herd", TerminationDate = DateTime.Parse("1/31/2012") });
            context.Employees.Add(new Employee() { FirstName = "Greta", LastName = "", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2012") });
            context.Employees.Add(new Employee() { FirstName = "Rick", LastName = "Noble", Salary = 8.00M, Position = "Green Shirt" });
            context.Employees.Add(new Employee() { FirstName = "Victoria", LastName = "Dunwoody", Salary = 8.00M, Position = "Green Shirt" });
            context.Employees.Add(new Employee() { FirstName = "Moses", LastName = "Finkelstein", Salary = 180.00M, Position = "Founder" });
            context.Employees.Add(new Employee() { FirstName = "Porter", LastName = "Bauer", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.Employees.Add(new Employee() { FirstName = "Marvin", LastName = "Gadsby", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.Employees.Add(new Employee() { FirstName = "Cecil", LastName = "Goldenberg", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.Employees.Add(new Employee() { FirstName = "Christina", LastName = "James", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.Employees.Add(new Employee() { FirstName = "Ruben", LastName = "Mango", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.Employees.Add(new Employee() { FirstName = "Sal", LastName = "Sawaya", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.Employees.Add(new Employee() { FirstName = "Skye", LastName = "Stoinitz", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.Employees.Add(new Employee() { FirstName = "Leticia", LastName = "Williams", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.Employees.Add(new Employee() { FirstName = "Sarah", LastName = "Yang", Salary = 8.00M, Position = "Green Shirt", TerminationDate = DateTime.Parse("1/31/2010") });
            context.SaveChanges();
            base.Seed(context);
        }
    }
}

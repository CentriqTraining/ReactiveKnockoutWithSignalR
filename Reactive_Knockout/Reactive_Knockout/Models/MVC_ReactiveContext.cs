using System.Data.Entity;
using Reactive_Knockout.Models;

namespace MVC_Reactive.Models
{
    public class MVC_ReactiveContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        public MVC_ReactiveContext() : base("DefaultConnection")
        {
            Database.SetInitializer<MVC_ReactiveContext>(new ReactiveInit());
        }

        public DbSet<Employee> Employees { get; set; }
    }
}

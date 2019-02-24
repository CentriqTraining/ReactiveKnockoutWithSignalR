using System.Data.Entity;

namespace ReactiveKnockoutVersion2.Models
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

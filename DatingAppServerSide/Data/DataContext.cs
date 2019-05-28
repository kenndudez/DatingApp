using DatingApp.API.Models;
using DatingAppServerSide.Models;
using Microsoft.EntityFrameworkCore;


namespace DatingApp.API.Data
{
    public class DataContext : DbContext  
    {
        public DataContext(DbContextOptions <DataContext> options) : base (options) {}
         public DbSet <Values> Values { get; set; }
         public DbSet <User> Users {get; set;}
    }
}




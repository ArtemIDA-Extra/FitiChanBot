using FitiChan.DL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitiChan.DL
{
    public class FitiDBContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();
        public string DBConnectionStr = "server=localhost; user=root; password=example; database=FitiTestBD;";
        public FitiDBContext()            // If you want to pass arguments to the constructor, forget about migrations or make a dbcontext factory.
        {
            //Database.EnsureDeleted();   // When creating the initial migration, make sure that the database is deleted.
            //Database.EnsureCreated();
        }
        public FitiDBContext(IDBSetting settings)
        {
            //Database.EnsureDeleted();   // When creating the initial migration, make sure that the database is deleted.
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(DBConnectionStr, new MySqlServerVersion(new Version(8, 1, 0)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.DSID);
            modelBuilder.Entity<User>().Property(u => u.DSID).ValueGeneratedNever();

            modelBuilder.Entity<Message>().Property(m => m.Id).ValueGeneratedOnAdd();
        }
    }
}

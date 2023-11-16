using FitiChan.DL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitiChan.DL
{
    public class FitiDBContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();
        public string DBConnectionStr = "server=localhost; user=root; password=example; database=FitiTestBD;";
        public FitiDBContext(DbContextOptions<FitiDBContext> dbContextOptions)
            : base(dbContextOptions) // If you want to pass arguments to the constructor, forget about migrations or make a dbcontext factory.
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.DSID);
            modelBuilder.Entity<User>().Property(u => u.DSID).ValueGeneratedNever();

            modelBuilder.Entity<Message>().Property(m => m.Id).ValueGeneratedOnAdd();
        }
    }
}

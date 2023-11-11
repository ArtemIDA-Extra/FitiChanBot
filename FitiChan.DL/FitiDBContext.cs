using FitiChan.DL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitiChan.DL
{
    public class FitiDBContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();
        private string DBConnectionStr { get; set; }
        public FitiDBContext(string dbConnectionStr)
        {
            Database.EnsureCreated();
            DBConnectionStr = dbConnectionStr;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(DBConnectionStr, new MySqlServerVersion(new Version(8, 1, 0)));
        }
    }
}

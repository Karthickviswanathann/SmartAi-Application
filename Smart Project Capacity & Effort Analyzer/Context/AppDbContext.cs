using Microsoft.EntityFrameworkCore;
using Smart_Project_Capacity___Effort_Analyzer.Models;

namespace Smart_Project_Capacity___Effort_Analyzer.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserMaster> Usermaster { get; set; }
        public DbSet<UserBehaviour> UserBehaviour { get; set; }
        public DbSet<NotesMaster> NotesMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBehaviour>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<NotesMaster>()
            .HasOne(b => b.User)
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }

}

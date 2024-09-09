using Microsoft.EntityFrameworkCore;

namespace proyectoCanchas.Models;

public class AppDbContext : DbContext{
    public AppDbContext(){}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Rent> Rents { get; set; }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

            // Configure User - User relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Owner)
                .WithMany(u => u.OwnedUsers)
                .HasForeignKey(u => u.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure User - Role many-to-many relationship
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Configure Field - User relationship
            modelBuilder.Entity<Field>()
                .HasOne(f => f.Owner)
                .WithMany(u => u.Fields)
                .HasForeignKey(f => f.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Rent - User relationship
            modelBuilder.Entity<Rent>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rents)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Rent - Field relationship
            modelBuilder.Entity<Rent>()
                .HasOne(r => r.Field)
                .WithMany(f => f.Rents)
                .HasForeignKey(r => r.FieldId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}

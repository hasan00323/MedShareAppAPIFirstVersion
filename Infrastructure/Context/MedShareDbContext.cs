using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class MedShareDbContext : DbContext
{
    public MedShareDbContext(DbContextOptions<MedShareDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Request>()
            .HasOne(r => r.Donation)
            .WithMany()
            .HasForeignKey(r => r.DonationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Donation>()
            .HasOne(d => d.Donor)
            .WithMany(u => u.MyDonations)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Request>()
            .HasOne(r => r.User)
            .WithMany(u => u.MyRequests)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }

}

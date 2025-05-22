using MeetingsService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingsService.Infrastructure.Data;

public class MeetingsContext(DbContextOptions<MeetingsContext> options) : DbContext(options)
{
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<AttendeeInfo> AttendeeInfos { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Topic> Topics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Topic>()
            .HasIndex(t => t.Name)
            .IsUnique();

        modelBuilder.Entity<Location>()
            .HasIndex(l => l.Place)
            .IsUnique();

        modelBuilder.Entity<AttendeeInfo>()
            .HasIndex(AttendeeInfo => new { AttendeeInfo.MeetingId, AttendeeInfo.UserId })
            .IsUnique();
    }
}

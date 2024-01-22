using ApiHost.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiHost.DbContexts;

public class VideoContext(DbContextOptions<VideoContext> options) : DbContext(options)
{

    public DbSet<Video> Videos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Video>().HasData(
            new Video("Sample")
            {
                Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                Path = @"videos\d28888e9-2ba9-473a-a40f-e38cb54f9b35\SampleVideo_1280x720_1mb.mp4"
            }
            );

        base.OnModelCreating(modelBuilder);
    }
}
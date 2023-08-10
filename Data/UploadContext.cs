using Microsoft.EntityFrameworkCore;
using UploadCsv.Models;

namespace UploadCsv.Data;

public class UploadContext : DbContext
{
    public UploadContext(DbContextOptions<UploadContext> options)
        : base(options)
    {
        
    }

    public DbSet<Point> Points { get; set; }

}
using magic_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace magic_backend.Data;

public class BoxDbContext(DbContextOptions<BoxDbContext> options) : DbContext(options)
{
    public DbSet<Box> Box { get; set; }
}
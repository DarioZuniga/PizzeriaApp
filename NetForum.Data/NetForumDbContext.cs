using Microsoft.EntityFrameworkCore;
using NetForum.Data.Entities;

namespace NetForum.Data;

public class NetForumDbContext : DbContext
{
    public NetForumDbContext(DbContextOptions<NetForumDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Post> Posts { get; set; }
}
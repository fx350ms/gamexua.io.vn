using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using GameXuaVN.Authorization.Roles;
using GameXuaVN.Authorization.Users;
using GameXuaVN.MultiTenancy;
using GameXuaVN.Entities;

namespace GameXuaVN.EntityFrameworkCore
{
    public class GameXuaVNDbContext : AbpZeroDbContext<Tenant, Role, User, GameXuaVNDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Score> Scores { get; set; }

        public GameXuaVNDbContext(DbContextOptions<GameXuaVNDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

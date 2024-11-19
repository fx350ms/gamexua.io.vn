using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace GameXuaVN.EntityFrameworkCore
{
    public static class GameXuaVNDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<GameXuaVNDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<GameXuaVNDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}

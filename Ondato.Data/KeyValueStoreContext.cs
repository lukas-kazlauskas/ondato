using Microsoft.EntityFrameworkCore;

namespace Ondato.Data
{
    public class KeyValueStoreContext : DbContext
    {
        public DbSet<KeyValuePair> KeyValuePairs { get; set; }

        public KeyValueStoreContext(DbContextOptions options) : base(options)
        {
        }
    }
}
using Claims.Models.Claims;
using Claims.Models.Cover;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.DbContexts
{
    public class ClaimsContext : DbContext
    {
        private DbSet<ClaimResponse> Claims { get; init; }
        public DbSet<CoverResponse> Covers { get; init; }

        public ClaimsContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClaimResponse>().ToCollection("claims");
            modelBuilder.Entity<CoverResponse>().ToCollection("covers");
        }

        public async Task<IEnumerable<ClaimResponse>> GetClaimsAsync()
        {
            return await Claims.ToListAsync();
        }

        public async Task<ClaimResponse?> GetClaimAsync(string id)
        {
            return await Claims.Where(claim => claim.Id == id).SingleOrDefaultAsync();
        }

        public async Task AddItemAsync(ClaimResponse item)
        {
            Claims.Add(item);
            await SaveChangesAsync();
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            int affectedRows = 0;
            var claim = await GetClaimAsync(id);

            if (claim is not null)
            {
                Claims.Remove(claim);
                affectedRows = await SaveChangesAsync();
            }

            return affectedRows > 0;
        }
    }
}

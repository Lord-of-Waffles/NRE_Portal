using DataLayer_NRE_Portal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataLayer_NRE_Portal
{
    // EF Tools use this at design-time for migrations
    public class NrePortalContextFactory : IDesignTimeDbContextFactory<NrePortalContext>
    {
        public NrePortalContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<NrePortalContext>()
                .UseSqlite("Data Source=nreportal.db")
                .Options;

            return new NrePortalContext(options);
        }
    }
}

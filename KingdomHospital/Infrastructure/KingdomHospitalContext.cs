using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure {
    public class KingdomHospitalContext : DbContext
    {
        public KingdomHospitalContext(DbContextOptions<KingdomHospitalContext> options)
            : base(options)
        {
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(KingdomHospitalContext context)
        {
            
            context.SaveChanges();
        }
    }
}
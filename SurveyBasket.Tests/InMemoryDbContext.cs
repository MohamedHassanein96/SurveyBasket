using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Survey_Basket.Persistence;


namespace SurveyBasket.Tests
{
    class InMemoryDbContext : ApplicationDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InMemoryDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();
        }
      
    }
}
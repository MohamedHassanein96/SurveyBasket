
namespace Survey_Basket.Persistence.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.OwnsMany(x => x.RefreshTokens).ToTable("RefreshTokens").WithOwner().HasForeignKey("UserId");
            builder.Property(x => x.FirstName).HasMaxLength(100);
            builder.Property(x => x.LastName).HasMaxLength(100);

            builder.HasData(new ApplicationUser
            {
                Id = DefaultUsers.Admin.ID,
                FirstName = DefaultUsers.Admin.FirstName,
                LastName = DefaultUsers.Admin.LastName,
                UserName = DefaultUsers.Admin.Email,
                NormalizedUserName = DefaultUsers.Admin.Email.ToUpper(),
                Email = DefaultUsers.Admin.Email,
                NormalizedEmail = DefaultUsers.Admin.Email.ToUpper(),
                SecurityStamp = DefaultUsers.Admin.SecurityStamp,
                ConcurrencyStamp = DefaultUsers.Admin.ConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = DefaultUsers.Admin.PasswordHash

            });
        }
    }
}

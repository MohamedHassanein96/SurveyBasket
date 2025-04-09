namespace SurveyBasket.Persistence.EntitiesConfiguration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData( new IdentityUserRole<string>
            {
                UserId = DefaultUsers.AdminID,
                RoleId = DefaultRoles.AdminRoleId
            });
        }
    }
}

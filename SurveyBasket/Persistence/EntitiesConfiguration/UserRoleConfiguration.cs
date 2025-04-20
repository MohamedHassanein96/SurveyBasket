namespace SurveyBasket.Persistence.EntitiesConfiguration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(new IdentityUserRole<string>
            {
                UserId = DefaultUsers.Admin.ID,
                RoleId = DefaultRoles.Admin.Id
            });
        }
    }
}

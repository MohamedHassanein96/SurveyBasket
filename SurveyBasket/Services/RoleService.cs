namespace SurveyBasket.Services
{
    public class RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<RoleDetailResponse>> GetAsync(string id, CancellationToken cancellationToken)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

            var permissions = await _roleManager.GetClaimsAsync(role);

            var response = new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(x => x.Value));
            return Result.Success(response);
        }

        public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default)
        {
            return await _roleManager.Roles
              .Where(x => !x.IsDefault && (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
              .ProjectToType<RoleResponse>()
              .ToListAsync(cancellationToken);
        }

        public async Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken)
        {
            var roleIsExists = await _roleManager.RoleExistsAsync(request.Name);
            if (roleIsExists)
                return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);


            var allowedPermissions = Permissions.GetAllPermissions();
            if (request.Permissions.Except(allowedPermissions).Any())
                return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

            var role = new ApplicationRole
            {
                ConcurrencyStamp = Guid.CreateVersion7().ToString(),
                Name = request.Name,
            };

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                #region 1st Way
                //var permissions = new List<IdentityRoleClaim<string>>();

                //for (int i = 0; i < request.Permissions.Count; i++)
                //{
                //    permissions.Add(new IdentityRoleClaim<string>
                //    {
                //        RoleId = role.Id,
                //        ClaimType = Permissions.Type,
                //        ClaimValue = request.Permissions[i]
                //    });
                //}
                //await _context.AddRangeAsync(permissions);
                //await _context.SaveChangesAsync();

                //var response = new RoleDetailResponse(role.Id, role.Name, false, permissions.Select(p => p.ClaimValue!));
                //return Result.Success(response); 
                #endregion
                var permissions = request.Permissions
                    .Select(p => new IdentityRoleClaim<string>
                    {
                        ClaimType = Permissions.Type,
                        ClaimValue = p,
                        RoleId = role.Id
                    });
                await _context.AddRangeAsync(permissions, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var response = new RoleDetailResponse(role.Id, role.Name, role.IsDeleted, request.Permissions);

                return Result.Success(response);
            }
            var error = result.Errors.First();

            return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }

        public async Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken)
        {
            var roleIsExists = await _roleManager.Roles.AnyAsync(r => r.Name == request.Name && r.Id != id);
            if (roleIsExists)
                return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);

            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure(RoleErrors.RoleNotFound);


            var allowedPermissions = Permissions.GetAllPermissions();
            if (request.Permissions.Except(allowedPermissions).Any())
                return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

            role.Name = request.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                var currentPermissions = await _context.RoleClaims
                    .Where(x => x.RoleId == role.Id && x.ClaimType == Permissions.Type).Select(x => x.ClaimValue)
                    .ToListAsync(cancellationToken: cancellationToken);

                var newPermissions = request.Permissions.Except(currentPermissions)
                  .Select(x => new IdentityRoleClaim<string>
                  {
                      ClaimType = Permissions.Type,
                      ClaimValue = x,
                      RoleId = role.Id
                  });

                var removedPermissions = currentPermissions.Except(request.Permissions);

                await _context.RoleClaims.Where(x => x.RoleId == role.Id && removedPermissions.Contains(x.ClaimValue)).ExecuteDeleteAsync();

                await _context.AddRangeAsync(newPermissions);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken)
        {
            if (await _roleManager.FindByIdAsync(id) is not { } role)
                return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

            role.IsDeleted = !role.IsDeleted;

            await _roleManager.UpdateAsync(role);
            return Result.Success();
        }
    }
}


using Microsoft.AspNetCore.Authentication.JwtBearer;
using SurveyBasket.Erorrs;

namespace Survey_Basket
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var conncetionString = configuration.GetConnectionString("DefaultConnection")
               ?? throw new InvalidOperationException("Connection String Not Found");

            services.AddDbContext<ApplicationDbContext>(options => options
            .UseSqlServer(conncetionString));

            services.AddControllers();

            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            options.AddDefaultPolicy(builer =>
            builer
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(allowedOrigins!)));


            services.AddAuthConfig(configuration);

            services.AddOpenApi();

            services.AddMapsterConfig();




            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPollService, PollService>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();


            services.AddFluentVlidationConfig();
            return services;
        }
        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var mapConfig = TypeAdapterConfig.GlobalSettings;
            mapConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mapConfig));
            return services;
        }
        private static IServiceCollection AddFluentVlidationConfig(this IServiceCollection services)
        {
            //Add FluentVlidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            return services;
        }
        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateDataAnnotations().ValidateOnStart();
            var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                        ValidIssuer = jwtSettings?.Issuer,
                        ValidAudience = jwtSettings?.Audience
                    };
                });

            return services;
        }
    }
}

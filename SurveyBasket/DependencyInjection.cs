﻿using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Health;
using SurveyBasket.Settings;
using System.Threading.RateLimiting;

namespace SurveyBasket
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
            services.AddHybridCache();

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
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IEmailSender, EmailSenderService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.AddBackgroundJobsConfig(configuration);

            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.AddHttpContextAccessor();
            services.AddFluentVlidationConfig();
            services.AddHealthChecks()
                .AddSqlServer(conncetionString, name: "database", tags: ["Sql"])
                .AddHangfire(options => { options.MinimumAvailableServers = 1; })
                .AddCheck<MailProviderHealthChecks>(name: "mail provider");

            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;


                rateLimiterOptions.AddPolicy("ipLimit", httpContext =>

                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(20),
                        }
                     )
                );
                rateLimiterOptions.AddPolicy("userLimit", httpContext =>

                   RateLimitPartition.GetFixedWindowLimiter(
                       partitionKey: httpContext.User.Identity?.Name?.ToString(),
                       factory: _ => new FixedWindowRateLimiterOptions
                       {
                           PermitLimit = 2,
                           Window = TimeSpan.FromSeconds(20),
                       }
                    )
               );


                rateLimiterOptions.AddConcurrencyLimiter("concurrency", options =>
                {
                    options.PermitLimit = 1000;
                    options.QueueLimit = 100;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

                //rateLimiterOptions.AddTokenBucketLimiter("token", options =>
                //{
                //    options.TokenLimit = 2;
                //    options.QueueLimit = 1;
                //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
                //    options.TokensPerPeriod = (2);
                //    options.AutoReplenishment = true;
                //});
                //rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
                //{
                //    options.PermitLimit = 2;
                //    options.QueueLimit = 1;
                //    options.Window = TimeSpan.FromSeconds(20);
                //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                //});

                //rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
                //{
                //    options.PermitLimit = 2;
                //    options.Window = TimeSpan.FromSeconds(20);
                //    options.SegmentsPerWindow = 2;
                //    options.QueueLimit = 1;
                //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                //});
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                //options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                options.ApiVersionReader = new MediaTypeApiVersionReader("x-api-version");
                options.ReportApiVersions = true;



            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });


            return services;
        }
        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            var mapConfig = TypeAdapterConfig.GlobalSettings;
            mapConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mapConfig));
            return services;
        }
        private static IServiceCollection AddBackgroundJobsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
           .UseSimpleAssemblyNameTypeSerializer()
           .UseRecommendedSerializerSettings()
           .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));
            services.AddHangfireServer();

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

            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyHandler>();


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
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;

            });
            return services;
        }
    }
}

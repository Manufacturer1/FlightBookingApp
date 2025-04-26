using BaseEntity.Configurations;
using BaseEntity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServerLibrary.Adapter;
using ServerLibrary.BackgroundServices;
using ServerLibrary.Data;
using ServerLibrary.MappingProfiles;
using ServerLibrary.Repositories.Implementations;
using ServerLibrary.Repositories.Interfaces;
using ServerLibrary.Services.Implementations;
using ServerLibrary.Services.Interfaces;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Flight Booking API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string> {}
        }
    });

});
//Stripe payment configuration
var stripeConf = new StripePaymentConf();
builder.Configuration.GetSection("STRIPE_CONFIGURATION").Bind(stripeConf);
builder.Services.AddSingleton(stripeConf);
StripeConfiguration.ApiKey = stripeConf.SecretKey;



//Database configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Your connection string is not found."));

});
// Automapper configurations
builder.Services.AddAutoMapper(typeof(MappingProfile));


//Identity configurations
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 5;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;


})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection!.Key!)),
        ValidateIssuer = true,
        ValidIssuer = jwtSection.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSection.Audience,
        ValidateLifetime = true,
    };


});
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
             .AllowCredentials();

        });
});

// Adapter register
builder.Services.AddScoped<IPaymentGateway,StripePaymentAdapter>();


// Memento register
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
});


// Background service
builder.Services.AddHostedService<FlightDateUpdaterService>();


// Repositories register
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IAirlineRepository, AirlineRepository>();
builder.Services.AddScoped<IPlaneRepository, PlaneRepository>();
builder.Services.AddScoped<IBaggageRepository,BaggageRepository>();
builder.Services.AddScoped<IItineraryRepository, ItineraryRepository>();
builder.Services.AddScoped<IAirportRepository,AirportRepository>();
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<IFlightAmenityRepository, FlightAmenityRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<IPassportIdentityRepository, PassportIdentityRepository>();
builder.Services.AddScoped<IBookingRepository,BookingRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();



//Services register
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileService, ServerLibrary.Services.Implementations.FileService>();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IAirlineService,AirlineService>();
builder.Services.AddScoped<IPlaneService,PlaneService>();
builder.Services.AddScoped<IBaggageService,BaggageService>();
builder.Services.AddScoped<IItineraryService, ItineraryService>();
builder.Services.AddScoped<IAirportService,AirportService>();
builder.Services.AddScoped<IAmenityService,AmenityService>();
builder.Services.AddScoped<IDiscountService,ServerLibrary.Services.Implementations.DiscountService>();
builder.Services.AddScoped<IPaymentService,StripePaymentService>();
builder.Services.AddScoped<IBookingService,BookingService>();
builder.Services.AddScoped<ITicketService, TicketService>();


var app = builder.Build();

// Admin Creation
/*using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var config = services.GetRequiredService<IConfiguration>();

    await AdminConfigurations.CreateAdminUser(userManager, roleManager, config);
}*/


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings = { [".svg"] = "image/svg+xml"}
    },
    RequestPath = "/Resources"
});

app.UseCors("AllowClient");

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();

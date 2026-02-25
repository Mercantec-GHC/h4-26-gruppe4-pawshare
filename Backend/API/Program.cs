using API.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Repositories.Context;
using Repositories.Interfaces;
using Scalar.AspNetCore;
using Services;
using Services.Interfaces;
using System;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);



builder.AddServiceDefaults();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

IConfiguration Configuration = builder.Configuration;

string connectionString = Configuration.GetConnectionString("db") 
                          ?? Configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'db' not found.");

builder.Services.AddDbContext<AppDBContext>(options => options.UseNpgsql(connectionString));

// Add dependcies for dependency injection
// repos
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAnimalRepo, AnimalRepo>();
builder.Services.AddScoped<IAnimalTypeRepo, AnimalTypeRepo>();
builder.Services.AddScoped<IAppointmentRepo, AppointmentRepo>();
builder.Services.AddScoped<IChatRepo, ChatRepo>();
builder.Services.AddScoped<IMessageRepo, MessageRepo>();
builder.Services.AddScoped<IBookingRepo, BookingRepo>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IMessageReadReceiptRepo, MessageReadReceiptRepo>();


// services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IAnimalTypeService, AnimalTypeService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddSignalR();
builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection(MinioOptions.SectionName));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Configuration["Jwt:Issuer"],
        ValidAudience = Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                Configuration["Jwt:SecretKey"]!
            )
        )
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken)
                && path.StartsWithSegments("/ws/chat"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

// Add services to the container.

builder.Services.AddControllers();

// Add CORS support for Flutter app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterApp", policy =>
    {
        policy.WithOrigins(
                "https://dev-pawshare-api.mercantec.tech",
                "https://dev-pawshare.mercantec.tech",
                "https://pawshare-api.mercantec.tech",
                "https://pawshare.mercantec.tech",
                "http://localhost:60947"
            )
            .AllowAnyMethod()               // Allow GET, POST, PUT, DELETE, etc.
            .AllowAnyHeader()               // Allow any headers
            .AllowCredentials();            // Allow cookies/auth headers
    });

    // Development policy - more permissive for local development
    options.AddPolicy("AllowAllLocalhost", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            {
                // Tillad alle localhost og 127.0.0.1 origins med alle porte
                var uri = new Uri(origin);
                return uri.Host == "localhost" ||
                       uri.Host == "127.0.0.1" ||
                       uri.Host == "0.0.0.0";
            })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// OpenAPI configuration will be handled by middleware

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    
    if (app.Configuration.GetValue<string>("DEV:ENV", "not_aspire") == "local")
    {
        await dbContext.Database.MigrateAsync();
    }

    if (app.Configuration.GetValue<bool>("DEV:SEED", false))
    {
        await SeedInitialDataAsync(dbContext);
    }
}





app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseForwardedHeaders();

app.MapOpenApi();

// Enable Swagger UI (klassisk dokumentation (Med Darkmode))
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    options.RoutePrefix = "swagger"; // Tilgængelig på /swagger
    options.AddSwaggerBootstrap(); // UI Pakke lavet af NHave - https://github.com/nhave
});

app.UseStaticFiles(); // Vigtig for SwaggerBootstrap pakken


// Enable Scalar UI (moderne alternativ til Swagger UI)
app.MapScalarApiReference(options =>
    {
        options.WithTitle("API Documentation")
               .WithTheme(ScalarTheme.Purple)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });


// Enable CORS - SKAL være før UseAuthorization
app.UseCors(app.Environment.IsDevelopment() ? "AllowAllLocalhost" : "AllowFlutterApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/ws/chat");

// Log API dokumentations URL'er ved opstart
app.Lifetime.ApplicationStarted.Register(() =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var addresses = app.Services.GetRequiredService<Microsoft.AspNetCore.Hosting.Server.IServer>()
        .Features.Get<Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>()?.Addresses;

    if (addresses != null && app.Environment.IsDevelopment())
    {
        foreach (var address in addresses)
        {
            logger.LogInformation("Swagger UI: {Address}/swagger", address);
            logger.LogInformation("Scalar UI:  {Address}/scalar", address);
        }
    }
});

app.Run();


static async Task SeedInitialDataAsync(AppDBContext dbContext)
{
    string[] defaultAnimalTypes = ["Dog", "Cat"];

    var existingAnimalTypeNames = await dbContext.AnimalTypes
        .Select(type => type.Name)
        .ToListAsync();

    foreach (var typeName in defaultAnimalTypes)
    {
        if (existingAnimalTypeNames.Any(name =>
                string.Equals(name, typeName, StringComparison.OrdinalIgnoreCase)))
        {
            continue;
        }

        dbContext.AnimalTypes.Add(new AnimalType
        {
            Id = Guid.NewGuid().ToString(),
            Name = typeName,
            Description = $"Default animal type: {typeName}",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
    }

    var testUserEmail = "test@test.dk";
    var testUserEmail2 = "test2@test.dk";
    var existingTestUser = await dbContext.Users
        .AnyAsync(user => user.Email == testUserEmail);
    var existingTestUser2 = await dbContext.Users
        .AnyAsync(user => user.Email == testUserEmail2);

    if (!existingTestUser)
    {
        var animalOwnerRole = await dbContext.Roles
            .FirstOrDefaultAsync(role => role.Name == "AnimalOwner");

        if (animalOwnerRole is not null)
        {
            dbContext.Users.Add(new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test User",
                Email = testUserEmail,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("test"),
                ProfilePictureKey = "TestImage.jpg",
                City = "TestCity",
                RoleId = animalOwnerRole.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }
    }

    if (!existingTestUser2)
    {
        var animalOwnerRole = await dbContext.Roles
            .FirstOrDefaultAsync(role => role.Name == "AnimalOwner");

        if (animalOwnerRole is not null)
        {
            dbContext.Users.Add(new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test User 2",
                Email = testUserEmail2,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("test"),
                ProfilePictureKey = "TestImage.jpg",
                City = "TestCity",
                RoleId = animalOwnerRole.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }
    }

    await dbContext.SaveChangesAsync();

    // Seed test chat if it doesn't exist
    var existingChat = await dbContext.Chats
        .Include(c => c.ChatUsers)
        .FirstOrDefaultAsync(chat =>
            chat.ChatUsers.Any(cu => cu.User.Email == testUserEmail) &&
            chat.ChatUsers.Any(cu => cu.User.Email == testUserEmail2));
    
    if (existingChat == null)
    {
        var testUser1 = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == testUserEmail);
        var testUser2 = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == testUserEmail2);

        if (testUser1 != null && testUser2 != null)
        {
            var newChat = new Chat
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test Chat",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ChatUsers = new List<ChatUserConvo>()
            };
            newChat.ChatUsers = new List<ChatUserConvo>
            {
                new ChatUserConvo { UserId = testUser1.Id, ChatId = newChat.Id },
                new ChatUserConvo { UserId = testUser2.Id, ChatId = newChat.Id }
            };
            

            dbContext.Chats.Add(newChat);
        }
    }

    await dbContext.SaveChangesAsync();
}

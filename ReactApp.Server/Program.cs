using Microsoft.AspNetCore.Identity;
using ReactApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Data;
using ReactApp.Server.Repository;
using ReactApp.Server.Services;
using ReactApp.Server.Repository.Interface;
using ReactApp.Server.Services.Interface;
using ReactApp.Server.Mappings;
using ReactApp.Server.Entity;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultString"); 
builder.Services.AddDbContext<ApplicationDbContext>(configurations =>
{
    configurations.UseSqlServer(connectionString); 
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientSideCORS", builder => // Give your policy a name
    {
        
        builder.WithOrigins("https://localhost:61502/") 
               .AllowAnyMethod() 
               .AllowAnyHeader() 
               .AllowCredentials(); 
    });
});
builder.Services.AddScoped<IGenericRepository<Glossary, Guid>, SQLGlossaryRepository>();
builder.Services.AddScoped<IGlossaryRepository,SQLGlossaryRepository>();
builder.Services.AddScoped<IGlossaryService, GlossaryService>();
builder.Services.AddScoped<IGenericRepository<Glossary, Guid>, SQLGlossaryRepository>();
builder.Services.AddAutoMapper(typeof(ApplicationProfile).Assembly);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await SeedData.InitializeUserRole(app.Services);
await SeedData.InitialGlossaryRecord(app.Services);
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("ClientSideCORS");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

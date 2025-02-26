using Microsoft.AspNetCore.Identity;
using ReactApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Data;
using ReactApp.Server.Repository;
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
builder.Services.AddScoped<SQLGlossaryRepository>();
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

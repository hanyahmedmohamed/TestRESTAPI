using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestRESTAP3.Data;
using TestRESTAPI3.Data.Models;
using TestRESTAPI3.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(op =>
      op.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("myCon")));


builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

//builder.Services.AddDbContext<AppDbContext>(op =>
//      op.UseLazyLoadingProxies()
//        .UseSqlServer(builder.Configuration.GetConnectionString("myCon")));



builder.Services.AddControllers().AddNewtonsoftJson();
//builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwtAuth();

builder.Services.AddCustomJwtAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosSqlServer>();
//string connectionStringSql = builder.Configuration.GetConnectionString("SqlHospital");
//builder.Services.AddDbContext<HospitalContext>(options => options.UseSqlServer(connectionStringSql));

builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosOracle>();
string connectionStringOracle = builder.Configuration.GetConnectionString("OracleHospital");
builder.Services.AddDbContext<HospitalContext>(options => options.UseOracle(connectionStringOracle));

//builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosMySql>();
//string connectionStringMySql = builder.Configuration.GetConnectionString("MySqlHospital");
//builder.Services.AddDbContext<HospitalContext>(options => options.UseMySQL(connectionStringMySql));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Empleados}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

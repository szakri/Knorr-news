using DataAccess;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Models;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddDbContext<KnorrNewsContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SqlServerConnection")));

builder.Services.AddScoped<INewsRepository, NewsRepository>();

builder.Build().Run();

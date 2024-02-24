using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.AutoMappers;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Services;
using WebApplication1.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repository
builder.Services.AddScoped<IRepository<Beer>, BeerRepository>();

// Entity framework
builder.Services.AddDbContext<StoreContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreConnection"));
});

// Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// services
builder.Services.AddKeyedScoped<ICommonService<BeerDTO, BeerInsertDTO, BeerUpdateDTO>, BeerService>("beerService");

//validators
builder.Services.AddScoped<IValidator<BeerInsertDTO>, BeerInsertValidator>();
builder.Services.AddScoped<IValidator<BeerUpdateDTO>, BeerUpdateValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

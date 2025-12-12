using ApiPeliculas.Data;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.ItRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("ConexionSQL");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Agregamos repositorios
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
// Configuramos AutoMapper

// Esta es la forma correcta si PeliculasMapper está en el proyecto de arranque
builder.Services.AddAutoMapper(typeof(PeliculasMapper));


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

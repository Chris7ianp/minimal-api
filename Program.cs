using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal.Dominio.Interfaces;
using minimal.Dominio.Servicos;
using minimal_api.DTOs;
using minimal_api.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServicos, AdministradorServicos>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("mysql"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
                    );
});

var app = builder.Build();



app.MapGet("/", () => "Hello World!");


app.MapPost("/login", ([FromBody]LoginDTO loginDTO, IAdministradorServicos administradorServicos) =>
{
    if (administradorServicos.Login(loginDTO) != null)
    {
        return Results.Ok("Login feito com sucesso");
    }
    else
        return Results.Unauthorized();
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();



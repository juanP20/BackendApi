using AutoMapper;
using BackendAPI.Dtos;
using BackendAPI.Models;
using BackendAPI.Service.Contrato;
using BackendAPI.Service.Implementacion;
using BackendAPI.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//"Server=JARENASW10\\SQLEXPRESSWORK;Database=DBEmpleado;Trusted_Connection=True;TrustServerCertificate=true"

builder.Services.AddDbContext<DbempleadoContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});

//Implemantar la inyeccion de serviciio
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddAutoMapper(typeof(AutomapperProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region PETICIONES API REST

app.MapGet("/departamento/lista", async (
    IDepartamentoService _departamentoServicio,
    IMapper _mapper
    ) =>
{
    var listaDepartamento = await _departamentoServicio.GetList();
    var listaDepartamentoDto = _mapper.Map<List<DepartamentoDto>>(listaDepartamento);

    if (listaDepartamentoDto.Count > 0)
    {
        return Results.Ok(listaDepartamentoDto);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapGet("/empleado/lista", async (
    IEmpleadoService _empleadoServicio,
    IMapper _mapper
    ) =>
{
    var empleadoLista = await _empleadoServicio.GetList();
    var listaEmpleadoDto = _mapper.Map<List<EmpleadoDto>>(empleadoLista);

    if (listaEmpleadoDto.Count > 0)
    {
        return Results.Ok(listaEmpleadoDto);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/Empleado/guardar", async (
    EmpleadoDto modelo,
    IEmpleadoService _empleadoServicio,
    IMapper _mapper

    ) =>
    {
    var empleado = _mapper.Map<Empleado>(modelo);
    var empleadoCreado = await _empleadoServicio.add(empleado);

    if (empleadoCreado.IdEmpleado != 0)
    {
        return Results.Ok(_mapper.Map<Empleado>(empleadoCreado));
    }
    else
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
}); ;

app.MapPut("/Empleado/actualizar", async (
    int  idEmpleado,
    EmpleadoDto modelo,
    IEmpleadoService _empleadoServicio,
    IMapper _mapper 
    ) => {

        var idProporcionado = await _empleadoServicio.Get(idEmpleado);
        
        if(idProporcionado is null)
        {
            return Results.NotFound();
        }

        var empleadoEncontrado = _mapper.Map<Empleado>(idProporcionado);

        idProporcionado.NombreCompleto = empleadoEncontrado.NombreCompleto;
        idProporcionado.IdDepartamento = empleadoEncontrado.IdDepartamento;
        idProporcionado.FechaCreacion = empleadoEncontrado.FechaCreacion;
        idProporcionado.Sueldo = empleadoEncontrado.Sueldo; 
        idProporcionado.FechaContrato = empleadoEncontrado.FechaContrato;

        var reespuesta = await _empleadoServicio.Update(empleadoEncontrado);

        if (reespuesta)
        {
            return Results.Ok(_mapper.Map<EmpleadoDto>(empleadoEncontrado));
        }
        else
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    });

app.MapDelete("/Empleado/Eliminar{idEmpleado}", async(
    int idEmpleado,
    IEmpleadoService _empleadoService
    ) => {

        var empleado = await _empleadoService.Get(idEmpleado);

        if (empleado is null)
        {
            return Results.NotFound();
        }

        var respuesta = await _empleadoService.Delete(empleado);

        if ( respuesta)
        {
            return Results.Ok();
        }
        else {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);

        }
    });

#endregion

app.UseCors("NuevaPolitica");

app.Run();
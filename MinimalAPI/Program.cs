using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<UsuariosModel>> GetUsuarios(AppDbContext context)
{
    return await context.Usuarios.ToListAsync();
}


app.MapGet("/Usuarios", async (AppDbContext context) =>
{
    return await GetUsuarios(context);
});

app.MapGet("/Usuario/{id}", async (AppDbContext context, int id) =>
{
    var usuario = await context.Usuarios.FindAsync(id);
    if(usuario == null)
    {
        return Results.NotFound("Usuario não localizado");
    }
    return Results.Ok(usuario); 
});

app.MapPut("/Usuario", async (AppDbContext context, UsuariosModel usuario) =>
{
    var UsuarioDB = await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == usuario.Id);

    if (UsuarioDB == null)
    {
        return Results.NotFound("Usuarios não localizado");
    }

    UsuarioDB.Nome = usuario.Nome;
    UsuarioDB.Email = usuario.Email;
    UsuarioDB.UserName = usuario.UserName;

    context.Update(usuario);
    await context.SaveChangesAsync();
    return Results.Ok(UsuarioDB);


});

app.MapDelete("/Usuario/{id}", async (AppDbContext context, int id) =>
{
    var usuarioDb = await context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
    if (usuarioDb == null)
    {
        return Results.NotFound("Usuario não localizado");
    }
    context.Remove(usuarioDb);
    await context.SaveChangesAsync();

    return Results.Ok(usuarioDb);
});

app.MapPost("/Usuario", async (AppDbContext context, UsuariosModel usuario) =>
{
    context.Add(usuario);
    await context.SaveChangesAsync();
    return await GetUsuarios(context);
 
});





app.Run();


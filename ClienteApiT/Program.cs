using ClienteApiT.Data;
using ClienteApiT.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Registrar DbContext ANTES de Build()
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Esto le dice a Swagger cómo mapear IFormFile
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});

var app = builder.Build();

// ✅ Crear migraciones automáticas al arrancar la app
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Esto crea las tablas automáticamente en el servidor
}

// ✅ Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

// ❌ Comentar HTTPS porque MonsterASP FreeSite no soporta
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

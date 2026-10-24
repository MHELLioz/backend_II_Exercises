using DotNetEnv;

// Carga las variables de entorno definidas en el archivo .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Servicios del contenedor
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Registrar el repositorio y el servicio para inyección de dependencias
builder.Services.AddScoped<backend_II_Exercises.Database.ProductRepository>();
builder.Services.AddScoped<backend_II_Exercises.Services.ProductsServices>();

var app = builder.Build();

// Configuración del pipeline de peticiones HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
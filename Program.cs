using BiblioAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin() // Permite cualquier origen
                   .AllowAnyMethod() // Permite cualquier método (GET, POST, etc.)
                   .AllowAnyHeader(); // Permite cualquier encabezado
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Acceso a configuración
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpClient<PrestamoService>(); // Esto lo hace el IHttpClientFactory

// Registramos los servicios personalizados
builder.Services.AddScoped<LibroService>();
builder.Services.AddScoped<PrestamoService>();
builder.Services.AddScoped<UsuarioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar la política de CORS
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();

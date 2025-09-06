using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None); // SQL команды
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Connection", LogLevel.None); // подключения
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.None); // EF
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Update", LogLevel.None); // обновления
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.None); // запросы

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<ContactsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ContactsContext>();
    context.Database.EnsureCreated();
}

Console.WriteLine("   Web API сервер запущен!");
Console.WriteLine("   Актуальная ссылка находится ниже в виде http://localhost:ПОРТ");
Console.WriteLine("   Swagger: http://localhost:ПОРТ/swagger");
Console.WriteLine("   Для остановки сервера нажмите Ctrl+C");
Console.WriteLine(new string('═', 50));


app.Run();
using DotNetEnv;
using App.Services; // Adicione esta linha

var builder = WebApplication.CreateBuilder(args);

// Carregar variáveis de ambiente do arquivo .env
Env.Load();

// Construir a string de conexão a partir das variáveis de ambiente
var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"User={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};"; //+
                    //    $"SslMode={Environment.GetEnvironmentVariable("DB_SSL_MODE")}";

// Adicionar o DbService com a string de conexão
builder.Services.AddSingleton(new DbService(connectionString));

// Adicione os serviços necessários
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure o pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "redirect",
    pattern: "{hash}",
    defaults: new { controller = "Home", action = "RedirectToOriginalUrl" });

app.Run();
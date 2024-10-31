using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Carregar variáveis de ambiente do arquivo .env
Env.Load();

// Construir a string de conexão a partir das variáveis de ambiente
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")}";

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

app.Run();
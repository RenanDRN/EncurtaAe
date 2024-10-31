var builder = WebApplication.CreateBuilder(args);

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
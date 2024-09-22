using InAndOut.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddControllersWithViews();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware personalizado para redirecionar URLs com letras maiúsculas para minúsculas
app.Use(async (context, next) =>
{
    var url = context.Request.Path.ToString();
    if (url != url.ToLowerInvariant())
    {
        // Redireciona para a versão em minúsculas
        var lowerCaseUrl = url.ToLowerInvariant();
        context.Response.Redirect($"{context.Request.Scheme}://{context.Request.Host}{lowerCaseUrl}{context.Request.QueryString}");
        return;
    }

    await next();
});

app.UseRouting();

app.UseAuthorization();

#region Configuração das rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Rotas personalizadas
app.MapControllerRoute(
    name: "blog",
    pattern: "blog/{year:int}/{month:int}/{slug}",
    defaults: new { controller = "Blog", action = "Post" });

app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Dashboard}/{id?}",
    defaults: new { controller = "Admin" });

app.MapControllerRoute(
    name: "products",
    pattern: "produtos/detalhes/{id:int}/{slug?}",
    defaults: new { controller = "Products", action = "Details" });

app.MapControllerRoute(
    name: "shop",
    pattern: "loja/{categoria}/{subcategoria?}",
    defaults: new { controller = "Shop", action = "Browse" });

#endregion

app.Run();

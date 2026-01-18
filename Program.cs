using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using YemekTarifleri.Mappings;
using YemekTarifleri.Models;
using YemekTarifleri.Repositories;
using YemekTarifleri.Services;
using YemekTarifleri.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Session Configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Entity Framework
builder.Services.AddDbContext<AppDbContext>();

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RecipeCreateDtoValidator>();

// Repository Pattern
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

// Service Layer
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tarif}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

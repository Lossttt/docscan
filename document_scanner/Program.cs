using document_scanner.Data;
using document_scanner.Middleware;
using document_scanner.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add database context to the container
// The AddDbContext method is used to register the FileDbContext class with the dependency injection container.
builder.Services.AddDbContext<FileDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAntiforgery(options => 
{
    options.HeaderName = "X-CSRF-TOKEN";
});

// Add the HttpContextAccessor and CORS services to the container
// The AddHttpContextAccessor method is used to register the HttpContextAccessor class with the dependency injection container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:44443")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Register the services with the dependency injection container
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IUploadService, UploadService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Add the antiforgery tokens middleware here
// The UseAntiforgeryTokens method is used to add the AntiforgeryTokensMiddleware to the HTTP request pipeline.
app.UseAntiforgeryTokens();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

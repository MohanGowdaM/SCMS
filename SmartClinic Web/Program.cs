using SmartClinic.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout =
        TimeSpan.FromHours(2);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient<
    AuthApiService>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7139/api/");
    });

builder.Services.AddHttpClient("SmartClinicAPI",
    client =>
    {
        client.BaseAddress =
            new Uri(
                "https://localhost:7139/api/");
    });
builder.Services.AddScoped<SystemAdminApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] =
        "no-cache, no-store, must-revalidate";

    context.Response.Headers["Pragma"] =
        "no-cache";

    context.Response.Headers["Expires"] =
        "0";

    await next();
});

app.MapStaticAssets();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();

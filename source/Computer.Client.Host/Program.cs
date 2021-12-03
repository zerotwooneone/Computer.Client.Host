using Computer.Client.Domain;
using Computer.Client.Domain.ToDoList;
using Computer.Client.Host.App;
using Computer.Client.Host.Controllers;
using Computer.Client.Host.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.AddContext<HostJsonContext>();
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        {
            builder
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddSingleton<IListService, ListService>();
builder.Services.AddSingleton<HostJsonContext>();
builder.Services.AddSingleton<IAppService, DummyAppService>();
builder.Services.AddSingleton<IAppConnectionRepo, AppConnectionRepo>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    //this must be AFTER UseRouting, but BEFORE UseAuthorization
    app.UseCors();
}

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<BusHub>("/bus");
app.Run();

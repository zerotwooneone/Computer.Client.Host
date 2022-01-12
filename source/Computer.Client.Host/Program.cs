using Computer.Client.App.Bus;
using Computer.Client.App.Domain;
using Computer.Client.App.ExternalBus;
using Computer.Client.App.Hubs;
using Computer.Client.Domain.App;
using Computer.Client.Domain.App.ToDoList;
using Computer.Client.Domain.Contracts.App;
using Computer.Client.Domain.Contracts.App.ToDoList;
using Computer.Client.Domain.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => { options.JsonSerializerOptions.AddContext<HostJsonContext>(); });

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


builder.Services.AddSingleton<HostJsonContext>();
builder.Services.AddExternalBus();

builder.Services.AddSignalR();
builder.Services.AddDomain(builder.Configuration);
builder.Services.AddBus();

builder.Services.AddControllersWithViews();

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
//this must be AFTER UseRouting, but BEFORE UseAuthorization
    app.UseCors();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapHub<BusHub>("/bus");
app.Run();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Enable Session Middleware
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	//options.IdleTimeout = TimeSpan.FromSeconds(60); // 1 menit / 60seconds 
	options.IdleTimeout = TimeSpan.FromMinutes(20); // Dev: 20–30 menit / Prod 60-30 menit
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

// Enable Session 
app.UseSession();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

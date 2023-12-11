var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/*var CadenaDeConexion = builder.Configuration.GetConnectionString("SqliteConexion")!.ToString();
builder.Services.AddSingleton<string>(CadenaDeConexion);
*/

builder.Services.AddDistributedMemoryCache();//se agrega para login***********************************
//builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();// se agrega para inyeccion de dependencia
//builder.Services.AddScoped<ITableroRepository, TableroRepository>();// se agrega para inyeccion de dependencia
//builder.Services.AddScoped<ITareaRepository, TareaRepository>();// se agrega para inyeccion de dependencia

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//var cadenaDeConexion = builder.Configuration.GetConnectionString("SqliteConexion")!;
//builder.Services
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

app.UseRouting();
app.UseSession();//se agrega para login***********************************
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

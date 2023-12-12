using EspacioRepositorios;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var cadenaDeConexion = builder.Configuration.GetConnectionString("SqliteConexion")!.ToString();

//Registra la cadena de conexión como un servicio singleton en el contenedor de dependencias. Significa que habrá una 
//única instancia de la cadena de conexión para toda la aplicación y será compartida entre las diferentes partes 
builder.Services.AddSingleton<string>(cadenaDeConexion);

builder.Services.AddDistributedMemoryCache(); //se agrega para login************

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); // se agrega para inyeccion de dependencia
builder.Services.AddScoped<ITableroRepository, TableroRepository>(); // se agrega para inyeccion de dependencia
builder.Services.AddScoped<ITareaRepository, TareaRepository>();     // se agrega para inyeccion de dependencia

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//----Middlewares----

//Pipeline de procesamiento (en asp.net core) = cadena de middlewares

//Si una solicitud llega a la aplicación a través de HTTP, este middleware la redirige a la versión HTTPS de la misma URL.
app.UseHttpsRedirection();

//Habilita el manejo de archivos estáticos, como archivos CSS, imágenes y scripts JavaScript. Permite a la aplicación 
//servir estos archivos directamente sin necesidad de pasar por el pipeline de MVC
app.UseStaticFiles();

//Configura el enrutamiento en la aplicación. Este middleware examina la solicitud y decide a qué controlador y acción 
//debe ser enviada según las reglas definidas en las rutas de la aplicación.
app.UseRouting();

//Habilita el uso de sesiones en la aplicación, permitiendo almacenar y recuperar datos específicos del usuario a lo 
//largo de múltiples solicitudes. En este caso, se ha configurado para utilizar la memoria como almacén de sesiones.
app.UseSession();

//se encarga de la autorización de los usuarios, determinando si tienen permiso para acceder a los recursos o realizar 
//acciones específicas en función de las políticas de autorización definidas en la aplicación
app.UseAuthorization();

//ruta predeterminada para las solicitudes que no coinciden con ninguna otra ruta definida. En este caso, las solicitudes 
//se enviarán al controlador "Home" y la acción "Index", con un parámetro opcional "id".
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Indica la 

app.Run();

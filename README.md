# tl2-tp10-2023-maurijs

Dentro del appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",  // quiere decir que como minimo  muestre los Log.Information
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

ModelState es el Estado del model. El estado del modelo representa los errores que proceden de dos subsistemas: el enlace de modelos y la validación de modelos. 
Los errores que se originan del enlace de modelos suelen ser errores de conversión de datos. Por ejemplo, se escribe una "x" en un campo numérico entero. Suele ocurrir al tomar los parametros recibidos de un formulario y asignarle el valor de ese parametro a un campo del modelo, y no coinciden los tipos de dato.
La validación del modelo se produce después del enlace de modelos y notifica los errores en los que los datos no cumplen las reglas de negocio. Por ejemplo, se especifica un 0 en un campo que espera una clasificación entre 1 y 5.

ViewModels
Son entidades que nos sirven para mostrar datos en las views y evitar corromper los Models originales. 
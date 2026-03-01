# Configuración de Variables de Entorno en Railway

## Variables MySQL (Proporcionadas automáticamente por Railway)

Cuando crees una base de datos MySQL en Railway, estas variables se generan automáticamente:

```
MYSQLHOST=     # Host de la base de datos
MYSQLPORT=     # Puerto (por defecto 3306)
MYSQLDATABASE= # Nombre de la base de datos
MYSQLUSER=     # Usuario de la base de datos
MYSQLPASSWORD= # Contraseña de la base de datos
```

También Railway proporciona:
- `MYSQL_URL` - URL completa de conexión
- `MYSQL_PUBLIC_URL` - URL pública de conexión

## Variables Personalizadas Requeridas

Debes agregar estas variables manualmente en el Railway Dashboard:

### 1. JWT_SECRET
```
JWT_SECRET=encrypted_by_Finlay_PharmaVigilance_administrators_to_make_it_impossible_to_decrypt
```
(O tu secret preferido)

### 2. ASPNETCORE_ENVIRONMENT (Opcional)
```
ASPNETCORE_ENVIRONMENT=Production
```

## Cómo configurar en Railway Dashboard

1. Ve a tu proyecto en https://railway.app
2. Haz click en tu aplicación (Finlay.PharmaVigilance.API)
3. Ve a la pestaña "Variables"
4. Agrega las variables personalizadas:
   - `JWT_SECRET` = tu secret
   - `ASPNETCORE_ENVIRONMENT` = Production (opcional)
5. Guarda los cambios

## Cómo funciona el código

El archivo `DependencyInjection.cs` ha sido actualizado para:

1. **Conexión a Base de Datos**: 
   - Primero busca variables de entorno de Railway (MYSQLHOST, MYSQLDATABASE, etc.)
   - Si no las encuentra, usa la cadena de conexión en `appsettings.json`
   - En desarrollo, usa `appsettings.Development.json`

2. **JWT Secret**:
   - Primero busca `JWT_SECRET` en variables de entorno (Railway)
   - Si no existe, usa el valor en `appsettings.json`

3. **Puerto**:
   - Busca la variable `PORT` (Railway asigna esto automáticamente)
   - Si no existe, usa el puerto 5137

## Prueba Local

Para probar localmente con las variables de entorno:

```bash
# En Linux/Mac
export MYSQLHOST=localhost
export MYSQLPORT=3306
export MYSQLDATABASE=finlay
export MYSQLUSER=jabel
export MYSQLPASSWORD=root_Jabel
export JWT_SECRET=tu_secret_aqui

dotnet run
```

## Variables de Railway (Ejemplo Real)

Railway te mostrará algo como esto después de crear la base de datos:

```
MYSQLHOST=monorail.proxy.rlway.app
MYSQLPORT=54729
MYSQLDATABASE=railway
MYSQLUSER=root
MYSQLPASSWORD=abc123xyz789
MYSQL_URL=mysql://root:abc123xyz789@monorail.proxy.rlway.app:54729/railway
```

Estos valores se cargarán automáticamente en tu aplicación.

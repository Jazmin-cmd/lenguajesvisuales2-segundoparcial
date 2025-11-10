# 📝 ClienteApiT - API de Gestión de Clientes

`ClienteApiT` es una **API ASP.NET Core 8** diseñada para gestionar clientes, subir archivos asociados y registrar errores automáticamente. Ideal para publicarse en **Monster ASP.NET**.  

---

## 🚀 Funcionalidades principales

### 1️⃣ Registro de Clientes
Registra la información básica de un cliente, incluyendo fotos de su vivienda.  
**Campos requeridos:**
- 🆔 CI  
- 🧑 Nombres  
- 🏠 Dirección  
- 📞 Teléfono  
- 📷 FotoCasa1, FotoCasa2, FotoCasa3  

**Endpoint:**  
`POST /api/clientes`  

💡 Tip: Las fotos se pueden almacenar en la base de datos o en disco.  

---

### 2️⃣ Carga de Múltiples Archivos
Permite subir archivos en **ZIP** (imágenes, documentos, videos) asociados a un cliente. La API descomprime y guarda los archivos automáticamente en `wwwroot/uploads`.  

**Endpoint:**  
`POST /api/files/uploadZip`  
**Campos requeridos:**  
- `CiCliente`  
- `ZipFile`  

✅ Archivos válidos: `.jpg`, `.jpeg`, `.png`, `.pdf`, `.docx`, `.mp4`  

---

### 3️⃣ Seguimiento y Registro de Errores
Todos los errores y eventos de la API se registran en la base de datos en la tabla `Logs`.  
Esto permite revisar problemas y depurar fácilmente.  

**Ejemplo de errores registrados:**
- Archivo ZIP vacío 🚫  
- Cliente no encontrado ❌  
- Error interno al procesar archivos ⚠️  

---

### 4️⃣ Publicación en Hosting
La API puede publicarse en **Monster ASP.NET** u otro hosting compatible.  
Se incluyen todos los archivos necesarios:  
- `.dll`, `.exe`  
- `appsettings.json`  
- Carpeta `wwwroot`  
- Scripts de EF en `EFSQLScripts`  

---

## 🛠 Tecnologías utilizadas
- ASP.NET Core 8  
- Entity Framework Core (SQL Server)  
- Swagger (documentación y pruebas de endpoints)  
- Middleware para manejo de errores  
- SFTP para publicación en hosting  

---

## 📂 Estructura del proyecto
- `Controllers/` → Endpoints (`ClientesController`, `FilesController`)  
- `Data/` → DbContext y migraciones  
- `Models/` → Entidades (`Cliente`, `ArchivoCliente`, `LogApi`)  
- `wwwroot/` → Archivos públicos subidos  
- `EFSQLScripts/` → Scripts de base de datos  

---

## 💻 Instrucciones de ejecución local
1. **Clonar el repositorio**
```bash
git clone <url-del-repo>
cd ClienteApiT
```

2. Configurar la cadena de conexión
Abrir appsettings.json
Reemplazar ConexionSql con tu servidor SQL local o remoto:
"ConnectionStrings": {
    "ConexionSql": "Server=TU_SERVIDOR;Database=TU_BD;User Id=USUARIO;Password=CONTRASEÑA;"
}

3. Instalar dependencias
   donet restore
4. Aplicar Migraciones
   dotnet ef database update
5. Ejecutar la API
   dotnet run

   

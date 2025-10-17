# 🩺 Controlador `DoctorController`

Este documento describe el funcionamiento del **controlador DoctorController** dentro del proyecto `appointment_backend`. Su propósito principal es gestionar las operaciones CRUD relacionadas con los doctores del sistema médico.

---

## 📋 Descripción general

El `DoctorController` se encarga de:

* Obtener la lista de todos los doctores.
* Registrar nuevos doctores.
* Editar la información de un doctor existente.
* Eliminar doctores de la base de datos.

Este controlador utiliza **Entity Framework Core** a través del contexto `AppDbContext` para comunicarse con la base de datos.

---

## 🧠 Dependencias

```csharp
using appointment_backend.Models;
using MedicalSys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
```

* **appointment_backend.Models** → Contiene el modelo `Doctor`.
* **MedicalSys.Data** → Proporciona el contexto de base de datos `AppDbContext`.
* **Microsoft.EntityFrameworkCore** → Permite trabajar con consultas asincrónicas.
* **Microsoft.AspNetCore.Mvc** → Controla las rutas, vistas y respuestas HTTP.

---

## ⚙️ Constructor

```csharp
public DoctorController(AppDbContext context)
{
    _context = context;
}
```

Inyecta el contexto `AppDbContext`, que permite acceder a la tabla `Doctors` en la base de datos.

---

## 🚀 Métodos principales

### 🔹 **Index()**

Muestra la vista principal y llama al método `GetAll()` para cargar todos los doctores.

```csharp
[HttpGet]
public async Task<IActionResult> Index()
```

**Flujo:**

1. Ejecuta `await GetAll()`.
2. Retorna la vista `Index()`.

---

### 🔹 **GetAll()**

Obtiene todos los doctores registrados.

```csharp
[HttpGet]
public async Task<IActionResult> GetAll()
```

**Proceso:**

* Usa `ToListAsync()` para obtener los doctores.
* Guarda un mensaje en `TempData["SuccessMessage"]` si la operación fue exitosa.
* Si ocurre un error, guarda el mensaje de error en `TempData["ErrorMessage"]`.

**Retorno:** Redirige a la acción `Index` junto con la lista de doctores.

---

### 🔹 **Create(Doctor doctor)**

Crea un nuevo registro de doctor.

```csharp
[HttpPost]
public async Task<IActionResult> Create(Doctor doctor)
```

**Validaciones:**

* Verifica si el número de documento (`Document`) ya existe.
* Si existe, agrega un error con `ModelState.AddModelError()`.

**Proceso:**

* Agrega el nuevo doctor a la base de datos con `_context.Doctors.Add(doctor)`.
* Guarda los cambios de forma asíncrona con `await _context.SaveChangesAsync()`.
* Guarda un mensaje de éxito en `TempData["SuccessMessage"]`.

**Retorno:** Redirige a la acción `Index`.

---

### 🔹 **Edit(Doctor doctor)**

Permite modificar la información de un doctor existente.

```csharp
public async Task<IActionResult> Edit(Doctor doctor)
```

**Validaciones:**

* Comprueba que el modelo sea válido.
* Busca al doctor por `Id` con `FindAsync()`.

**Proceso:**

* Si no se encuentra el doctor, devuelve `NotFound()`.
* Actualiza los campos del doctor con los valores del parámetro `doctor`.
* Aplica los cambios con `_context.Doctors.Update()` y los guarda.

**Retorno:** Redirige a `Index` tras la actualización.

---

### 🔹 **Delete(int id)**

Elimina un doctor de la base de datos según su `Id`.

```csharp
public async Task<ActionResult> Delete(int id)
```

**Flujo:**

1. Busca el doctor con `FirstOrDefaultAsync()`.
2. Si no se encuentra, muestra un mensaje en consola.
3. Si se encuentra, lo elimina con `_context.Doctors.Remove()`.
4. Guarda los cambios y redirige a `Index`.

---

## 🧩 Posibles mejoras

* Corregir la validación de documento en el método `Create`, ya que `AnyAsync()` devuelve una tarea (`Task<bool>`) y requiere `await`.
* Retornar vistas con mensajes más claros en lugar de redirigir directamente.
* Implementar logs con `ILogger` en lugar de `Console.WriteLine`.
* Añadir validación de campos como correo electrónico y teléfono.
* Optimizar el método `GetAll()` para retornar datos en formato JSON si se requiere una API.

---

## 🧪 Flujo básico de uso

1. `GET /Doctor/Index` → Muestra la lista de doctores.
2. `POST /Doctor/Create` → Crea un nuevo doctor tras validar sus datos.
3. `POST /Doctor/Edit` → Modifica los datos del doctor seleccionado.
4. `POST /Doctor/Delete/{id}` → Elimina al doctor correspondiente.

---

## 💬 Autoría

**Desarrollado por:** Faragon 🧠☕

> “A veces, simplemente hay que dejar que el código sea código.” — *J.A.R.V.I.S., asistente personal de Faragon*

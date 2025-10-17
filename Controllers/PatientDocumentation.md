# 🏥 Controlador `PatientController`

Este documento describe el funcionamiento del **controlador PatientController** dentro del proyecto `appointment_backend`. Su propósito principal es manejar las operaciones CRUD relacionadas con los pacientes en el sistema médico.

---

## 📋 Descripción general

El `PatientController` permite:

* Listar todos los pacientes registrados.
* Registrar nuevos pacientes.
* Editar la información de pacientes existentes.
* Eliminar pacientes de la base de datos.
* Validar datos como edad y número telefónico antes de guardar los cambios.

El controlador utiliza **Entity Framework Core** para interactuar con la base de datos a través del contexto `AppDbContext`.

---

## 🧠 Dependencias

```csharp
using appointment_backend.Models;
using MedicalSys.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
```

* **appointment_backend.Models** → Contiene la clase `Patient` (modelo de datos del paciente).
* **MedicalSys.Data** → Contiene el contexto de base de datos `AppDbContext`.
* **Microsoft.EntityFrameworkCore** → Permite el uso de métodos asincrónicos para acceder a la base de datos.
* **Microsoft.AspNetCore.Mvc** → Permite manejar las rutas y vistas del controlador.

---

## ⚙️ Constructor

```csharp
public PatientController(AppDbContext context)
{
    _context = context;
}
```

Inyecta el contexto de la base de datos `AppDbContext` para permitir las operaciones CRUD sobre la entidad `Patient`.

---

## 🚀 Métodos principales

### 🔹 **Index()**

Obtiene la lista completa de pacientes desde la base de datos.

```csharp
[HttpGet]
public async Task<IActionResult> Index()
```

* Usa `ToListAsync()` para traer todos los pacientes de forma asíncrona.
* Muestra un mensaje de error si ocurre una excepción.

**Vista:** `View(patients)`

---

### 🔹 **Register(Patient patient)**

Registra un nuevo paciente en la base de datos.

```csharp
[HttpPost]
public async Task<IActionResult> Register(Patient patient)
```

**Validaciones:**

* Comprueba si el modelo es válido (`ModelState.IsValid`).
* Llama a `AgeValid(patient)` para asegurar que la edad esté dentro del rango permitido.

**Acción final:**

* Agrega el paciente a la base de datos con `_context.Patients.Add(patient)`.
* Guarda los cambios con `await _context.SaveChangesAsync()`.

---

### 🔹 **Delete(int id)**

Elimina un paciente por su `id`.

```csharp
[HttpPost]
public async Task<IActionResult> Delete(int id)
```

**Proceso:**

1. Busca el paciente con `FindAsync(id)`.
2. Si lo encuentra, lo elimina con `_context.Patients.Remove(delete)`.
3. Guarda los cambios con `SaveChangesAsync()`.

---

### 🔹 **Edit(int id)**

Busca un paciente por su `id` y retorna la vista para editarlo.

```csharp
[HttpPost]
public IActionResult Edit(int id)
```

* Usa `_context.Patients.Find(id)`.
* Si no encuentra el paciente, devuelve `NotFound()`.

---

### 🔹 **SaveEdit(Patient patient)**

Guarda los cambios de un paciente editado.

```csharp
[HttpPost]
public async Task<IActionResult> SaveEdit(Patient patient)
```

**Validaciones:**

* Revisa `ModelState` y edad antes de guardar.

**Proceso:**

* Actualiza la información con `_context.Patients.Update(patient)`.
* Guarda los cambios con `SaveChangesAsync()`.

---

## 🧩 Métodos de validación

### 🔹 **AgeValid(Patient patient)**

Valida que la edad esté dentro del rango de 1 a 100 años.

```csharp
[HttpPost]
public bool AgeValid(Patient patient)
```

* Agrega un error al modelo si la edad es nula, 0, menor que 1 o mayor que 100.

---

### 🔹 **PhoneValid(Patient patient)**

Valida que el número telefónico tenga **10 dígitos exactos** y **solo números**.

```csharp
public bool PhoneValid(Patient patient)
```

* Si el número está vacío o contiene letras, agrega un error al modelo.


---

## 🧪 Ejemplo de flujo básico

1. Usuario abre la vista principal → `GET /Patient/Index` → se listan los pacientes.
2. Usuario llena el formulario → `POST /Patient/Register` → se valida y guarda.
3. Usuario desea editar → `POST /Patient/Edit/{id}` → se muestra el formulario con los datos.
4. Usuario guarda cambios → `POST /Patient/SaveEdit` → se actualiza en la base de datos.
5. Usuario elimina → `POST /Patient/Delete/{id}` → se borra el registro.

---

## 💬 Autoría

**Desarrollado por:** Faragon 🧠☕

> “El buen código es como un buen espresso: concentrado, potente y mejor si se sirve caliente.” — *J.A.R.V.I.S., asistente personal de Faragon*

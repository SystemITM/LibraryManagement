# Library Management

Sistema de Gestión de Biblioteca desarrollado como proyecto final para la asignatura de Programación Web.

El sistema permite administrar libros, autores, categorías, miembros y préstamos de una biblioteca, aplicando una arquitectura por capas en el backend con .NET 8 Web API y un frontend en Angular.

---

## Integrantes

Juan Manuel Colorado Navas

---

## Descripción del proyecto

Library Management es una aplicación web full-stack que permite gestionar los procesos principales de una biblioteca:

- Administración de categorías.
- Administración de autores.
- Administración de miembros.
- Administración de libros.
- Registro de préstamos.
- Devolución de libros.
- Control de copias disponibles.
- Consulta de detalles por módulo.

El sistema fue desarrollado siguiendo los patrones trabajados en el proyecto de referencia SportsLeague, usando una arquitectura de tres capas: Domain, DataAccess y API.

---

## Tecnologías utilizadas

### Backend

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server / SQL Server LocalDB
- AutoMapper
- Swagger / OpenAPI
- Repository Pattern
- Services con validaciones de negocio
- Code First Migrations
- DataSeeder

### Frontend

- Angular
- TypeScript
- Bootstrap
- Angular Router
- HttpClient

---

## Arquitectura del backend

El backend está dividido en tres capas principales:

LibraryManagement.Domain

Contiene la lógica principal del dominio:

Entidades.
Enums.
Interfaces de repositorios.
Interfaces de servicios.
Servicios con reglas de negocio.

LibraryManagement.DataAccess

Contiene la configuración de acceso a datos:

DbContext.
Repositorios.
Migraciones.
DataSeeder.

LibraryManagement.API

Contiene la capa de exposición HTTP:

Controllers.
DTOs de request.
DTOs de response.
Perfil de AutoMapper.
Configuración de Swagger.
Configuración de CORS.

Entidades principales

El sistema maneja las siguientes entidades:

Book
Author
Category
Member
Loan
También se incluye la entidad intermedia:

BookAuthor
Esta entidad permite manejar la relación muchos a muchos entre libros y autores.
Relaciones implementadas
Relación 1:N

Una categoría puede tener muchos libros.

Category 1 ---- N Book

Un miembro puede tener muchos préstamos.

Member 1 ---- N Loan

Un libro puede tener muchos préstamos.

Book 1 ---- N Loan
Relación N:M

Un libro puede tener varios autores y un autor puede estar asociado a varios libros.

Book N ---- M Author

Esta relación se implementa mediante la tabla intermedia:

BookAuthor
Enum implementado

El sistema incluye el enum LoanStatus para controlar el estado de los préstamos:

Active
Returned
Overdue
Funcionalidades principales
Categorías
Listar categorías.
Crear categoría.
Editar categoría.
Ver detalle de categoría.
Eliminar categoría.
Validar categorías duplicadas.
Evitar eliminar categorías con libros asociados.
Autores
Listar autores.
Crear autor.
Editar autor.
Ver detalle de autor.
Eliminar autor.
Validar autores duplicados.
Evitar eliminar autores con libros asociados.
Miembros
Listar miembros.
Crear miembro.
Editar miembro.
Ver detalle de miembro.
Eliminar miembro.
Activar o inactivar miembros.
Validar documento duplicado.
Validar correo duplicado.
Evitar eliminar miembros con préstamos registrados.
Libros
Listar libros.
Crear libro.
Editar libro.
Ver detalle de libro.
Eliminar libro.
Asociar libro con categoría.
Asociar libro con uno o varios autores.
Validar ISBN duplicado.
Controlar total de copias y copias disponibles.
Evitar eliminar libros con préstamos registrados.
Préstamos
Listar préstamos.
Crear préstamo.
Ver detalle de préstamo.
Registrar devolución.
Eliminar préstamos devueltos.
Validar que el libro tenga copias disponibles.
Validar que el miembro esté activo.
Validar que la fecha límite sea posterior a la fecha actual.
Evitar préstamos duplicados activos para el mismo miembro y libro.
Actualizar automáticamente las copias disponibles al prestar o devolver un libro.
Requisitos previos

Antes de ejecutar el proyecto, se debe tener instalado:

Visual Studio Community 2022 o superior.
.NET 8 SDK.
SQL Server LocalDB o SQL Server.
Node.js.
Angular CLI.

Para verificar Node.js y npm:

node -v
npm -v

Para instalar Angular CLI:

npm install -g @angular/cli
Configuración de base de datos

El proyecto utiliza Entity Framework Core con enfoque Code First.

La cadena de conexión se encuentra en:

LibraryManagement.API/appsettings.json

Configuración usada para desarrollo local:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LibraryManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}

Si se usa SQL Server Express, se puede cambiar por:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=LibraryManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
Ejecutar el backend

Abrir la solución en Visual Studio Community:

LibraryManagement.sln

Establecer como proyecto de inicio:

LibraryManagement.API

Luego ejecutar el proyecto.

Swagger debe abrir en:

http://localhost:5038/swagger/index.html
Crear o actualizar la base de datos

Desde Visual Studio, abrir la Consola del Administrador de paquetes NuGet:

Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes

Ejecutar:

Update-Database -Project LibraryManagement.DataAccess -StartupProject LibraryManagement.API -Context LibraryDbContext

El proyecto incluye un DataSeeder que carga datos iniciales automáticamente al iniciar la API.

No es necesario insertar datos manualmente para probar el sistema.

Ejecutar el frontend

Entrar a la carpeta del frontend:

cd LibraryManagement.Frontend

Instalar dependencias:

npm install

Ejecutar Angular:

ng serve

Abrir en el navegador:

http://localhost:4200
Configuración de conexión del frontend con la API

La URL base de la API se encuentra en:

LibraryManagement.Frontend/src/app/core/services/api.config.ts

Contenido:

export const API_BASE_URL = 'http://localhost:5038/api';

La API debe estar ejecutándose antes de usar el frontend.

CORS

La API está configurada para permitir peticiones desde Angular:

http://localhost:4200

La configuración se encuentra en:

LibraryManagement.API/Program.cs
Endpoints principales

*Categories
GET     /api/categories
GET     /api/categories/{id}
POST    /api/categories
PUT     /api/categories/{id}
DELETE  /api/categories/{id}

*Authors
GET     /api/authors
GET     /api/authors/{id}
POST    /api/authors
PUT     /api/authors/{id}
DELETE  /api/authors/{id}

*Members
GET     /api/members
GET     /api/members/{id}
POST    /api/members
PUT     /api/members/{id}
DELETE  /api/members/{id}

*Books
GET     /api/books
GET     /api/books/{id}
POST    /api/books
PUT     /api/books/{id}
DELETE  /api/books/{id}

*Loans
GET     /api/loans
GET     /api/loans/{id}
POST    /api/loans
PUT     /api/loans/{id}/return
DELETE  /api/loans/{id}

Estructura general del repositorio
LibraryManagement
├── LibraryManagement.Domain
│   ├── Entities
│   ├── Enums
│   ├── Interfaces
│   └── Services
│
├── LibraryManagement.DataAccess
│   ├── Context
│   ├── Migrations
│   ├── Repositories
│   └── Seeders
│
├── LibraryManagement.API
│   ├── Controllers
│   ├── DTOs
│   ├── Mappings
│   ├── Program.cs
│   └── appsettings.json
│
└── LibraryManagement.Frontend
    └── src
        └── app
            ├── core
            ├── pages
            └── shared
Flujo principal del sistema
Crear préstamo
El usuario ingresa al módulo de préstamos.
Selecciona "Nuevo préstamo".
Selecciona un libro con copias disponibles.
Selecciona un miembro activo.
Selecciona una fecha límite de devolución.
El sistema registra el préstamo.
El sistema disminuye en uno las copias disponibles del libro.
Devolver préstamo
El usuario ingresa al módulo de préstamos.
Selecciona un préstamo activo o vencido.
Registra la devolución.
El sistema cambia el estado del préstamo a Returned.
El sistema aumenta en uno las copias disponibles del libro.
Validaciones implementadas

El sistema incluye validaciones de negocio en la capa Domain, entre ellas:

No crear categorías duplicadas.
No crear autores duplicados.
No crear miembros con documento repetido.
No crear miembros con correo repetido.
No crear libros con ISBN repetido.
No crear libros sin categoría.
No crear libros sin autores.
No crear préstamos para miembros inactivos.
No crear préstamos para libros sin copias disponibles.
No crear préstamos con fecha límite inválida.
No crear préstamos duplicados activos para el mismo miembro y libro.
No eliminar entidades con información relacionada importante.
Pruebas realizadas

Se probaron los endpoints desde Swagger y desde el frontend Angular.

Pruebas principales:

Crear, editar, ver y eliminar categorías.
Crear, editar, ver y eliminar autores.
Crear, editar, ver y eliminar miembros.
Crear, editar, ver y eliminar libros.
Crear préstamos.
Registrar devoluciones.
Validar actualización de copias disponibles.
Validar errores de duplicados.
Validar restricciones de eliminación.
URLs de ejecución local

Backend Swagger:

http://localhost:5038/swagger/index.html

Frontend Angular:

http://localhost:4200
Notas importantes
El backend debe ejecutarse antes que el frontend.
La base de datos se crea mediante migraciones de Entity Framework Core.
Los datos iniciales se insertan automáticamente mediante DataSeeder.
El frontend consume la API mediante HttpClient.
El sistema usa DTOs para no exponer directamente las entidades del dominio en los controladores.
La lógica de negocio se encuentra en los Services de la capa Domain.


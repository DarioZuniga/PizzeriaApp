# NetForum - Layered Architecture E-Commerce & Discussion Platform

Este repositorio contiene el desarrollo del proyecto final para el curso **Programación Avanzada (PA - 601)**. Se trata de una plataforma web de discusión y foro estructurada bajo los estándares más exigentes de la ingeniería de software moderna, utilizando el ecosistema de desarrollo de Microsoft.

El objetivo principal es demostrar el dominio de arquitecturas empresariales desacopladas, inyección de dependencias, patrones de diseño de persistencia y control de accesos seguro.

---

## 🏛️ Arquitectura del Sistema

El proyecto implementa una **Arquitectura en N-Capas (4 proyectos independientes)** que garantiza la separación de responsabilidades, alta mantenibilidad y escalabilidad del código:

* **`NetForum.Mvc` (Capa de Presentación):** Desarrollada en ASP.NET Core MVC. Contiene las vistas dinámicas (Razor Pages), controladores web, estilos basados en Bootstrap 5 y los modelos de visualización (`ViewModels`) para la captura y validación de datos en el cliente.
* **`NetForum.Business` (Capa de Lógica de Negocio):** El núcleo operativo del sistema. Centraliza las reglas del negocio, lógica de servicios distribuidos, encriptación de seguridad y validaciones de identidad.
* **`NetForum.Repository` (Capa de Abstracción de Datos):** Implementa el patrón *Repository*. Funciona como un mediador desacoplado entre la lógica de negocio y la persistencia física, exponiendo contratos limpios mediante interfaces.
* **`NetForum.Data` (Capa de Persistencia):** Gestiona el acceso directo a la base de datos a través de Entity Framework Core. Mapea las entidades de dominio y define el contexto transaccional.

---

## 🛠️ Stack Tecnológico

* **Framework Global:** .NET moderno (C# 12 / .NET 8.0 - 9.0)
* **FrontEnd:** ASP.NET Core MVC, Razor HTML5, Bootstrap 5
* **ORM / Persistencia:** Entity Framework Core 10.0 (Enfoque *Code-First*)
* **Motor de Base de Datos:** Microsoft SQL Server
* **Control de Versiones:** Git & GitHub

---

## 📁 Estructura del Repositorio

```text
NetForum/
│
├── NetForum.Mvc/               # Interfaz de Usuario (Controladores y Vistas)
│   └── Models/LoginViewModel.cs
│
├── NetForum.Business/          # Servicios y Reglas de Negocio
│   └── Services/AuthService.cs
│
├── NetForum.Repository/        # Interfaces y Patrones de Consulta
│   └── Interfaces/IUsuarioRepository.cs
│
└── NetForum.Data/              # Entidades y Contexto de Base de Datos
    ├── Entities/
    │   ├── Usuario.cs
    │   └── Post.cs
    └── NetForumDbContext.cs

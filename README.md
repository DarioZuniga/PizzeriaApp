# NewsFlow - Agregador de Noticias

Proyecto final del curso **SC-701 Programación Avanzada Web**.

## Descripción

NewsFlow es un agregador de noticias que permite:
- Consumir múltiples fuentes externas (APIs, RSS, XML, HTML)
- Guardar items en base de datos (solo usuarios autorizados)
- Exportar e importar items en formato JSON (interoperabilidad entre grupos)
- Gestionar fuentes y secretos (solo Admin)

## Arquitectura

- **Backend:** ASP.NET Core MVC (.NET 10)
- **Base de Datos:** SQL Server (LocalDB / Express)
- **Arquitectura:** N-Capas (Data, Repository, Business, MVC)
- **Patrones:** Repository, Service Layer, Factory, DTO, Dependency Injection
- **Principios:** SOLID

## Estructura del Proyecto

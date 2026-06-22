USE [master]
GO
CREATE DATABASE [NetForumDb]
GO
ALTER DATABASE [NetForumDb] SET COMPATIBILITY_LEVEL = 160
GO
USE [NetForumDb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
    [MigrationId] [nvarchar](150) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Categorias](
    [CategoriaId] [int] IDENTITY(1,1) NOT NULL,
    [EstadoId] [int] NOT NULL,
    [Nombre] [nvarchar](max) NOT NULL,
    [Descripcion] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED ([CategoriaId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Equipos](
    [EquipoId] [int] IDENTITY(1,1) NOT NULL,
    [LigaId] [int] NOT NULL,
    [Nombre] [nvarchar](max) NOT NULL,
    [Pais] [nvarchar](max) NOT NULL,
    [Fundacion] [int] NOT NULL,
 CONSTRAINT [PK_Equipos] PRIMARY KEY CLUSTERED ([EquipoId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Estados](
    [EstadoId] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](max) NOT NULL,
    [Descripcion] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Estados] PRIMARY KEY CLUSTERED ([EstadoId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Etiquetas](
    [EtiquetaId] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Etiquetas] PRIMARY KEY CLUSTERED ([EtiquetaId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Jugadores](
    [JugadorId] [int] IDENTITY(1,1) NOT NULL,
    [EquipoId] [int] NOT NULL,
    [Nombre] [nvarchar](max) NOT NULL,
    [Posicion] [nvarchar](max) NOT NULL,
    [Nacionalidad] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Jugadores] PRIMARY KEY CLUSTERED ([JugadorId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Ligas](
    [LigaId] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](max) NOT NULL,
    [Pais] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Ligas] PRIMARY KEY CLUSTERED ([LigaId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Notificaciones](
    [NotificacionId] [int] IDENTITY(1,1) NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [Mensaje] [nvarchar](max) NOT NULL,
    [Leida] [bit] NOT NULL,
    [FechaCreacion] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Notificaciones] PRIMARY KEY CLUSTERED ([NotificacionId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Reacciones](
    [ReaccionId] [int] IDENTITY(1,1) NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [TemaId] [int] NOT NULL,
    [TipoReaccionId] [int] NOT NULL,
 CONSTRAINT [PK_Reacciones] PRIMARY KEY CLUSTERED ([ReaccionId] ASC)
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Reportes](
    [ReporteId] [int] IDENTITY(1,1) NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [TemaId] [int] NOT NULL,
    [EstadoId] [int] NOT NULL,
    [Motivo] [nvarchar](max) NOT NULL,
    [FechaReporte] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Reportes] PRIMARY KEY CLUSTERED ([ReporteId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Respuestas](
    [RespuestaId] [int] IDENTITY(1,1) NOT NULL,
    [TemaId] [int] NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [EstadoId] [int] NOT NULL,
    [Contenido] [nvarchar](max) NOT NULL,
    [FechaCreacion] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Respuestas] PRIMARY KEY CLUSTERED ([RespuestaId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Roles](
    [RolId] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](max) NOT NULL,
    [Descripcion] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RolId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[TemaEtiquetas](
    [TemaEtiquetaId] [int] IDENTITY(1,1) NOT NULL,
    [TemaId] [int] NOT NULL,
    [EtiquetaId] [int] NOT NULL,
 CONSTRAINT [PK_TemaEtiquetas] PRIMARY KEY CLUSTERED ([TemaEtiquetaId] ASC)
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[Temas](
    [TemaId] [int] IDENTITY(1,1) NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [CategoriaId] [int] NOT NULL,
    [EstadoId] [int] NOT NULL,
    [Titulo] [nvarchar](max) NOT NULL,
    [Contenido] [nvarchar](max) NOT NULL,
    [FechaCreacion] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Temas] PRIMARY KEY CLUSTERED ([TemaId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[TiposReaccion](
    [TipoReaccionId] [int] IDENTITY(1,1) NOT NULL,
    [Nombre] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TiposReaccion] PRIMARY KEY CLUSTERED ([TipoReaccionId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE TABLE [dbo].[Usuarios](
    [UsuarioId] [int] IDENTITY(1,1) NOT NULL,
    [RolId] [int] NOT NULL,
    [EstadoId] [int] NOT NULL,
    [NombreUsuario] [nvarchar](max) NOT NULL,
    [Correo] [nvarchar](max) NOT NULL,
    [PasswordHash] [nvarchar](max) NOT NULL,
    [FechaRegistro] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([UsuarioId] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260620180713_InitialCreate', N'10.0.9')
GO
SET IDENTITY_INSERT [dbo].[Categorias] ON
GO
INSERT [dbo].[Categorias] ([CategoriaId], [EstadoId], [Nombre], [Descripcion]) VALUES (1, 1, N'Champions League', N'Debates sobre la Champions League')
GO
INSERT [dbo].[Categorias] ([CategoriaId], [EstadoId], [Nombre], [Descripcion]) VALUES (2, 1, N'Premier League', N'Foro sobre fútbol inglés')
GO
INSERT [dbo].[Categorias] ([CategoriaId], [EstadoId], [Nombre], [Descripcion]) VALUES (3, 1, N'Liga Promerica', N'Foro sobre fútbol de Costa Rica')
GO
SET IDENTITY_INSERT [dbo].[Categorias] OFF
GO
SET IDENTITY_INSERT [dbo].[Estados] ON
GO
INSERT [dbo].[Estados] ([EstadoId], [Nombre], [Descripcion]) VALUES (1, N'Activo', N'Registro activo')
GO
INSERT [dbo].[Estados] ([EstadoId], [Nombre], [Descripcion]) VALUES (2, N'Inactivo', N'Registro inactivo')
GO
INSERT [dbo].[Estados] ([EstadoId], [Nombre], [Descripcion]) VALUES (3, N'Suspendido', N'Registro suspendido')
GO
SET IDENTITY_INSERT [dbo].[Estados] OFF
GO
SET IDENTITY_INSERT [dbo].[Etiquetas] ON
GO
INSERT [dbo].[Etiquetas] ([EtiquetaId], [Nombre]) VALUES (1, N'Fichajes')
GO
INSERT [dbo].[Etiquetas] ([EtiquetaId], [Nombre]) VALUES (2, N'VAR')
GO
INSERT [dbo].[Etiquetas] ([EtiquetaId], [Nombre]) VALUES (3, N'Clásico')
GO
SET IDENTITY_INSERT [dbo].[Etiquetas] OFF
GO
SET IDENTITY_INSERT [dbo].[Ligas] ON
GO
INSERT [dbo].[Ligas] ([LigaId], [Nombre], [Pais]) VALUES (1, N'La Liga', N'España')
GO
INSERT [dbo].[Ligas] ([LigaId], [Nombre], [Pais]) VALUES (2, N'Premier League', N'Inglaterra')
GO
INSERT [dbo].[Ligas] ([LigaId], [Nombre], [Pais]) VALUES (3, N'Liga Promerica', N'Costa Rica')
GO
SET IDENTITY_INSERT [dbo].[Ligas] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
GO
INSERT [dbo].[Roles] ([RolId], [Nombre], [Descripcion]) VALUES (1, N'Administrador', N'Control total del sistema')
GO
INSERT [dbo].[Roles] ([RolId], [Nombre], [Descripcion]) VALUES (2, N'Moderador', N'Gestiona contenido del foro')
GO
INSERT [dbo].[Roles] ([RolId], [Nombre], [Descripcion]) VALUES (3, N'Usuario', N'Usuario general del foro')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[TiposReaccion] ON
GO
INSERT [dbo].[TiposReaccion] ([TipoReaccionId], [Nombre]) VALUES (1, N'Me gusta')
GO
INSERT [dbo].[TiposReaccion] ([TipoReaccionId], [Nombre]) VALUES (2, N'Golazo')
GO
INSERT [dbo].[TiposReaccion] ([TipoReaccionId], [Nombre]) VALUES (3, N'Polémico')
GO
SET IDENTITY_INSERT [dbo].[TiposReaccion] OFF
GO
ALTER TABLE [dbo].[Categorias]  WITH CHECK ADD  CONSTRAINT [FK_Categorias_Estados_EstadoId] FOREIGN KEY([EstadoId])
REFERENCES [dbo].[Estados] ([EstadoId])
GO
ALTER TABLE [dbo].[Equipos]  WITH CHECK ADD  CONSTRAINT [FK_Equipos_Ligas_LigaId] FOREIGN KEY([LigaId])
REFERENCES [dbo].[Ligas] ([LigaId])
GO
ALTER TABLE [dbo].[Jugadores]  WITH CHECK ADD  CONSTRAINT [FK_Jugadores_Equipos_EquipoId] FOREIGN KEY([EquipoId])
REFERENCES [dbo].[Equipos] ([EquipoId])
GO
ALTER TABLE [dbo].[Notificaciones]  WITH CHECK ADD  CONSTRAINT [FK_Notificaciones_Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[Reacciones]  WITH CHECK ADD  CONSTRAINT [FK_Reacciones_Temas_TemaId] FOREIGN KEY([TemaId])
REFERENCES [dbo].[Temas] ([TemaId])
GO
ALTER TABLE [dbo].[Reacciones]  WITH CHECK ADD  CONSTRAINT [FK_Reacciones_TiposReaccion_TipoReaccionId] FOREIGN KEY([TipoReaccionId])
REFERENCES [dbo].[TiposReaccion] ([TipoReaccionId])
GO
ALTER TABLE [dbo].[Reacciones]  WITH CHECK ADD  CONSTRAINT [FK_Reacciones_Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[Reportes]  WITH CHECK ADD  CONSTRAINT [FK_Reportes_Estados_EstadoId] FOREIGN KEY([EstadoId])
REFERENCES [dbo].[Estados] ([EstadoId])
GO
ALTER TABLE [dbo].[Reportes]  WITH CHECK ADD  CONSTRAINT [FK_Reportes_Temas_TemaId] FOREIGN KEY([TemaId])
REFERENCES [dbo].[Temas] ([TemaId])
GO
ALTER TABLE [dbo].[Reportes]  WITH CHECK ADD  CONSTRAINT [FK_Reportes_Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[Respuestas]  WITH CHECK ADD  CONSTRAINT [FK_Respuestas_Estados_EstadoId] FOREIGN KEY([EstadoId])
REFERENCES [dbo].[Estados] ([EstadoId])
GO
ALTER TABLE [dbo].[Respuestas]  WITH CHECK ADD  CONSTRAINT [FK_Respuestas_Temas_TemaId] FOREIGN KEY([TemaId])
REFERENCES [dbo].[Temas] ([TemaId])
GO
ALTER TABLE [dbo].[Respuestas]  WITH CHECK ADD  CONSTRAINT [FK_Respuestas_Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[TemaEtiquetas]  WITH CHECK ADD  CONSTRAINT [FK_TemaEtiquetas_Etiquetas_EtiquetaId] FOREIGN KEY([EtiquetaId])
REFERENCES [dbo].[Etiquetas] ([EtiquetaId])
GO
ALTER TABLE [dbo].[TemaEtiquetas]  WITH CHECK ADD  CONSTRAINT [FK_TemaEtiquetas_Temas_TemaId] FOREIGN KEY([TemaId])
REFERENCES [dbo].[Temas] ([TemaId])
GO
ALTER TABLE [dbo].[Temas]  WITH CHECK ADD  CONSTRAINT [FK_Temas_Categorias_CategoriaId] FOREIGN KEY([CategoriaId])
REFERENCES [dbo].[Categorias] ([CategoriaId])
GO
ALTER TABLE [dbo].[Temas]  WITH CHECK ADD  CONSTRAINT [FK_Temas_Estados_EstadoId] FOREIGN KEY([EstadoId])
REFERENCES [dbo].[Estados] ([EstadoId])
GO
ALTER TABLE [dbo].[Temas]  WITH CHECK ADD  CONSTRAINT [FK_Temas_Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([UsuarioId])
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Estados_EstadoId] FOREIGN KEY([EstadoId])
REFERENCES [dbo].[Estados] ([EstadoId])
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Roles_RolId] FOREIGN KEY([RolId])
REFERENCES [dbo].[Roles] ([RolId])
GO

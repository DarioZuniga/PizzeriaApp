namespace NetForum.MVC.Security;

// SOLID: Single Responsibility Principle
// Centraliza los nombres de roles utilizados por la capa MVC, por lo que si se cambia
// el nombre de un rol, solo se debe cambiar en este archivo.
public static class RolesSistema
{
    public const string Administrador = "Administrador";

    public const string Moderador = "Moderador";

    public const string Usuario = "Usuario";
}

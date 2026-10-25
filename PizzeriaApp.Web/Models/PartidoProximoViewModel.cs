namespace PizzeriaApp.MVC.Models;

public class PartidoProximoViewModel
{
    public int PartidoId { get; set; }

    public string Competicion { get; set; } = string.Empty;

    public string Local { get; set; } = string.Empty;

    public string Visitante { get; set; } = string.Empty;

    public string? EscudoLocal { get; set; }

    public string? EscudoVisitante { get; set; }

    public DateTime FechaUtc { get; set; }
}

/*

    Prop�sito de UI: Transporta �nicamente la informaci�n visual relevante para la pantalla (nombres de equipos, escudos, 
competici�n y fecha), aislando la vista de las entidades complejas de la base de datos o de las respuestas crudas de la
API (DTOs).

    Propiedades opcionales (string?): EscudoLocal y EscudoVisitante admiten valores nulos (null) por si alg�n equipo no
cuenta con la URL de su logo cargada.

    Valores por defecto (= string.Empty): Previene excepciones de referencia nula (NullReferenceException) 
inicializando las cadenas no nulas con texto vac�o.*/
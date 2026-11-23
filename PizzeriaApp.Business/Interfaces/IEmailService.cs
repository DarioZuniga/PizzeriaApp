namespace PizzeriaApp.Business.Interfaces;
public interface IEmailService {
    Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml);
}
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using PizzeriaApp.Business.Interfaces;

namespace PizzeriaApp.Business.Services;

public class EmailService : IEmailService
{
    public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml)
    {
        var mensaje = new MimeMessage();
        // REMITENTE (La pizzería)
        mensaje.From.Add(new MailboxAddress("Las Perlas Pizzeria", "josedar2003@gmail.com"));
        // DESTINATARIO (El cliente que se registró)
        mensaje.To.Add(new MailboxAddress("Cliente", destinatario));
        mensaje.Subject = asunto;

        var builder = new BodyBuilder { HtmlBody = cuerpoHtml };
        mensaje.Body = builder.ToMessageBody();

        using var cliente = new SmtpClient();
        await cliente.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        
        // AUTENTICACIÓN (Usa la contraseña de aplicación de 16 letras)
        await cliente.AuthenticateAsync("josedario2003@gmail.com", "xxfeqpqklbqwjfuc ");
        
        await cliente.SendAsync(new SendRequest(mensaje));
        
        await cliente.DisconnectAsync(true);
    }
}
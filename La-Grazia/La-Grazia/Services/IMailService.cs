using System;
using La_Grazia.Database.Models;

namespace La_Grazia.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
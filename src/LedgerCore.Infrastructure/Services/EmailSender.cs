using CSharpFunctionalExtensions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using StockCore.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Infrastructure.Services
{
    internal class EmailSender(IConfiguration configuration) : IEmailSender
    {
        public async Task<Result> SendEmail(string title, string content, string userAdress)
        {
            var ledgerEmail = configuration.GetValue<string>("Email:Name");
            var StockEmailPassword = configuration.GetValue<string>("Email:Password");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("LedgerCore", ledgerEmail));
            message.To.Add(new MailboxAddress("User",userAdress));
            message.Subject = title;
            message.Body = new TextPart("plain")
            {
                Text = content
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(ledgerEmail, ledgerEmailPassword);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return Result.Success();
        }
    }
}

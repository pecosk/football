using FootballLeague.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;

namespace FootballLeague.Services
{
    public class EmailNotifier : INotifier
    {
        public void Notify(User user, IEnumerable<User> invites, DateTime time)
        {
            if(string.IsNullOrEmpty(user.Mail))
                return;

            var mailServer = ConfigurationManager.AppSettings["mailServer"];
            var appAddress = ConfigurationManager.AppSettings["appAddress"];
            if (string.IsNullOrEmpty(mailServer) || string.IsNullOrEmpty(appAddress))
                return;

            if (!appAddress.EndsWith("/"))
                appAddress = appAddress + "/";

            foreach (var invitee in invites)
            {
                if(string.IsNullOrEmpty(invitee.Mail))
                    continue;

                var mail = new MailMessage(user.Mail, invitee.Mail);
                var client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = mailServer;
                mail.Subject = "Invitation for Football match";
                mail.Body = string.Format(@"Hi {0},
you have been invited to play a match on {1} by organizer {2} {3}. 
You can confirm your participation on {4}#/matches", invitee.FirstName, time, user.FirstName, user.LastName, appAddress);
                client.Send(mail);
            }
        }
    }
}
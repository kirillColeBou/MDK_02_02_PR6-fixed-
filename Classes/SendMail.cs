using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RegIn_Тепляков.Classes
{
    public class SendMail
    {
        public static void SendMessage(string message, string to)
        {
            var smptClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("yandex@yandex.ru", "password"),
                EnableSsl = true
            };
            smptClient.Send("Kirillteplycov123@yandex.ru", to, "Проект RegIn", message);
        }
    }
}

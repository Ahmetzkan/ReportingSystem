using Business.Dtos.Requests.MailRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstracts
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(SendMailRequest sendMailRequest);
        Task SendPasswordResetMailAsync(SendPasswordResetMailRequest sendPasswordResetMailRequest);
    }
}

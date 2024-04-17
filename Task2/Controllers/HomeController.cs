using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task2.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using Microsoft.Extensions.Options;

namespace Task2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IOptionsMonitor<SmtpConnectingInfo> _connectingMonitor;

        public HomeController(IOptionsMonitor<SmtpConnectingInfo> connecting, ILogger<HomeController> logger)
        {
            _connectingMonitor = connecting;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(MessageModel model)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_connectingMonitor.CurrentValue.OwnerAddress);
            message.To.Add(new MailAddress(model.Receiver));
            message.Subject = model.Subject;
            message.Body = model.Message;

            SmtpClient smtpClient = new SmtpClient(_connectingMonitor.CurrentValue.Address);
            smtpClient.Port = _connectingMonitor.CurrentValue.Port;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_connectingMonitor.CurrentValue.OwnerAddress, _connectingMonitor.CurrentValue.Password);

            try
            {
                smtpClient.Send(message);
                return View("Success");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.ToString());
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

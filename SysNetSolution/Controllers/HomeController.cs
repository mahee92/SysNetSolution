using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using SysNetSolution.Models;
using System.Diagnostics;
using System.Net.Mail;

namespace SysNetSolution.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
           
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<JsonResult> Email(string name,string email, string phone,string msg)
        {
            try {



                string subject = "Message from NetSys";
                string content = msg;// + "   phone-"+ phone + "    email-"+ email;
                string fromAddress = "info@netsyssolutions.net";
                //string fromAddress = email;
                string toAddress = "info@netsyssolutions.net";
                string? tenantId = "35a4a008-a798-43db-9a34-ac2d4b2a332e";
                string? clientId = "c9088a6c-92f0-446e-bcfc-46fe9b28a4a5";
                string? clientSecret = "AqH8Q~bwEobVnED5snLynvd4U5hYBdemVQm_iaY_";

                ClientSecretCredential credential = new(tenantId, clientId, clientSecret);
                GraphServiceClient graphClient = new(credential);

                Message message = new()
                {
                    Subject = subject,
                    Body = new ItemBody
                    {
                        ContentType = BodyType.Text,
                        Content = content
                    },
                    ToRecipients = new List<Recipient>()
            {
                new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = toAddress
                    }
                }
            }
                };

                bool saveToSentItems = true;



              await graphClient.Users[fromAddress]
                  .SendMail(message, saveToSentItems)
                  .Request()
                  .PostAsync();

                return Json("true");



            }
            catch(Exception ex)
            {

                return Json("false");


            }
           
        }



        //static async Task Send()
        //{
        //    // Set your Office 365 (Microsoft 365) client ID and secret
        //    string clientId = "your-client-id";
        //    string clientSecret = "your-client-secret";
        //    string tenantId = "your-tenant-id";

        //    // Set the email address you want to send the email to
        //    string toEmailAddress = "recipient@example.com";

        //    // Authenticate using OAuth
        //    var confidentialClientApplication = ConfidentialClientApplicationBuilder
        //        .Create(clientId)
        //        .WithClientSecret(clientSecret)
        //        .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
        //    .Build();
        //    var scopes = new[] { "https://graph.microsoft.com/.default" };
        //    var authResult = await confidentialClientApplication.AcquireTokenForClient(scopes)
        //        .ExecuteAsync();

        //    var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider(requestMessage =>
        //    {
        //        requestMessage.Headers.Authorization =
        //            new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

        //        return Task.CompletedTask;
        //    }));

        //    // Compose and send the email
        //    var message = new Message
        //    {
        //        Subject = "Test Email",
        //        Body = new ItemBody
        //        {
        //            Content = "This is a test email sent from a .NET application.",
        //            ContentType = BodyType.Text,
        //        },
        //        ToRecipients = new List<Recipient>
        //    {
        //        new Recipient
        //        {
        //            EmailAddress = new EmailAddress
        //            {
        //                Address = toEmailAddress,
        //            },
        //        },
        //    },
        //    };

        //    try
        //    {
        //        await graphServiceClient.Me.SendMail(message, true).Request().PostAsync();
        //        Console.WriteLine("Email sent successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error sending email: {ex.Message}");
        //    }
        //}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
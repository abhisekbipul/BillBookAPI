using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MyBillBook.Data;
using MyBillBook.Users;
using System.Collections.Concurrent;
using MailKit.Net.Smtp;
using System.Net.Mail;

namespace MyBillBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private static ConcurrentDictionary<string, string> otpCache = new ConcurrentDictionary<string, string>();
        private readonly IConfiguration configuration;
        public AuthController(ApplicationDbContext db,IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }
        [Route("Register")]
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            var user = new User { Email = model.Email , Name=model.Name};
            db.Users.Add(user);
            db.SaveChanges();

            return Ok("user registered successfully");
        }

        [Route("requestOtp")]
        [HttpPost]
        public IActionResult RequestOtp(OtpRequestModel model)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var otp = new Random().Next(100000, 999999).ToString();
            otpCache[user.Email] = otp;

            string subject = "Your OTP Code";
            string body = $"<p>Your OTP code is: <strong>{otp}</strong></p>";

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("BillBook",configuration["SmtpSettings:From"]));
            email.To.Add(new MailboxAddress(user.Name,user.Email));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                 smtp.Connect(configuration["SmtpSettings:Host"], int.Parse(configuration["SmtpSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                 smtp.Authenticate(configuration["SmtpSettings:UserName"], configuration["SmtpSettings:Password"]);
                 smtp.Send(email);
                 smtp.Disconnect(true);
            }
            return Ok("OTP sent to email");
        }
        [Route("verifyOtp")]
        [HttpPost]
        public IActionResult VerifyOtp(OtpVerificationModel model)
        {
            if(otpCache.TryGetValue(model.Email,out string validOtp) && validOtp == model.Otp)
            {
                otpCache.TryRemove(model.Email,out _);
                return Ok("OTP verified successfully");
            }
            return Unauthorized("Invalid or expired OTP");
        }

    }
}

using Application.Email;
using Microsoft.AspNetCore.Mvc;

namespace Real_Estate_Management_System.Controllers.AtomicControllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmailController(EmailService emailService) : ControllerBase
    {
        private readonly EmailService _emailService = emailService;

        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerificationEmail(string email)
        {
            string verificationLink = $"https://yourapp.com/verify?email={email}&token=your-token";

            string emailBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                    <table role='presentation' style='width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; padding: 20px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
                        <tr>
                            <td style='text-align: center;'>
                                <h2 style='color: #333333;'>Welcome to Real-Estate-Manager!</h2>
                                <p style='color: #666666; font-size: 16px;'>Thank you for registering with us. To complete your registration, please verify your email address by clicking the button below.</p>
                            </td>
                        </tr>
                        <tr>
                            <td style='text-align: center; padding-top: 20px;'>
                                <a href='{verificationLink}' style='background-color: #007bff; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; font-size: 16px; font-weight: bold;'>Verify Email Address</a>
                            </td>
                        </tr>
                        <tr>
                            <td style='text-align: center; padding-top: 20px; font-size: 14px; color: #888888;'>
                                <p>If you did not register for an account, you can ignore this email.</p>
                                <p style='margin-top: 10px;'>For any inquiries, please contact iustin.spiridon@com.</p>
                            </td>
                        </tr>
                    </table>
                </body>
            </html>";

            await _emailService.SendEmailAsync(email, "Complete Your Registration", emailBody);

            return Ok("Verification email sent.");
        }

    }

}

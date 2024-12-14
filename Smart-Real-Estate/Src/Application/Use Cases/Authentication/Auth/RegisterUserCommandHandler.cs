using Application.Email;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Authentication
{
    public class RegisterUserCommandHandler(IUserRepository repository,EmailService email) : IRequestHandler<RegisterUserCommand, Result<string>>
    {
        private readonly EmailService emailService = email;

        public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var token = repository.GenerateEmailConfirmationToken(request.Email, request.Username, request.Password);

            var emailSent = await SendConfirmationEmail(request.Email, token);
            if (!emailSent)
            {
                return Result<string>.Failure("Failed to send confirmation email.");
            }

            return Result<string>.Success("Registration initiated. Please check your email to confirm.");
        }


        private async Task<bool> SendConfirmationEmail(string email, string token)
        {
            try
            {
                var confirmationLink = $"http://localhost:4200/email-verification?token={token}";

                string emailBody = $@"
        <html>
            <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                <table role='presentation' style='width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; padding: 20px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
                    <tr>
                        <td style='text-align: center;'>
                            <h2 style='color: #333333;'>Welcome to Smart-Real-Estate!</h2>
                            <p style='color: #666666; font-size: 16px;'>Thank you for registering with us. To complete your registration, please confirm your email address by clicking the button below.</p>
                        </td>
                    </tr>
                    <tr>
                        <td style='text-align: center; padding-top: 20px;'>
                            <a href='{confirmationLink}' style='background-color: #007bff; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; font-size: 16px; font-weight: bold;'>Confirm Email Address</a>
                        </td>
                    </tr>
                    <tr>
                        <td style='text-align: center; padding-top: 20px; font-size: 14px; color: #888888;'>
                            <p>If you did not register for an account, you can ignore this email.</p>
                            <p style='margin-top: 10px;'>For any inquiries, please contact iustin.spiridon@gmail.com.</p>
                        </td>
                    </tr>
                </table>
            </body>
        </html>";

                await emailService.SendEmailAsync(email, "Confirm Your Email Address", emailBody);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }



    }
}

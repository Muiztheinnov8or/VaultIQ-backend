using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration; // Needed to read appsettings
using System.Threading.Tasks; // Needed for async/await

public class EmailService
{
    // This variable will hold your secret key
    private readonly string _apiKey;

    // This is the "Constructor". It runs when the service is created.
    // It reads the configuration (appsettings) and finds your key.
    public EmailService(IConfiguration configuration)
    {
        // This line gets the key from your appsettings.Development.json
        // and stores it safely in the _apiKey variable.
        _apiKey = configuration["SendGridKey"];
    }

    // This is the main function you will call to send the email
    public async Task SendOtpEmailAsync(string userEmail, string userName, string otpCode)
    {
        // 1. Create the SendGrid client with your secret API key
        var client = new SendGridClient(_apiKey);

        // 2. Set up the "From" address
        // IMPORTANT: This email MUST be the "Single Sender" email
        // you verified in your SendGrid account.
        var from = new EmailAddress("your-verified-email@example.com", "Your Hackathon Project");

        // 3. Set up the "To" address (this is your new user)
        var to = new EmailAddress(userEmail, userName);

        // 4. Set the email content
        var subject = "Your Verification Code";
        var plainTextContent = $"Hello {userName}, your verification code is: {otpCode}";
        var htmlContent = $"<strong>Hello {userName},</strong><p>Your verification code is: <strong>{otpCode}</strong></p><p>Welcome to our hackathon project!</p>";

        // 5. Create the final email message
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        // 6. Send the email!
        // We use 'await' to wait for SendGrid to confirm it's sent.
        var response = await client.SendEmailAsync(msg);

        // Optional: You can check if it worked
        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            // If it failed, you could log an error here
        }
    }
}

namespace CityInfo.API.Service
{
    public class LocalMailService: IMailService
    {
        private readonly string _mailTo;
        private readonly string _mailFrom;

        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"] = null!;
            _mailFrom = configuration["mailSettings:mailFromAddress"] = null!;
        }

        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_mailTo} to {_mailFrom},\n with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
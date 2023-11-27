namespace CityInfo.API.Service
{
    public class CloudMailService: IMailService
    {
        private readonly string _mailTo;
        private readonly string _mailFrom;

        public CloudMailService(IConfiguration configuration) // Allows reading from a configuration file, in this case it's the appsettings.json file because that's already registered from calling builder
        {
            _mailTo = configuration["mailSettings:mailToAddress"] = null!;
            _mailFrom = configuration["mailSettings:mailFromAddress"] = null!;
        }

        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_mailTo} to {_mailFrom},\n with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
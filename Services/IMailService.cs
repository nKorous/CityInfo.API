namespace CityInfo.API.Service
{
    public interface IMailService
    {
        public void Send(string subject, string message);
    }
}
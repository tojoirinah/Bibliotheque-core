namespace Bibliotheque.Services.Contracts
{
    public interface ILoggerService
    {
        void SetError(string error);
        void SetDebug(string text);
    }
}

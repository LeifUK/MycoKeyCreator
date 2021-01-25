namespace MycoKeys.WebApplication.Services
{
    public interface IKeyManagerFactory
    {
        MycoKeys.Library.Database.IKeyManager GetKeyManager();
    }
}

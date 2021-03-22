namespace MycoKeyMaker.WebApplication.Services
{
    public interface IKeyManagerFactory
    {
        MycoKeyMaker.Library.Database.IKeyManager GetKeyManager();
    }
}

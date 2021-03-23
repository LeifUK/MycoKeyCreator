namespace MycoKeyCreator.WebApplication.Services
{
    public interface IKeyManagerFactory
    {
        MycoKeyCreator.Library.Database.IKeyManager GetKeyManager();
    }
}

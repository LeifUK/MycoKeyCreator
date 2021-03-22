namespace MycoKeys.Application.ViewModel
{
    public interface IImporter
    {
        void Import(Library.Database.IKeyManager targetKeyManager);
    }
}

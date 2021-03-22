namespace MycoKeyMaker.Application.ViewModel
{
    public interface ISerialise
    {
        void Import(Library.Database.IKeyManager targetKeyManager);
        void Export(Library.Database.IKeyManager targetKeyManager);
    }
}

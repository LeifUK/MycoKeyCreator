namespace MycoKeys.Application.Model
{
    public delegate void OnChangedHandler(ISpeciesAttributeValue sender);

    public interface ISpeciesAttributeValue
    {
        string Title { get; }
        bool IsUsed { get; set; }
        object Value { get; set; }

        event OnChangedHandler OnChanged;
    }
}

namespace MycoKeys.Application.Model
{
    public delegate void OnChangedHandler(ISpeciesAttributeValueModel sender);

    public interface ISpeciesAttributeValueModel
    {
        string Title { get; }
        bool IsUsed { get; set; }
        object Value { get; set; }

        event OnChangedHandler OnChanged;
    }
}

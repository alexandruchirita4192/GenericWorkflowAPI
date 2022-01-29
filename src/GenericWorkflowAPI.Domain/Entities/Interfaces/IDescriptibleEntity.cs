namespace GenericWorkflowAPI.Domain.Entities
{
    public interface IDescriptibleEntity : ICodeEntity
    {
        string? Name { get; set; }
        string? Description { get; set; }
    }
}
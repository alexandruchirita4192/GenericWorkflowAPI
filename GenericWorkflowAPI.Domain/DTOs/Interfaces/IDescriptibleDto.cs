namespace GenericWorkflowAPI.Domain.DTOs
{
    public interface IDescriptibleDto : ICodeDto
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}

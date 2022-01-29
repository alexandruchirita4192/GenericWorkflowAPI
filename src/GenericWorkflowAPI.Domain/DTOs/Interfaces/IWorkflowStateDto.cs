namespace GenericWorkflowAPI.Domain.DTOs
{
    public interface IWorkflowStateDto : IBaseDto
    {
        string? StateCode { get; set; }
    }
}
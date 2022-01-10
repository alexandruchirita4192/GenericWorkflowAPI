namespace GenericWorkflowAPI.Domain.DTOs
{
    public interface IWorkflowDto : IBaseDto
    {
        string WorkflowCode { get; set; }
    }
}
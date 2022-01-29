namespace GenericWorkflowAPI.Domain.DTOs
{
    public interface IWorkflowInstanceDto : IBaseDto
    {
        public string? InstanceCode { get; set; }
    }
}
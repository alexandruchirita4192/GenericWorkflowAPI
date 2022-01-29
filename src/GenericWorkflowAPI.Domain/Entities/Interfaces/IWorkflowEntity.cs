namespace GenericWorkflowAPI.Domain.Entities
{
    public interface IWorkflowEntity : IBaseEntity
    {
        public long? WorkflowId { get; set; }
        public Workflow? Workflow { get; set; }
    }
}
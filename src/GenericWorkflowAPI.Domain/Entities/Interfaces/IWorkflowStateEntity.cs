namespace GenericWorkflowAPI.Domain.Entities
{
    public interface IWorkflowStateEntity : IBaseEntity
    {
        public long? StateId { get; set; }
        public WorkflowState? State { get; set; }
    }
}
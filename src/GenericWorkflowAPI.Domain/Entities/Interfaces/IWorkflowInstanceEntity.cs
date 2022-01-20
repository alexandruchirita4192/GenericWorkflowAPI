namespace GenericWorkflowAPI.Domain.Entities
{
    public interface IWorkflowInstanceEntity : IBaseEntity
    {
        public long? InstanceId { get; set; }
        public WorkflowInstance Instance { get; set; }
    }
}
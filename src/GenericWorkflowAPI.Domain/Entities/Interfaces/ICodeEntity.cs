namespace GenericWorkflowAPI.Domain.Entities
{
    public interface ICodeEntity : IBaseEntity
    {
        public string? Code { get; set; }
    }
}
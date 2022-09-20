using System;

namespace GenericWorkflowAPI.Domain.DTOs
{
    [Flags]
    public enum SqlType
    {
        SqlServer = 1,
        Sqlite = 2
    }
}
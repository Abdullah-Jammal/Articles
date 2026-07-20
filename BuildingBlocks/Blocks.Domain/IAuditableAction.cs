namespace Blocks.Domain;

public interface IAuditableAction
{
    DateTime CreatedOn { get; }
    int CreatedById { get; set; }
}

public interface ICurrentUser
{
    int UserId { get; }
}

public interface IAuditableAction<TActionType> : IAuditableAction
    where TActionType : Enum
{
    TActionType ActionType { get; }
}

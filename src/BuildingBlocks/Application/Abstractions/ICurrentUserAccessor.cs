namespace TaskHub.BuildingBlocks.Application.Abstractions;

// يرجّع الـ UserId بتاع المستخدم الحالي (من الـ JWT) لأي module
public interface ICurrentUserAccessor
{
    Guid? UserId { get; }
}

using MediatR;
namespace TaskHub.BuildingBlocks.Application.Abstractions.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}

using Sdk.Domain.Common;
using System.Threading.Tasks;

namespace Sdk.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}

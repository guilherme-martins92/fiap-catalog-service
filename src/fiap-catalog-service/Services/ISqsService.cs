using fiap_catalog_service.Models;

namespace fiap_catalog_service.Services
{
    public interface ISqsService
    {
        Task SendMessageAsync(string eventType, Vehicle vehicle);
    }
}

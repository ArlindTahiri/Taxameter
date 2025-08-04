using Microsoft.AspNetCore.SignalR;
using Taxameter.Servies;

namespace Taxameter.Hubs
{
    public class TaxameterHub : Hub
    {
        private readonly TaxameterService _service;

        public TaxameterHub(TaxameterService service)
        {
            _service = service;
        }

        public async Task SendLocation(double lat, double lng, string timestamp)
        {
            var parsedTime = DateTime.Parse(timestamp);
            await _service.AddLocation(lat, lng, parsedTime);

            await Clients.All.SendAsync("LocationUpdated", lat, lng, timestamp);
        }
    }

}

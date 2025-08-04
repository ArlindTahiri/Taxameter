using Microsoft.AspNetCore.SignalR;
using System.Timers;
using Taxameter.Hubs;

namespace Taxameter.Servies
{
    public class TaxameterService
    {
        private readonly IHubContext<TaxameterHub> _hubContext;
        private bool _isRunning = false;

        private decimal kilometerFare = 0;
        private decimal timeFare = 0;
        private decimal _fare => kilometerFare + timeFare;

        private (double lat, double lng)? _lastLocation;

        private System.Timers.Timer? _timer;

        private double _kilometerDriven = 0;
        private double _timeDrivenMinutes = 0;


        public TaxameterService(IHubContext<TaxameterHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;

            _timer = new System.Timers.Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            _timer.Elapsed += (s, e) => OnTimeElapsed();
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;

            _ = BroadcastFare();
        }

        public async Task Reset()
        {
            if (_isRunning) return;

            kilometerFare = 0;
            timeFare = 0;
            _lastLocation = null;
            _kilometerDriven = 0;
            _timeDrivenMinutes = 0;

            await BroadcastFare();
        }

        // 50 cent per Minute
        private async void OnTimeElapsed()
        {
            timeFare += 0.5m;
            _timeDrivenMinutes += 1;
            await BroadcastFare();
        }

        // 2€ per kilometer
        public async Task AddLocation(double lat, double lng, DateTime timestamp)
        {
            if (!_isRunning) return;

            if (_lastLocation != null)
            {
                var distanceKm = GetDistanceKm(_lastLocation.Value.lat, _lastLocation.Value.lng, lat, lng);
                kilometerFare += (decimal)(distanceKm * 2.00); // z.B. 2 €/km
                _kilometerDriven += distanceKm;
                await BroadcastFare();
            }

            _lastLocation = (lat, lng);
        }

        private async Task BroadcastFare()
        {
            var fareInfo = new
            {
                Fare = _fare,
                KilometerDriven = _kilometerDriven,
                TimeDrivenMinutes = _timeDrivenMinutes
            };

            await _hubContext.Clients.All.SendAsync("FareUpdated", fareInfo);
        }


        private double GetDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the Earth in km
            var dLat = Math.PI / 180 * (lat2 - lat1);
            var dLon = Math.PI / 180 * (lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(Math.PI / 180 * lat1) * Math.Cos(Math.PI / 180 * lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}

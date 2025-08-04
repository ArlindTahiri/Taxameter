let map;
let marker;

export function initMap() {
    map = L.map('map').setView([51.505, -0.09], 13); // Startposition

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
    }).addTo(map);

    marker = L.marker([51.505, -0.09]).addTo(map)
        .bindPopup('Startposition')
        .openPopup();
}

export function updateMarker(lat, lng) {
    if (!marker) return;
    marker.setLatLng([lat, lng]);
    marker.getPopup().setContent(`Aktuelle Position: ${lat.toFixed(5)}, ${lng.toFixed(5)}`);
    marker.openPopup();
    map.setView([lat, lng]);
}

let connection = null;
let isOnline = navigator.onLine;

// localStorage key für gepufferte GPS-Daten
const BUFFER_KEY = "taxameter_gps_buffer";

// Init Verbindung zu SignalR Hub
async function initConnection() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/taxameterHub")
        .build();

    connection.onclose(() => {
        console.warn("SignalR disconnected");
        isOnline = false;
    });

    try {
        await connection.start();
        console.log("SignalR connected");
        isOnline = true;
        await flushBuffer(); // Puffer senden
    } catch (e) {
        console.error("SignalR connection failed", e);
        isOnline = false;
    }
}

// GPS-Daten speichern (Puffer in localStorage)
function saveGpsLocation(position) {
    const coords = {
        lat: position.coords.latitude,
        lng: position.coords.longitude,
        time: new Date().toISOString()
    };

    const buffer = JSON.parse(localStorage.getItem(BUFFER_KEY) || "[]");
    buffer.push(coords);
    localStorage.setItem(BUFFER_KEY, JSON.stringify(buffer));

    console.log("GPS logged:", coords);

    if (isOnline) {
        flushBuffer();
    }
}

// Puffer an Server senden
async function flushBuffer() {
    if (!connection || connection.state !== "Connected") return;

    const buffer = JSON.parse(localStorage.getItem(BUFFER_KEY) || "[]");
    if (buffer.length === 0) return;

    try {
        for (const item of buffer) {
            await connection.invoke("SendLocation", item.lat, item.lng, item.time);
        }
        console.log(`🛰️ Sent ${buffer.length} buffered points`);
        localStorage.removeItem(BUFFER_KEY);
    } catch (e) {
        console.warn("Failed to flush buffer", e);
    }
}

let gpsInterval = null;

function startGpsTracking() {
    if (!navigator.geolocation) {
        alert("GPS not supported.");
        return;
    }

    gpsInterval = setInterval(() => {
        navigator.geolocation.getCurrentPosition(saveGpsLocation, err => {
            console.error("GPS error:", err);
        }, {
            enableHighAccuracy: true,
            maximumAge: 0,
            timeout: 10000
        });
    }, 5000); // alle 5 Sekunden
}

// Online-/Offline-Status behandeln
window.addEventListener("online", () => {
    isOnline = true;
    console.log("Back online");
    initConnection();
});

window.addEventListener("offline", () => {
    isOnline = false;
    console.warn("Offline mode activated");
});

// Haupt-Initialisierung
function initTaxameterClient() {
    initConnection();
    startGpsTracking();
}


function stopTaxameterClient() {
    stopGpsTracking();
    stopConnection();
}

function stopGpsTracking() {
    if (gpsInterval !== null) {
        clearInterval(gpsInterval);
        gpsInterval = null;
        console.log("GPS tracking stopped");
    }
}

function stopConnection() {
    if (connection && connection.state === "Connected") {
        connection.stop().then(() => {
            console.log("SignalR connection stopped");
        }).catch(e => {
            console.error("Error stopping connection", e);
        });
    }
}

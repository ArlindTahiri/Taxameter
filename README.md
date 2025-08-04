# 🧭 Digital Taximeter Web Application

A small web application that digitally simulates a taximeter using GPS.

⚠️ This project was developed in ~3 hours as a **proof of concept** and contains only the most essential features. It is **not production-ready**.

---

## 🚀 Features

- 📍 Live GPS tracking of the driver's position  
- 💰 Real-time display of:
  - Total fare  
  - Distance driven (km)  
  - Time driven (min)  
- 🧮 Server-side fare calculation based on distance & duration  
- 🔄 Live data updates via SignalR  
- 🧪 Minimal server-side API with the following endpoints:
  - `Start`
  - `Stop`
  - `Reset`

---

## 🔗 Views / Pages

- 👤 **Customer View**: `/`  
  → Displays fare, distance, time, and a live map

- 🚗 **Driver View**: `/control`  
  → Sends GPS data to the backend

> ⚠️ **Note**: You must **allow location tracking** in your browser when visiting `/control` for it to work correctly.

---

## 🔒 Limitations (Proof of Concept Only)

- 🧍 Designed for **only one user/session** at a time  
  → If you plan to use this for multiple drivers/customers, the architecture would need significant changes (e.g., driver IDs, sessions, etc.)

- 🛂 **No authentication** is implemented  
  → Anyone who knows the `/control` route can act as a driver. In a real implementation, access to this view should be **authenticated and restricted**.

- 🗃️ **No persistent data storage**  
  → Ride history and fare data are **not saved**.  
  In a production-grade app, all data should be stored in a database for **traceability, billing, and legal compliance**.

- 🎯 Built purely for **learning/fun purposes** and quick testing with friends  
  → Not intended for real-world deployment or commercial use

---

## 🛠 Tech Stack

- **Frontend**: Blazor Server (.NET)
- **Backend**: ASP.NET Core + SignalR
- **Map Integration**: Leaflet.js (via JS interop)
- **Styling**: Bootstrap 5

---

## ▶️ Getting Started

1. Clone the repository  
2. Open in Visual Studio or your preferred .NET IDE  
3. Restore NuGet packages  
4. Run the project  
5. Navigate to:
   - `/` for customer view  
   - `/control` for driver view (with location access)

---

## 📝 License

This project has no license and is intended for educational and personal testing only.

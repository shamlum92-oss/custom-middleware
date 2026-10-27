# ASP.NET Core Custom Request Profiler & Security Middleware

A production-grade, custom middleware implementation for **.NET 10 / ASP.NET Core Minimal APIs**. This project demonstrates request lifecycle management, bi-directional HTTP pipeline inspection, performance tracking, dynamic configuration-driven authentication, and short-circuit mechanics.

---

## 🛠️ Architectural Highlights

* **Pipeline Short-Circuiting:** Intercepts incoming requests and validates API key headers (`X-Api-Key`) against `appsettings.json` early in the execution pipeline, rejecting unauthorized requests with a `401 Unauthorized` before reaching endpoint routing.
* **Bi-Directional Request Profiling:** Captures incoming request details (HTTP Method, Path) and logs outbound metrics (Status Code, Execution Duration in milliseconds).
* **Distributed Tracing:** Generates a unique 8-character `X-Trace-Id` per request, injecting it into both application logs and outbound HTTP response headers for end-to-end telemetry.
* **Clean Extension Pattern:** Implements an `IApplicationBuilder` extension method (`UseRequestProfiler()`) to keep `Program.cs` clean and modular.

---

## 📁 Repository Structure

```text
custom-middleware/
├── CUSTOMMIDDLEWARE/
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Program.cs                  # Minimal API setup & middleware registration
│   ├── RequestProfilerMiddleware.cs # Core middleware logic & telemetry
│   ├── MiddlewareExtensions.cs     # IApplicationBuilder extension method
│   ├── appsettings.json            # Security & logging configuration
│   └── CUSTOMMIDDLEWARE.csproj     # Project file (.NET 10)
├── CUSTOMMIDDLEWARE.sln
└── README.md

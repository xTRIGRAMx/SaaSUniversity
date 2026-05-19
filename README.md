SaaS University – Blazor WASM Course Enrollment App
======================================================
Overview
SaaS University is a Blazor WebAssembly application that allows students to:

  Register and log in securely using JWT authentication.

  View available courses and classes.

  Enroll and deregister from courses.

  See their personalized “My Courses” dashboard.

This project demonstrates a SaaS‑style architecture with modular components, authentication, and state management.
======================================================
Features
  Authentication: JWT‑based login with secure token storage.

  User State: Persistent login state across refreshes using local storage.

  Course Management: Students can enroll/deregister in courses.

  My Courses Page: Displays all enrolled courses and classes dynamically.

  Logout: Clears local storage and resets user state instantly.

Seed Data: Preloaded students, courses, and classes for testing.

Tech Stack
======================================================
Frontend: Blazor WebAssembly

Backend: ASP.NET Core Web API

Database: EF Core (InMemory Database)

Authentication: JWT

State Management: Scoped UserState service + Local Storage


Prerequisites
.NET 8 SDK
Visual Studio / VS Code
============================

SETUP:
Run the backend API:
dotnet run --project SaaSUniversity.Server
Run the Blazor client:
dotnet run --project SaaSUniversity.Client
==========================================



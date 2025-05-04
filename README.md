# Course Management

This project demonstrates one way to implement a software system using Domain-Driven Design (DDD) and Event Sourcing principles with the help of [Marten](https://martendb.io/) as the event store. The architecture follows the Onion Architecture pattern to promote separation of concerns and maintainability. This project uses Aspire to simplify local development and comes with a simple Blazor frontend for demonstration purposes.

## Disclaimer

This project is intended for educational purposes and to provide a practical example of some architectural concepts. It is **not** meant to serve as a one-size-fits-all blueprint. The design choices and patterns shown here should be carefully evaluated and adapted to fit the unique requirements and constraints of your own projects. 

## Prerequisites

- .NET 9.0 SDK or later
- Container runtime such as Docker or Podman (required for Aspire, the embedded PostgreSQL database)

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/andrefroehlich/CourseManagement.git
   ```
2. Make sure Docker or Podman is running in the background.
3. Restore dependencies and build the project:
   ```bash
   dotnet restore
   dotnet build
   ```
4. Run the project:
   ```bash
   dotnet run --project 3_Infrastructure/CourseManagement.Infrastructure.AspireAppHost
   ```
5. Use the Aspire dashboard to access the application with the shown URL. 

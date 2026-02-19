# TimeToWork - Service Booking Management System

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-7.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-7.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

An educational service booking management system project, developed with emphasis on proper database architecture, adherence to normalization principles, and work with Entity Framework ORM.

## About the Project

This is an old university project, completed 3-4 years ago (2021-2022) at university as part of the **"Databases"** discipline. The main goal of the project is to demonstrate knowledge and skills in:

- **Database Architecture Design** — creating logical and physical data models
- **Database Normalization** — bringing the structure to third normal form (3NF)
- **Working with Entity Framework Core** — using ORM for database interaction
- **Implementing Relationships** — One-to-Many, Many-to-Many through intermediate tables
- **CRUD Operations** — creating a fully functional web application with basic operations

> The project was developed on .NET 7, which is no longer supported, because it was made a long time ago.

## Database Structure

### Main Entities

#### 1. **Client** (Clients)
```csharp
- ID (Primary Key)
- FirstName
- LastName
- PhoneNumber
- Appointments (Navigation Property)
```

#### 2. **Service** (Services)
```csharp
- ServiceId (Primary Key)
- ServiceName
- ShortDescription
- Description
- Price
- ExecutionTimeHours
- ExecutionTimeMinutes
- Appointments (Navigation Property)
- ServiceAssignments (Navigation Property)
```

#### 3. **ServiceProvider** (Service Providers / Employees)
```csharp
- ID (Primary Key)
- FirstName
- LastName
- HireDate
- PlaceOfWorkID (Foreign Key)
- PlaceOfWork (Navigation Property)
- ServiceAssignments (Navigation Property)
- Appointments (Navigation Property)
```

#### 4. **PlaceOfWork** (Workplaces)
```csharp
- PlaceOfWorkID (Primary Key)
- Location
- ServiceProviders (Navigation Property)
```

#### 5. **Appointment** (Service Appointments)
```csharp
- AppointmentId (Primary Key)
- ClientId (Foreign Key)
- ServiceId (Foreign Key)
- ServiceProviderId (Foreign Key)
- Date
- Navigation Properties for relationships
```

#### 6. **ServiceAssignment** (Service Assignment to Employees)
```csharp
- ServiceProviderId (Composite Key)
- ServiceId (Composite Key)
- Navigation Properties
```

#### 7. **Done** (Completed Services History)
```csharp
- DoneId (Primary Key)
- ClientId (Foreign Key)
- ServiceId (Foreign Key)
- ServiceProviderId (Foreign Key)
- Date
```

### Relationships Between Tables

```
PlaceOfWork (1) ──→ (N) ServiceProvider
Client (1) ──→ (N) Appointment
Service (1) ──→ (N) Appointment
ServiceProvider (1) ──→ (N) Appointment
ServiceProvider (N) ←──→ (N) Service (through ServiceAssignment)
```

### Normalization

The database is normalized to **third normal form (3NF)**:
- ✅ **1NF:** All attributes are atomic, no repeating groups
- ✅ **2NF:** No partial dependency on the key
- ✅ **3NF:** No transitive dependency

The intermediate table **ServiceAssignment** implements a Many-to-Many relationship between employees and the services they can provide.

## Technologies

- **ASP.NET Core MVC 7.0** — Framework for creating web applications
- **Entity Framework Core 7.0.4** — ORM for database work
- **SQL Server** — Database management system
- **Razor Pages** — Template engine for views
- **Bootstrap** — UI styling
- **Code First Migrations** — Approach to creating database schema

## Main Functionality

- 📋 **Client Management** — adding, editing, viewing and deleting clients
- 💼 **Service Management** — service catalog with descriptions, prices and execution time
- 👥 **Employee Management** — accounting of service providers and their workplaces
- 📅 **Appointment System** — creating and tracking client appointments for services
- 🏢 **Workplaces** — managing locations where services are provided
- ✅ **Completion History** — accounting of completed appointments
- 🔗 **Service Assignment** — connection between employees and services they provide

## Local Setup

### Prerequisites
- .NET 7 SDK (or newer)
- SQL Server (LocalDB or full version)
- Visual Studio 2022 or Rider (optional)

### Steps to Run

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/TimeToWork.git
cd TimeToWork
```

2. **Restore NuGet packages**
Packages are already specified in `TimeToWork.csproj` and will be installed automatically on first run, but you can explicitly restore them:

```bash
cd TimeToWork
dotnet restore
```

3. **Configure Connection String** (optional)
If you need to change the database connection, edit `TimeToWork/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "TimeToWorkContext": "Server=(localdb)\\mssqllocaldb;Database=TimeToWork.Data;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

4. **Apply migrations**

#### Option A: Through CLI (command line / terminal)
```bash
cd TimeToWork
dotnet ef database update
```

#### Option B: Through Package Manager Console (Visual Studio)
1. Open menu **Tools → NuGet Package Manager → Package Manager Console**
2. Make sure `TimeToWork` is selected as the default project in the dropdown
3. Enter the command:
```powershell
Update-Database
```

> **Note:** EF Core Tools are already included in the project as a dependency (`Microsoft.EntityFrameworkCore.Tools`), so no additional installation is required.

5. **Run the application**
```bash
dotnet run
```

## Project Structure

```
TimeToWork/
├── Controllers/         # MVC controllers
├── Models/              # Data models (entities)
├── Views/               # Razor views
├── Data/                # DbContext and EF Core configuration
├── Migrations/          # Entity Framework migrations
├── wwwroot/             # Static files (CSS, JS, images)
└── Program.cs           # Application entry point
```

## Learning Objectives Achieved

✅ Designing a normalized database  
✅ Implementing complex relationships between tables  
✅ Working with Entity Framework Core (Code First approach)  
✅ Using migrations for database schema versioning  
✅ CRUD operations using ORM  
✅ Scaffolding controllers and views  
✅ Working with Navigation Properties and Lazy/Eager Loading  
✅ Data validation through Data Annotations  
✅ Creating composite primary key (Composite Key)  

## Screenshots

### Appointment Management
![Appointment List](screenshots/appointment_screen.png)
*Viewing all service appointments*

![Create Appointment](screenshots/appointment_create_screen.png)
*Creating a new service appointment*

![Advanced Appointment Creation](screenshots/appointment_create2_screen.png)
*Appointment creation form with client and service selection*

![Edit Appointment](screenshots/appointment_edit_screen.png)
*Editing an existing appointment*

![Delete Appointment](screenshots/appointment_delete_screen.png)
*Appointment deletion confirmation*

### Client Management
![Client List](screenshots/clients_screen.png)
*Viewing all system clients*

![Client Details](screenshots/clients_details_screen.png)
*Detailed information about client and their appointments*

### Service Management
![Service List](screenshots/service_screen.png)
*Catalog of all available services*

![Create Service](screenshots/service_create_screen.png)
*Adding a new service to the system*

![Edit Service](screenshots/service_edit_screen.png)
*Editing service information*

![Service Details](screenshots/service_details_screen.png)
*Detailed service view*

### Employee Management
![Provider List](screenshots/executor_screen.png)
*Viewing all service providers*

![Create Provider](screenshots/executor_create_screen.png)
*Adding a new employee*

![Edit Provider](screenshots/executor_edit_screen.png)
*Editing employee information*

![Provider Details](screenshots/executor_details_screen.png)
*Detailed information about employee and their services*

## Author

**Denys Zaidun**  
- GitHub: [dzaidun](https://github.com/dzaidun)
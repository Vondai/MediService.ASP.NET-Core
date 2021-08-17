# MediService
Subscription based platform for medical services at home.🏡 Users can make appointments with specialists.🗓️

Web Project for SoftUni ASP.NET Core course (August 2021). 🚀

## :large_blue_circle: Specification
- Guests may:
  - View the home page with recent reviews.
  - Read about the medical services.
  - Register as a user.
  - Open and read the faq.
  - Review all the specialists and their services.
  - Check subscription plans.
- Logged in users may:
  - Make an appointment for a free service from the home page.
  - Subscribe to the platform.
  - Write a review.
- Subscribers may:
  - Based on their subscription plan make multiple appointments.
  - Cancel appointment.
  - Review past appointments and their status.
- Specialists may:
  - Finish appointments.
  - See details about an appointment.
- Administrator
  - Add specialist, service and subscription plan.
  - Edit services (may make a service paid or free).
  - View statistics about the platform.
- ✨Additional features:
   - System will automatically mark all unfinished appointments as finished after their scheduled date and time.
   - Memory cache in several pages.

## Application Screenshots 📸

### Home Page
![Home Page](https://github.com/Vondai/MediService.ASP.NET-Core/blob/master/ReadmeImg/HomeUser.PNG)

### Specialists Page
![Specialists Page](https://github.com/Vondai/MediService.ASP.NET-Core/blob/master/ReadmeImg/Specialists.PNG)

### Services Page
![Services Page](https://github.com/Vondai/MediService.ASP.NET-Core/blob/master/ReadmeImg/Services.PNG)

### Subscriptions Page
![Subscriptions Page](https://github.com/Vondai/MediService.ASP.NET-Core/blob/master/ReadmeImg/Subscriptions.PNG)

### My Appointments Page
![Appointments Page](https://github.com/Vondai/MediService.ASP.NET-Core/blob/master/ReadmeImg/MyAppointments.PNG)

### Statistics Page
![Statistics Page](https://github.com/Vondai/MediService.ASP.NET-Core/blob/master/ReadmeImg/Statistics.PNG)

## Technology Used :white_check_mark:

- ASP.NET Core 5
- ASP.NET Identity System
- Entity Framework (EF) Core 5
- Sorting, Filtering and Projecting with EF Core
- Microsoft SQL Server
- MVC Areas
- Dependency Injection
- Data Validation (Client-side and Server-side)
- Custom Validation Attributes
- Responsive Design
- Bootstrap
- MyTested.AspNetCore.Universe
- XUnit

## :point_down: Configuration

### Connection strings 
In the `appsettings.json` file. If you use SQLEXPRESS, you should replace `Server=.;` with `Server=.\\SQLEXPRESS;`

### Sample data
Seeders will automatically seed sample data in the database inluding:
  - Specialists
  - Services
  - User

Test accounts:
  - User:
    - Username: test1
    - Password: 123456
  - Specialist:
    - Username: johnMd
    - Password: 123456
  - Administrator:
    - Username: admin
    - Password: 123456


# La Grazia

**La Grazia** is a full-stack e-commerce web application built with
**ASP.NET Core MVC** and **Entity Framework Core**.\
It provides a complete online shopping experience with product
management, authentication, cart and wishlist functionality, order
processing, and an administrative dashboard.

------------------------------------------------------------------------

## Features

### User Features

-   User registration and login (ASP.NET Identity)
-   Product browsing and filtering
-   Shopping cart and wishlist
-   Checkout and order creation
-   Product reviews and ratings
-   Profile management and order history
-   Password reset via email

### Admin Features

-   Product CRUD management
-   Category, region, and variety management
-   Order status management
-   Review moderation
-   Blog, FAQ, and content management
-   Site settings and slider management

------------------------------------------------------------------------

## Tech Stack

**Backend**

-   ASP.NET Core MVC\
-   Entity Framework Core\
-   ASP.NET Identity\
-   C#

**Frontend**

-   Razor Views\
-   HTML / CSS / JavaScript\
-   Bootstrap\
-   jQuery

**Database**

-   MS SQL Server

**Other**

-   MailKit (Email service)\
-   Session & Cookies\
-   File upload handling

------------------------------------------------------------------------

## Architecture

The application follows a layered MVC architecture:

Controllers\
Services\
ViewModels\
Models\
Entity Framework\
SQL Server

------------------------------------------------------------------------

## Project Structure

    La-Grazia
    │
    ├── Controllers
    ├── Database
    │   ├── Models
    │   └── DataContext
    ├── Services
    ├── ViewModels
    ├── Infrastructure
    ├── Validators
    ├── Views
    ├── wwwroot
    └── appsettings.json

------------------------------------------------------------------------

## Setup

1.  Clone the repository

    git clone https://github.com/nqasanova/La-Grazia

2.  Configure the database connection in:

    appsettings.json

3.  Apply migrations

    dotnet ef database update

4.  Run the application

    dotnet run

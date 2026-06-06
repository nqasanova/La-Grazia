# La Grazia — Luxury Fragrance E-Commerce

A full-stack ASP.NET Core 6 MVC e-commerce application for a luxury perfume and fragrance brand. Features a polished customer-facing storefront and a comprehensive admin panel, built with ASP.NET Core Identity, Entity Framework Core, and SQL Server.

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [Architecture](#architecture)
- [Database Schema](#database-schema)
- [Client Storefront](#client-storefront)
- [Admin Panel](#admin-panel)
- [Services](#services)
- [Email Templates](#email-templates)
- [Frontend Assets](#frontend-assets)
- [Roles & Access Control](#roles--access-control)

---

## Overview

La Grazia is a luxury fragrance e-commerce platform built with ASP.NET Core 6 MVC. It supports the full shopping journey — product browsing, wishlisting, cart management, checkout, and order tracking — alongside a fully featured admin panel for managing every aspect of the store.

The application uses **ASP.NET Core Identity** for authentication and role-based access, a **dual-mode cart/wishlist** (cookie for guests, database for authenticated users), and **MailKit** for transactional email sending (order confirmation, registration, and password reset).

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 6 MVC |
| ORM | Entity Framework Core 7 |
| Database | SQL Server (MSSQL) |
| Authentication | ASP.NET Core Identity |
| Password Hashing | BCrypt.Net-Core |
| Email | MailKit 3.6.0 (SMTP) |
| Validation | FluentValidation 11.2.2 |
| JSON | Newtonsoft.Json 13.0.3 + System.Text.Json |
| Admin UI | Quix Admin Theme (Bootstrap-based) |
| Client UI | Custom storefront theme |

### NuGet Packages

```
Microsoft.AspNetCore.Identity.EntityFrameworkCore 6.0.15
Microsoft.AspNetCore.Identity.UI 6.0.15
Microsoft.EntityFrameworkCore 7.0.4
Microsoft.EntityFrameworkCore.SqlServer 7.0.4
Microsoft.EntityFrameworkCore.Tools 7.0.4
BCrypt.Net-Core 1.6.0
FluentValidation.AspNetCore 11.2.2
MailKit 3.6.0
Newtonsoft.Json 13.0.3
Microsoft.AspNetCore.Mvc.NewtonsoftJson 6.0.15
Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation 6.0.15
AspNetCore.IServiceCollection.AddIUrlHelper 1.1.0
```

---

## Project Structure

```
La-Grazia/
└── La-Grazia/
    └── La-Grazia/
        ├── Areas/
        │   └── Admin/
        │       ├── Controllers/        # Admin CRUD controllers
        │       ├── ViewModels/         # Admin-specific view models
        │       └── Views/              # Razor views for admin panel
        ├── Constants/
        │   └── RoleConstants.cs        # "Admin", "Moderator", "User" role strings
        ├── Controllers/                # Client-facing controllers
        ├── Database/
        │   ├── DataContext.cs          # EF Core DbContext with all 18 DbSets
        │   └── Models/
        │       ├── Enums/
        │       │   └── OrderStatus.cs  # Pending→Created→Approved→Sent→Completed│Rejected
        │       └── *.cs                # All entity models
        ├── Infastructure/
        │   ├── Configurations/         # DI, MVC, options configuration
        │   └── Extentions/             # IServiceCollection / IApplicationBuilder extensions
        ├── Migrations/                 # 13 EF Core migrations (all created 2023-03-31)
        ├── Services/
        │   ├── IMailService.cs         # Email service interface
        │   ├── MailService.cs          # MailKit SMTP implementation
        │   ├── EmailService.cs         # Secondary email helper
        │   └── LayoutService.cs        # Shared layout data (basket, wishlist, settings)
        ├── Validators/
        │   └── FileValidator.cs        # File save/delete utility (GUID-prefixed filenames)
        ├── ViewModels/                 # Client-facing view models
        ├── Views/                      # Client Razor views + shared partials
        ├── wwwroot/
        │   ├── Assets/
        │   │   ├── css/               # Per-page client stylesheets
        │   │   ├── images/            # All brand/product images
        │   │   └── js/               # Client-side JS (cart, wishlist, shop, index)
        │   ├── lib/                   # jQuery, Bootstrap, jQuery Validation
        │   ├── manage/                # Quix admin theme (CSS, JS, icons, images)
        │   └── templates/             # HTML email templates (Order, Register, ForgotPassword)
        ├── appsettings.json
        ├── Startup.cs
        └── Program.cs
```

---

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express / LocalDB)
- A Gmail account (for email functionality)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/nqasanova/La-Grazia
   cd La-Grazia/La-Grazia
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the application** — see [Configuration](#configuration) below.

4. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run**
   ```bash
   dotnet run --project La-Grazia
   ```

6. Visit `https://localhost:5001` in your browser.

---

## Configuration

Update `appsettings.json` with your own values:

```json
{
  "ConnectionStrings": {
    "NatavanMAC": "Server=localhost;Database=La-Grazia;Trusted_Connection=false;User=SA;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  },
  "MailSettings": {
    "Mail": "your-email@gmail.com",
    "DisplayName": "La Grazia",
    "Password": "your-app-password",
    "Host": "smtp.gmail.com",
    "Port": 587
  }
}
```

> **Gmail SMTP:** Use a [Gmail App Password](https://support.google.com/accounts/answer/185833), not your regular password. Two-factor authentication must be enabled on the Gmail account.

> **Connection string key:** The app reads `"NatavanMAC"` by name. Rename it and update `Startup.cs` if needed.

---

## Database Setup

The project uses **EF Core Code-First** with 13 migrations that were all created together, reflecting the full initial schema.

```bash
# Apply all migrations and create the database
dotnet ef database update

# To add a new migration after model changes
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Migration History

| Migration | Table(s) Created |
|---|---|
| `SlidersTable` | Sliders |
| `BlogsTable` | Blogs |
| `FaqsTable` | Faqs |
| `CollaborationsTable` | Collaborations |
| `AboutsTable` | Abouts |
| `TeamsTable` | Teams |
| `RegionsAndVarietiesTable` | Regions, Varieties |
| `ProductsTable` | Products |
| `ProductImagesTable` | ProductImages |
| `UsersTable` | AspNetUsers (Identity) |
| `TypesTableName` | Types |
| `SettingsTable` | Settings |
| `WishlistProductsTable` | WishlistProducts |

> Note: `BasketProducts` and `OrderedProducts` tables are defined in the DbContext but do not have their own dedicated migration entries — they are created as part of the Identity/products schema.

---

## Architecture

### Multi-Area MVC

The app is split into two ASP.NET Core Areas:

- **`Admin`** — Protected area for store management. Route: `/admin`
- **Client** (default) — Public storefront. Route: `/`

### Identity & Roles

Authentication is handled by **ASP.NET Core Identity** with a custom `User : IdentityUser` that adds `FullName` and `IsAdmin` fields.

Three roles are defined in `RoleConstants.cs`:

| Role | Description |
|---|---|
| `Admin` | Full access to all admin panel features |
| `Moderator` | Partial admin access |
| `User` | Standard customer account |

### Dual-Mode Cart & Wishlist

Both the cart and wishlist support two storage modes:

- **Guest users** — items stored as JSON-serialized cookies (`"Products"` and `"Wishlist"`)
- **Authenticated users** — items persisted in `BasketProducts` and `WishlistProducts` tables

The `LayoutService` handles reading from either source and hydrating product details, and is injected into the shared layout so the navbar cart/wishlist counters always reflect the correct state.

### Order Status Lifecycle

Orders progress through a `byte`-based enum:

```
Pending (0) → Created (1) → Approved (2) → Sent (8) → Completed (16)
                                         ↘ Rejected (4)
```

### File Uploads

`FileValidator` handles all image uploads: it generates a `GUID`-prefixed filename (truncating the original to 64 characters), saves to disk under `wwwroot`, and provides a delete method for cleanup.

---

## Database Schema

### Core Entities

| Table | Key Fields |
|---|---|
| `Products` | `Id`, `Name`, `Description`, `Price`, `SalePrice`, `IsFeatured`, `Rate`, `TypeId`, `RegionId`, `VarietyId` |
| `ProductImages` | `Id`, `Image` (filename), `ProductId` |
| `Types` | `Id`, `Name` — product type/category |
| `Regions` | `Id`, `Name` — fragrance region of origin |
| `Varieties` | `Id`, `Name` — fragrance variety/family |
| `Reviews` | `Id`, `Rate`, `Context`, `ProductId`, `UserId`, `CreatedAt`, `IsAccepted` |
| `Orders` | `Total`, `UserId`, `FullName`, `Email`, `Phone`, `Address`, `City`, `Country`, `ZipCode`, `Note`, `CreatedAt`, `Status` |
| `OrderedProducts` | Junction table linking `Order` ↔ `Product` |
| `BasketProducts` | `UserId`, `ProductId`, `Count` |
| `WishlistProducts` | `UserId`, `ProductId` |

### Content Entities

| Table | Purpose |
|---|---|
| `Sliders` | Homepage hero banner slides |
| `Blogs` | Blog posts |
| `Faqs` | FAQ entries |
| `Abouts` | About page content |
| `Teams` | Team member profiles |
| `Collaborations` | Brand partnership logos |
| `Features` | Homepage feature highlights |
| `Settings` | Site-wide settings (logo, social media URLs & icons) |

### Identity Tables (auto-generated by ASP.NET Core Identity)

`AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`, `AspNetUserClaims`, `AspNetUserLogins`, `AspNetUserTokens`, `AspNetRoleClaims`

---

## Client Storefront

### Pages & Routes

| Route | View | Description |
|---|---|---|
| `/` | `Home/Index` | Homepage — sliders, featured products, collaborations, blog preview |
| `/product` | `Product/Index` | Product catalog with filtering |
| `/product/{id}` | `Product/Detail` | Product detail with image gallery, reviews, add-to-cart |
| `/cart` | `Cart/Index` | Shopping cart with quantity management |
| `/order/checkout` | `Order/Checkout` | Checkout form (guest or pre-filled for logged-in users) |
| `/wishlist` | `Wishlist/Index` | Saved products |
| `/blog` | `Blog/Index` | Blog listing |
| `/blog/{id}` | `Blog/Detail` | Blog post detail |
| `/faq` | `FAQ/Index` | FAQ page |
| `/about` | `About/Index` | About page with team section |
| `/contact` | `Contact/Index` | Contact form |
| `/account/login` | `Account/Login` | Login (modal also available) |
| `/account/register` | `Account/Register` | Registration (modal also available) |
| `/account/profile` | `Account/Profile` | User profile and order history |
| `/account/resetpassword` | `Account/ResetPassword` | Password reset via email link |

### Shared Partials

| Partial | Description |
|---|---|
| `_Layout.cshtml` | Main layout with nav, header, footer |
| `_BasketPartial.cshtml` | Slide-in cart drawer |
| `_WishlistPartial.cshtml` | Slide-in wishlist drawer |
| `_LoginModalPartial.cshtml` | Login modal |
| `_RegisterModalPartial.cshtml` | Registration modal |
| `_ForgotModalPartial.cshtml` | Forgot password modal |
| `_ProductModalPartial.cshtml` | Quick-view product modal |
| `_SearchPartial.cshtml` | Search overlay |

### Client-Side JavaScript

| File | Responsibilities |
|---|---|
| `index.js` | Homepage interactions (sliders, hero effects) |
| `shop.js` | Product catalog filtering and sorting |
| `cart.js` | Add/remove/update cart items via AJAX; cookie sync for guests |
| `wishlist.js` | Add/remove wishlist items via AJAX; cookie sync for guests |
| `main.js` | Shared UI behaviours across all pages |

---

## Admin Panel

Access at `/admin`. Requires a user with `IsAdmin = true`.

### Admin Modules

| Module | Route | Operations |
|---|---|---|
| Dashboard | `/admin/dashboard` | Store overview and stats |
| Products | `/admin/product` | Create, Edit, Delete (with multi-image upload) |
| Orders | `/admin/order` | List all orders, update `OrderStatus` |
| Reviews | `/admin/review` | List, approve/reject, edit customer reviews |
| Blogs | `/admin/blog` | Create, Edit, Delete blog posts |
| FAQs | `/admin/faq` | Create, Edit, Delete FAQ entries |
| Sliders | `/admin/slider` | Create, Edit, Delete homepage hero slides |
| Team | `/admin/team` | Create, Edit, Delete team member profiles |
| Collaborations | `/admin/collaboration` | Create, Edit, Delete brand logo entries |
| About | `/admin/about` | Edit about page content |
| Types | `/admin/type` | Manage product types/categories |
| Regions | `/admin/region` | Manage fragrance regions of origin |
| Varieties | `/admin/variety` | Manage fragrance variety classifications |
| Admins | `/admin/admin` | Create new admin users |
| Roles | `/admin/role` | Create and assign Identity roles |
| Settings | `/admin/setting` | Edit site logo and social media links/icons |

### Admin ViewModels

- `DashboardViewModel` — aggregated store stats
- `AdminViewModel` — admin user creation form
- `LoginViewModel` — admin login form

---

## Services

### `MailService` / `IMailService`

Sends transactional HTML emails via **MailKit** over SMTP (configured for Gmail/StartTLS).

Supports:
- HTML body content
- File attachments (reads `IFormFile` attachments into byte arrays)
- Configured via `MailSetting` options object (bound from `appsettings.json`)

### `EmailService`

A secondary email helper service also registered in DI — handles specific email flows (registration confirmation, password reset).

### `LayoutService`

Injected into the shared `_Layout.cshtml` to supply:
- **`GetSetting()`** — retrieves the single `Setting` record (logo, social URLs)
- **`GetBasketProducts()`** — resolves basket from cookie (guest) or database (authenticated)
- **`GetWishlistItems()`** — resolves wishlist from cookie (guest) or database (authenticated)

---

## Email Templates

HTML email templates are stored in `wwwroot/templates/` and loaded by the email services at runtime:

| Template | Trigger |
|---|---|
| `Register.html` | Sent on successful customer registration |
| `Order.html` | Sent on order placement confirmation |
| `forgotpassword.html` | Sent when a user requests a password reset |

---

## Frontend Assets

### Client (`wwwroot/Assets/`)

- **CSS** — per-page stylesheets: `style.css` (global), `shop.css`, `cart.css`, `checkout.css`, `wishlist.css`, `blog.css`, `contact.css`, `faq.css`, `about.css`, `account.css`, `login.css`, `register.css`
- **JS** — `index.js`, `shop.js`, `cart.js`, `wishlist.js`, `main.js`
- **Images** — all brand and product images (fragrance photos, collaboration logos, team portraits, slider backgrounds)

### Admin (`wwwroot/manage/`)

Uses the **Quix Admin Template** — a Bootstrap-based admin theme with:
- Multiple colour themes (blue, green, orange, pink, purple, white — light and dark variants each)
- Material Design Icons
- DataTables, Chart.js, ApexCharts, and many other UI plugin initialisation scripts

---

## Roles & Access Control

| Area | Access |
|---|---|
| Admin panel (`/admin/*`) | Requires `IsAdmin == true` on the `User` entity |
| Customer account pages | Requires authentication (redirects to login modal) |
| Cart / Wishlist (read) | Available to guests (cookie) and authenticated users (DB) |
| Checkout | Available to guests (cookie data) and authenticated users |
| Review submission | Requires authentication |
| Public pages | No authentication required |

> **Note:** The `RoleConstants` defines `Admin`, `Moderator`, and `User` string constants for use with `[Authorize(Roles = "...")]` attributes. The `IsAdmin` boolean on `User` is additionally checked in several controllers to gate admin-level access.

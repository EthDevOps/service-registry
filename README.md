# QuokkaServiceRegistry

A comprehensive service catalog and vendor management system built with ASP.NET Core, designed to help organizations track, manage, and maintain their software services, vendors, and compliance requirements.

## 🚀 Features

### Core Functionality
- **Service Catalog Management** - Track and organize all your software services
- **Vendor Management** - Centralized vendor database with payment method integration
- **License Management** - Monitor software licenses and compliance requirements
- **GDPR Compliance Tracking** - Built-in GDPR compliance management and documentation
- **Lifecycle Management** - Track service lifecycle stages from conception to retirement
- **Payment Method Management** - Centralized payment methods shared across vendors

### Key Highlights
- **Modern Web Interface** - Bootstrap 5 responsive design with Font Awesome icons
- **Multi-Payment Support** - Credit Card, Bank Transfer, SEPA, and Prepaid payment methods
- **Cost Center Integration** - Link payments and services to organizational cost centers
- **Comprehensive GDPR Tools** - Data processing registers, controller information, and compliance tracking
- **Rich Data Models** - Detailed service hosting, subscription, and lifecycle information

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: PostgreSQL with Entity Framework Core
- **Frontend**: Razor Pages, Bootstrap 5, jQuery
- **Authentication**: ASP.NET Core Identity
- **Icons**: Font Awesome 6.0
- **Architecture**: MVC Pattern with Repository Pattern elements

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/) 12.0 or higher
- [Git](https://git-scm.com/downloads)

## 🚀 Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/QuokkaServiceRegistry.git
cd QuokkaServiceRegistry
```

### 2. Database Setup
1. Create a PostgreSQL database named `quokka_service_registry`
2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=quokka_service_registry;Username=your_username;Password=your_password"
  }
}
```

### 3. Run Database Migrations
```bash
dotnet ef database update
```

### 4. Start the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5211` or `http://localhost:5212`.

## 🏗️ Database Schema

### Core Entities
- **Services** - Central service catalog entries
- **Vendors** - Service providers and vendors
- **PaymentMethods** - Centralized payment method management
- **Licenses** - Software license tracking
- **CostCenters** - Organizational cost centers
- **GdprRegisters** - GDPR compliance data

### Key Relationships
- Services belong to Vendors
- Vendors can use centralized PaymentMethods
- PaymentMethods are linked to CostCenters
- Services have Licenses and Lifecycle information
- GDPR registers track data processing compliance

## 📱 User Interface

### Main Sections
1. **Service Catalog** - Browse and manage all services
2. **Vendors** - Vendor management with payment integration
3. **Payment Methods** - Centralized payment method management
4. **Licenses** - Software license catalog
5. **GDPR Compliance** - Data protection and compliance tools

### Payment Method Types
- **Credit Card** - Card holder name, number, expiry date
- **Bank Transfer** - Bank details, account numbers, SWIFT codes
- **SEPA** - SEPA mandate information and IBAN
- **Prepaid** - Voucher codes, balances, and expiry dates

## 🔧 Configuration

### Seed Data
The application automatically seeds:
- **Software Licenses** - Common open-source and proprietary licenses
- **Cost Centers** - IT, Marketing, Sales, Operations, Finance, HR departments

### Environment Variables
- `ASPNETCORE_ENVIRONMENT` - Development/Production environment
- `ConnectionStrings__DefaultConnection` - Database connection string

## 🔒 Security & Compliance

### GDPR Features
- **Data Processing Registers** - Track what data is processed and why
- **Controller Information** - Maintain data controller details
- **DPO Integration** - Data Protection Officer contact management
- **Compliance Notes** - Document compliance measures and assessments

### Security Measures
- ASP.NET Core Identity for authentication
- Secure password policies
- SQL injection prevention via Entity Framework
- XSS protection with Razor encoding
- CSRF protection with anti-forgery tokens

## 📊 Features Overview

### Service Management
- Service lifecycle tracking (Development → Testing → Production → Retirement)
- Hosting information (On-premise, Cloud, SaaS)
- Subscription management with seat counting
- Integration with vendor and license information

### Vendor Management
- Centralized payment method assignment
- Address and contact information
- GDPR processing agreement tracking
- Service usage statistics

### Payment Management
- Multi-type payment method support
- Cost center allocation
- Vendor payment method reuse
- Payment status tracking

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines
- Follow C# coding conventions
- Use Entity Framework for database operations
- Implement proper error handling
- Add appropriate validation
- Include unit tests for new features

## 📝 API Documentation

### Key Controllers
- **CatalogServiceController** - Service CRUD operations
- **CatalogVendorController** - Vendor management
- **PaymentController** - Payment method management
- **LicenseController** - License catalog management

### Data Models
- Rich domain models with proper validation
- Entity Framework navigation properties
- View models for complex forms
- Enum types for categorization

## 🚀 Deployment

### Production Deployment
1. Set `ASPNETCORE_ENVIRONMENT=Production`
2. Configure production database connection
3. Run `dotnet publish -c Release`
4. Deploy to your web server
5. Run database migrations in production

### Docker Support
```dockerfile
# Dockerfile example
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["QuokkaServiceRegistry.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuokkaServiceRegistry.dll"]
```

## 🆘 Troubleshooting

### Common Issues
1. **Database Connection Failed** - Check PostgreSQL service and connection string
2. **Migration Errors** - Ensure database exists and user has proper permissions
3. **Port Conflicts** - Change ports in `launchSettings.json` if 5211/5212 are in use

### Getting Help
- Check the [Issues](https://github.com/yourusername/QuokkaServiceRegistry/issues) page
- Review the application logs in the console
- Verify database connectivity and migrations

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- UI powered by [Bootstrap](https://getbootstrap.com/)
- Icons from [Font Awesome](https://fontawesome.com/)
- Database by [PostgreSQL](https://www.postgresql.org/)

## 📊 Project Status

- ✅ Core service catalog functionality
- ✅ Vendor management with payment integration
- ✅ GDPR compliance tracking
- ✅ License management
- ✅ Centralized payment methods
- 🚧 Advanced reporting features
- 🚧 API endpoints for external integration
- 🚧 Mobile responsive improvements

---

**QuokkaServiceRegistry** - Streamlining service catalog management for modern organizations.
<h1 align="center">🌞🌱 Agri-Energy Connect 🔋 🌞</h1>

<div align="center">
  
  ![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-green?style=for-the-badge&logo=dotnet)
  ![C#](https://img.shields.io/badge/C%23-13.0-purple?style=for-the-badge&logo=csharp)
  ![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-blue?style=for-the-badge&logo=microsoftsqlserver)
  ![Bootstrap](https://img.shields.io/badge/Bootstrap-5.0-blueviolet?style=for-the-badge&logo=bootstrap)
  
<h3>Where Sustainable Farming Powers a Greener Future</h3>

<p>Agri-Energy Connect bridges eco-conscious farmers with innovative renewable energy solutions for a cleaner, smarter tomorrow.</p>

</div>

##  Overview

Agri-Energy Connect is a web-based platform dedicated to uniting sustainable agriculture with green energy solutions across South Africa. It serves as a dynamic hub where farmers and energy experts can connect, share knowledge, access valuable resources, and collaborate on initiatives that promote eco-friendly farming practices and the adoption of renewable energy.

##  Key Features

- **User Authentication**: Secure login and role-based access control
- **Farmer Profiles**: Detailed farmer information and farm details
- **Product Marketplace**: Platform for farmers to showcase their sustainable agricultural products
- **Responsive Design**: Mobile-friendly interface using Bootstrap 5
- **Data Management**: Entity Framework Core with SQL Server backend

##  User Roles

The platform supports two primary user roles:

1. **Employee**: System administrators who can:
   - Manage farmer accounts and profiles
   - View all products across the platform
   - Access farmer details and performance metrics
   
2. **Farmer**: Agricultural producers who can:
   - Manage their profile and farm information
   - Add and manage their product listings
   - View products from other farmers

##  Technology Stack

- **Framework**: ASP.NET Core 9.0 MVC
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **Backend**: C#, Entity Framework Core
- **Database**: Microsoft SQL Server
- **Authentication**: Session-based authentication with password hashing
- **Styling**: Bootstrap Icons, Google Fonts (Montserrat, Poppins)

##  Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) (recommended) or [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express edition is sufficient for development)
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (or any SQL client)

##  Setup Instructions

### 1. Clone the Repository

bash
git clone https://github.com/ST10254797/AgriEnergyConnects
cd AgriEnergyConnects


### 2. Database Configuration

1. Open SQL Server Management Studio or your preferred SQL client
2. Create a new database named AgriEnergyDB
3. Update the connection string in appsettings.json to match your SQL Server instance:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=LAPTOP-KM8P2INL;Database=AgriEnergyDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}


### 3. Apply Database Migrations

Run the following commands in the project directory to create the database schema:

Using Visual Studio:
- Open the Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
- Run: Add-Database InitialCreate
- Run: Update-Database

Using the command line:
bash
dotnet ef migrations add InitialCreate
dotnet ef database update


### 4. Build and Run the Application

#### Using Visual Studio:
1. Open the solution file (AgriEnergyConnects.sln) in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. Press F5 to run the application

#### Using the command line:
bash
dotnet build
dotnet run


The application should now be running at https://localhost:7066/.

##  Using the Application

### Landing Page
- The landing page provides a **welcome screen**.
- It includes access to the **Login** page (via the **Login** button) and the **Register** page (via the **Register** button) for creating a new account.

### Authentication
- Use the following demo credentials to test the application:
  - **Employee**: Email: `AD@gmail.com`, Password: `Ameen@123`
  - **Farmer**: Email: `RP@gmail.com`, Password: `Ryan@123`

### Dashboard
- When a user successfully registers or logs in, they are redirected to the **Home** page.
- The home page displays a **welcome message**, and the **navigation bar** is dynamically filled with views relevant to the user's role.
### For Employees
1. **Managing Farmers**:
   - Navigate to the "Farmers" section to view the list of registered farmers
   - Add new farmers using the "Create New" button
   - View detailed farmer profiles by clicking on "Details"
   - Delete Farmer profiles by clicking on "Delete"

2. **Viewing Products**:
   - Access the "All Products" section to browse products from all farmers
   - Filter products by category or date range

### For Farmers
1. **Managing Profile**:
   - Access your profile from the "Farmers Profile" tab
   - Edit/Update your profile information using the "Save Changes" button

2. **Managing Products**:
   - Navigate to "Products" to view your current product listings
   - Add new products using the "Create New Product" button
   - Edit, View, or delete existing products from this page

3. **Forum/Farmers chat**:
    - Navigate to the "Farmers Chat" to chat with other farmers
    - Tap on send message to send a message to the community

##  Development Workflow

### Project Structure

The application follows the standard ASP.NET Core MVC structure:

- **Controllers/**: Contains the application's controllers
  - AppRolesController.cs: Handles user authentication
  - ChatController.cs:Handles the chats between the farmers
  - FarmersController.cs: Manages farmer-related operations
  - HomeController.cs: Handles dashboard and home page
  - ProductsController.cs: Manages product operations

- **Models/**: Contains the application's data models
  - ApplicationUser.cs: Represents users (employees and farmers)
  - ChatMessage.cs: Represents the chats
  - Product.cs: Represents agricultural products
  - ErrorViewModel.cs: Used for error handling
  - Farmer.cs: Represents the Farmers details

- **Views/**: Contains the application's views, organized by controller
  - AppRoles/: Login and registration views
  - Farmers/: Farmer management views
  - Home/: Dashboard views
  - Products/: Product management views
  - Chats/: Manages the messages
  
- **Data/**: Contains database context and configurations
  - ApplicationDbContext.cs: Entity Framework database context

- **wwwroot/**: Contains static files
  - css/: Stylesheet files
  - js/: JavaScript files
  - lib/: Client-side libraries

### Extending the Application

1. **Adding New Features**:
   - Create new controllers in the Controllers/ directory
   - Add corresponding views in the Views/ directory
   - Update navigation in _Layout.cshtml

2. **Modifying Database Schema**:
   - Update model classes in the Models/ directory
   - Create a new migration using Entity Framework:
     - Add-Migration MigrationName (Visual Studio)
     - dotnet ef migrations add MigrationName (command line)
   - Apply the migration to the database:
     - Update-Database (Visual Studio)
     - dotnet ef database update (command line)

##  Testing

The application includes sample data to facilitate testing:

- **Sample Users**:
  - Employee: Email: employee1@gmail.com, Password: password123
  - Farmers: Email: farmer1@gmail.com, Password: password123, Email: farmer2@gmail.com, Password: password123

- **Sample Products**:
  - Organic Tomatoes
  - Free-Range Eggs
  - Grass-Fed Beef
  - Honey
  - Organic Spinach

##  License

This project is licensed under the MIT License - see the LICENSE file for details.

##  Contact

For questions or support, please contact:
- Email: cristianoronaldonaidoo@gmail.com
- GitHub: [ST10254797](https://github.com/ST10254797)

---

Thank you for being part of Agri-Energy Connect!
Together, we’re cultivating a future where sustainable agriculture and clean energy go hand in hand.

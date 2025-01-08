# Herbg

Herbg is a fictional global e-commerce website for selling herbs, developed using **ASP.NET Core MVC**. The application provides distinct functionalities for unregistered users, registered users, and administrators, making it a robust and comprehensive platform.

---

## Admin Credentials

To access the admin area, use the following credentials:

- **Email**: `admin@herbg.com`
- **Password**: `Admin@123`

---

## Database Diagram

Below is the database diagram that outlines the data relationships:

![Database Diagram](https://github.com/user-attachments/assets/d5c070cf-3d72-4529-8d4d-bfc6e3f0d6f2)

---

## Features and Functionality

### 1. Functionality for Unregistered Users

Unregistered users can:
- Register an account.
- Browse product categories.
- View products and their details.

![Unregistered Users - Categories and Products](https://github.com/user-attachments/assets/c28d3fa3-aa98-4533-a9d5-6ddba5885453)

---

### 2. Functionality for Registered Users

Registered users gain access to additional features:
- **Shopping Cart**: Add and manage products for purchase.
- **Wishlist**: Save products for later and move items between the cart and wishlist.
- **Purchases**: Buy products and track orders.
- **Reviews**: Write product reviews.
- **Shipping**: Add and manage shipping addresses.

#### Product Page

![Product Page](https://github.com/user-attachments/assets/0ad7b456-51a5-416f-84bf-ffe0ec0cae4f)

#### Wishlist

![Wishlist](https://github.com/user-attachments/assets/79bab724-ba85-4cd0-bd52-d4050a0cd3ee)

#### Cart

![Cart](https://github.com/user-attachments/assets/59509e75-ba3c-464b-a4e3-02822a88db69)

#### Checkout

![Checkout](https://github.com/user-attachments/assets/d40884d0-e4af-45f9-ad00-9531aafbb2c7)

#### Orders

![Orders](https://github.com/user-attachments/assets/7f5f7f92-72ed-446d-ac8e-9e65e904080f)

#### User Account Management

![User Account Management](https://github.com/user-attachments/assets/c449b44e-7dcd-4db5-af56-dede3e31d32c)

---

### 3. Admin Area

Admins have complete control over managing the e-commerce platform:

- **Dashboard**: View key metrics and platform insights.
- **Manage Products**: Create, edit, and delete products.
- **Manage Categories**: Create, edit, and delete categories.

#### Admin Dashboard

![Admin Dashboard](https://github.com/user-attachments/assets/05d9fdf2-fbd9-4474-ba7e-5d8927471cf8)

#### Manage Products

![Manage Products](https://github.com/user-attachments/assets/c5a67100-7d54-42ad-8424-2fdb8749088f)

#### Manage Categories

![Manage Categories](https://github.com/user-attachments/assets/d7c7da5a-6e06-449b-8723-52ab1a2e7204)

---

## How to Run the Application

1. Open the solution file in **Visual Studio**.
2. Build the solution to restore dependencies and ensure all projects are compiled successfully.
3. Update the connection string in the `appsettings.json` file to point to your Microsoft SQL Server instance.
4. Run the application using **IIS Express** or your preferred server setup directly from Visual Studio.

---

## Prerequisites

- **.NET SDK** (version 9.0 or higher)
- **MS SQL Server**
- **Visual Studio IDE** (latest version recommended)

---

## License

This project is licensed under the MIT License. See the LICENSE file for more details.

---

## Contact

For any inquiries, please reach out through the project's repository or open an issue. Contributions and suggestions are welcome!


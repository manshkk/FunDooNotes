# FundooNotes API

A scalable and secure Notes Management REST API inspired by Google Keep, built with **ASP.NET Core 8**, **Entity Framework Core**, **SQL Server**, **JWT Authentication**, **Redis Caching**, and **RabbitMQ Messaging**.

## 🚀 Features

* User Registration & Login
* JWT Authentication & Authorization
* Create, Read, Update, Delete Notes
* Pin, Archive, Trash & Restore Notes
* Label Management
* Redis Caching for Performance Optimization
* RabbitMQ for Asynchronous Event Processing
* Global Exception Handling
* Structured Logging
* Swagger API Documentation
* Layered Architecture (Controller → Business → Repository → Database)

## 🛠️ Tech Stack

* ASP.NET Core 8 Web API
* C#
* Entity Framework Core
* SQL Server
* JWT Authentication
* Redis
* RabbitMQ
* Swagger/OpenAPI
* Git & GitHub

## 📂 Architecture

```text
Controller Layer
       ↓
Business Layer
       ↓
Repository Layer
       ↓
SQL Server Database
```

## 📌 Key Modules

* User Management
* Notes Management
* Labels Management
* Authentication & Authorization
* Redis Caching
* RabbitMQ Messaging
* Logging & Exception Handling

## ⚡ Getting Started

```bash
git clone <repository-url>
cd FundooNotes
dotnet restore
Update-Database
dotnet run
```

Access Swagger:

```text
https://localhost:<port>/swagger
```

## 🎯 Learning Outcomes

This project demonstrates real-world implementation of:

* RESTful API Development
* Clean Layered Architecture
* Entity Framework Core
* JWT Security
* Redis Caching
* RabbitMQ Messaging
* Dependency Injection
* Global Exception Handling
* API Documentation

## 👨‍💻 Author

**Manish Kumar Kaushal**
ASP.NET Core Backend Developer

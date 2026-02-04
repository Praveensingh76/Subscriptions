create database db_subscription

use db_subscription

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    SubscriptionStatus NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Subscriptions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    PlanType NVARCHAR(50),
    StartDate DATETIME,
    EndDate DATETIME,
    IsActive BIT DEFAULT 1
);

CREATE TABLE Questions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    QuestionText NVARCHAR(500),
    Category NVARCHAR(100)
);

CREATE TABLE UserResponses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    QuestionId INT FOREIGN KEY REFERENCES Questions(Id),
    Response NVARCHAR(500),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Guidance (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Category NVARCHAR(100),
    GuidanceText NVARCHAR(1000),
    CreatedAt DATETIME DEFAULT GETDATE()
);

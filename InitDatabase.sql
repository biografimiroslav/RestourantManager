IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'RestaurantManagerDb')
BEGIN
    CREATE DATABASE [RestaurantManagerDb];
END
GO

USE [RestaurantManagerDb];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Tables')
BEGIN
    CREATE TABLE [dbo].[Tables] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(MAX) NOT NULL,
        [Seats] INT NOT NULL,
        [IsOccupied] BIT NOT NULL,
        CONSTRAINT [PK_Tables] PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    INSERT INTO [dbo].[Tables] ([Name], [Seats], [IsOccupied]) VALUES 
    (N'Стіл 1', 2, 0),
    (N'Стіл 2', 4, 0),
    (N'Стіл 3', 4, 0),
    (N'Стіл 4', 6, 0);
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'Dishes')
BEGIN
    CREATE TABLE [dbo].[Dishes] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(MAX) NOT NULL,
        [Price] DECIMAL(18, 2) NOT NULL,
        [CategoryName] NVARCHAR(MAX) NOT NULL,
        CONSTRAINT [PK_Dishes] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Create database
CREATE DATABASE SpaceXDB;
GO

USE SpaceXDB;
GO

-- Users table (for authentication)
CREATE TABLE [dbo].[Users] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [PasswordHash] VARBINARY(512) NOT NULL,
    [PasswordSalt] VARBINARY(512) NOT NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
GO


-- Index for quick lookup by email
CREATE UNIQUE INDEX IX_Users_Email ON [dbo].[Users]([Email]);
GO
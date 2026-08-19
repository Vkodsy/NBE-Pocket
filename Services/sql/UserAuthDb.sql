IF DB_ID(N'UserAuthDb') IS NULL
BEGIN
    CREATE DATABASE [UserAuthDb];
END
GO
USE [UserAuthDb];
GO
IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Users]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [FirstName] NVARCHAR(100) NOT NULL,
        [LastName] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(320) NOT NULL,
        [PasswordHash] NVARCHAR(500) NOT NULL,
        [CreatedAt] DATETIME2 NOT NULL,
         [UpdatedAt] DATETIME2 NOT NULL,
        CONSTRAINT [PK_Users]
            PRIMARY KEY ([Id])
    );
END
GO
IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UX_Users_Email'
      AND object_id = OBJECT_ID(N'dbo.Users')
)
BEGIN
    CREATE UNIQUE INDEX [UX_Users_Email]
    ON [dbo].[Users] ([Email]);
END
GO
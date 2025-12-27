using System;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.App_Start
{
    /// <summary>
    /// Ensures that the database schema required for the extended rental features exists.
    /// Executed on application start – safe to run multiple times.
    /// </summary>
    public static class DatabaseInitializer
    {
        public static void EnsureSchema()
        {
            string cs = Functions.GetConnectionString();

            using (var conn = new SqlConnection(cs))
            {
                conn.Open();

                // Core lookup tables
                Execute(conn, @"
IF OBJECT_ID('Brand', 'U') IS NULL
BEGIN
    CREATE TABLE Brand(
        BrandId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL UNIQUE
    );
END");

                Execute(conn, @"
IF OBJECT_ID('CarModel', 'U') IS NULL
BEGIN
    CREATE TABLE CarModel(
        ModelId INT IDENTITY(1,1) PRIMARY KEY,
        BrandId INT NOT NULL FOREIGN KEY REFERENCES Brand(BrandId),
        Name NVARCHAR(120) NOT NULL,
        CONSTRAINT UX_CarModel UNIQUE (BrandId, Name)
    );
END");

                Execute(conn, @"
IF OBJECT_ID('CarCategory', 'U') IS NULL
BEGIN
    CREATE TABLE CarCategory(
        CategoryId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(60) NOT NULL UNIQUE,
        MinExperienceYears INT NOT NULL DEFAULT 0,
        IsPremium BIT NOT NULL DEFAULT 0,
        MaxDailyPrice DECIMAL(10,2) NULL
    );
END");

                // Storage for any files (car photos, licenses, etc.)
                Execute(conn, @"
IF OBJECT_ID('FileStorage', 'U') IS NULL
BEGIN
    CREATE TABLE FileStorage(
        FileId INT IDENTITY(1,1) PRIMARY KEY,
        FilePath NVARCHAR(400) NOT NULL,
        OriginalName NVARCHAR(255) NOT NULL,
        ContentType NVARCHAR(120) NULL,
        FileKind NVARCHAR(50) NOT NULL,
        UploadedBy INT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT(GETDATE())
    );
END");

                Execute(conn, @"
IF OBJECT_ID('CarImages', 'U') IS NULL
BEGIN
    CREATE TABLE CarImages(
        CarImageId INT IDENTITY(1,1) PRIMARY KEY,
        CarPlate NVARCHAR(50) NOT NULL,
        FileId INT NOT NULL FOREIGN KEY REFERENCES FileStorage(FileId),
        IsPrimary BIT NOT NULL DEFAULT(0)
    );
END");

                Execute(conn, @"
IF OBJECT_ID('DrivingLicense', 'U') IS NULL
BEGIN
    CREATE TABLE DrivingLicense(
        LicenseId INT IDENTITY(1,1) PRIMARY KEY,
        CustomerId INT NOT NULL FOREIGN KEY REFERENCES CustomerTbl(CustId),
        LicenseNumber NVARCHAR(100) NULL,
        IssuedDate DATE NULL,
        ExpiryDate DATE NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT('Pending'),
        FileId INT NULL FOREIGN KEY REFERENCES FileStorage(FileId),
        VerifiedBy INT NULL,
        VerifiedByAI BIT NOT NULL DEFAULT(0),
        VerifiedAt DATETIME NULL,
        Comment NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT(GETDATE())
    );
END");

                Execute(conn, @"
IF OBJECT_ID('LicenseVerificationLog', 'U') IS NULL
BEGIN
    CREATE TABLE LicenseVerificationLog(
        LogId INT IDENTITY(1,1) PRIMARY KEY,
        LicenseId INT NOT NULL FOREIGN KEY REFERENCES DrivingLicense(LicenseId),
        CheckedBy INT NULL,
        CheckedByAI BIT NOT NULL DEFAULT(0),
        Result NVARCHAR(20) NOT NULL,
        Comment NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT(GETDATE())
    );
END

IF OBJECT_ID('CustomerAllowedCategory', 'U') IS NULL
BEGIN
    CREATE TABLE CustomerAllowedCategory(
        CustomerId INT NOT NULL FOREIGN KEY REFERENCES CustomerTbl(CustId),
        CategoryId INT NOT NULL FOREIGN KEY REFERENCES CarCategory(CategoryId),
        PRIMARY KEY (CustomerId, CategoryId)
    );
END");

                // Extend existing tables
                Execute(conn, @"
IF COL_LENGTH('CarTbl', 'CarId') IS NULL
    ALTER TABLE CarTbl ADD CarId INT IDENTITY(1,1);
IF COL_LENGTH('CarTbl', 'BrandId') IS NULL
    ALTER TABLE CarTbl ADD BrandId INT NULL;
IF COL_LENGTH('CarTbl', 'ModelId') IS NULL
    ALTER TABLE CarTbl ADD ModelId INT NULL;
IF COL_LENGTH('CarTbl', 'CategoryId') IS NULL
    ALTER TABLE CarTbl ADD CategoryId INT NULL;
IF COL_LENGTH('CarTbl', 'MainImageFileId') IS NULL
    ALTER TABLE CarTbl ADD MainImageFileId INT NULL;");

                Execute(conn, @"
IF COL_LENGTH('CustomerTbl', 'DrivingExperienceYears') IS NULL
    ALTER TABLE CustomerTbl ADD DrivingExperienceYears INT NULL;
IF COL_LENGTH('CustomerTbl', 'LicenseIssueDate') IS NULL
    ALTER TABLE CustomerTbl ADD LicenseIssueDate DATE NULL;");

                // Seed categories if empty
                Execute(conn, @"
IF NOT EXISTS (SELECT 1 FROM CarCategory)
BEGIN
    INSERT INTO CarCategory (Name, MinExperienceYears, IsPremium, MaxDailyPrice) VALUES
    (N'Эконом', 0, 0, 80),
    (N'Стандарт', 1, 0, 150),
    (N'Бизнес', 2, 0, 250),
    (N'Премиум', 3, 1, NULL);
END");

                // SEED UPDATE: Assign categories to existing cars based on price/model if they are null
                // This fills the database logic requested by the user.
                Execute(conn, @"
                DECLARE @EcoId INT = (SELECT Top 1 CategoryId FROM CarCategory WHERE Name = N'Эконом');
                DECLARE @StdId INT = (SELECT Top 1 CategoryId FROM CarCategory WHERE Name = N'Стандарт');
                DECLARE @BusId INT = (SELECT Top 1 CategoryId FROM CarCategory WHERE Name = N'Бизнес');
                DECLARE @PremId INT = (SELECT Top 1 CategoryId FROM CarCategory WHERE Name = N'Премиум');

                -- Assign Premium (Price > 300 or specific models)
                UPDATE CarTbl SET CategoryId = @PremId 
                WHERE CategoryId IS NULL AND (Price > 300 OR Model LIKE '%Vanquish%' OR Brand LIKE '%Aston Martin%' OR Brand LIKE '%Ferrari%' OR Brand LIKE '%Lamborghini%');

                -- Assign Business (Price between 150 and 300)
                UPDATE CarTbl SET CategoryId = @BusId
                WHERE CategoryId IS NULL AND (Price > 150 AND Price <= 300);

                -- Assign Standard (Price between 80 and 150)
                UPDATE CarTbl SET CategoryId = @StdId
                WHERE CategoryId IS NULL AND (Price > 80 AND Price <= 150);

                -- Assign Economy (Price <= 80)
                UPDATE CarTbl SET CategoryId = @EcoId
                WHERE CategoryId IS NULL AND (Price <= 80);

                -- Fallback: Assign Standard to anything remaining
                UPDATE CarTbl SET CategoryId = @StdId WHERE CategoryId IS NULL;
                ");
            }
        }

        private static void Execute(SqlConnection conn, string sql)
        {
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}


-- Create the database
CREATE DATABASE WarehouseDB;
GO

USE WarehouseDB;
GO

-- Create Products table
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    BeginningBalance DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT UQ_Products_Name UNIQUE (Name)
);
GO

-- Create Transactions table
CREATE TABLE Transactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    Quantity DECIMAL(18,2) NOT NULL,
    Type CHAR(1) NOT NULL CHECK (Type IN ('I', 'O')),
    Date DATETIME NOT NULL DEFAULT GETDATE(),
    Notes NVARCHAR(500),
    CONSTRAINT FK_Transactions_Products FOREIGN KEY (ProductID) 
        REFERENCES Products(ProductID)
);
GO

-- Create index on Transactions for better query performance
CREATE INDEX IX_Transactions_ProductID ON Transactions(ProductID);
CREATE INDEX IX_Transactions_Date ON Transactions(Date);
GO

-- Insert sample products
INSERT INTO Products (Name, BeginningBalance) VALUES 
('Laptop Dell XPS 13', 10),
('Microsoft Surface Pro', 15);
GO

-- Insert sample transactions
INSERT INTO Transactions (ProductID, Quantity, Type, Date, Notes) VALUES 
(1, 5, 'I', GETDATE(), 'Initial stock'),
(2, 8, 'I', GETDATE(), 'Initial stock'),
(1, 2, 'O', GETDATE(), 'First order');
GO 
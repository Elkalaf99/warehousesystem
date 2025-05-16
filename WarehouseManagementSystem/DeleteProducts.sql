USE WarehouseDB;
GO

-- Delete all transactions first (due to foreign key constraint)
DELETE FROM Transactions;
GO

-- Delete all products
DELETE FROM Products;
GO

-- Reset identity seed
DBCC CHECKIDENT ('Products', RESEED, 0);
DBCC CHECKIDENT ('Transactions', RESEED, 0);
GO 
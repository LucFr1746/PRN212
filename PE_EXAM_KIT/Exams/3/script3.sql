
CREATE DATABASE PRN212_26SprB1_2;
GO

USE PRN212_26SprB1_2;
GO

-- =========================================
-- 1) TABLES
-- =========================================

CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Products
(
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    CategoryId INT NOT NULL,
    CONSTRAINT FK_Products_Categories
        FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
GO

CREATE TABLE Suppliers
(
    SupplierId INT IDENTITY(1,1) PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL,
    ContactEmail NVARCHAR(150) NOT NULL
);
GO

CREATE TABLE ProductSuppliers
(
    ProductId INT NOT NULL,
    SupplierId INT NOT NULL,
    CONSTRAINT PK_ProductSuppliers PRIMARY KEY (ProductId, SupplierId),
    CONSTRAINT FK_ProductSuppliers_Products
        FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
    CONSTRAINT FK_ProductSuppliers_Suppliers
        FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
);
GO

-- =========================================
-- 2) DEMO DATA
-- =========================================

INSERT INTO Categories (CategoryName)
VALUES
(N'Computers'),
(N'Accessories'),
(N'Audio'),
(N'Wearables'),
(N'Smart Home'),
(N'Gaming'),
(N'Office');
GO

INSERT INTO Suppliers (SupplierName, ContactEmail)
VALUES
(N'TechCorp',     N'contact@techcorp.com'),
(N'GlobalDist',   N'sales@globaldist.com'),
(N'FastShip',     N'hello@fastship.com'),
(N'DigiSource',   N'info@digisource.com'),
(N'NovaTrade',    N'support@novatrade.com'),
(N'PrimeSupply',  N'contact@primesupply.com'),
(N'MegaWholesale',N'sales@megawholesale.com');
GO

INSERT INTO Products (ProductName, Price, Stock, CategoryId)
VALUES
(N'Laptop Pro',            999.99,  50, 1),
(N'Wireless Mouse',         29.99, 200, 2),
(N'Wireless Headphones',   149.99, 120, 3),
(N'Smart Watch',           299.99,  80, 4),
(N'Smart Bulb',             19.99, 300, 5),
(N'Gaming Keyboard',        79.99, 150, 6),
(N'Office Chair',          199.99,  40, 7),
(N'USB-C Hub',              49.99, 180, 2);
GO

INSERT INTO ProductSuppliers (ProductId, SupplierId)
VALUES
(1,1), (1,2), (1,4),
(2,2), (2,3), (2,5),
(3,1), (3,4), (3,6),
(4,1), (4,5),
(5,3), (5,7),
(6,2), (6,6), (6,7),
(7,4), (7,5),
(8,2), (8,3), (8,6);
GO

-- =========================================
-- 3) QUICK CHECK
-- =========================================

SELECT * FROM Categories;
SELECT * FROM Suppliers;
SELECT * FROM Products;
SELECT * FROM ProductSuppliers;

SELECT
    p.ProductId,
    p.ProductName,
    p.Price,
    p.Stock,
    c.CategoryName
FROM Products p
JOIN Categories c ON p.CategoryId = c.CategoryId
ORDER BY p.ProductId;
GO
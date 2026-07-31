CREATE DATABASE PRN212_26Spr_Exam7;
GO

USE PRN212_26Spr_Exam7;
GO

-- ============================================
-- 1. TABLES
-- ============================================

CREATE TABLE RoomTypes
(
    RoomTypeId INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

CREATE TABLE Rooms
(
    RoomId INT IDENTITY(1,1) PRIMARY KEY,
    RoomNumber NVARCHAR(50) NOT NULL,
    Capacity INT NOT NULL,
    PricePerNight DECIMAL(18,2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    RoomTypeId INT NOT NULL,
    CONSTRAINT FK_Rooms_RoomTypes
        FOREIGN KEY (RoomTypeId) REFERENCES RoomTypes(RoomTypeId)
);
GO

CREATE TABLE Amenities
(
    AmenityId INT IDENTITY(1,1) PRIMARY KEY,
    AmenityName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE RoomAmenities
(
    RoomId INT NOT NULL,
    AmenityId INT NOT NULL,
    CONSTRAINT PK_RoomAmenities PRIMARY KEY (RoomId, AmenityId),
    CONSTRAINT FK_RoomAmenities_Rooms
        FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId) ON DELETE CASCADE,
    CONSTRAINT FK_RoomAmenities_Amenities
        FOREIGN KEY (AmenityId) REFERENCES Amenities(AmenityId) ON DELETE CASCADE
);
GO

-- ============================================
-- 2. DEMO DATA
-- ============================================

-- RoomTypes: 5 records
INSERT INTO RoomTypes (TypeName, Description)
VALUES
    (N'Standard Single', N'Single bed, city view'),
    (N'Standard Double', N'Double bed, standard amenities'),
    (N'Deluxe Suite', N'King bed, ocean view with balcony'),
    (N'Executive Suite', N'King bed, living area, executive lounge access'),
    (N'Presidential Suite', N'Penthouse luxury, private pool & butler service');
GO

-- Rooms: 10 records
INSERT INTO Rooms (RoomNumber, Capacity, PricePerNight, IsActive, RoomTypeId)
VALUES
    (N'R101', 1, 50.00, 1, 1),
    (N'R102', 1, 55.00, 1, 1),
    (N'R201', 2, 90.00, 1, 2),
    (N'R202', 2, 95.00, 1, 2),
    (N'R301', 2, 180.00, 1, 3),
    (N'R302', 3, 220.00, 1, 3),
    (N'R401', 4, 350.00, 1, 4),
    (N'R402', 4, 400.00, 0, 4),
    (N'R501', 6, 850.00, 1, 5),
    (N'R502', 6, 950.00, 1, 5);
GO

-- Amenities: 6 records
INSERT INTO Amenities (AmenityName)
VALUES
    (N'High-Speed Wi-Fi'),
    (N'Ocean View'),
    (N'Mini Bar & Wine'),
    (N'Jacuzzi Bathtub'),
    (N'Balcony'),
    (N'Private Swimming Pool');
GO

-- RoomAmenities (Bridge table records)
INSERT INTO RoomAmenities (RoomId, AmenityId)
VALUES
    (1, 1),
    (2, 1),
    (3, 1), (3, 5),
    (4, 1), (4, 3),
    (5, 1), (5, 2), (5, 5),
    (6, 1), (6, 2), (6, 4), (6, 5),
    (7, 1), (7, 2), (7, 3), (7, 4), (7, 5),
    (8, 1), (8, 3), (8, 4),
    (9, 1), (9, 2), (9, 3), (9, 4), (9, 5), (9, 6),
    (10, 1), (10, 2), (10, 3), (10, 4), (10, 5), (10, 6);
GO

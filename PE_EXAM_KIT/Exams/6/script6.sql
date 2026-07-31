-- ============================================
-- PRN212 Practical Exam — Script 6
-- Database: BookStoreDB
-- ============================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'BookStoreDB')
BEGIN
    ALTER DATABASE BookStoreDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BookStoreDB;
END
GO

CREATE DATABASE BookStoreDB;
GO

USE BookStoreDB;
GO

-- ============================================
-- Table 1: Authors (Lookup / Category table)
-- ============================================
CREATE TABLE Authors (
    AuthorId    INT IDENTITY(1,1) PRIMARY KEY,
    AuthorName  NVARCHAR(100) NOT NULL
);
GO

-- ============================================
-- Table 2: Books (Main entity)
-- ============================================
CREATE TABLE Books (
    BookId      INT IDENTITY(1,1) PRIMARY KEY,
    Title       NVARCHAR(150) NOT NULL,
    Price       DECIMAL(18,2) NOT NULL,
    PublishYear INT           NOT NULL,
    AuthorId    INT           NOT NULL,
    CONSTRAINT FK_Books_Authors FOREIGN KEY (AuthorId) REFERENCES Authors(AuthorId)
);
GO

-- ============================================
-- Table 3: Genres (Many-to-many secondary)
-- ============================================
CREATE TABLE Genres (
    GenreId   INT IDENTITY(1,1) PRIMARY KEY,
    GenreName NVARCHAR(100) NOT NULL
);
GO

-- ============================================
-- Table 4: BookGenres (Bridge / Junction table)
-- ============================================
CREATE TABLE BookGenres (
    BookId  INT NOT NULL,
    GenreId INT NOT NULL,
    CONSTRAINT PK_BookGenres PRIMARY KEY (BookId, GenreId),
    CONSTRAINT FK_BookGenres_Books  FOREIGN KEY (BookId)  REFERENCES Books(BookId),
    CONSTRAINT FK_BookGenres_Genres FOREIGN KEY (GenreId) REFERENCES Genres(GenreId)
);
GO

-- ============================================
-- Seed Data: Authors
-- ============================================
INSERT INTO Authors (AuthorName) VALUES
    (N'Nguyen Nhat Anh'),
    (N'Paulo Coelho'),
    (N'Haruki Murakami'),
    (N'J.K. Rowling'),
    (N'Yuval Noah Harari');
GO

-- ============================================
-- Seed Data: Genres
-- ============================================
INSERT INTO Genres (GenreName) VALUES
    (N'Fiction'),
    (N'Non-Fiction'),
    (N'Fantasy'),
    (N'Self-Help'),
    (N'Science'),
    (N'Romance');
GO

-- ============================================
-- Seed Data: Books
-- ============================================
INSERT INTO Books (Title, Price, PublishYear, AuthorId) VALUES
    (N'Mat Biec',               85000.00,  2019, 1),
    (N'Toi Thay Hoa Vang Tren Co Xanh', 79000.00, 2010, 1),
    (N'The Alchemist',         250000.00,  1988, 2),
    (N'Norwegian Wood',        195000.00,  1987, 3),
    (N'Kafka on the Shore',    220000.00,  2002, 3),
    (N'Harry Potter and the Philosopher''s Stone', 350000.00, 1997, 4),
    (N'Harry Potter and the Chamber of Secrets',   320000.00, 1998, 4),
    (N'Sapiens',               280000.00,  2011, 5),
    (N'Homo Deus',             290000.00,  2015, 5),
    (N'21 Lessons for the 21st Century', 260000.00, 2018, 5);
GO

-- ============================================
-- Seed Data: BookGenres (Bridge)
-- ============================================
INSERT INTO BookGenres (BookId, GenreId) VALUES
    (1, 1),   -- Mat Biec -> Fiction
    (1, 6),   -- Mat Biec -> Romance
    (2, 1),   -- Toi Thay Hoa Vang -> Fiction
    (3, 1),   -- The Alchemist -> Fiction
    (3, 4),   -- The Alchemist -> Self-Help
    (4, 1),   -- Norwegian Wood -> Fiction
    (4, 6),   -- Norwegian Wood -> Romance
    (5, 1),   -- Kafka on the Shore -> Fiction
    (5, 3),   -- Kafka on the Shore -> Fantasy
    (6, 3),   -- HP Philosopher's Stone -> Fantasy
    (6, 1),   -- HP Philosopher's Stone -> Fiction
    (7, 3),   -- HP Chamber of Secrets -> Fantasy
    (7, 1),   -- HP Chamber of Secrets -> Fiction
    (8, 2),   -- Sapiens -> Non-Fiction
    (8, 5),   -- Sapiens -> Science
    (9, 2),   -- Homo Deus -> Non-Fiction
    (9, 5),   -- Homo Deus -> Science
    (10, 2),  -- 21 Lessons -> Non-Fiction
    (10, 4);  -- 21 Lessons -> Self-Help
GO

-- Skrypt tworz¹cy bazê danych dla Systemu Zarz¹dzania Flot¹
-- Wersja: .NET 8, C# 12, zgodny z modelem EF Core

USE MASTER;
CREATE DATABASE CarManagementDB;
GO
USE CarManagementDB;
GO

CREATE TABLE KategoriaPojazdu (
    Id INT IDENTITY PRIMARY KEY,
    Nazwa NVARCHAR(100) NOT NULL
);

CREATE TABLE Kategoria (
    Id INT IDENTITY PRIMARY KEY,
    Nazwa NVARCHAR(40) NOT NULL
);

CREATE TABLE Uprawnienia (
    Id INT IDENTITY PRIMARY KEY,
    Nazwa NVARCHAR(40) NOT NULL
);

CREATE TABLE Kierowca (
    Id INT IDENTITY PRIMARY KEY,
    Email NVARCHAR(254) NOT NULL UNIQUE,
    HasloHash NVARCHAR(256) NOT NULL,
    Imie NVARCHAR(50) NOT NULL,
    Nazwisko NVARCHAR(50) NOT NULL,
    Id_Uprawnienia INT NULL,
    FOREIGN KEY (Id_Uprawnienia) REFERENCES Uprawnienia(Id)
);

CREATE TABLE PracownikAdministracyjny (
    Id INT IDENTITY PRIMARY KEY,
    Email NVARCHAR(254) NOT NULL UNIQUE,
    Haslo NVARCHAR(128) NOT NULL,
    Imie NVARCHAR(50) NOT NULL,
    Nazwisko NVARCHAR(50) NOT NULL,
    Id_Uprawnienia INT NULL,
    FOREIGN KEY (Id_Uprawnienia) REFERENCES Uprawnienia(Id)
);

CREATE TABLE Pojazd (
    Id INT IDENTITY PRIMARY KEY,
    NumerRejestracyjny NVARCHAR(20) NOT NULL UNIQUE,
    VIN NVARCHAR(30) NOT NULL,
    Marka NVARCHAR(50),
    Model NVARCHAR(50),
    Rok INT,
    TypNapedu NVARCHAR(30),
    PrzebiegKm INT,
    Status NVARCHAR(20),
    KategoriaId INT NULL,
    FOREIGN KEY (KategoriaId) REFERENCES KategoriaPojazdu(Id)
);

CREATE TABLE PrzypisaniePojazdu (
    Id INT IDENTITY PRIMARY KEY,
    PojazdId INT NOT NULL,
    KierowcaId INT NOT NULL,
    DataOd DATE NOT NULL,
    DataDo DATE NULL,
    TrybUzytkowania NVARCHAR(30),
    FOREIGN KEY (PojazdId) REFERENCES Pojazd(Id),
    FOREIGN KEY (KierowcaId) REFERENCES Kierowca(Id)
);

CREATE TABLE PolisaUbezpieczeniowa (
    Id INT IDENTITY PRIMARY KEY,
    PojazdId INT NOT NULL,
    Typ NVARCHAR(10) NOT NULL, -- OC/AC/NW/AS
    Numer NVARCHAR(40) NOT NULL,
    DataOd DATE NOT NULL,
    DataDo DATE NOT NULL,
    FOREIGN KEY (PojazdId) REFERENCES Pojazd(Id)
);

CREATE TABLE SerwisPojazdu (
    Id INT IDENTITY PRIMARY KEY,
    PojazdId INT NOT NULL,
    Data DATE NOT NULL,
    PrzebiegKm INT,
    Rodzaj NVARCHAR(30),
    Koszt DECIMAL(12,2),
    FOREIGN KEY (PojazdId) REFERENCES Pojazd(Id)
);

CREATE TABLE KartaPaliwowa (
    Id INT IDENTITY PRIMARY KEY,
    Numer NVARCHAR(40) NOT NULL,
    LimitMiesieczny DECIMAL(12,2),
    Aktywna BIT NOT NULL,
    KierowcaId INT NULL,
    FOREIGN KEY (KierowcaId) REFERENCES Kierowca(Id)
);

CREATE TABLE Tankowanie (
    Id INT IDENTITY PRIMARY KEY,
    PojazdId INT NOT NULL,
    KierowcaId INT NULL,
    KartaPaliwowaId INT NULL,
    Data DATE NOT NULL,
    IloscLitrow DECIMAL(8,2) NOT NULL,
    CenaZaLitr DECIMAL(8,2),
    Kwota DECIMAL(12,2) NOT NULL,
    PrzebiegKm INT,
    FOREIGN KEY (PojazdId) REFERENCES Pojazd(Id),
    FOREIGN KEY (KierowcaId) REFERENCES Kierowca(Id),
    FOREIGN KEY (KartaPaliwowaId) REFERENCES KartaPaliwowa(Id)
);

-- Przyk³adowe dane
INSERT INTO Uprawnienia (Nazwa) VALUES ('Administrator'), ('Kierowca'), ('Pracownik');
INSERT INTO KategoriaPojazdu (Nazwa) VALUES ('Osobowy'), ('Dostawczy'), ('SUV');
INSERT INTO Kategoria (Nazwa) VALUES ('S³u¿bowy'), ('Prywatny');

-- Konto administratora (has³o: 'admin123' zahashowane przyk³adowo, dostosuj do produkcji!)
INSERT INTO Kierowca (Email, HasloHash, Imie, Nazwisko, Id_Uprawnienia)
VALUES ('admin@carfleet.local', 'AQAAAAIAAYagAAAAEJpQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQw==', 'Admin', 'Systemowy', 1);

-- Przyk³adowy pojazd
INSERT INTO Pojazd (NumerRejestracyjny, VIN, Marka, Model, Rok, TypNapedu, PrzebiegKm, Status, KategoriaId)
VALUES ('WX12345', '1HGCM82633A004352', 'Toyota', 'Corolla', 2022, 'Benzyna', 15000, 'Aktywny', 1);

-- Przyk³adowe przypisanie pojazdu
INSERT INTO PrzypisaniePojazdu (PojazdId, KierowcaId, DataOd, DataDo, TrybUzytkowania)
VALUES (1, 1, '2025-01-01', NULL, 'S³u¿bowy');

-- Przyk³adowa polisa
INSERT INTO PolisaUbezpieczeniowa (PojazdId, Typ, Numer, DataOd, DataDo)
VALUES (1, 'OC', 'POL123456', '2025-01-01', '2026-01-01');

-- Przyk³adowy serwis
INSERT INTO SerwisPojazdu (PojazdId, Data, PrzebiegKm, Rodzaj, Koszt)
VALUES (1, '2025-03-15', 16000, 'Przegl¹d', 500.00);

-- Przyk³adowa karta paliwowa
INSERT INTO KartaPaliwowa (Numer, LimitMiesieczny, Aktywna, KierowcaId)
VALUES ('CARD001', 1000.00, 1, 1);

-- Przyk³adowe tankowanie
INSERT INTO Tankowanie (PojazdId, KierowcaId, KartaPaliwowaId, Data, IloscLitrow, CenaZaLitr, Kwota, PrzebiegKm)
VALUES (1, 1, 1, '2025-04-01', 40.5, 6.20, 251.10, 16500);
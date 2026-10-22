# CarManagement (System Zarządzania Flotą)

Nowoczesna aplikacja (.NET 8 / C# 12 / Razor Pages + API) do ewidencji i zarządzania flotą pojazdów: pojazdy, przypisania kierowców, polisy, serwis, tankowania, raporty oraz uwierzytelnianie z wykorzystaniem JWT.


Autor: Tomasz Warchoł

## Spis treści
1. Opis i cel
2. Stos technologiczny
3. Kluczowe funkcjonalności
4. Struktura (skrót)
5. Instalacja i uruchomienie
6. Konfiguracja bazy danych (skrypt)
7. Testy
8. Bezpieczeństwo
9. Dalszy rozwój
10. Licencja / Uwagi

## 1. Opis i cel
Celem systemu jest centralizacja danych floty oraz udostępnienie interfejsu WWW (Razor Pages / MVC Controllers) i prostych endpointów API (np. logowanie) do integracji. System pozwala śledzić historię eksploatacji pojazdów oraz podstawowe operacje administracyjne.

## 2. Stos technologiczny
- .NET 8, C# 12
- ASP.NET Core (Razor Pages + Controllers / API)
- Entity Framework Core
- JWT (autoryzacja)
- SQL Server
- xUnit + FluentAssertions (testy)
- Minimalne skrypty seeda SQL (poniżej)

## 3. Kluczowe funkcjonalności (aktualny zakres)
- Uwierzytelnianie (LoginController + generowanie tokenów JWT)
- Zarządzanie pojazdami (podstawowe CRUD)
- Kategorie / Role / Uprawnienia (modele bazowe)
- Przypisania pojazd–kierowca z zakresem dat
- Polisy ubezpieczeniowe, serwis, tankowania (ewidencja operacyjna)
- Widoki Razor (Dashboard, szczegóły pojazdu, formularze)
- Warstwa usług (JWT, integracja SOAP – szkielet)
- Testy integracyjne endpointu logowania

## 4. Struktura (skrót)
- CarManagement/Program.cs – konfiguracja hosta
- CarManagement/Models – encje + ViewModel-e
- CarManagement/Services/JWT – generowanie tokenów
- CarManagement/API – kontrolery API (logowanie, przykłady)
- CarManagement/Controllers – kontrolery MVC/Razor
- CarManagement/Views – widoki Razor
- Testy/ – testy jednostkowe i integracyjne (xUnit)

## 5. Instalacja i uruchomienie
Wymagania:
- .NET 8 SDK
- SQL Server (LocalDB lub pełny)
- Visual Studio (uruchom jako konfigurację CarManagement)

Kroki:
1. Sklonuj repozytorium:
   git clone https://github.com/TomasWarchol/CarManagement
2. Utwórz bazę danych uruchamiając poniższy skrypt w SSMS (lub sqlcmd).
3. Skonfiguruj connection string w pliku appsettings.json (CarManagement/appsettings.json) – sekcja ConnectionStrings.
4. Ustaw klucz podpisu tokenów JWT. Klucz nie jest przechowywany w repozytorium – lokalnie trzymaj go w user-secrets:
   ```powershell
   # wygeneruj losowy klucz (64 bajty, Base64)
   $bytes = New-Object byte[] 64; [Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)
   dotnet user-secrets set "API:Secret" ([Convert]::ToBase64String($bytes)) --project CarManagement
   ```
   Na serwerze ustaw zmienną środowiskową `API__Secret` (lub użyj Azure Key Vault).
5. (Opcjonalnie) Jeśli używasz EF migracji — dodaj i zastosuj (o ile projekt je posiada):
   dotnet ef database update
6. Uruchom aplikację (F5 / __Debug__).
7. Otwórz przeglądarkę pod adresem z __Output__ / launchSettings (np. https://localhost:xxxx).

## 6. Konfiguracja bazy danych (skrypt)

-- Skrypt tworzący bazę danych dla Systemu Zarządzania Flotą
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

-- Przykładowe dane
INSERT INTO Uprawnienia (Nazwa) VALUES ('Administrator'), ('Kierowca'), ('Pracownik');
INSERT INTO KategoriaPojazdu (Nazwa) VALUES ('Osobowy'), ('Dostawczy'), ('SUV');
INSERT INTO Kategoria (Nazwa) VALUES ('Służbowy'), ('Prywatny');

-- Konto administratora (hasło: 'admin123' zahashowane przykładowo, dostosuj do produkcji!)
INSERT INTO Kierowca (Email, HasloHash, Imie, Nazwisko, Id_Uprawnienia)
VALUES ('admin@carfleet.local', 'AQAAAAIAAYagAAAAEJpQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQwQw==', 'Admin', 'Systemowy', 1);

-- Przykładowy pojazd
INSERT INTO Pojazd (NumerRejestracyjny, VIN, Marka, Model, Rok, TypNapedu, PrzebiegKm, Status, KategoriaId)
VALUES ('WX12345', '1HGCM82633A004352', 'Toyota', 'Corolla', 2022, 'Benzyna', 15000, 'Aktywny', 1);

-- Przykładowe przypisanie pojazdu
INSERT INTO PrzypisaniePojazdu (PojazdId, KierowcaId, DataOd, DataDo, TrybUzytkowania)
VALUES (1, 1, '2025-01-01', NULL, 'Służbowy');

-- Przykładowa polisa
INSERT INTO PolisaUbezpieczeniowa (PojazdId, Typ, Numer, DataOd, DataDo)
VALUES (1, 'OC', 'POL123456', '2025-01-01', '2026-01-01');

-- Przykładowy serwis
INSERT INTO SerwisPojazdu (PojazdId, Data, PrzebiegKm, Rodzaj, Koszt)
VALUES (1, '2025-03-15', 16000, 'Przegląd', 500.00);

-- Przykładowa karta paliwowa
INSERT INTO KartaPaliwowa (Numer, LimitMiesieczny, Aktywna, KierowcaId)
VALUES ('CARD001', 1000.00, 1, 1);

-- Przykładowe tankowanie
INSERT INTO Tankowanie (PojazdId, KierowcaId, KartaPaliwowaId, Data, IloscLitrow, CenaZaLitr, Kwota, PrzebiegKm)
VALUES (1, 1, 1, '2025-04-01', 40.5, 6.20, 251.10, 16500);

## 7. Testy
Uruchamianie testów:
- Visual Studio: __Test Explorer__
- CLI:
  dotnet test

Zakres:
- Testy integracyjne logowania (statusy HTTP, token JWT)
- Testy generatora JWT

## 8. Bezpieczeństwo
- Hasła przechowywane jako hash (sprawdź implementację – dopasuj do produkcji)
- Tokeny JWT (typ Bearer) – konfiguracja w usługach
- Klucz podpisu JWT (`API:Secret`) poza repozytorium: user-secrets lokalnie, zmienna środowiskowa / Key Vault na serwerze
- Zalecane: dodać odświeżanie tokenów, polityki ról, walidację wejścia

## 9. Dalszy rozwój (propozycje)
- Audyt zmian (logi)
- Paginated API dla list
- Role-based UI
- Eksport raportów (CSV / PDF)
- Integracje z zewnętrznymi systemami (SOAP/REST)

## 10. Licencja / Uwagi
Projekt edukacyjny / demonstracyjny. Przed użyciem w środowisku produkcyjnym: wzmocnij bezpieczeństwo, dodaj walidacje, logowanie błędów i testy wydajnościowe.


using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class mssql_migration_588 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KategoriePojazdow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KategoriePojazdow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uprawnienia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uprawnienia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pojazdy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumerRejestracyjny = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rok = table.Column<int>(type: "int", nullable: true),
                    TypNapedu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrzebiegKm = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KategoriaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pojazdy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pojazdy_KategoriePojazdow_KategoriaId",
                        column: x => x.KategoriaId,
                        principalTable: "KategoriePojazdow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Kierowcy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasloHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Uprawnienia = table.Column<int>(type: "int", nullable: true),
                    UprawnieniaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kierowcy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kierowcy_Uprawnienia_UprawnieniaId",
                        column: x => x.UprawnieniaId,
                        principalTable: "Uprawnienia",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Uzytkownicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Haslo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Imie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Uprawnienia = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzytkownicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uzytkownicy_Uprawnienia_Id_Uprawnienia",
                        column: x => x.Id_Uprawnienia,
                        principalTable: "Uprawnienia",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Polisy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PojazdId = table.Column<int>(type: "int", nullable: false),
                    Typ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polisy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Polisy_Pojazdy_PojazdId",
                        column: x => x.PojazdId,
                        principalTable: "Pojazdy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Serwisy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PojazdId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrzebiegKm = table.Column<int>(type: "int", nullable: true),
                    Rodzaj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Koszt = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Serwisy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Serwisy_Pojazdy_PojazdId",
                        column: x => x.PojazdId,
                        principalTable: "Pojazdy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KartyPaliwowe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LimitMiesieczny = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Aktywna = table.Column<bool>(type: "bit", nullable: false),
                    KierowcaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KartyPaliwowe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KartyPaliwowe_Kierowcy_KierowcaId",
                        column: x => x.KierowcaId,
                        principalTable: "Kierowcy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Przypisania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PojazdId = table.Column<int>(type: "int", nullable: false),
                    KierowcaId = table.Column<int>(type: "int", nullable: false),
                    DataOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrybUzytkowania = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Przypisania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Przypisania_Kierowcy_KierowcaId",
                        column: x => x.KierowcaId,
                        principalTable: "Kierowcy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Przypisania_Pojazdy_PojazdId",
                        column: x => x.PojazdId,
                        principalTable: "Pojazdy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tankowania",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PojazdId = table.Column<int>(type: "int", nullable: false),
                    KierowcaId = table.Column<int>(type: "int", nullable: true),
                    KartaPaliwowaId = table.Column<int>(type: "int", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IloscLitrow = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CenaZaLitr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kwota = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrzebiegKm = table.Column<int>(type: "int", nullable: true),
                    PojazdId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tankowania", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tankowania_KartyPaliwowe_KartaPaliwowaId",
                        column: x => x.KartaPaliwowaId,
                        principalTable: "KartyPaliwowe",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tankowania_Kierowcy_KierowcaId",
                        column: x => x.KierowcaId,
                        principalTable: "Kierowcy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tankowania_Pojazdy_PojazdId1",
                        column: x => x.PojazdId1,
                        principalTable: "Pojazdy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tankowania_Tankowania_PojazdId",
                        column: x => x.PojazdId,
                        principalTable: "Tankowania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KartyPaliwowe_KierowcaId",
                table: "KartyPaliwowe",
                column: "KierowcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Kierowcy_Email",
                table: "Kierowcy",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kierowcy_UprawnieniaId",
                table: "Kierowcy",
                column: "UprawnieniaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pojazdy_KategoriaId",
                table: "Pojazdy",
                column: "KategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pojazdy_NumerRejestracyjny",
                table: "Pojazdy",
                column: "NumerRejestracyjny",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polisy_PojazdId",
                table: "Polisy",
                column: "PojazdId");

            migrationBuilder.CreateIndex(
                name: "IX_Przypisania_KierowcaId",
                table: "Przypisania",
                column: "KierowcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Przypisania_PojazdId",
                table: "Przypisania",
                column: "PojazdId");

            migrationBuilder.CreateIndex(
                name: "IX_Serwisy_PojazdId",
                table: "Serwisy",
                column: "PojazdId");

            migrationBuilder.CreateIndex(
                name: "IX_Tankowania_KartaPaliwowaId",
                table: "Tankowania",
                column: "KartaPaliwowaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tankowania_KierowcaId",
                table: "Tankowania",
                column: "KierowcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tankowania_PojazdId",
                table: "Tankowania",
                column: "PojazdId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tankowania_PojazdId1",
                table: "Tankowania",
                column: "PojazdId1");

            migrationBuilder.CreateIndex(
                name: "IX_Uzytkownicy_Id_Uprawnienia",
                table: "Uzytkownicy",
                column: "Id_Uprawnienia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kategoria");

            migrationBuilder.DropTable(
                name: "Polisy");

            migrationBuilder.DropTable(
                name: "Przypisania");

            migrationBuilder.DropTable(
                name: "Serwisy");

            migrationBuilder.DropTable(
                name: "Tankowania");

            migrationBuilder.DropTable(
                name: "Uzytkownicy");

            migrationBuilder.DropTable(
                name: "KartyPaliwowe");

            migrationBuilder.DropTable(
                name: "Pojazdy");

            migrationBuilder.DropTable(
                name: "Kierowcy");

            migrationBuilder.DropTable(
                name: "KategoriePojazdow");

            migrationBuilder.DropTable(
                name: "Uprawnienia");
        }
    }
}

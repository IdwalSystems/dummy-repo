using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddInitialTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaraBayar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaraBayar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jenis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jenis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KW",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KW", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modul",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuncId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FuncName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modul", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Negeri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negeri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkCarta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KWId = table.Column<int>(type: "int", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JenisId = table.Column<int>(type: "int", nullable: false),
                    ParasId = table.Column<int>(type: "int", nullable: false),
                    DebitKredit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UmumDetail = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Catatan1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Catatan2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkCarta", x => x.id);
                    table.ForeignKey(
                        name: "FK_AkCarta_Jenis_JenisId",
                        column: x => x.JenisId,
                        principalTable: "Jenis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkCarta_KW_KWId",
                        column: x => x.KWId,
                        principalTable: "KW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkCarta_Paras_ParasId",
                        column: x => x.ParasId,
                        principalTable: "Paras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkAkaun",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KWId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId1 = table.Column<int>(type: "int", nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AkCartaId2 = table.Column<int>(type: "int", nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkAkaun", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkAkaun_AkCarta_AkCartaId1",
                        column: x => x.AkCartaId1,
                        principalTable: "AkCarta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkAkaun_AkCarta_AkCartaId2",
                        column: x => x.AkCartaId2,
                        principalTable: "AkCarta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkAkaun_KW_KWId",
                        column: x => x.KWId,
                        principalTable: "KW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KWId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoAkaun = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBank_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkBank_Bank_BankId",
                        column: x => x.BankId,
                        principalTable: "Bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkBank_KW_KWId",
                        column: x => x.KWId,
                        principalTable: "KW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkTerima",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KWId = table.Column<int>(type: "int", nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    KodPembayar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoKp = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bandar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NegeriId = table.Column<int>(type: "int", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sebab = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AkAkaunId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTerima", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTerima_AkAkaun_AkAkaunId",
                        column: x => x.AkAkaunId,
                        principalTable: "AkAkaun",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTerima_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTerima_KW_KWId",
                        column: x => x.KWId,
                        principalTable: "KW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTerima_Negeri_NegeriId",
                        column: x => x.NegeriId,
                        principalTable: "Negeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pembekal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodSykt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NamaSykt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NoPendaftaran = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Alamat1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Bandar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NegeriId = table.Column<int>(type: "int", nullable: false),
                    Telefon1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AkaunBank = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AkBankId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pembekal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pembekal_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pembekal_Negeri_NegeriId",
                        column: x => x.NegeriId,
                        principalTable: "Negeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkTerima1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTerimaId = table.Column<int>(type: "int", nullable: false),
                    AkAkaunId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AkCartaid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTerima1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTerima1_AkAkaun_AkAkaunId",
                        column: x => x.AkAkaunId,
                        principalTable: "AkAkaun",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTerima1_AkCarta_AkCartaid",
                        column: x => x.AkCartaid,
                        principalTable: "AkCarta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTerima1_AkTerima_AkTerimaId",
                        column: x => x.AkTerimaId,
                        principalTable: "AkTerima",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkTerima2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTerimaId = table.Column<int>(type: "int", nullable: false),
                    CaraBayarId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoCek = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    JenisCek = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    KodBankCek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TempatCek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoSlip = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TarSlip = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTerima2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTerima2_AkTerima_AkTerimaId",
                        column: x => x.AkTerimaId,
                        principalTable: "AkTerima",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTerima2_CaraBayar_CaraBayarId",
                        column: x => x.CaraBayarId,
                        principalTable: "CaraBayar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoPO = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PembekalId = table.Column<int>(type: "int", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Posting = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    KWId = table.Column<int>(type: "int", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Batal = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PO_KW_KWId",
                        column: x => x.KWId,
                        principalTable: "KW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PO_Pembekal_PembekalId",
                        column: x => x.PembekalId,
                        principalTable: "Pembekal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PO1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PO1_PO_POId",
                        column: x => x.POId,
                        principalTable: "PO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PO2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POId = table.Column<int>(type: "int", nullable: false),
                    KWId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PO2_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PO2_KW_KWId",
                        column: x => x.KWId,
                        principalTable: "KW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PO2_PO_POId",
                        column: x => x.POId,
                        principalTable: "PO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkAkaun_AkCartaId1",
                table: "AkAkaun",
                column: "AkCartaId1");

            migrationBuilder.CreateIndex(
                name: "IX_AkAkaun_AkCartaId2",
                table: "AkAkaun",
                column: "AkCartaId2");

            migrationBuilder.CreateIndex(
                name: "IX_AkAkaun_KWId",
                table: "AkAkaun",
                column: "KWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBank_AkCartaId",
                table: "AkBank",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBank_BankId",
                table: "AkBank",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBank_KWId",
                table: "AkBank",
                column: "KWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCarta_JenisId",
                table: "AkCarta",
                column: "JenisId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCarta_KWId",
                table: "AkCarta",
                column: "KWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCarta_ParasId",
                table: "AkCarta",
                column: "ParasId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_AkAkaunId",
                table: "AkTerima",
                column: "AkAkaunId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_AkBankId",
                table: "AkTerima",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_KWId",
                table: "AkTerima",
                column: "KWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_NegeriId",
                table: "AkTerima",
                column: "NegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima1_AkAkaunId",
                table: "AkTerima1",
                column: "AkAkaunId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima1_AkCartaid",
                table: "AkTerima1",
                column: "AkCartaid");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima1_AkTerimaId",
                table: "AkTerima1",
                column: "AkTerimaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima2_AkTerimaId",
                table: "AkTerima2",
                column: "AkTerimaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima2_CaraBayarId",
                table: "AkTerima2",
                column: "CaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Pembekal_AkBankId",
                table: "Pembekal",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_Pembekal_NegeriId",
                table: "Pembekal",
                column: "NegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_PO_KWId",
                table: "PO",
                column: "KWId");

            migrationBuilder.CreateIndex(
                name: "IX_PO_PembekalId",
                table: "PO",
                column: "PembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_PO1_POId",
                table: "PO1",
                column: "POId");

            migrationBuilder.CreateIndex(
                name: "IX_PO2_AkCartaId",
                table: "PO2",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_PO2_KWId",
                table: "PO2",
                column: "KWId");

            migrationBuilder.CreateIndex(
                name: "IX_PO2_POId",
                table: "PO2",
                column: "POId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkTerima1");

            migrationBuilder.DropTable(
                name: "AkTerima2");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Modul");

            migrationBuilder.DropTable(
                name: "PO1");

            migrationBuilder.DropTable(
                name: "PO2");

            migrationBuilder.DropTable(
                name: "AkTerima");

            migrationBuilder.DropTable(
                name: "CaraBayar");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PO");

            migrationBuilder.DropTable(
                name: "AkAkaun");

            migrationBuilder.DropTable(
                name: "Pembekal");

            migrationBuilder.DropTable(
                name: "AkBank");

            migrationBuilder.DropTable(
                name: "Negeri");

            migrationBuilder.DropTable(
                name: "AkCarta");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Jenis");

            migrationBuilder.DropTable(
                name: "KW");

            migrationBuilder.DropTable(
                name: "Paras");
        }
    }
}

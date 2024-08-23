IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Gebruiker] (
    [Id] int NOT NULL IDENTITY,
    [Naam] nvarchar(max) NOT NULL,
    [Voornamen] nvarchar(max) NOT NULL,
    [Emailadres] nvarchar(max) NOT NULL,
    [Wachtwoord] nvarchar(max) NOT NULL,
    [Role] int NOT NULL,
    [Actief] bit NOT NULL,
    CONSTRAINT [PK_Gebruiker] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Actief', N'Emailadres', N'Naam', N'Role', N'Voornamen', N'Wachtwoord') AND [object_id] = OBJECT_ID(N'[Gebruiker]'))
    SET IDENTITY_INSERT [Gebruiker] ON;
INSERT INTO [Gebruiker] ([Id], [Actief], [Emailadres], [Naam], [Role], [Voornamen], [Wachtwoord])
VALUES (1, CAST(1 AS bit), N'test@gebruiker.nl', N'Gebruiker', 1, N'Eerste', N'$2a$11$PArrF7Nsejza2sfTHyxyOeRUTqHALFZPGfXGTz/ERXOIcYi9UNPe2');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Actief', N'Emailadres', N'Naam', N'Role', N'Voornamen', N'Wachtwoord') AND [object_id] = OBJECT_ID(N'[Gebruiker]'))
    SET IDENTITY_INSERT [Gebruiker] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240216111823_AddGebruikersTable', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Gebruiker]') AND [c].[name] = N'Wachtwoord');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Gebruiker] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Gebruiker] ALTER COLUMN [Wachtwoord] nvarchar(max) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Gebruiker]') AND [c].[name] = N'Voornamen');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Gebruiker] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Gebruiker] ALTER COLUMN [Voornamen] nvarchar(max) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Gebruiker]') AND [c].[name] = N'Naam');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Gebruiker] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Gebruiker] ALTER COLUMN [Naam] nvarchar(max) NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Gebruiker]') AND [c].[name] = N'Emailadres');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Gebruiker] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Gebruiker] ALTER COLUMN [Emailadres] nvarchar(max) NULL;
GO

CREATE TABLE [Eigenaar] (
    [Id] int NOT NULL IDENTITY,
    [Naam] nvarchar(max) NULL,
    [Voornamen] nvarchar(max) NULL,
    [Voorletters] nvarchar(max) NULL,
    [Straatnaam] nvarchar(max) NULL,
    [Huisnummer] int NOT NULL,
    [Toevoeging] nvarchar(max) NULL,
    [Postcode] nvarchar(max) NULL,
    [Woonplaats] nvarchar(max) NULL,
    [Debiteurnummer] nvarchar(max) NULL,
    [Ingangsdatum] datetime2 NOT NULL,
    [Einddatum] datetime2 NOT NULL,
    CONSTRAINT [PK_Eigenaar] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Eigendom] (
    [Id] int NOT NULL IDENTITY,
    [Relatienummer] nvarchar(max) NULL,
    [Ingangsdatum] datetime2 NOT NULL,
    [Einddatum] datetime2 NULL,
    [Complexnummer] nvarchar(max) NULL,
    [EconomischeWaarde] real NOT NULL,
    [VerzekerdeWaarde] real NOT NULL,
    [Notities] nvarchar(max) NULL,
    CONSTRAINT [PK_Eigendom] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Adres] (
    [Id] int NOT NULL IDENTITY,
    [EigendomId] int NULL,
    [Straatnaam] nvarchar(max) NULL,
    [Huisnummer] int NOT NULL,
    [Toevoeging] nvarchar(max) NULL,
    [Postcode] nvarchar(max) NULL,
    [Woonplaats] nvarchar(max) NULL,
    CONSTRAINT [PK_Adres] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Adres_Eigendom_EigendomId] FOREIGN KEY ([EigendomId]) REFERENCES [Eigendom] ([Id])
);
GO

CREATE TABLE [Bestand] (
    [Id] int NOT NULL IDENTITY,
    [EigendomId] int NULL,
    [Naam] nvarchar(max) NULL,
    [GrootteInKb] int NOT NULL,
    [SoortBestand] int NOT NULL,
    [Beschrijving] nvarchar(max) NULL,
    [Pad] nvarchar(max) NULL,
    CONSTRAINT [PK_Bestand] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bestand_Eigendom_EigendomId] FOREIGN KEY ([EigendomId]) REFERENCES [Eigendom] ([Id])
);
GO

CREATE TABLE [EigenaarEigendom] (
    [EigenaarId] int NOT NULL,
    [EigendomId] int NOT NULL,
    CONSTRAINT [PK_EigenaarEigendom] PRIMARY KEY ([EigenaarId], [EigendomId]),
    CONSTRAINT [FK_EigenaarEigendom_Eigenaar_EigenaarId] FOREIGN KEY ([EigenaarId]) REFERENCES [Eigenaar] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_EigenaarEigendom_Eigendom_EigendomId] FOREIGN KEY ([EigendomId]) REFERENCES [Eigendom] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Herziening] (
    [Id] int NOT NULL IDENTITY,
    [EigendomId] int NULL,
    [Herzieningsdatum] datetime2 NOT NULL,
    [VolgendeHerziening] int NOT NULL,
    CONSTRAINT [PK_Herziening] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Herziening_Eigendom_EigendomId] FOREIGN KEY ([EigendomId]) REFERENCES [Eigendom] ([Id])
);
GO

CREATE TABLE [Kadaster] (
    [Id] int NOT NULL IDENTITY,
    [EigendomId] int NULL,
    [KadastraalNummer] nvarchar(max) NULL,
    [Deeloppervlakte] real NOT NULL,
    [KadastraleGrootte] real NOT NULL,
    [ObjectType] nvarchar(max) NULL,
    CONSTRAINT [PK_Kadaster] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Kadaster_Eigendom_EigendomId] FOREIGN KEY ([EigendomId]) REFERENCES [Eigendom] ([Id])
);
GO

CREATE TABLE [Overeenkomst] (
    [Id] int NOT NULL IDENTITY,
    [EigendomId] int NULL,
    [Dossiernummer] nvarchar(max) NULL,
    [Ingangsdatum] datetime2 NOT NULL,
    [Einddatum] datetime2 NOT NULL,
    [Grondwaarde] real NOT NULL,
    [DatumAkte] datetime2 NOT NULL,
    [Rentepercentage] real NOT NULL,
    CONSTRAINT [PK_Overeenkomst] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Overeenkomst_Eigendom_EigendomId] FOREIGN KEY ([EigendomId]) REFERENCES [Eigendom] ([Id])
);
GO

CREATE TABLE [Financien] (
    [Id] int NOT NULL IDENTITY,
    [OvereenkomstId] int NOT NULL,
    [Ingangsdatum] datetime2 NOT NULL,
    [Einddatum] datetime2 NOT NULL,
    [Bedrag] real NOT NULL,
    [FactureringsWijze] int NOT NULL,
    [Frequentie] int NOT NULL,
    CONSTRAINT [PK_Financien] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Financien_Overeenkomst_OvereenkomstId] FOREIGN KEY ([OvereenkomstId]) REFERENCES [Overeenkomst] ([Id]) ON DELETE CASCADE
);
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$uJIJxdKuWzAFa/0D9KZTm.GkAu07jCJH9SuDTkDLJI4cO6VBsHCIG'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

CREATE UNIQUE INDEX [IX_Adres_EigendomId] ON [Adres] ([EigendomId]) WHERE [EigendomId] IS NOT NULL;
GO

CREATE INDEX [IX_Bestand_EigendomId] ON [Bestand] ([EigendomId]);
GO

CREATE INDEX [IX_EigenaarEigendom_EigendomId] ON [EigenaarEigendom] ([EigendomId]);
GO

CREATE UNIQUE INDEX [IX_Financien_OvereenkomstId] ON [Financien] ([OvereenkomstId]);
GO

CREATE UNIQUE INDEX [IX_Herziening_EigendomId] ON [Herziening] ([EigendomId]) WHERE [EigendomId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Kadaster_EigendomId] ON [Kadaster] ([EigendomId]) WHERE [EigendomId] IS NOT NULL;
GO

CREATE INDEX [IX_Overeenkomst_EigendomId] ON [Overeenkomst] ([EigendomId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240301105841_AddEigendomData', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eigendom]') AND [c].[name] = N'VerzekerdeWaarde');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Eigendom] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Eigendom] ALTER COLUMN [VerzekerdeWaarde] real NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eigendom]') AND [c].[name] = N'EconomischeWaarde');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Eigendom] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Eigendom] ALTER COLUMN [EconomischeWaarde] real NULL;
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$6hUN2Td4kL211a3WHlx6F.2pO/qLF/whuAULguAUnv7Wln8k31ilm'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240301125132_EditEigendomNullvelden', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eigenaar]') AND [c].[name] = N'Woonplaats');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Eigenaar] DROP CONSTRAINT [' + @var6 + '];');
UPDATE [Eigenaar] SET [Woonplaats] = N'' WHERE [Woonplaats] IS NULL;
ALTER TABLE [Eigenaar] ALTER COLUMN [Woonplaats] nvarchar(max) NOT NULL;
ALTER TABLE [Eigenaar] ADD DEFAULT N'' FOR [Woonplaats];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eigenaar]') AND [c].[name] = N'Voorletters');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Eigenaar] DROP CONSTRAINT [' + @var7 + '];');
UPDATE [Eigenaar] SET [Voorletters] = N'' WHERE [Voorletters] IS NULL;
ALTER TABLE [Eigenaar] ALTER COLUMN [Voorletters] nvarchar(max) NOT NULL;
ALTER TABLE [Eigenaar] ADD DEFAULT N'' FOR [Voorletters];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eigenaar]') AND [c].[name] = N'Straatnaam');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Eigenaar] DROP CONSTRAINT [' + @var8 + '];');
UPDATE [Eigenaar] SET [Straatnaam] = N'' WHERE [Straatnaam] IS NULL;
ALTER TABLE [Eigenaar] ALTER COLUMN [Straatnaam] nvarchar(max) NOT NULL;
ALTER TABLE [Eigenaar] ADD DEFAULT N'' FOR [Straatnaam];
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eigenaar]') AND [c].[name] = N'Postcode');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Eigenaar] DROP CONSTRAINT [' + @var9 + '];');
UPDATE [Eigenaar] SET [Postcode] = N'' WHERE [Postcode] IS NULL;
ALTER TABLE [Eigenaar] ALTER COLUMN [Postcode] nvarchar(max) NOT NULL;
ALTER TABLE [Eigenaar] ADD DEFAULT N'' FOR [Postcode];
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eigenaar]') AND [c].[name] = N'Naam');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Eigenaar] DROP CONSTRAINT [' + @var10 + '];');
UPDATE [Eigenaar] SET [Naam] = N'' WHERE [Naam] IS NULL;
ALTER TABLE [Eigenaar] ALTER COLUMN [Naam] nvarchar(max) NOT NULL;
ALTER TABLE [Eigenaar] ADD DEFAULT N'' FOR [Naam];
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Eigenaar]') AND [c].[name] = N'Einddatum');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Eigenaar] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Eigenaar] ALTER COLUMN [Einddatum] datetime2 NULL;
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$kOE.FcxI00N2lAvFHx0Am.cnH6XS5CxQ8A8RE9Bu8ayUblliRe1N.'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240311101731_EditEigenaarNullableFields', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Kadaster] ADD [LaatsteSynchronisatie] datetime2 NULL;
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$XJSlCEqoKKfEEc8jyU/gDOriA3796G3R25Do6p0gLBSO1zyz4SB9a'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240318084444_EditKadasterAddLaatsteSync', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Bestand]') AND [c].[name] = N'GrootteInKb');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Bestand] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Bestand] ALTER COLUMN [GrootteInKb] bigint NOT NULL;
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$bZsjEqm5qrUZrJvTmje33e/RaVW3.G6Bm6RJzmteOtDE5Sj6XgVFq'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240318112635_EditBestandModBestandgr', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Overeenkomst]') AND [c].[name] = N'Rentepercentage');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Overeenkomst] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Overeenkomst] ALTER COLUMN [Rentepercentage] real NULL;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Overeenkomst]') AND [c].[name] = N'Grondwaarde');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Overeenkomst] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Overeenkomst] ALTER COLUMN [Grondwaarde] real NULL;
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Overeenkomst]') AND [c].[name] = N'Einddatum');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Overeenkomst] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Overeenkomst] ALTER COLUMN [Einddatum] datetime2 NULL;
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Overeenkomst]') AND [c].[name] = N'DatumAkte');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Overeenkomst] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [Overeenkomst] ALTER COLUMN [DatumAkte] datetime2 NULL;
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$ApMMa.vvGc6.u1mjrD1mYuibT2bL3HgztS1hLhYrcKVT13BJjOL3W'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240322125646_EditOvereenkomstNullableFields', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Financien]') AND [c].[name] = N'Einddatum');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Financien] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [Financien] DROP COLUMN [Einddatum];
GO

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Financien]') AND [c].[name] = N'Ingangsdatum');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [Financien] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [Financien] DROP COLUMN [Ingangsdatum];
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$6DkF7BUijzRSiPxvKgDQ2uy9EAUNeheheP68uMSnH6zx6d.3hFDiy'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240327192825_EditFinancienRemoveDatums', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Gebruiker] ADD [LogingPoging] int NOT NULL DEFAULT 0;
GO

UPDATE [Gebruiker] SET [LogingPoging] = 0, [Wachtwoord] = N'$2a$11$Z8YcmKs6xz3hPbHRTNDVUOq60htPap6TAi1syz8FWGf1sUr4CIREK'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240412143002_EditGebruikerAddLoginPoging', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Gebruiker] ADD [ExportId] int NULL;
GO

CREATE TABLE [Export] (
    [Id] int NOT NULL IDENTITY,
    [Formaat] int NOT NULL,
    [AanmaakDatum] datetime2 NOT NULL,
    CONSTRAINT [PK_Export] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TaskQueue] (
    [Id] int NOT NULL IDENTITY,
    [SoortTaak] int NOT NULL,
    [Status] int NOT NULL,
    [Prioriteit] int NOT NULL,
    [AanmaakDatum] datetime2 NOT NULL,
    [AfgerondDatum] datetime2 NOT NULL,
    [ExportId] int NULL,
    CONSTRAINT [PK_TaskQueue] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TaskQueue_Export_ExportId] FOREIGN KEY ([ExportId]) REFERENCES [Export] ([Id])
);
GO

CREATE TABLE [Template] (
    [Id] int NOT NULL IDENTITY,
    [Naam] nvarchar(max) NOT NULL,
    [Maker] nvarchar(max) NOT NULL,
    [Model] nvarchar(max) NOT NULL,
    [Filters] nvarchar(max) NULL,
    [ExportId] int NULL,
    [AanmaakDatum] datetime2 NOT NULL,
    [WijzigingsDatum] datetime2 NOT NULL,
    CONSTRAINT [PK_Template] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Template_Export_ExportId] FOREIGN KEY ([ExportId]) REFERENCES [Export] ([Id])
);
GO

CREATE TABLE [RapportData] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NOT NULL,
    [Naam] nvarchar(max) NOT NULL,
    [TemplateId] int NOT NULL,
    CONSTRAINT [PK_RapportData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RapportData_Template_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template] ([Id]) ON DELETE CASCADE
);
GO

UPDATE [Gebruiker] SET [ExportId] = NULL, [Wachtwoord] = N'$2a$11$SFtGvsdL7IZ6Wm..I3DSreEJlCfMztpOyVkehh7cdd.RLjbwEibX2'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

CREATE UNIQUE INDEX [IX_Gebruiker_ExportId] ON [Gebruiker] ([ExportId]) WHERE [ExportId] IS NOT NULL;
GO

CREATE INDEX [IX_RapportData_TemplateId] ON [RapportData] ([TemplateId]);
GO

CREATE UNIQUE INDEX [IX_TaskQueue_ExportId] ON [TaskQueue] ([ExportId]) WHERE [ExportId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Template_ExportId] ON [Template] ([ExportId]) WHERE [ExportId] IS NOT NULL;
GO

ALTER TABLE [Gebruiker] ADD CONSTRAINT [FK_Gebruiker_Export_ExportId] FOREIGN KEY ([ExportId]) REFERENCES [Export] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240416113130_AddExportTemplateModels', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Template]') AND [c].[name] = N'Filters');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [Template] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [Template] DROP COLUMN [Filters];
GO

CREATE TABLE [Filter] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(max) NOT NULL,
    [Operation] int NOT NULL,
    [Value] nvarchar(max) NOT NULL,
    [TemplateId] int NOT NULL,
    CONSTRAINT [PK_Filter] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Filter_Template_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [Template] ([Id]) ON DELETE CASCADE
);
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$SOlz6ulWACMQo6xrytW6MuOcZ0bTE/XplzVKw2CPm14ZaG3IFaHAK'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

CREATE INDEX [IX_Filter_TemplateId] ON [Filter] ([TemplateId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240422132758_EditTemplateAddFiltersRel', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [TaskQueue] ADD [Fout] nvarchar(max) NULL;
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$AFO0TL559owZKMXgNu0qneQ8N2/3jiFBGCTe1pBvwInzTNv.G9/AS'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240429141734_EditTaskQueueAddFoutmelding', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Export] ADD [ExportPad] nvarchar(max) NULL;
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$QkqRfsIjwS26DMlJkbLTae3pw8Cek3vx.8j6oiF3FZZ1eBy.BHKz6'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240503123808_EditExportAddExportPad', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [TaskQueue] ADD [ImportId] int NULL;
GO

ALTER TABLE [Gebruiker] ADD [ImportId] int NULL;
GO

CREATE TABLE [Import] (
    [Id] int NOT NULL IDENTITY,
    [Aanmaakdatum] datetime2 NOT NULL,
    [WijzigingsDatum] datetime2 NOT NULL,
    [importPad] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Import] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TranslateModel] (
    [Id] int NOT NULL IDENTITY,
    [Naam] nvarchar(max) NOT NULL,
    [Maker] nvarchar(max) NOT NULL,
    [Model] nvarchar(max) NOT NULL,
    [ImportId] int NULL,
    [AanmaakDatum] datetime2 NOT NULL,
    [WijzigingsDatum] datetime2 NULL,
    CONSTRAINT [PK_TranslateModel] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TranslateModel_Import_ImportId] FOREIGN KEY ([ImportId]) REFERENCES [Import] ([Id])
);
GO

CREATE TABLE [Translation] (
    [Id] int NOT NULL IDENTITY,
    [CSVColummnName] nvarchar(max) NOT NULL,
    [ModelColumnName] nvarchar(max) NOT NULL,
    [TranslateModelId] int NULL,
    CONSTRAINT [PK_Translation] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Translation_TranslateModel_TranslateModelId] FOREIGN KEY ([TranslateModelId]) REFERENCES [TranslateModel] ([Id])
);
GO

UPDATE [Gebruiker] SET [ImportId] = NULL, [Wachtwoord] = N'$2a$11$QcbyWzSwQRFWx5EBlxG2Q.ICvieefhwsv6SQjmltWOWlxoUSKkjry'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

CREATE UNIQUE INDEX [IX_TaskQueue_ImportId] ON [TaskQueue] ([ImportId]) WHERE [ImportId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Gebruiker_ImportId] ON [Gebruiker] ([ImportId]) WHERE [ImportId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_TranslateModel_ImportId] ON [TranslateModel] ([ImportId]) WHERE [ImportId] IS NOT NULL;
GO

CREATE INDEX [IX_Translation_TranslateModelId] ON [Translation] ([TranslateModelId]);
GO

ALTER TABLE [Gebruiker] ADD CONSTRAINT [FK_Gebruiker_Import_ImportId] FOREIGN KEY ([ImportId]) REFERENCES [Import] ([Id]);
GO

ALTER TABLE [TaskQueue] ADD CONSTRAINT [FK_TaskQueue_Import_ImportId] FOREIGN KEY ([ImportId]) REFERENCES [Import] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240522110849_AddImportModels', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [TaskQueue] ADD [FactuurJobId] int NULL;
GO

ALTER TABLE [Gebruiker] ADD [FactuurJobId] int NULL;
GO

ALTER TABLE [Financien] ADD [FactureringsPeriode] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [FactuurJob] (
    [Id] int NOT NULL IDENTITY,
    [AanmaakDatum] datetime2 NOT NULL,
    [AfrondDatum] datetime2 NOT NULL,
    [FactureringsPeriode] int NOT NULL,
    CONSTRAINT [PK_FactuurJob] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Factuur] (
    [Id] int NOT NULL IDENTITY,
    [FinancienId] int NULL,
    [EigenaarId] int NOT NULL,
    [Datum] datetime2 NOT NULL,
    [Nummer] nvarchar(max) NOT NULL,
    [Bedrag] real NOT NULL,
    [FactuurJobId] int NOT NULL,
    CONSTRAINT [PK_Factuur] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Factuur_Eigenaar_EigenaarId] FOREIGN KEY ([EigenaarId]) REFERENCES [Eigenaar] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Factuur_FactuurJob_FactuurJobId] FOREIGN KEY ([FactuurJobId]) REFERENCES [FactuurJob] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Factuur_Financien_FinancienId] FOREIGN KEY ([FinancienId]) REFERENCES [Financien] ([Id])
);
GO

CREATE TABLE [FactuurRegels] (
    [Id] int NOT NULL IDENTITY,
    [Aantal] int NOT NULL,
    [Beschrijving] nvarchar(max) NOT NULL,
    [Prijs] real NOT NULL,
    [Totaal] real NOT NULL,
    [FactuurId] int NOT NULL,
    CONSTRAINT [PK_FactuurRegels] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FactuurRegels_Factuur_FactuurId] FOREIGN KEY ([FactuurId]) REFERENCES [Factuur] ([Id]) ON DELETE CASCADE
);
GO

UPDATE [Gebruiker] SET [FactuurJobId] = NULL, [Wachtwoord] = N'$2a$11$PER98YhJOWbpEIgsN/7xRew5tuZCv00foOUJCEvh1SlRpHK5bCuka'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

CREATE UNIQUE INDEX [IX_TaskQueue_FactuurJobId] ON [TaskQueue] ([FactuurJobId]) WHERE [FactuurJobId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Gebruiker_FactuurJobId] ON [Gebruiker] ([FactuurJobId]) WHERE [FactuurJobId] IS NOT NULL;
GO

CREATE INDEX [IX_Factuur_EigenaarId] ON [Factuur] ([EigenaarId]);
GO

CREATE INDEX [IX_Factuur_FactuurJobId] ON [Factuur] ([FactuurJobId]);
GO

CREATE INDEX [IX_Factuur_FinancienId] ON [Factuur] ([FinancienId]);
GO

CREATE INDEX [IX_FactuurRegels_FactuurId] ON [FactuurRegels] ([FactuurId]);
GO

ALTER TABLE [Gebruiker] ADD CONSTRAINT [FK_Gebruiker_FactuurJob_FactuurJobId] FOREIGN KEY ([FactuurJobId]) REFERENCES [FactuurJob] ([Id]);
GO

ALTER TABLE [TaskQueue] ADD CONSTRAINT [FK_TaskQueue_FactuurJob_FactuurJobId] FOREIGN KEY ([FactuurJobId]) REFERENCES [FactuurJob] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240603091142_AddInvoicingModels', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [FactuurJob] ADD [StoragePad] nvarchar(max) NOT NULL DEFAULT N'';
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$DCJD93gvt7vjre.Xw8u6U.sNS9hrz40ksy8fhEI49jhaGCI.nTMP2'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240603135545_AddStoragePadFctJob', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Adres] ADD [Huisletter] nvarchar(max) NULL;
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$SntTFq0Tb38YnUySOTtMpeCX.8FLxSnz/z4HuDi9//qNOw9RBKl3y'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240723125650_EditAdresAddHuisletter', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Settings] (
    [Id] int NOT NULL IDENTITY,
    [BAGAPI] nvarchar(max) NULL,
    CONSTRAINT [PK_Settings] PRIMARY KEY ([Id])
);
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$didHBqcx3BOvAVvl5izQHOwU37giXZ3yWHOVTJTlR5vAJp3Enb0fu'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BAGAPI') AND [object_id] = OBJECT_ID(N'[Settings]'))
    SET IDENTITY_INSERT [Settings] ON;
INSERT INTO [Settings] ([Id], [BAGAPI])
VALUES (1, N'');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BAGAPI') AND [object_id] = OBJECT_ID(N'[Settings]'))
    SET IDENTITY_INSERT [Settings] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240723130616_AddSettings', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[Kadaster].[ObjectType]', N'Gebruiksdoel', N'COLUMN';
GO

EXEC sp_rename N'[Kadaster].[KadastraleGrootte]', N'Oppervlakte', N'COLUMN';
GO

EXEC sp_rename N'[Kadaster].[KadastraalNummer]', N'BAGID', N'COLUMN';
GO

EXEC sp_rename N'[Kadaster].[Deeloppervlakte]', N'Bouwjaar', N'COLUMN';
GO

UPDATE [Gebruiker] SET [Wachtwoord] = N'$2a$11$Uc5.4aivaWigaWNIFNC7ceQmgjlxu1kXf71Rf3CRiVBlgh.ZTKcaq'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240726074320_EditKadasterStructure', N'8.0.7');
GO

COMMIT;
GO


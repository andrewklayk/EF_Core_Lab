IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE TABLE [Customers] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([ID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE TABLE [PartType] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Units] nvarchar(max) NULL,
        CONSTRAINT [PK_PartType] PRIMARY KEY ([ID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE TABLE [Stocks] (
        [ID] int NOT NULL IDENTITY,
        [Address] nvarchar(max) NULL,
        CONSTRAINT [PK_Stocks] PRIMARY KEY ([ID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE TABLE [Parts] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [Type] int NOT NULL,
        [PartTypeID] int NULL,
        CONSTRAINT [PK_Parts] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_Parts_PartType_PartTypeID] FOREIGN KEY ([PartTypeID]) REFERENCES [PartType] ([ID]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE TABLE [Employees] (
        [ID] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NULL,
        [Surname] nvarchar(max) NULL,
        [StockID] int NOT NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_Employees_Stocks_StockID] FOREIGN KEY ([StockID]) REFERENCES [Stocks] ([ID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE TABLE [Contracts] (
        [ID] int NOT NULL IDENTITY,
        [CustomerID] int NOT NULL,
        [SignDate] datetime2 NOT NULL,
        [PartID] int NOT NULL,
        CONSTRAINT [PK_Contracts] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_Contracts_Parts_PartID] FOREIGN KEY ([PartID]) REFERENCES [Parts] ([ID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE TABLE [Lefts] (
        [ID] int NOT NULL IDENTITY,
        [PartID] int NOT NULL,
        [StockID] int NOT NULL,
        [ArrDate] datetime2 NOT NULL,
        [Quantity] int NOT NULL,
        CONSTRAINT [PK_Lefts] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_Lefts_Parts_PartID] FOREIGN KEY ([PartID]) REFERENCES [Parts] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Lefts_Stocks_StockID] FOREIGN KEY ([StockID]) REFERENCES [Stocks] ([ID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE INDEX [IX_Contracts_PartID] ON [Contracts] ([PartID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE INDEX [IX_Employees_StockID] ON [Employees] ([StockID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE INDEX [IX_Lefts_PartID] ON [Lefts] ([PartID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE INDEX [IX_Lefts_StockID] ON [Lefts] ([StockID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    CREATE INDEX [IX_Parts_PartTypeID] ON [Parts] ([PartTypeID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190504181830_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190504181830_InitialCreate', N'2.1.11-servicing-32099');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181521_ModifyStockEmplScheme')
BEGIN
    ALTER TABLE [Employees] DROP CONSTRAINT [FK_Employees_Stocks_StockID];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181521_ModifyStockEmplScheme')
BEGIN
    DROP INDEX [IX_Employees_StockID] ON [Employees];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181521_ModifyStockEmplScheme')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'StockID');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Employees] DROP COLUMN [StockID];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181521_ModifyStockEmplScheme')
BEGIN
    ALTER TABLE [Employees] ADD [PositionID] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181521_ModifyStockEmplScheme')
BEGIN
    CREATE TABLE [Positions] (
        [ID] int NOT NULL IDENTITY,
        [EmployeeID] int NOT NULL,
        [StockID] int NOT NULL,
        CONSTRAINT [PK_Positions] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_Positions_Employees_EmployeeID] FOREIGN KEY ([EmployeeID]) REFERENCES [Employees] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Positions_Stocks_StockID] FOREIGN KEY ([StockID]) REFERENCES [Stocks] ([ID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181521_ModifyStockEmplScheme')
BEGIN
    CREATE INDEX [IX_Positions_EmployeeID] ON [Positions] ([EmployeeID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181521_ModifyStockEmplScheme')
BEGIN
    CREATE INDEX [IX_Positions_StockID] ON [Positions] ([StockID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181521_ModifyStockEmplScheme')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190506181521_ModifyStockEmplScheme', N'2.1.11-servicing-32099');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181753_PosIdInEmplRemoved')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'PositionID');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Employees] DROP COLUMN [PositionID];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190506181753_PosIdInEmplRemoved')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190506181753_PosIdInEmplRemoved', N'2.1.11-servicing-32099');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    ALTER TABLE [Contracts] DROP CONSTRAINT [FK_Contracts_Parts_PartID];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    ALTER TABLE [Parts] DROP CONSTRAINT [FK_Parts_PartType_PartTypeID];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    DROP INDEX [IX_Contracts_PartID] ON [Contracts];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    ALTER TABLE [PartType] DROP CONSTRAINT [PK_PartType];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Contracts]') AND [c].[name] = N'PartID');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Contracts] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Contracts] DROP COLUMN [PartID];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    EXEC sp_rename N'[PartType]', N'PartTypes';
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    ALTER TABLE [PartTypes] ADD CONSTRAINT [PK_PartTypes] PRIMARY KEY ([ID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    CREATE TABLE [ContractItems] (
        [ID] int NOT NULL IDENTITY,
        [ContractID] int NOT NULL,
        [PartID] int NOT NULL,
        [Quantity] int NOT NULL,
        CONSTRAINT [PK_ContractItems] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_ContractItems_Contracts_ContractID] FOREIGN KEY ([ContractID]) REFERENCES [Contracts] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_ContractItems_Parts_PartID] FOREIGN KEY ([PartID]) REFERENCES [Parts] ([ID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    CREATE INDEX [IX_Contracts_CustomerID] ON [Contracts] ([CustomerID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    CREATE INDEX [IX_ContractItems_ContractID] ON [ContractItems] ([ContractID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    CREATE INDEX [IX_ContractItems_PartID] ON [ContractItems] ([PartID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    ALTER TABLE [Contracts] ADD CONSTRAINT [FK_Contracts_Customers_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [Customers] ([ID]) ON DELETE CASCADE;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    ALTER TABLE [Parts] ADD CONSTRAINT [FK_Parts_PartTypes_PartTypeID] FOREIGN KEY ([PartTypeID]) REFERENCES [PartTypes] ([ID]) ON DELETE NO ACTION;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190507144432_ContractChangeMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190507144432_ContractChangeMigration', N'2.1.11-servicing-32099');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190910154125_2')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Parts]') AND [c].[name] = N'Name');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Parts] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Parts] ALTER COLUMN [Name] nvarchar(max) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190910154125_2')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'Surname');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Employees] ALTER COLUMN [Surname] nvarchar(10) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190910154125_2')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employees]') AND [c].[name] = N'FirstName');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Employees] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Employees] ALTER COLUMN [FirstName] nvarchar(max) NOT NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190910154125_2')
BEGIN
    CREATE TABLE [Users] (
        [ID] int NOT NULL IDENTITY,
        [Username] nvarchar(max) NULL,
        [Password] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([ID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190910154125_2')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190910154125_2', N'2.1.11-servicing-32099');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190910154403_412')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190910154403_412', N'2.1.11-servicing-32099');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20190930082854_a')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190930082854_a', N'2.1.11-servicing-32099');
END;

GO


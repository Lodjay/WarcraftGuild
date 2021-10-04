CREATE TABLE [dbo].[Realm]
(
	[RealmId] BIGINT NOT NULL PRIMARY KEY, 
    [BlizzardRealmId] BIGINT NOT NULL, 
    [Region] BIGINT NOT NULL, 
    [Name] NVARCHAR(50) NULL, 
    [IsTournament] BIT NOT NULL DEFAULT 0, 
    [Slug] VARCHAR(50) NULL, 
    [Category] NVARCHAR(50) NULL, 
    [Locale] VARCHAR(50) NULL, 
    [Type] VARCHAR(50) NULL, 
    [TimeZone] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Realm_To_Region] FOREIGN KEY ([Region]) REFERENCES [Region]([RegionId])
)

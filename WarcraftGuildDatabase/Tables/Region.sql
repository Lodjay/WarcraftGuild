CREATE TABLE [dbo].[Region]
(
	[RegionId] BIGINT NOT NULL PRIMARY KEY, 
    [BlizzardRegionId] BIGINT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Tag] VARCHAR(2) NOT NULL
)

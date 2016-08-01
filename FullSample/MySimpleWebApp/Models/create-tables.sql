SELECT @@VERSION
SELECT @@SERVERNAME
EXEC sp_databases
SELECT DB_NAME()
EXEC sp_helpfile



IF OBJECT_ID('MyUser', 'U') IS NOT NULL
	DROP TABLE [dbo].[MyUser]

IF OBJECT_ID('Movie', 'U') IS NOT NULL
	DROP TABLE [dbo].[Movie]



CREATE TABLE [dbo].[MyUser]
(
	[Id]				NVARCHAR(50)		NOT NULL, 
    [Name]				NVARCHAR(100)		NULL, 
    [Address]			NVARCHAR(100)		NULL, 
    [PhoneNumber]		NVARCHAR(50)		NULL, 
    [BirthDate]			DATETIME2			NULL,

	CONSTRAINT [PK_MyUser] PRIMARY KEY CLUSTERED ([ID] ASC)
)



CREATE TABLE [dbo].[Movie] (
    [ID]				INT					IDENTITY (1, 1) NOT NULL,
    [Title]				NVARCHAR (100)		NULL,
    [ReleaseDate]		DATETIME			NOT NULL,
    [Genre]				NVARCHAR (100)		NULL,
    [Price]				DECIMAL (18, 2)		NOT NULL,
    [ThumbnailImgUrl]	NVARCHAR (1000)		NULL,
	[AuthorId]			NVARCHAR(50)		NULL,
    
	CONSTRAINT [PK_Movie] PRIMARY KEY CLUSTERED ([ID] ASC)
);



SELECT * FROM information_schema.tables
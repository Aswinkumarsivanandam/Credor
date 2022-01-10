-- TABLE DROP SCRIPTS
/*IF OBJECT_ID (N'Notifications', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.Notifications
END
IF OBJECT_ID (N'CredorEmailTemplate', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.CredorEmailTemplate
END
*/
-- CREATE TABLE SCRIPTS
IF OBJECT_ID (N'Notifications', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Notifications
(
	Id INT NOT NULL CONSTRAINT PK_Notifications PRIMARY KEY (Id) IDENTITY (1,1),
	UserId INT NULL CONSTRAINT FK_Notifications_UserAccount FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id),
	Title VARCHAR(500) NOT NULL,
	[Message] VARCHAR(1000) NOT NULL,
	[Status] INT NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
	[Type] INT NULL,
	CreatedBy VARCHAR(50) NULL,
	CreatedOn DATETIME NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
)
END

IF OBJECT_ID (N'CredorEmailTemplate', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.CredorEmailTemplate
(
	Id INT NOT NULL CONSTRAINT PK_CredorEmailTemplate PRIMARY KEY (Id) IDENTITY (1,1),		
	[Subject] VARCHAR(MAX) NOT NULL,
	BodyContent VARCHAR(MAX) NOT NULL,
	[Status] INT NOT NULL,
	TemplateName VARCHAR(100) NOT NULL
)
END

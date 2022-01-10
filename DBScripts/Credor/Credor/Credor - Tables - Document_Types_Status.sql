
-----------------------------------------------------------------------------------------------------------------------------------------------
/*	CREATE TABLE SCRIPTS 

	1. DocumentTypes
	2. DocumentStatus
	3. Document
	4. DocumentNameDelimiter
	5. DocumentNamePosition
	6. DocumentNameSeperator
	7. DocumentBatchDetail

*/
-----------------------------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID (N'DocumentTypes', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.DocumentTypes
END
ELSE
BEGIN
	CREATE TABLE dbo.DocumentTypes
(
	Id INT NOT NULL CONSTRAINT PK_DocumentTypes PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
	Visibility BIT NOT NULL DEFAULT 1
)
END
DELETE FROM dbo.DocumentTypes

INSERT dbo.DocumentTypes ([Name],Active) 
VALUES 
('Tax',1,1),
('Subscriptions',1,1),
('Accreditation',1,1),
('Offering Documents',1,1),
('Miscellaneous',1,1),
('Welcome Document',1,0),
('UserProfileImage',1,0),
('eSigned Documents',1,0),
('GalleryImages',1,0)


SELECT * FROM dbo.DocumentTypes
-----------------------------------------------------------------------------------------------------------------------------------------------

IF OBJECT_ID (N'DocumentStatus', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.DocumentStatus
END
ELSE
BEGIN
	CREATE TABLE dbo.DocumentStatus
(
	Id INT NOT NULL CONSTRAINT PK_DocumentStatus PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,
	Active BIT NOT NULL DEFAULT 1
)
END

DELETE FROM dbo.DocumentStatus

INSERT dbo.DocumentStatus ([Name],Active) 
VALUES 
('Pending',1),
('Manual Signature',1),
('Approved',1)

SELECT * FROM dbo.DocumentStatus

-----------------------------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID (N'Document', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.Document
END
ELSE
BEGIN
	CREATE TABLE dbo.Document
(
	Id INT NOT NULL CONSTRAINT PK_Document PRIMARY KEY (Id) IDENTITY (1,1),
	UserId INT NULL CONSTRAINT FK_UserAccount_Document FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id),
	ProfileId INT NULL CONSTRAINT FK_UserProfile_Document FOREIGN KEY (ProfileId) REFERENCES dbo.UserProfile(Id),
	OfferingId INT NULL CONSTRAINT FK_PortfolioOffering_Document FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id), 
	BatchId INT NULL CONSTRAINT FK_DocumentBatchDetail_Document FOREIGN KEY (BatchId) REFERENCES dbo.DocumentBatchDetail(Id),
	InvestmentId INT NULL CONSTRAINT FK_Investment_Document FOREIGN KEY (InvestmentId) REFERENCES dbo.Investment(Id),
	[Type] INT NOT NULL CONSTRAINT FK_DocumentTypes_Document FOREIGN KEY ([Type]) REFERENCES dbo.DocumentTypes(Id), 	
	[Name] VARCHAR(MAX) NOT NULL,
	FilePath VARCHAR(MAX) NOT NULL,
	Extension  VARCHAR(10) NULL,
	Size VARCHAR(MAX) NULL,
	IsPrivate BIT NULL,
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(100) NULL,
	ApprovedOn DATETIME NULL,
	ApprovedBy VARCHAR(50) NULL	
)
END


CREATE CLUSTERED INDEX Index_Document_OfferingId
ON dbo.Document (OfferingId);

-----------------------------------------------------------------------------------------------------------------------------------------------

IF OBJECT_ID (N'DocumentNameDelimiter', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.DocumentNameDelimiter
END
ELSE
BEGIN
	CREATE TABLE dbo.DocumentNameDelimiter
(
	Id INT NOT NULL CONSTRAINT PK_DocumentNameDelimiter PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,
	[Value] VARCHAR(10) NOT NULL,
	Active BIT NOT NULL DEFAULT 1
)
END

DELETE FROM dbo.DocumentNameDelimiter

INSERT dbo.DocumentNameDelimiter ([Name],[Value],Active) 
VALUES 
('Underscore (_)','_',1),
('Hyphen (-)','-',1)

SELECT * FROM dbo.DocumentNameDelimiter

-----------------------------------------------------------------------------------------------------------------------------------------------

IF OBJECT_ID (N'DocumentNamePosition', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.DocumentNamePosition
END
ELSE
BEGIN
	CREATE TABLE dbo.DocumentNamePosition
(
	Id INT NOT NULL CONSTRAINT PK_DocumentNamePosition PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL DEFAULT 1
)
END

DELETE FROM dbo.DocumentNamePosition

INSERT dbo.DocumentNamePosition ([Name],Active) 
VALUES 
('Start of File Name ',1),
('End of File Name',1)

SELECT * FROM dbo.DocumentNamePosition

-----------------------------------------------------------------------------------------------------------------------------------------------

IF OBJECT_ID (N'DocumentNameSeparator', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.DocumentNameSeparator
END
ELSE
BEGIN
	CREATE TABLE dbo.DocumentNameSeparator
(
	Id INT NOT NULL CONSTRAINT PK_DocumentNameSeparator PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,
	[Value] VARCHAR(10) NOT NULL,
	Active BIT NOT NULL DEFAULT 1
)
END

DELETE FROM dbo.DocumentNameSeparator

INSERT dbo.DocumentNameSeparator ([Name],[Value],Active) 
VALUES 
('Underscore (_)','_',1),
('Hyphen (-)','-',1)

SELECT * FROM dbo.DocumentNameSeparator

-----------------------------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID (N'DocumentBatchDetail', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.DocumentBatchDetail
END
ELSE
BEGIN
	CREATE TABLE dbo.DocumentBatchDetail
	(
		Id INT NOT NULL CONSTRAINT PK_DocumentBatchDetail PRIMARY KEY (Id) IDENTITY (1,1),		
		BatchName VARCHAR(100) NOT NULL,
		TotalDocuments INT NOT NULL,
		DocumentType INT NOT NULL CONSTRAINT FK_DocumentTypes_DocumentBatchDetail FOREIGN KEY (DocumentType) REFERENCES dbo.DocumentTypes(Id),
		NameDelimiter INT NOT NULL CONSTRAINT FK_DocumentNameDelimiter_DocumentBatchDetail FOREIGN KEY (NameDelimiter) REFERENCES dbo.DocumentNameDelimiter(Id),
		NamePosition INT NOT NULL CONSTRAINT FK_DocumentNamePosition_DocumentBatchDetail FOREIGN KEY (NamePosition) REFERENCES dbo.DocumentNamePosition(Id),
		NameSeparator INT NOT NULL CONSTRAINT FK_DocumentNameSeparator_DocumentBatchDetail FOREIGN KEY (NameSeparator) REFERENCES dbo.DocumentNameSeparator(Id),
		[Status] INT NOT NULL CONSTRAINT FK_DocumentStatus_DocumentBatchDetail FOREIGN KEY (Status) REFERENCES dbo.DocumentStatus(Id),
		Active BIT NOT NULL DEFAULT 1,
		CreatedOn DATETIME NOT NULL,
		CreatedBy VARCHAR(100) NOT NULL,
		ModifiedOn DATETIME NULL,
		ModifiedBy VARCHAR(100) NULL
	)
END

-----------------------------------------------------------------------------------------------------------------------------------------------
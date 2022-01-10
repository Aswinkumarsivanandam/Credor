-----------------------------------------------------------------------------------------------------------------------------------------------
/*	CREATE TABLE SCRIPTS 

	1. OfferingType
	2. OfferingStatus
	3. OfferingVisibility
	4. PortfolioOffering
	5. KeyHighlight
	6. PortfolioKeyHighlight
	7. PortfolioLocation
	8. PortfolioGallery
	9. PortfolioSummary
	10. PortfolioFundingInstructions
	11. PortfolioUpdates
	12. InvestmentStatus
	13. Investment
	14. PortfolioOfferingVisibility

*/
-----------------------------------------------------------------------------------------------------------------------------------------------

-- TABLE DROP SCRIPTS
/*
IF OBJECT_ID (N'PortfolioKeyHighlight', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioKeyHighlight
END
IF OBJECT_ID (N'KeyHighlight', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.KeyHighlight
END
IF OBJECT_ID (N'PortfolioDocument', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioDocument
END
IF OBJECT_ID (N'PortfolioLocation', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioLocation
END
IF OBJECT_ID (N'PortfolioGallery', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioGallery
END
IF OBJECT_ID (N'PortfolioSummary', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioSummary
END
IF OBJECT_ID (N'PortfolioFundingInstructions', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioFundingInstructions
END
IF OBJECT_ID (N'Investment', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.Investment
END
IF OBJECT_ID (N'InvestmentStatus', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.InvestmentStatus
END
IF OBJECT_ID (N'PortfolioOffering', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioOffering
END
IF OBJECT_ID (N'OfferingVisibility', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.OfferingVisibility
END
IF OBJECT_ID (N'PortfolioUpdates', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioUpdates
END
IF OBJECT_ID (N'PortfolioOfferingVisibility', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PortfolioOfferingVisibility
END
*/

-----------------------------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID (N'OfferingType', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.OfferingType
	PRINT 'Table [OfferingType] Dropped'
END
ELSE
BEGIN
	CREATE TABLE dbo.OfferingType
(
	Id INT NOT NULL CONSTRAINT PK_OfferingType PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,
	[Description] VARCHAR(500) NULL,
	Active BIT NOT NULL DEFAULT 1,
	CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
	CreatedBy VARCHAR(50) NOT NULL DEFAULT 'DEV'
)
PRINT 'Table [OfferingType] Created'
END

DELETE FROM dbo.OfferingType

INSERT dbo.OfferingType ([Name],[Description],Active) 
VALUES 
('Investment','Your Investor will Fund the entire Investment amount upfront',1),
('Commitment','Your Investors will fund a portion of their commitment upfront and you will use Capital calls to draw down the Commitments over time',1)

SELECT * FROM dbo.OfferingType
-----------------------------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID (N'OfferingStatus', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.OfferingStatus
	PRINT 'Table [OfferingStatus] Dropped'
END
ELSE
BEGIN
	CREATE TABLE dbo.OfferingStatus
(
	Id INT NOT NULL CONSTRAINT PK_OfferingStatus PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,
	[Description] VARCHAR(500) NULL,
	Active BIT NOT NULL DEFAULT 1,
	CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
	CreatedBy VARCHAR(50) NOT NULL DEFAULT 'DEV'
)
PRINT 'Table [OfferingStatus] Created'
END

DELETE FROM dbo.OfferingStatus

INSERT dbo.OfferingStatus ([Name],[Description],Active) 
VALUES 
('Draft','No one will see a draft offering',1),
('Open','Accepting Investment requests',1),
('Manage','New investment requests are closed',1),
('Past','The offering has gone the full cycle and is no longer being managed',1)

SELECT * FROM dbo.OfferingType
-----------------------------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID (N'OfferingVisibility', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.OfferingVisibility
	PRINT 'Table [OfferingVisibility] Dropped'
END
ELSE
BEGIN
BEGIN
	CREATE TABLE dbo.OfferingVisibility
(
	Id INT NOT NULL CONSTRAINT PK_OfferingVisibility PRIMARY KEY (Id) IDENTITY (1,1),
	Accessto VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL,
	CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
	CreatedBy VARCHAR(50) NOT NULL DEFAULT 'DEV'
)
PRINT 'Table [OfferingVisibility] Created'
END

DELETE FROM dbo.OfferingVisibility

INSERT dbo.OfferingVisibility (AccessTo,Active) 
VALUES 
('No Users',1),
('All Users',1),
('Verified Users',1),
('Accredited Only',1),
('Test (IT and Admin)',1)

SELECT * FROM dbo.OfferingVisibility
-----------------------------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID (N'PortfolioOffering', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.PortfolioOffering
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioOffering PRIMARY KEY (Id) IDENTITY (1,1),	
	[Name] VARCHAR(100) NOT NULL,
	PictureUrl VARCHAR(MAX) NOT NULL,
	EntityName VARCHAR(MAX) NOT NULL,
	Size DECIMAL NOT NULL,
	MinimumInvestment DECIMAL NOT NULL,
	[Type] INT NOT NULL,
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	IsReservation BIT NOT NULL,
	PublicLandingPageUrl VARCHAR(MAX) NULL,
	IsPrivate BIT NULL,
	ShowPercentageRaised BIT NULL,
	Visibility INT NOT NULL ,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL,
	ApprovedOn DATETIME NULL,
	ApprovedBy VARCHAR(50) NULL	
)
END

CREATE CLUSTERED INDEX Index_PortfolioOffering_Visibility
ON dbo.PortfolioOffering (Visibility);

IF OBJECT_ID (N'PortfolioOfferingVisibility', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PortfolioOfferingVisibility
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioOfferingVisibility PRIMARY KEY (Id) IDENTITY (1,1),
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOfferingVisibility_PortfolioOffering FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),
	OfferingVisibilityId INT NULL CONSTRAINT FK_PortfolioOfferingVisibility_OfferingVisibility FOREIGN KEY (OfferingVisibilityId) REFERENCES dbo.OfferingVisibility(Id),
	OfferingGroupId INT NULL CONSTRAINT FK_PortfolioOfferingVisibility_PortfolioOffering_OfferingGroup FOREIGN KEY (OfferingGroupId) REFERENCES dbo.PortfolioOffering(Id),	
	Active BIT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
)
END

IF OBJECT_ID (N'KeyHighlight', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.KeyHighlight
(
	Id INT NOT NULL CONSTRAINT PK_KeyHighlight PRIMARY KEY (Id) IDENTITY (1,1),
	[Name] VARCHAR(MAX) NOT NULL,	
	Active BIT NOT NULL	
)
END
DELETE FROM KeyHighlight
INSERT INTO dbo.KeyHighlight (Name,Active)
VALUES
('Cash on Cash Return(From)',1),
('Cash on Cash Return(To)',1),
('Target IRR(From)',1),
('Target IRR(To)',1),
('Target ARR(From)',1),
('Target ARR(To)',1),
('Term',1),
('Type',1)

SELECT * FROM KeyHighlight

IF OBJECT_ID (N'PortfolioKeyHighlight', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PortfolioKeyHighlight
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioKeyHighlight PRIMARY KEY (Id) IDENTITY (1,1),
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOffering_PortfolioKeyHighlight FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),
	KeyHighlightId INT NULL CONSTRAINT FK_KeyHighlight_PortfolioKeyHighlight FOREIGN KEY (KeyHighlightId) REFERENCES dbo.KeyHighlight(Id),
	Field VARCHAR(MAX) NULL,
	Value VARCHAR(MAX) NOT NULL,
	Active BIT NOT NULL,
	Visibility BIT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
)
END

CREATE CLUSTERED INDEX Index_PortfolioKeyHighlight_OfferingId
ON dbo.PortfolioKeyHighlight (OfferingId);


IF OBJECT_ID (N'PortfolioLocation', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PortfolioLocation
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioLocation PRIMARY KEY (Id) IDENTITY (1,1),
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOffering_PortfolioLocation FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),
	[Location] VARCHAR(MAX) NOT NULL,
	Latitutde VARCHAR(100) NULL,
	Longitude VARCHAR(100) NULL,
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
) 
END

CREATE INDEX Index_PortfolioLocation_OfferingId_Active ON dbo.PortfolioLocation (OfferingId, Active);
CREATE CLUSTERED INDEX Index_PortfolioLocation_OfferingId ON dbo.PortfolioLocation (OfferingId);


IF OBJECT_ID (N'PortfolioGallery', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PortfolioGallery
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioGallery PRIMARY KEY (Id) IDENTITY (1,1),
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOffering_PortfolioGallery FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),
	ImageUrl VARCHAR(MAX) NOT NULL,	
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	IsDefaultImage BIT NOT NULL DEFAULT 0,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
)
END

CREATE CLUSTERED INDEX Index_PortfolioGallery_OfferingId
ON dbo.PortfolioGallery (OfferingId);


IF OBJECT_ID (N'PortfolioSummary', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PortfolioSummary
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioSummary PRIMARY KEY (Id) IDENTITY (1,1),
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOffering_PortfolioSummary FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),
	Summary VARCHAR(MAX) NOT NULL,	
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
)
END

CREATE CLUSTERED INDEX Index_PortfolioSummary_OfferingId
ON dbo.PortfolioSummary (OfferingId);


IF OBJECT_ID (N'PortfolioFundingInstructions', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PortfolioFundingInstructions
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioFundingInstructions PRIMARY KEY (Id) IDENTITY (1,1),
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOffering_PortfolioFundingInstructions FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),
	InstructionType INT NOT NULL,
	ReceivingBank VARCHAR(100) NULL,
	BankAddress VARCHAR(500) NULL,
	Beneficiary VARCHAR(500) NULL,
	CheckBenificiary VARCHAR(500) NULL,
	BeneficiaryAddress VARCHAR(500) NULL,	
	RoutingNumber VARCHAR(50) NULL,
	AccountNumber VARCHAR(50) NULL,
	AccountType INT NOT NULL DEFAULT 0,	
    Reference VARCHAR(MAX) NULL,
	OtherInstructions VARCHAR(MAX) NULL,
	CheckOtherInstructions VARCHAR(MAX) NULL,
	MailingAddress VARCHAR(100) NULL,
	[Custom] VARCHAR(MAX) NULL,
	Memo VARCHAR(1000) NULL,
	Active BIT NOT NULL DEFAULT 1,
	[Status] INT NOT NULL DEFAULT 1,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
)
END

CREATE CLUSTERED INDEX Index_PortfolioFundingInstructions_OfferingId
ON dbo.PortfolioFundingInstructions (OfferingId);

IF OBJECT_ID (N'Investment', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Investment
(
	Id INT NOT NULL CONSTRAINT PK_Investment PRIMARY KEY (Id) IDENTITY (1,1),
	UserId INT NOT NULL CONSTRAINT FK_UserAccount_Investment FOREIGN KEY (UserID) REFERENCES dbo.UserAccount(Id),
	UserProfileId INT NOT NULL CONSTRAINT FK_UserProfile_Investment FOREIGN KEY (UserProfileId) REFERENCES dbo.UserProfile(Id),
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOffering_Investment FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),
	Amount DECIMAL NULL,	
	FundedDate DATETIME NULL,
	IsConfirmed BIT NOT NULL DEFAULT 0,
	IseSignCompleted BIT NOT NULL DEFAULT 0,
	DocumenteSignedDate DATETIME NULL,
	eSignedDocumentPath VARCHAR(MAX) NULL,
	WireTransferDate DATETIME NULL,
	IsReservation BIT NOT NULL,
	IsConverted BIT NOT NULL DEFAULT 0,
	ConvertedOn DATETIME NULL,
	Notes VARCHAR(MAX) NULL,
	ReservationStatus INT NULL,
	ConfidenceLevel INT NOT NULL DEFAULT 0,
	Active BIT NOT NULL,
	[Status] INT  NOT NULL CONSTRAINT FK_PortfolioOffering_InvestmentStatus FOREIGN KEY ([Status]) REFERENCES dbo.InvestmentStatus(Id),	
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL,
	ApprovedOn DATETIME NULL,
	ApprovedBy VARCHAR(50) NULL	
)
END

IF OBJECT_ID (N'InvestmentStatus', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.InvestmentStatus
	(
	Id INT NOT NULL CONSTRAINT PK_InvestmentStatus PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL DEFAULT 1	 
	)
END

Select * from InvestmentStatus

INSERT INTO dbo.InvestmentStatus([Name]) VALUES
('Approved'),('Pending'),('Declined'),('Waitlisted'),('Ownership Sold')

SELECT * FROM InvestmentStatus

IF OBJECT_ID (N'PortfolioUpdates', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PortfolioUpdates
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioUpdates PRIMARY KEY (Id) IDENTITY (1,1),		
	InvestmentId INT NOT NULL CONSTRAINT FK_Investment_PortfolioUpdates FOREIGN KEY (InvestmentId) REFERENCES dbo.Investment(Id),
	[Subject] VARCHAR(MAX) NOT NULL,
	Content VARCHAR(MAX) NOT NULL,
	ImageUrl VARCHAR(MAX) NULL,
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(100) NULL	
)
END
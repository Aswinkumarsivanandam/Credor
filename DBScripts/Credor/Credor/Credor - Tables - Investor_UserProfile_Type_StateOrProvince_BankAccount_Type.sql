-- TABLE DROP SCRIPTS
/*
IF OBJECT_ID (N'Investor', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.Investor
END
IF OBJECT_ID (N'UserProfile', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.UserProfile
END
IF OBJECT_ID (N'DistributionType', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.DistributionType
END
IF OBJECT_ID (N'StateOrProvince', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.StateOrProvince
END
IF OBJECT_ID (N'UserProfileType', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.UserProfileType
END
IF OBJECT_ID (N'BankAccount', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.BankAccount
END
IF OBJECT_ID (N'BankAccountType', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.BankAccountType
END
*/
-- CREATE TABLE SCRIPTS
IF OBJECT_ID (N'StateOrProvince', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.StateOrProvince
(
	Id INT NOT NULL CONSTRAINT PK_StateOrProvince PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL DEFAULT 1,	
)
END
DELETE FROM dbo.StateOrProvince

INSERT INTO dbo.StateOrProvince ([Name],Active)
VALUES
('Non US-Resident',1),
('Alabama',1),
('Alaska',1),
('Arizona',1),
('Arkansas',1),
('California',1),
('Colorida',1),
('Connecticut',1),
('Delaware',1),
('District of Columbia',1),
('Florida',1),
('Georgia',1),
('Hawaii',1),
('Idaho',1),
('Illinois',1),
('Indiana',1),
('Iowa',1),
('Kansas',1),
('Kentucky',1),
('Louisiana',1),
('Maine',1),
('Maryland',1),
('Massachusetts',1),
('Michigan',1),
('Minnesota',1),
('Mississippi',1),
('Missouri',1),
('Montana',1),
('Nebraska',1),
('Nevada',1),
('New Hampshire',1),
('New Jersey',1),
 ('New Mexico',1),
 ('New York',1),
 ('North Carolina',1),
 ('North Dakota',1),
 ('Ohio',1),
 ('Oklahoma',1),
 ('Oregon',1),
 ('Palau',1),
 ('Pennsylvania',1),
 ('Rhode Island',1),
 ('South Carolina',1),
 ('South Dakota',1),
 ('Tennessee',1), 
 ('Texas',1),
 ('Utah',1),
 ('Vermont',1),
 ('Virginia',1),
 ('Washington',1),
 ('West Virginia',1),
 ('Wisconsin',1),
 ('Wyoming',1),
 ('American Samoa',1),
 ('Guam',1),
 ('Northern Mariana Islands',1),
 ('Puerto Rico',1),
 ('Virgin Islands',1),
 ('United States Minor Outlying Islands',1),
 ('Alberta',1),
 ('British Columbia',1),
 ('Manitoba',1),
 ('New Brunswick',1),
 ('Newfoundland',1),
 ('Northwest Territories',1),
 ('Nova Scotia',1),
 ('Nunavut',1),
 ('Ontario',1),
 ('Prince Edward Island',1),
 ('Quebec',1)


SELECT * FROM dbo.StateOrProvince


IF OBJECT_ID (N'UserProfileType', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.UserProfileType
(
	Id INT NOT NULL CONSTRAINT PK_UserProfileType PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL,	
)
END
DELETE FROM dbo.UserProfileType

INSERT INTO dbo.UserProfileType (Name,Active)
VALUES('IRA',1),('LLC, Corporation, or Partnership',1),('Individual',1),('Trust',1),('Joint Registration',1),('Retirement Plan ',1)

SELECT * FROM dbo.UserProfileType

IF OBJECT_ID (N'DistributionType', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.DistributionType
(
	Id INT NOT NULL CONSTRAINT PK_DistributionType PRIMARY KEY (Id) IDENTITY (1,1),		
	[Name] VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL,	
)
END

DELETE FROM dbo.DistributionType

INSERT INTO dbo.DistributionType (Name,Active)
VALUES('ACH',1),('Check',1),('Others',1)

SELECT * FROM dbo.DistributionType

IF OBJECT_ID (N'UserProfile', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.UserProfile
(
	Id INT NOT NULL CONSTRAINT PK_UserProfile PRIMARY KEY (Id) IDENTITY (1,1),	
	DisplayId VARCHAR(10) NOT NULL UNIQUE,
	UserId INT NOT NULL CONSTRAINT FK_UserAccount_UserProfile FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id), 
	[Type] INT NOT NULL CONSTRAINT FK_UserProfileType_UserProfile FOREIGN KEY ([Type]) REFERENCES dbo.UserProfileType (Id),	
	BankAccountId INT NULL CONSTRAINT FK_BankAccount_UserProfile FOREIGN KEY (BankAccountId) REFERENCES dbo.BankAccount (Id),
	IsOwner BIT NOT NULL DEFAULT 0,
	[Name] VARCHAR(100) NULL,
	FirstName VARCHAR(100) NULL,
	LastName VARCHAR(100) NULL,
	TrustName VARCHAR(100) NULL,
	RetirementPlanName VARCHAR(100) NULL,
	SignorName  VARCHAR(100) NULL,
	InCareOf VARCHAR(100) NULL,
	StreetAddress1 VARCHAR(MAX) NULL,
	StreetAddress2 VARCHAR(MAX) NULL,
	City VARCHAR(100) NULL,
	StateOrProvinceId INT NULL,
	Country VARCHAR(100) NULL,
	[State] VARCHAR(100) NULL,
	ZipCode VARCHAR(50) NULL,
	TaxId VARCHAR(100)  NULL,
	DistributionTypeId INT NULL CONSTRAINT FK_BankAccount_DistributionType FOREIGN KEY (DistributionTypeId) REFERENCES dbo.DistributionType(Id),
	IsDisregardedEntity BIT NULL,
	IsIRALLC BIT NULL,
	OwnerTaxId VARCHAR(100) NULL,
	DistributionDetail VARCHAR(MAX) NULL,
	CheckInCareOf VARCHAR(100) NULL,
	CheckAddressLine1 VARCHAR(500) NULL,
	CheckAddressLine2 VARCHAR(500) NULL,
	CheckCity VARCHAR(100) NULL,
	CheckStateId INT NULL,
	CheckZip VARCHAR(50) NULL,
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL,
	ApprovedOn DATETIME NULL,
	ApprovedBy VARCHAR(50) NULL
)
END


IF OBJECT_ID (N'BankAccountType', N'U') IS  NULL
	BEGIN
	CREATE TABLE dbo.BankAccountType
(
	Id INT NOT NULL CONSTRAINT PK_BankAccountType PRIMARY KEY (Id) IDENTITY (1,1),		
	[Type] VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL,	
)
END

DELETE FROM dbo.BankAccountType

INSERT INTO dbo.BankAccountType (Type,Active)
VALUES('Savings',1),('Checking',1)

SELECT * FROM dbo.BankAccountType

IF OBJECT_ID (N'BankAccount', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.BankAccount
(
	Id INT NOT NULL CONSTRAINT PK_BankAccount PRIMARY KEY (Id) IDENTITY (1,1),	
	UserId INT NOT NULL CONSTRAINT FK_UserAccount_BankAccount FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id), 	
	BankName VARCHAR(MAX) NOT NULL,
	AccountType INT NOT NULL CONSTRAINT FK_BankAccountType_BankAccount FOREIGN KEY (AccountType) REFERENCES dbo.BankAccountType(Id),
	RoutingNumber VARCHAR(50) NOT NULL,
	AccountNumber VARCHAR(50) NOT NULL,
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL,
	ApprovedOn DATETIME NULL,
	ApprovedBy VARCHAR(50) NULL,
)
END

IF OBJECT_ID (N'Investor', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.Investor
(
	Id INT NOT NULL CONSTRAINT PK_Investor PRIMARY KEY (Id) IDENTITY (1,1),		
	UserProfileId INT NOT NULL CONSTRAINT FK_UserProfile_Investor FOREIGN KEY (UserProfileId) REFERENCES dbo.UserProfile(Id), 	
	FirstName VARCHAR(100) NULL,
	LastName VARCHAR(100) NULL,
	EmailId VARCHAR(100) NOT NULL,
	Phone VARCHAR(50) NOT NULL,
	IsNotificationEnabled BIT NOT NULL,	
	IsOwner BIT NOT NULL DEFAULT 0,
	Active BIT NOT NULL,
	[Status] INT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
)
END


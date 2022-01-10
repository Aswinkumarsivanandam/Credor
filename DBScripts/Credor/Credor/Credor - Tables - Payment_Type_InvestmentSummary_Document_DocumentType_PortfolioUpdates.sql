-- TABLE DROP SCRIPTS
/*
IF OBJECT_ID (N'Payment', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.Payment
END
IF OBJECT_ID (N'PaymentType', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.PaymentType
END
IF OBJECT_ID (N'InvestmentSummary', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.InvestmentSummary
END
IF OBJECT_ID (N'OfferingDistribution', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.OfferingDistribution
END
*/
-- CREATE TABLE SCRIPTS
IF OBJECT_ID (N'PaymentType', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PaymentType
(
	Id INT NOT NULL CONSTRAINT PK_PaymentType PRIMARY KEY (Id) IDENTITY (1,1),
	[Name] VARCHAR(MAX) NOT NULL,	
	Active BIT NOT NULL DEFAULT 1
)
END
INSERT dbo.PaymentType ([Name],Active)
VALUES ('Credit',1),('Debit',1),('Refunded',1)
SELECT * FROM dbo.PaymentType

IF OBJECT_ID (N'PortfolioDistributionType', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.PortfolioDistributionType
(
	Id INT NOT NULL CONSTRAINT PK_PortfolioDistributionType PRIMARY KEY (Id) IDENTITY (1,1),
	[Name] VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL DEFAULT 1
)
END
INSERT dbo.PortfolioDistributionType ([Name],Active)
VALUES ('Operating Income',1),
('Retained Earning',1),
('Return of Capital',1),
('Gain from Sale',1),
('Proceeds from Refi',1),
('Preferred Return',1),
('Interest',1)

SELECT * FROM dbo.PortfolioDistributionType

IF OBJECT_ID (N'Payment', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.Payment
(
	Id INT NOT NULL CONSTRAINT PK_Payment PRIMARY KEY (Id) IDENTITY (1,1),		
	InvestmentId INT NOT NULL CONSTRAINT FK_Investment_Payment FOREIGN KEY (InvestmentId) REFERENCES dbo.Investment(Id),
	Amount DECIMAL NOT NULL,
	[Type] INT NOT NULL CONSTRAINT FK_PaymentType_Payment FOREIGN KEY ([Type]) REFERENCES dbo.PaymentType(Id),
	Active BIT NOT NULL DEFAULT 1,
	[Status] INT NOT NULL,
	Comment VARCHAR(MAX) NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(100) NULL	
)
END

SELECT * FROM OfferingDistribution

IF OBJECT_ID (N'OfferingDistribution', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.OfferingDistribution
(
	Id INT NOT NULL CONSTRAINT PK_OfferingDistribution PRIMARY KEY (Id) IDENTITY (1,1),		
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOffering_OfferingDistribution FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),			
	Amount DECIMAL(18,2) NOT NULL,
	[Type] INT NOT NULL CONSTRAINT FK_PortfolioDistributionType_OfferingDistribution FOREIGN KEY ([Type]) REFERENCES dbo.PortfolioDistributionType(Id),
	CalculationMethod INT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	PaymentDate DATETIME NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
	[Status] INT NOT NULL,
	Memo VARCHAR(MAX) NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(100) NULL	
)
END

DROP TABLE Distributions
IF OBJECT_ID (N'Distributions', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.Distributions
(
	Id INT NOT NULL CONSTRAINT PK_Distributions PRIMARY KEY (Id) IDENTITY (1,1),		
	OfferingDistributionId INT NULL CONSTRAINT FK_OfferingDistribution_Distributions FOREIGN KEY (OfferingDistributionId) REFERENCES dbo.OfferingDistribution(Id),			
	InvestmentId INT NULL CONSTRAINT FK_Investment_Distributions FOREIGN KEY (InvestmentId) REFERENCES dbo.Investment(Id),		
	InvestorId INT NULL CONSTRAINT FK_UserAccount_Distributions FOREIGN KEY (InvestorId) REFERENCES dbo.UserAccount(Id),		
	PercentageFunded DECIMAL(18,2) NOT NULL,
	PercentageOwnership DECIMAL(18,2) NULL,
	PaymentAmount DECIMAL(18,2) NOT NULL,
	[Type] INT NOT NULL CONSTRAINT FK_PortfolioDistributionType_Distributions FOREIGN KEY ([Type]) REFERENCES dbo.PortfolioDistributionType(Id),
	DistributionMethod INT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	PaymentDate DATETIME NOT NULL,
	Memo VARCHAR(MAX) NULL,
	Active BIT NOT NULL DEFAULT 1,	
	[Status] INT NOT NULL DEFAULT 1,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(100) NULL	
)
END	


IF OBJECT_ID (N'OfferingCapTable', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.OfferingCapTable
(
	Id INT NOT NULL CONSTRAINT PK_OfferingCapTable PRIMARY KEY (Id) IDENTITY (1,1),		
	OfferingId INT NOT NULL CONSTRAINT FK_PortfolioOffering_OfferingCapTable FOREIGN KEY (OfferingId) REFERENCES dbo.PortfolioOffering(Id),
	InvestmentId INT NOT NULL CONSTRAINT FK_InvestmentId_OfferingCapTable FOREIGN KEY (InvestmentId) REFERENCES dbo.Investment(Id),				
	PercentageFunded DECIMAL(18,2) NOT NULL,
	PercentageOwnership DECIMAL(18,2) NOT NULL,
	Active BIT NOT NULL DEFAULT 1,
	[Status] INT NOT NULL DEFAULT 1,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(100) NULL	
)
END	

Select * from OfferingCapTable



IF OBJECT_ID (N'InvestmentSummary', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.InvestmentSummary
(	
    Id INT NOT NULL CONSTRAINT PK_InvestmentSummary PRIMARY KEY (Id) IDENTITY (1,1),	
	UserId INT NOT NULL UNIQUE CONSTRAINT FK_UserAccount_InvestmentSummary FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id),
	ActiveInvestments INT NULL,
	TotalInvested DECIMAL NULL,
	TotalEarnings DECIMAL NULL,
	TotalReturn DECIMAL NULL,
	PendingInvestments INT NULL
)
END

	
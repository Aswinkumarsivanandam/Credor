-- TABLE DROP SCRIPTS
/*IF OBJECT_ID (N'UserAccount', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.UserAccount
END
IF OBJECT_ID (N'UserRole', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.UserRole
END
IF OBJECT_ID (N'Capacity', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.Capacity
END
IF OBJECT_ID (N'UserPermission', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.UserPermission
END
IF OBJECT_ID (N'RoleFeatureMapping', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.RoleFeatureMapping
END
IF OBJECT_ID (N'UserFeatureMapping', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.UserFeatureMapping
END
IF OBJECT_ID (N'UserNotes', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.UserNotes
END
IF OBJECT_ID (N'UserOTP', N'U') IS NOT NULL
BEGIN
	DROP TABLE dbo.UserOTP
END
*/
-- CREATE TABLE SCRIPTS
IF OBJECT_ID (N'UserRole', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.UserRole
(
	Id INT NOT NULL PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	Description VARCHAR(MAX) NOT NULL
)
END

IF OBJECT_ID (N'RoleFeatureMapping', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.RoleFeatureMapping
(
	Id INT NOT NULL CONSTRAINT PK_RoleFeatureMapping PRIMARY KEY (Id) IDENTITY (1,1),
	RoleId INT NOT NULL CONSTRAINT FK_RoleFeatureMapping_UserRole FOREIGN KEY (RoleId) REFERENCES dbo.UserRole(Id),
	FeatureName VARCHAR(100) NOT NULL,
	Active BIT NOT NULL DEFAULT 1
)
END

IF OBJECT_ID (N'Capacity', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.Capacity
(
	Id INT NOT NULL PRIMARY KEY,
	CapacityRange VARCHAR(100) NOT NULL,
	Active BIT NOT NULL
)
END

IF OBJECT_ID (N'UserAccount', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.[UserAccount] 
	(
	 Id INT NOT NULL  CONSTRAINT PK_User PRIMARY KEY NONCLUSTERED (Id) IDENTITY (1,1),
	 RoleId INT NOT NULL CONSTRAINT FK_UserAccount_UserRole FOREIGN KEY (RoleId) REFERENCES dbo.UserRole(Id),
	 FirstName VARCHAR(50) NOT NULL,
	 LastName VARCHAR(50) NOT NULL,
	 NickName VARCHAR(50) NULL,
	 UserName VARCHAR(50) NULL,
	 EmailId VARCHAR(100) NOT NULL,
	 DateOfBirth VARCHAR(10) NULL,
	 ProfileImageUrl VARCHAR(MAX) NULL,
	 SecondaryEmail VARCHAR(100) NULL,
	 ReceiveEmailNotifications BIT NULL,
	 PhoneNumber VARCHAR(50) NULL,
	 Residency INT NOT NULL,
	 Country VARCHAR(100) NULL,
	 [Password] VARCHAR(50) NULL,
	 PasswordSalt VARBINARY(50) NULL,
	 PasswordChangedOn DATETIME NULL,
	 OldPassword VARCHAR(50) NULL,
	 Capacity INT NULL CONSTRAINT FK_UserAccount_Capacity FOREIGN KEY (Capacity) REFERENCES dbo.Capacity(Id),
	 IsAccreditedInvestor BIT NULL,
	 HeardFrom VARCHAR(MAX) NULL,
	 IsTOCApproved BIT NOT NULL DEFAULT 0,
	 [Status] INT NOT NULL,
	 Active BIT NOT NULL,
	 IsEmailVerified BIT NOT NULL DEFAULT 0,
	 IsPhoneVerified BIT NOT NULL DEFAULT 0,
	 IsTwoFactorAuthEnabled BIT NOT NULL DEFAULT 0,
	 OneTimePassword VARCHAR(10) NULL,
	 LastLogin DATETIME NULL,
	 VerifyAccount BIT NULL,	 
	 CompanyNewsLetterUpdates BIT NULL,
	 NewInvestmentAnnouncements BIT NULL,	 
	 CreatedOn DATETIME NOT NULL,
	 CreatedBy VARCHAR(50) NOT NULL,
	 ModifiedOn DATETIME NULL,
	 ModifiedBy VARCHAR(50) NULL,
	 ApprovedOn DATETIME NULL,
	 ApprovedBy DATETIME NULL	 
	)	
END

IF OBJECT_ID (N'IX_UserAccount_EmailId', N'U') IS NULL
BEGIN
	CREATE CLUSTERED INDEX IX_UserAccount_EmailId   
    ON dbo.UserAccount(EmailId)
END

IF OBJECT_ID (N'UserPermission', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.UserPermission
(
	Id INT NOT NULL  CONSTRAINT PK_UserPermission PRIMARY KEY (Id) IDENTITY (1,1),
	UserId INT NOT NULL CONSTRAINT FK_UserPermission_UserAccount FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id),
	AccessGrantedTo INT NOT NULL CONSTRAINT FK_UserPermission_UserAccount_AccessTo FOREIGN KEY (AccessGrantedTo) REFERENCES dbo.UserAccount(Id),
	Permission INT NOT NULL,
	IsNotificationEnabled BIT NOT NULL DEFAULT 0,
	Active BIT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(50) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(50) NULL
)
END
IF OBJECT_ID (N'UserNotes', N'U') IS  NULL
BEGIN
	CREATE TABLE dbo.UserNotes
(
	Id INT NOT NULL  CONSTRAINT PK_UserNotes PRIMARY KEY (Id) IDENTITY (1,1),
	UserId INT NOT NULL CONSTRAINT FK_UserNotes_UserAccount FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id),
	Notes VARCHAR(MAX) NOT NULL,	
	Active BIT NOT NULL,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(100) NULL
)
END

IF OBJECT_ID (N'UserOTP', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.UserOTP
(
	Id INT NOT NULL CONSTRAINT PK_UserOTP PRIMARY KEY (Id) IDENTITY (1,1),
	UserId INT NOT NULL CONSTRAINT FK_UserOTP_UserAccount FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id),
	OTP VARCHAR(100) NOT NULL,	
	Active BIT NOT NULL DEFAULT 1,
	CreatedOn DATETIME NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
	ModifiedOn DATETIME NULL,
	ModifiedBy VARCHAR(100) NULL
)
END

IF OBJECT_ID (N'UserFeatureMapping', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.UserFeatureMapping
(
	Id INT NOT NULL CONSTRAINT PK_UserFeatureMapping PRIMARY KEY (Id) IDENTITY (1,1),
	UserId INT NOT NULL CONSTRAINT FK_UserFeatureMapping_UserAccount FOREIGN KEY (UserId) REFERENCES dbo.UserAccount(Id),
	FeatureName VARCHAR(100) NOT NULL,
	Active BIT NOT NULL DEFAULT 1
)
END

INSERT INTO dbo.[UserRole] (Id, Name, Description) VALUES
(1,'Investor', 'User will be treated as Investor'),
(2,'Lead', 'User will have only lead level permissions. Lead can be converted to Investor once investment made'),
(3,'Admin', 'Admin User will have admin related permissions')
	

INSERT INTO dbo.Capacity(Id, CapacityRange, Active) VALUES (1,'Less than $10,00', 1),
(2,'$10,000 - $50,000', 1),
(3,'$50,000 - $100,000', 1),
(4,'$100,000 - $250,000', 1),
(5,'More than $250,000', 1)


INSERT INTO dbo.RoleFeatureMapping
(RoleId,
FeatureName,
Active)  
VALUES 
(3,'Admin Dashboard', 1),
(3,'Leads', 1),
(3,'Investors', 1),
(3,'Portfolio', 1),
(3,'Email', 1),
(3,'Reports',1),
(3,'Payments',1),
(3,'Services',1),
(3,'Settings',1),
(1,'Invest',1),
(1,'My Investments', 1),
(1, 'Updates',1),
(1, 'Distributions',1),
(1,'Documents',1),
(1,'Profiles',1),
(1,'Account',1),
(2,'Invest',1),
(2,'My Investments', 1),
(2,'Leads',1),
(2, 'Updates',1),
(2, 'Distributions',1),
(2,'Documents',1),
(2,'Profiles',1),
(2,'Account',1),
(4,'Signup',1),
(4,'Account',1)






Select * from UserRole
Select * from RoleFeatureMapping
Select * from UserFeatureMapping
Select * from UserNotes
Select * from UserOTP









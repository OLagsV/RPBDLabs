
--    данный скрипт создает базу данных toplivo с тремя таблицами и генерирует тестовые записи:
-- 1. виды топлива (Fuels) - 1000 штук
-- 2. список емкостей (Tanks) - 100 штук
-- 3. факты совершения операций прихода, расхода топлива (Operations) - 300000 штук

--Создание базы данных
USE master
CREATE DATABASE OrganizationsWaterSupply

GO
ALTER DATABASE OrganizationsWaterSupply SET RECOVERY SIMPLE
GO

USE OrganizationsWaterSupply
--DROP TABLE Fuels, Tanks, Operations
--DROP VIEW View_AllOperations

-- Создание таблиц
CREATE TABLE dbo.Organizations(OrganizationID int IDENTITY(1,1) NOT NULL PRIMARY KEY, 
OrgName nvarchar(50),
OwnershipType nvarchar(50), 
Adress nvarchar(50), 
DirectorFullname nvarchar(50), 
DirectorPhone varchar(11),
ResponsibleFullname nvarchar(50),
ResponsiblePhone varchar(11))

CREATE TABLE dbo.CounterModels (ModelID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
ModelName nvarchar(50),
Manufacturer nvarchar(50),
ServiceTime int)

CREATE TABLE dbo.Counters (RegistrationNumber int IDENTITY(1,1) NOT NULL PRIMARY KEY,
ModelID int,
TimeOfInstallation date,
OrganizationID int)

CREATE TABLE dbo.CountersData (CountersDataID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
RegistrationNumber int,
DataCheckDate date,
RateID int,
Volume int,)

CREATE TABLE dbo.CountersChecks (CountersCheckID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
RegistrationNumber int,
CheckDate date,
CheckResult nvarchar(50))

CREATE TABLE dbo.Rates (RateID int IDENTITY(1,1) NOT NULL PRIMARY KEY,
RateName nvarchar(50),
Price decimal(18,2))

CREATE TABLE dbo.RateOrg (RateID int NOT NULL PRIMARY KEY,
OrganizationID int)

-- Добавление связей между таблицами
ALTER TABLE dbo.Counters  WITH CHECK ADD  CONSTRAINT FK_Counters_CounterModels FOREIGN KEY(ModelID)
REFERENCES dbo.CounterModels (ModelID) ON DELETE CASCADE
GO
ALTER TABLE dbo.Counters  WITH CHECK ADD  CONSTRAINT FK_Counters_Organizations FOREIGN KEY(OrganizationID)
REFERENCES dbo.Organizations (OrganizationID) ON DELETE CASCADE
GO
ALTER TABLE dbo.CountersChecks  WITH CHECK ADD  CONSTRAINT FK_CountersChecks_Counters FOREIGN KEY(RegistrationNumber)
REFERENCES dbo.Counters (RegistrationNumber) ON DELETE CASCADE
GO
ALTER TABLE dbo.RateOrg  WITH CHECK ADD  CONSTRAINT FK_RateOrg_Rates FOREIGN KEY(RateID)
REFERENCES dbo.Rates (RateID) ON DELETE CASCADE
GO
ALTER TABLE dbo.RateOrg  WITH CHECK ADD  CONSTRAINT FK_RateOrg_Organizations FOREIGN KEY(OrganizationID)
REFERENCES dbo.Organizations (OrganizationID) ON DELETE CASCADE
GO
ALTER TABLE dbo.CountersData  WITH CHECK ADD  CONSTRAINT FK_CountersData_Counters FOREIGN KEY(RegistrationNumber)
REFERENCES dbo.Counters (RegistrationNumber) ON DELETE CASCADE
GO
ALTER TABLE dbo.CountersData  WITH CHECK ADD  CONSTRAINT FK_CountersData_RateOrg FOREIGN KEY(RateID)
REFERENCES dbo.RateOrg (RateID) ON DELETE NO ACTION
GO

SET NOCOUNT ON


DECLARE @Symbol CHAR(52)= 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz',
		@Position int,
		@OrgName nvarchar(50),
		@OwnershipType nvarchar(50), 
		@Adress nvarchar(50), 
		@DirectorFullname nvarchar(50), 
		@DirectorPhone varchar(11),
		@ResponsibleFullname nvarchar(50),
		@ResponsiblePhone varchar(11),
		@ModelName nvarchar(50),
		@Manufacturer nvarchar(50),
		@ServiceTime int,
		@ModelID int,
		@TimeOfInstallation date,
		@OrganizationID int,
		@i int,
		@NameLimit int,
		@odate date,
		@Inc_Exp real,
		@RowCount INT,
		@NumberOrganizations int,
		@NumberCounterModels int,
		@NumberCounters int,
		@MinNumberSymbols int,
		@MaxNumberSymbols int



SET @NumberOrganizations = 500
SET @NumberCounterModels = 500
SET @NumberCounters = 20000

BEGIN TRAN

SELECT @i=0 FROM dbo.Organizations WITH (TABLOCKX) WHERE 1=0
-- Заполнение видов топлива 
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberOrganizations
	BEGIN		
		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
        SET @OrgName=''
		SET @OwnershipType=''
		SET @Adress=''
		SET @DirectorFullname=''
		SET @DirectorPhone=''
		SET @ResponsibleFullname=''
		SET @ResponsiblePhone=''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @OrgName = @OrgName + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @OwnershipType = @OwnershipType + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @Adress = @Adress + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @DirectorFullname = @DirectorFullname + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @DirectorPhone = @DirectorPhone + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @ResponsibleFullname = @ResponsibleFullname + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @ResponsiblePhone = @ResponsiblePhone + SUBSTRING(@Symbol, @Position, 1)
			SET @i=@i+1
		END

		INSERT INTO dbo.Organizations(OrgName, OwnershipType,Adress, DirectorFullname, DirectorPhone, ResponsibleFullname,ResponsiblePhone) SELECT @OrgName, @OwnershipType,@Adress, @DirectorFullname, @DirectorPhone, @ResponsibleFullname,@ResponsiblePhone
		
		SET @RowCount +=1
	END
SELECT @i=0 FROM dbo.CounterModels WITH (TABLOCKX) WHERE 1=0
	
	SET @RowCount=1

	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberCounterModels
	BEGIN		

		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
        SET @ModelName=''
		SET @Manufacturer=''
		WHILE @i<=@NameLimit
		BEGIN
			SET @Position=RAND()*52
			SET @ModelName = @ModelName + SUBSTRING(@Symbol, @Position, 1)
			SET @Position=RAND()*52
			SET @Manufacturer = @Manufacturer + SUBSTRING(@Symbol, @Position, 1)
			SET @ServiceTime = CAST(RAND()*52 as int)
			SET @i=@i+1
		END

		INSERT INTO dbo.CounterModels(ModelName, Manufacturer, ServiceTime) SELECT @ModelName, @Manufacturer, @ServiceTime
		
		SET @RowCount +=1
	END

SELECT @i=0 FROM dbo.Counters WITH (TABLOCKX) WHERE 1=0
	SET @RowCount=1
	SET @MinNumberSymbols=5
	SET @MaxNumberSymbols=50	
	WHILE @RowCount<=@NumberCounters
	BEGIN		
		SET @NameLimit=@MinNumberSymbols+RAND()*(@MaxNumberSymbols-@MinNumberSymbols) -- имя от 5 до 50 символов
		SET @i=1
		WHILE @i<=@NameLimit
		BEGIN
			SET @ModelID = CAST( (1+RAND()*(@NumberCounterModels-1)) as int)
			SET @TimeOfInstallation=dateadd(day,-RAND()*15000,GETDATE())
			SET @OrganizationID = CAST( (1+RAND()*(@NumberOrganizations-1)) as int)
			SET @i=@i+1
		END

		INSERT INTO dbo.Counters(ModelID, TimeOfInstallation, OrganizationID) SELECT @ModelID, @TimeOfInstallation, @OrganizationID
		

		SET @RowCount +=1
	END
COMMIT TRAN
GO

CREATE VIEW [dbo].[View_OrganizationsCounters]
AS
SELECT        dbo.Organizations.OrganizationID, dbo.Organizations.OrgName, dbo.Organizations.OwnershipType, dbo.Organizations.Adress, dbo.Organizations.DirectorFullname, dbo.Organizations.DirectorPhone, dbo.Organizations.ResponsibleFullname, dbo.Organizations.ResponsiblePhone,
				dbo.CounterModels.ModelID, dbo.CounterModels.ModelName, dbo.CounterModels.Manufacturer, dbo.CounterModels.ServiceTime,
					dbo.Counters.RegistrationNumber, dbo.Counters.TimeOfInstallation
FROM            dbo.CounterModels INNER JOIN
                         dbo.Counters ON dbo.CounterModels.ModelID = dbo.Counters.ModelID INNER JOIN
						 dbo.Organizations ON dbo.Counters.OrganizationID = dbo.Organizations.OrganizationID
GO

CREATE PROCEDURE ChangeOrganizationAdress (@OrganizationID int, @NewAdress nvarchar(50))
        AS UPDATE dbo.Organizations
        SET dbo.Organizations.Adress = @NewAdress
		WHERE(
		dbo.Organizations.OrganizationID = @OrganizationID
		);
GO
CREATE PROCEDURE AddOrganization (@OrgName nvarchar(50), @OwnershipType nvarchar(50), @Adress nvarchar(50), @DirectorFullname nvarchar(50), @DirectorPhone nvarchar(11), @ResponsibleFullname nvarchar(50), @ResponsiblePhone nvarchar(11))
        AS INSERT INTO dbo.Organizations(
		OrgName,
		OwnershipType,
		Adress,
		DirectorFullname,
		DirectorPhone, 
		ResponsibleFullname, 
		ResponsiblePhone) 
		VALUES
		(@OrgName, 
		@OwnershipType , 
		@Adress,
		@DirectorFullname,
		@DirectorPhone, 
		@ResponsibleFullname, 
		@ResponsiblePhone)
GO
CREATE PROCEDURE AddCounter (@ModelID int, @TimeOfInstallation date, @OrganizationID int)
        AS INSERT INTO dbo.Counters(
		ModelID,
		TimeOfInstallation,
		OrganizationID) 
		VALUES(
		@ModelID,
		@TimeOfInstallation,
		@OrganizationID)
GO
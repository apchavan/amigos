CREATE TABLE [dbo].[user_creds]
(
	[UserID] NUMERIC NOT NULL PRIMARY KEY IDENTITY, 
    [RoleID] INT NOT NULL, 
    [firstname] NVARCHAR(100) NOT NULL, 
    [lastname] NVARCHAR(100) NOT NULL, 
    [mobileno] NUMERIC(10) NOT NULL, 
    [dob] DATE NOT NULL, 
    [email] VARCHAR(100) NOT NULL, 
    [upassword] VARCHAR(100) NOT NULL, 
    [secque] VARCHAR(200) NOT NULL, 
    [secans] VARCHAR(200) NOT NULL, 
    [gender] VARCHAR(10) NOT NULL, 
    [active] BIT NOT NULL, 
    [islogin] BIT NOT NULL
)

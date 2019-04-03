CREATE TABLE [dbo].[user_profile] (
    [ProfileID]  NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [UserID]     NUMERIC (18)   NOT NULL,
    [photo]      VARCHAR (MAX)  NULL,
    [profession] VARCHAR (100)  NULL,
    [at]         VARCHAR (100)  NULL,
    PRIMARY KEY CLUSTERED ([ProfileID] ASC)
);


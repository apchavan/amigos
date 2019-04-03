CREATE TABLE [dbo].[friends]
(
	[FriendID] NUMERIC NOT NULL PRIMARY KEY IDENTITY, 
    [from_UserID] NUMERIC NOT NULL, 
    [to_UserID] NUMERIC NOT NULL, 
    [confirmed] BIT NOT NULL
)

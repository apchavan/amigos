CREATE TABLE [dbo].[user_chat] (
    [ChatID]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [sender_UserID]   NUMERIC (18) NOT NULL,
    [receiver_UserID] NUMERIC (18) NOT NULL,
    [chat_date_time]  DATETIME     NOT NULL,
    [chat_message] NVARCHAR(200) NOT NULL, 
    PRIMARY KEY CLUSTERED ([ChatID] ASC)
);


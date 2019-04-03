CREATE TABLE [dbo].[posts_mst] (
    [PostID]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [UserID]     NUMERIC (18)   NOT NULL,
    [post_heading] NVARCHAR(100) NOT NULL,
	[post_text]  NVARCHAR (MAX) NOT NULL,
    [post_image] VARCHAR (MAX)  NULL,
    [dated]      DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([PostID] ASC)
);


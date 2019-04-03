CREATE TABLE [dbo].[posts_comments]
(
	[CommentID] NUMERIC NOT NULL PRIMARY KEY IDENTITY, 
    [PostID] NUMERIC NOT NULL, 
    [commenter_UserID] NUMERIC NOT NULL, 
    [comment_text] NVARCHAR(MAX) NOT NULL, 
    [dated] DATETIME NOT NULL
)

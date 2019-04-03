CREATE TABLE [dbo].[reported_posts]
(
	[ReportID] NUMERIC NOT NULL PRIMARY KEY IDENTITY, 
    [PostID] NUMERIC NOT NULL, 
    [poster_UserID] NUMERIC NOT NULL, 
    [reporter_UserID] NUMERIC NOT NULL, 
    [report_text] NVARCHAR(MAX) NULL, 
    [dated] DATETIME NOT NULL
)

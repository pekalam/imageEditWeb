CREATE TABLE [dbo].[DbImgTaskResult]
(
	[GroupId] UNIQUEIDENTIFIER,
	[TaskId] UNIQUEIDENTIFIER, 
	[Image] VARBINARY(MAX), 
	[Extension] VARCHAR(5),
	PRIMARY KEY CLUSTERED ([GroupId], [TaskId])
)

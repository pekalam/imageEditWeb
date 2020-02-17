CREATE TABLE [dbo].[DbEditTaskResult]
(
	[Id] BIGINT PRIMARY KEY NONCLUSTERED IDENTITY,
	[TaskId] UNIQUEIDENTIFIER NOT NULL, 
	[GroupId] UNIQUEIDENTIFIER NOT NULL,
	[Image] VARBINARY(MAX) NOT NULL, 
	[Extension] NCHAR(5) NOT NULL,
	CONSTRAINT FK_DbEditTaskResult_DbEditTaskProgress
	FOREIGN KEY ([TaskId], [GroupId]) 
	REFERENCES [dbo].[DbEditTaskProgress]([TaskId], [GroupId]),
	INDEX FK_Index UNIQUE CLUSTERED([TaskId], [GroupId])
)

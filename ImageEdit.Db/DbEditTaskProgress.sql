CREATE TABLE [dbo].[DbEditTaskProgress]
(
	[TaskId] UNIQUEIDENTIFIER NOT NULL, 
	[GroupId] UNIQUEIDENTIFIER NOT NULL, 
	[EditTaskState] INT NOT NULL, 
	PRIMARY KEY ([TaskId], [GroupId])
)

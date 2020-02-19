CREATE TABLE [dbo].[DbImgTaskProgress]
(
	[GroupId] UNIQUEIDENTIFIER NOT NULL,
	[TaskId] UNIQUEIDENTIFIER NOT NULL,  
	[ImgTaskState] INT NOT NULL, 
	PRIMARY KEY CLUSTERED ([GroupId], [TaskId]),
	CONSTRAINT FK_DbImgTaskProgress_DbImgTaskResult
	FOREIGN KEY ([GroupId], [TaskId]) 
	REFERENCES [dbo].[DbImgTaskResult]([GroupId], [TaskId])
)

﻿CREATE TABLE [dbo].[DbImg]
(
	[ImgId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[Image] VARBINARY(MAX) NOT NULL, 
	[Extension] VARCHAR(5) NOT NULL
)

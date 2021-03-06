SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArriveCity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ArriveCity](
	[PKId] [int] NOT NULL,
	[ArrivePKId] [int] NOT NULL,
	[CityName] [nvarchar](200) NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_ArriveCity] PRIMARY KEY CLUSTERED 
(
	[PKId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SendCity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SendCity](
	[PKId] [int] NOT NULL,
	[SendPKId] [int] NOT NULL,
	[CityName] [nvarchar](200) NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_SendCity] PRIMARY KEY CLUSTERED 
(
	[PKId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SendProvince]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SendProvince](
	[PKId] [int] NOT NULL,
	[ProvinceName] [nvarchar](200) NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_SendProvince] PRIMARY KEY CLUSTERED 
(
	[PKId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArriveProvince]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ArriveProvince](
	[PKId] [int] NOT NULL,
	[ProvinceName] [nvarchar](200) NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_ArriveProvince] PRIMARY KEY CLUSTERED 
(
	[PKId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END

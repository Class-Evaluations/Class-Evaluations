USE [Survey_DB_Test]
GO
/****** Object:  User [svr_user]    Script Date: 06/21/2011 10:31:14 ******/
CREATE USER [svr_user] FOR LOGIN [svr_user] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[COURSE]    Script Date: 06/21/2011 10:30:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COURSE](
	[course_id] [int] NOT NULL,
	[activity_id] [int] NOT NULL,
	[course_title] [nchar](60) NULL,
	[activity_title] [nchar](60) NULL,
	[barcode_number] [nchar](15) NOT NULL,
	[course_status_id] [nchar](1) NULL,
	[survey_status_flag] [nchar](1) NULL,
	[import_datestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_COURSE] PRIMARY KEY CLUSTERED 
(
	[course_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spGetCourseInformation]    Script Date: 06/21/2011 10:30:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donna Taylor
-- Create date: June 12, 2011
-- Description:	This stored procedure is executed form
-- spspGetMaxCourseID which passes in the Max course_id 
-- from Survey_DB_Test -.dbo.COURSE. Drop the temporiary table 
-- in Survey_DB_Test and selects the coutse information into the temporary
-- table, this tablw will be added to the course table
-- the currently exisits.
-- =============================================
CREATE PROCEDURE [dbo].[spGetCourseInformation](
	-- parameter is passed from spGetMaxCourseID where 
	-- this procedure is executed from
	@MaxCourseID int) 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;
SELECT 	c.course_id AS course_id, 
			a.activity_id AS activity_id,
			RTRIM(c.title) AS coures_title,  
			RTRIM(a.title) AS actvity_title,
			c.barcode_number AS barcode_number,
			c.course_status_id AS course_status_id, 
			NULL AS survey_status_flag,
			GETDATE() AS import_datestamp
	INTO Survey_DB_Test.dbo.COURSE_UPDATE
	FROM CLASS_701.dbo.COURSE c
	Join CLASS_701.dbo.ACTIVITY a ON a.activity_id = c.activity_id
	WHERE c.course_id > @MaxCourseID AND 
		 c.first_end_datetime > '2010-01-01 00:00:00.000' AND 
		(c.course_status_id not in ('A', 'X', 'I') OR 
		 c.last_end_datetime >= c.last_end_datetime + 7)

-- Insert the rows seleted from the above conditions and insert into the COURSE table
INSERT INTO Survey_DB_Test.dbo.COURSE  SELECT * FROM Survey_DB_Test.dbo.COURSE_UPDATE


-- Delete the temporary tale
DROP TABLE Survey_DB_Test.dbo.COURSE_UPDATE
END
GO
/****** Object:  Table [dbo].[REGISTRATION]    Script Date: 06/21/2011 10:31:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[REGISTRATION](
	[registration_id] [int] NOT NULL,
	[course_id] [int] NOT NULL,
	[client_id] [int] NOT NULL,
	[import_datestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_REGISTRATION] PRIMARY KEY CLUSTERED 
(
	[registration_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PERSON]    Script Date: 06/21/2011 10:30:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PERSON](
	[person_id] [int] NOT NULL,
	[client_id] [int] NOT NULL,
	[last_name] [char](40) NULL,
	[first_name] [char](40) NULL,
	[address_1] [char](45) NULL,
	[address_2] [char](75) NULL,
	[city] [char](40) NULL,
	[providence] [char](40) NULL,
	[postal_code] [char](10) NULL,
	[birthdate] [datetime] NULL,
	[email_address] [char](50) NULL,
	[email_private] [tinyint] NULL,
	[invalid_email] [char](1) NULL,
	[import_datestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_PERSON] PRIMARY KEY CLUSTERED 
(
	[person_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[spGetRegistrationInfo]    Script Date: 06/21/2011 10:30:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donna Taylor
-- Create date: <Create Date,,>June 16, 2011
-- Description:	This stored procedure updates the
-- REGISTRATION table from the Master DB in this
-- case it is CLASS_701
-- =============================================
CREATE PROCEDURE [dbo].[spGetRegistrationInfo](
	-- parameter is passed from spUpdateFromMasterDB where 
	-- this procedure is executed from
	@MaxCourseID int) 
	
AS
BEGIN

SET NOCOUNT ON;

SELECT	registration_id,
		course_id,
		client_id,
		GETDATE() AS import_datestamp
	INTO Survey_DB_Test.dbo.REGISTRATION_UPDATE
	FROM CLASS_701.dbo.REGISTRATION
	WHERE course_id > @MaxCourseID

-- Insert the rows seleted from the above conditions and insert into the COURSE table
INSERT INTO Survey_DB_Test.dbo.REGISTRATION  SELECT * FROM Survey_DB_Test.dbo.REGISTRATION_UPDATE


-- Delete the temporary tale
DROP TABLE Survey_DB_Test.dbo.REGISTRATION_UPDATE
END
GO
/****** Object:  Table [dbo].[SURVEY_SECTION]    Script Date: 06/21/2011 10:31:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SURVEY_SECTION](
	[survey_section_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[survey_id] [int] NOT NULL,
	[title] [nchar](200) NOT NULL,
	[datestamp] [datetime] NOT NULL,
	[user_stamp] [int] NOT NULL,
 CONSTRAINT [PK_SURVEY_SECTION] PRIMARY KEY CLUSTERED 
(
	[survey_section_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANSWER_TYPE]    Script Date: 06/21/2011 10:30:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANSWER_TYPE](
	[answer_type_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[answer_type_name] [nchar](40) NOT NULL,
	[multi_choice_single] [bit] NOT NULL,
	[multi_choice_multi] [bit] NOT NULL,
	[scale] [bit] NOT NULL,
	[answer_long] [bit] NOT NULL,
	[answer_short] [bit] NOT NULL,
	[true_false] [bit] NOT NULL,
	[datestamp] [datetime] NOT NULL,
	[user_stamp] [int] NOT NULL,
 CONSTRAINT [PK_ANSWER_TYPE] PRIMARY KEY CLUSTERED 
(
	[answer_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spGetPersonInfo]    Script Date: 06/21/2011 10:30:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donna Taylor
-- Create date: <Create Date,,>June 16, 2011
-- Description:	This stored procedure updates the
-- REGISTRATION table from the Master DB in this
-- case it is CLASS_701
-- =============================================
CREATE PROCEDURE [dbo].[spGetPersonInfo](
	-- parameter is passed from spUpdateFromMasterDB where 
	-- this procedure is executed from
	@MaxCourseID int) 
	
AS
BEGIN

SET NOCOUNT ON;

SELECT
	p.person_id,
	r.client_id,
	p.last_name,
	p.first_name,
	a.address1,
	a.address2,
	c.city_name,
	a.province_code,
	a.postal_code,
	p.birthdate,
	p.email_address,
	p.email_private,
	NULL as invalid_email,
	GETDATE() AS import_datestamp
INTO Survey_DB_Test.dbo.PERSON_UPDATE
FROM CLASS_701.dbo.REGISTRATION r
JOIN CLASS_701.dbo.CLIENT cl ON cl.client_id = r.client_id
JOIN CLASS_701.dbo.PERSON p ON p.person_id = cl.person_id
JOIN CLASS_701.dbo.ADDRESS a ON a.address_id = p.address_id
JOIN CLASS_701.dbo.ACCOUNT ac ON ac.account_id = cl.account_id
JOIN CLASS_701.dbo.CITY c ON c.city_id = a.city_id
WHERE course_id > @MaxCourseID

-- Insert the rows seleted from the above conditions and insert into the PESON table
INSERT INTO Survey_DB_Test.dbo.PERSON  
SELECT DISTINCT(person_id), client_id, last_name, first_name, address1 AS address_1, address2 AS address_2, city_name, province_code,
				postal_code, birthdate, email_address, email_private, invalid_email,
				import_datestamp FROM Survey_DB_Test.dbo.PERSON_UPDATE


-- Delete the temporary tale
DROP TABLE Survey_DB_Test.dbo.PERSON_UPDATE
END
GO
/****** Object:  Table [dbo].[QUESTION]    Script Date: 06/21/2011 10:30:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QUESTION](
	[question_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[question_text] [nchar](200) NOT NULL,
	[answer_type_id] [int] NOT NULL,
	[datestamp] [datetime] NOT NULL,
	[user_stamp] [int] NOT NULL,
 CONSTRAINT [PK_QUESTION] PRIMARY KEY CLUSTERED 
(
	[question_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SURVEY]    Script Date: 06/21/2011 10:31:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SURVEY](
	[survey_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[title] [nchar](40) NOT NULL,
	[header_text] [nchar](200) NULL,
	[survey_status] [nchar](1) NOT NULL,
	[lifetime] [int] NOT NULL,
	[datestamp] [datetime] NOT NULL,
	[user_stamp] [int] NOT NULL,
 CONSTRAINT [PK_SURVEY] PRIMARY KEY CLUSTERED 
(
	[survey_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[USERS]    Script Date: 06/21/2011 10:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[USERS](
	[user_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[user_name] [nchar](20) NOT NULL,
	[password] [nchar](20) NOT NULL,
	[datestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_USERS] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SURVEY_QUESTIONS]    Script Date: 06/21/2011 10:31:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SURVEY_QUESTIONS](
	[survey_quesrions_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[survey_id] [int] NOT NULL,
	[question_id] [int] NOT NULL,
	[section_id] [int] NOT NULL,
 CONSTRAINT [PK_SURVEY_QUESTIONS] PRIMARY KEY CLUSTERED 
(
	[survey_quesrions_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COURSE_SURVEYED]    Script Date: 06/21/2011 10:30:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COURSE_SURVEYED](
	[course_surveyed_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[course_id] [int] NOT NULL,
	[survey_id] [int] NOT NULL,
	[datestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_COURSE_SURVEYED] PRIMARY KEY CLUSTERED 
(
	[course_surveyed_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANSWER_LONG]    Script Date: 06/21/2011 10:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANSWER_LONG](
	[answer_long_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[answer_id] [int] NOT NULL,
	[submittted_answer] [nchar](2000) NOT NULL,
 CONSTRAINT [PK_ANSWER_LONG] PRIMARY KEY CLUSTERED 
(
	[answer_long_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANSWER_MULTIPLE_CHOICE]    Script Date: 06/21/2011 10:30:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANSWER_MULTIPLE_CHOICE](
	[answer_multiple_choice_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[answer_id] [int] NOT NULL,
	[submitted_answer] [int] NOT NULL,
 CONSTRAINT [PK_ANSWER_MULTIPLE_CHOIC] PRIMARY KEY CLUSTERED 
(
	[answer_multiple_choice_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANSWER_SCALE]    Script Date: 06/21/2011 10:30:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANSWER_SCALE](
	[answer_scale_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[answer_id] [int] NOT NULL,
	[submitted_answer] [int] NOT NULL,
 CONSTRAINT [PK_ANSWER_SCALE] PRIMARY KEY CLUSTERED 
(
	[answer_scale_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANSWER_SHORT]    Script Date: 06/21/2011 10:30:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANSWER_SHORT](
	[answer_short_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[answer_id] [int] NOT NULL,
	[submitted_answer_text] [nchar](250) NOT NULL,
 CONSTRAINT [PK_ANSWER_SHORT] PRIMARY KEY CLUSTERED 
(
	[answer_short_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANSWER_TRUE_FALSE]    Script Date: 06/21/2011 10:30:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANSWER_TRUE_FALSE](
	[answer_true_false_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[answer_id] [int] NOT NULL,
	[submitted_answer] [bit] NOT NULL,
 CONSTRAINT [PK_ANSWER_TRUE_FALSE] PRIMARY KEY CLUSTERED 
(
	[answer_true_false_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ANSWER]    Script Date: 06/21/2011 10:30:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ANSWER](
	[answer_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[question_id] [int] NOT NULL,
	[answer_type_id] [int] NOT NULL,
 CONSTRAINT [PK_ANSWER] PRIMARY KEY CLUSTERED 
(
	[answer_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QUESTION_MULTIPLE_CHOICE]    Script Date: 06/21/2011 10:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QUESTION_MULTIPLE_CHOICE](
	[question_multiple_choice_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[question_id] [int] NOT NULL,
	[choice_text] [nchar](200) NOT NULL,
 CONSTRAINT [PK_QUESTION_MULTIPLE_CHOICE] PRIMARY KEY CLUSTERED 
(
	[question_multiple_choice_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QUESTION_SCALE]    Script Date: 06/21/2011 10:31:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QUESTION_SCALE](
	[question_scale_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[question_id] [int] NOT NULL,
	[scale_top_number] [int] NOT NULL,
	[scale_bottom_number] [int] NOT NULL,
	[scale_top_text] [nchar](40) NOT NULL,
	[scale_bottom_text] [nchar](40) NOT NULL,
 CONSTRAINT [PK_QUESTION_SCALE] PRIMARY KEY CLUSTERED 
(
	[question_scale_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SURVEY_REQUEST_SENT]    Script Date: 06/21/2011 10:31:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SURVEY_REQUEST_SENT](
	[survey_request_sent_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[survey_id] [int] NOT NULL,
	[person_hash] [nchar](160) NOT NULL,
	[expiration_date] [datetime] NOT NULL,
	[email_address] [nchar](75) NOT NULL,
	[status_flag] [nchar](1) NOT NULL,
	[datestamp] [datetime] NOT NULL,
	[user_stamp] [int] NOT NULL,
	[course_id] [int] NOT NULL,
 CONSTRAINT [PK_SURVEY_REQUEST_SENT] PRIMARY KEY CLUSTERED 
(
	[survey_request_sent_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spGetSurveyQuestions]    Script Date: 06/21/2011 10:30:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donna Taylor
-- Create date: 5/26/2011
-- Description:	The parameter passed in is the 
--				survey number the purpose is to extract
--				the needed information to build the 
--				user requested survey.
-- =============================================
CREATE PROCEDURE [dbo].[spGetSurveyQuestions](
	-- Add the parameters for the stored procedure here
	@survey_id int) 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- The sql statements extract the rows needed for the 
	-- building the survey questins

	SELECT	sq.survey_id, 
			sq.section_id, 
			ss.title, 
			q.question_id, 
			q.question_text, 
			at.answer_type_id, 
			at.answer_type_name 
	FROM Survey_DB_Test.dbo.QUESTION q
	JOIN Survey_DB_Test.dbo.ANSWER_TYPE at on at.answer_type_id = q.answer_type_id
	JOIN Survey_DB_Test.dbo.SURVEY_QUESTIONS sq on sq.question_id = q.question_id
	RIGHT JOIN Survey_DB_Test.dbo.SURVEY_SECTION ss on ss.survey_id = sq.survey_id
	WHERE ss.survey_section_id = sq.section_id
	AND sq.survey_id = @survey_id

END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateDataFromMastDB]    Script Date: 06/21/2011 10:30:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donna Taylor
-- Create date: June 14, 2011
-- Description:	This stored procedure calls the 
-- stored procedures that update Survey_DB_Test from
-- the Master DB in this case CLASS_701
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateDataFromMastDB]
				@MaxCourseID INT OUTPUT AS
	
BEGIN
SELECT @MaxCourseID = MAX(course_id) FROM Survey_DB_Test.dbo.COURSE	

EXEC dbo.spGetCourseInformation @MaxCourseID

EXEC dbo.spGetRegistrationInfo @MaxCourseID

EXEC dbo.spGetPersonInfo @MaxCourseID

END
GO

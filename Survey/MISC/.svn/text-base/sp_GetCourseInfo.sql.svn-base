USE [Survey_DB]
GO
/****** Object:  StoredProcedure [dbo].[spGetCourseInfo]    Script Date: 06/10/2011 09:29:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Dona Taylor 
-- Create date:  May 10, 2011 
-- Description:	This stored procedure extracts the 
-- information for the courses that have been marked
-- as coompleted or are 7 days passed the last_end_date
-- parameters returned are the course title concatenated
-- with the activity title, and the course id 
-- =============================================
CREATE PROCEDURE [dbo].[spGetCourseInfo]
AS
BEGIN

    -- Select the course that are not activit, cancelled or Incomplete
	-- or the course end date is past a week
	SET NOCOUNT ON;
	SELECT 	c.course_id AS course_id, 
			a.activity_id AS activity_id,
			RTRIM(c.title) AS coures_title,  
			RTRIM(a.title) AS actvity_title,
			c.barcode_number AS barcode_number,
			c.course_status_id AS course_status_id, 
			NULL AS survey_status_flag,
			GETDATE() AS import_datestamp
	FROM CLASS_701.dbo.COURSE c
	Join CLASS_701.dbo.ACTIVITY a ON a.activity_id = c.activity_id
	WHERE c.first_end_datetime > '2010-01-01 00:00:00.000' AND 
		(c.course_status_id not in ('A', 'X', 'I') OR 
		 c.last_end_datetime >= c.last_end_datetime + 7)
END

GO

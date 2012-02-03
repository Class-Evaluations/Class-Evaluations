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
CREATE PROCEDURE spGetSurveyQuestions(
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
	FROM Survey_DB.dbo.QUESTION q
	JOIN Survey_DB.dbo.ANSWER_TYPE at on at.answer_type_id = q.answer_type_id
	JOIN Survey_DB.dbo.SURVEY_QUESTIONS sq on sq.question_id = q.question_id
	RIGHT JOIN Survey_DB.dbo.SURVEY_SECTION ss on ss.survey_id = sq.survey_id
	WHERE ss.survey_section_id = sq.section_id
	AND sq.survey_id = @survey_id

END
GO

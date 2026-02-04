CREATE PROCEDURE [dbo].[sp_Assessment]
	@QueryType NVARCHAR(100)  = '',
	 @UserId INT = 0,
    @QuestionId INT = 0,
    @Response NVARCHAR(500) = '',
	@Category NVARCHAR(100) = ''
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @QueryType = 'GetAllQuestions'
	BEGIN
		SELECT Id, QuestionText, Category
		FROM Questions;
	END

	ELSE IF @QueryType = 'GetQuestionsByCategory'
	BEGIN
		  SELECT Id, QuestionText
		  FROM Questions
		  WHERE Category = @Category;
	END

	ELSE IF @QueryType = 'SaveUserResponse'
	BEGIN
		 INSERT INTO UserResponses (UserId, QuestionId, Response)
		 VALUES (@UserId, @QuestionId, @Response);
	END

	ELSE IF @QueryType = 'GetUserResponses'
	BEGIN
		 SELECT ur.Id, q.QuestionText, ur.Response
		FROM UserResponses ur
		JOIN Questions q ON ur.QuestionId = q.Id
		WHERE ur.UserId = @UserId;
	END


	ELSE IF @QueryType = 'GetGuidanceRecommendations'
	BEGIN
		 SELECT g.GuidanceText
		FROM Guidance g
		WHERE g.Category IN (
			SELECT q.Category
			FROM UserResponses ur
			JOIN Questions q ON ur.QuestionId = q.Id
			WHERE ur.UserId = @UserId
			);
	END
END

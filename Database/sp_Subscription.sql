CREATE PROCEDURE [dbo].[sp_Subscription]
     @UserId INT = 0,
    @PlanType NVARCHAR(50) = '',
    @StartDate DATETIME = null,
    @EndDate DATETIME = null,
	@QueryType NVARCHAR(100)  = ''
AS
BEGIN
 
	SET NOCOUNT ON;
	IF @QueryType = 'Create'
	BEGIN

		IF EXISTS (SELECT 1 FROM Subscriptions WHERE UserId = @UserId)
		BEGIN
			-- Update existing subscription
			UPDATE Subscriptions
			SET PlanType = @PlanType, StartDate = @StartDate, EndDate = @EndDate, IsActive = 1
			WHERE UserId = @UserId;
		END
		ELSE
		BEGIN
			-- Insert new subscription
			INSERT INTO Subscriptions (UserId, PlanType, StartDate, EndDate, IsActive)
			VALUES (@UserId, @PlanType, @StartDate, @EndDate, 1);
		END
	END
    
	ELSE IF @QueryType = 'Check'
	BEGIN
		SELECT Id, UserId, PlanType, StartDate, EndDate, IsActive
		FROM Subscriptions
		WHERE UserId = @UserId;
	END

	ELSE IF @QueryType = 'Deactivate'
	BEGIN
		 UPDATE Subscriptions
		 SET IsActive = 0
		 WHERE EndDate < GETDATE() AND IsActive = 1;
	END

	
END
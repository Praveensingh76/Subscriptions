CREATE PROCEDURE [dbo].[sp_User]
    @Username NVARCHAR(100) = '',
    @Email NVARCHAR(100) = '' ,
    @PasswordHash NVARCHAR(256) = '' ,
    @SubscriptionStatus NVARCHAR(50) = '' ,
	@QueryType NVARCHAR(100)  = ''
AS
BEGIN

	SET NOCOUNT ON;

	IF @QueryType = 'Register'
		BEGIN

		IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
		BEGIN
			INSERT INTO Users (Username, Email, PasswordHash, SubscriptionStatus)
			VALUES (@Username, @Email, @PasswordHash, @SubscriptionStatus);
			SELECT Id  from Users WHERE Email = @Email
		END
		ELSE
			BEGIN
			 SELECT 0
			END
	END
    
	ELSE IF @QueryType = 'Login'
		BEGIN
			  SELECT Id, Username, Email, SubscriptionStatus
			  FROM Users
			  WHERE Email = @Email AND PasswordHash = @PasswordHash;
		END

    
END
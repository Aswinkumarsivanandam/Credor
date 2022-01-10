ALTER TRIGGER dbo.UserProfile_Insert
ON  dbo.UserProfile
AFTER INSERT
AS 
BEGIN   
SET NOCOUNT ON;

DECLARE @ProfileId AS Int,
@DisplayId AS VARCHAR(10)

SELECT @ProfileId = Id, @DisplayId = DisplayId FROM inserted 

UPDATE dbo.UserProfile 
SET  DisplayId =  @DisplayId +  CAST(@ProfileId AS VARCHAR(10))
WHERE Id = @ProfileId 

END
GO

CREATE PROCEDURE [dbo].[DeleteMessageById]
	@param1 int
AS
	DELETE Messages where Id = @param1
RETURN 0

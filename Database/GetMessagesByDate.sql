CREATE PROCEDURE [dbo].[GetMessagesByDate]
	@param1 datetime
AS
begin
	SELECT * from Messages where DelayDateTime < @param1
end

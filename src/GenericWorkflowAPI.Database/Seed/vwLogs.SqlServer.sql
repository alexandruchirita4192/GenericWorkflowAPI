IF OBJECT_ID('vwLogs') IS NOT NULL
	DROP VIEW vwLogs;
GO
CREATE VIEW vwLogs
AS
SELECT
	L.Id,
	L.[Message],
	L.MessageTemplate,
	L.[Level],
	L.[TimeStamp],
	L.Exception,
	CAST(L.Properties AS XML) AS Properties
FROM Logs L
GO

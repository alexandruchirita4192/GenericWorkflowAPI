CREATE VIEW IF NOT EXISTS vwLogs
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

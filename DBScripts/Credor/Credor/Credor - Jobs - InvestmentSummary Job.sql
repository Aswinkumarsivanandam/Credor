CREATE TABLE #Summary_ActiveInvestments(UserId INT, ActiveInvestments INT)
INSERT INTO #Summary_ActiveInvestments
(UserId, ActiveInvestments)
SELECT  userAccount.Id,
COUNT(investment.Id)
FROM dbo.UserAccount AS userAccount
INNER JOIN dbo.Investment AS investment ON userAccount.Id = investment.UserId
WHERE 
	userAccount.Active = 1
	AND investment.Status = 1 
	AND investment.Active = 1
GROUP BY(userAccount.Id)

CREATE TABLE #Summary_PendingInvestments(UserId INT, PendingInvestments INT)
INSERT INTO #Summary_PendingInvestments
(UserId, PendingInvestments)
SELECT  userAccount.Id,
COUNT(investment.Id)
FROM dbo.UserAccount AS userAccount
INNER JOIN dbo.Investment AS investment ON userAccount.Id = investment.UserId
WHERE 
	userAccount.Active = 1
	AND investment.Status IN (3,4)
	AND investment.Active = 1
GROUP BY(userAccount.Id)

CREATE TABLE #Summary_TotalInvested(UserId INT, TotalInvested DECIMAL)
INSERT INTO #Summary_TotalInvested
(UserId, TotalInvested)
SELECT  userAccount.Id,
SUM(investment.AMOUNT)
FROM dbo.UserAccount AS userAccount
INNER JOIN dbo.Investment AS investment ON userAccount.Id = investment.UserId
WHERE 
	userAccount.Active = 1
	AND investment.Status = 1
	AND investment.Active = 1
GROUP BY(userAccount.Id)

CREATE TABLE #Summary_TotalEarnings(UserId INT, TotalEarnings DECIMAL)
INSERT INTO #Summary_TotalEarnings
(UserId, TotalEarnings)
SELECT  userAccount.Id,
SUM(payment.AMOUNT)
FROM dbo.UserAccount AS userAccount
INNER JOIN dbo.Investment AS investment ON userAccount.Id = investment.UserId
INNER JOIN dbo.Payment AS payment ON  investment.Id = payment.InvestmentId
WHERE 
	userAccount.Active = 1	
	AND payment.Type = 1
GROUP BY(userAccount.Id)

TRUNCATE TABLE dbo.InvestmentSummary 
INSERT INTO dbo.InvestmentSummary (UserId)
SELECT Id
FROM dbo.UserAccount
WHERE Active = 1

UPDATE dbo.InvestmentSummary 
SET ActiveInvestments = active.ActiveInvestments
FROM #Summary_ActiveInvestments AS active 
INNER JOIN dbo.InvestmentSummary AS summary ON summary.UserId = active.UserId

UPDATE dbo.InvestmentSummary 
SET PendingInvestments = pending.PendingInvestments
FROM #Summary_PendingInvestments AS pending 
INNER JOIN dbo.InvestmentSummary AS summary ON summary.UserId = pending.UserId

UPDATE dbo.InvestmentSummary 
SET TotalInvested = invested.TotalInvested
FROM #Summary_TotalInvested AS invested 
INNER JOIN dbo.InvestmentSummary AS summary ON summary.UserId = invested.UserId


UPDATE dbo.InvestmentSummary 
SET TotalEarnings = earnings.TotalEarnings
FROM #Summary_TotalEarnings AS earnings 
INNER JOIN dbo.InvestmentSummary AS summary ON summary.UserId = earnings.UserId

UPDATE dbo.InvestmentSummary 
SET TotalReturn = CASE WHEN summary.TotalEarnings > summary.TotalInvested
					THEN((summary.TotalEarnings / summary.TotalInvested)*100) - 100
				  WHEN summary.TotalEarnings < summary.TotalInvested
					THEN((summary.TotalEarnings / summary.TotalInvested)*100)
				  ELSE 100
				  END
FROM #Summary_TotalEarnings AS earnings 
INNER JOIN dbo.InvestmentSummary AS summary ON summary.UserId = earnings.UserId


DROP TABLE #Summary_ActiveInvestments
DROP TABLE #Summary_PendingInvestments
DROP TABLE #Summary_TotalInvested
DROP TABLE #Summary_TotalEarnings







            
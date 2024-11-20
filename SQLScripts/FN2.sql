DROP FUNCTION IF EXISTS fn_GetCurrentCurrencyExchangeCourse;
CREATE FUNCTION fn_GetCurrentCurrencyExchangeCourse()
RETURNS TABLE(CurrencyId UUID, CurrencyISOName CurrencyISONames, SaleValue NUMERIC, PurchaseValue NUMERIC, CreationDate TIMESTAMP)
AS
$$
BEGIN
	
	RETURN QUERY WITH cteMaxDate AS
	(
		SELECT c.CurrencyId, c.CurrencyISOName, MAX(ces.CreationDate) as MaxCreationDate
		FROM CurrencyExchangeCourse ces
		INNER JOIN Currency c
			ON ces.CurrencyId = c.CurrencyId
		WHERE ces.CreationDate::date = CURRENT_DATE
		GROUP BY c.CurrencyId, c.CurrencyISOName
	)
    SELECT 
	    c.CurrencyId
		, c.CurrencyISOName
		, ces.SaleValue
		, ces.PurchaseValue
		, cmd.MaxCreationDate
	FROM CurrencyExchangeCourse ces
	INNER JOIN Currency c
		ON ces.CurrencyId = c.CurrencyId
	INNER JOIN cteMaxDate cmd
		ON c.CurrencyISOName = cmd.CurrencyISOName
	WHERE ces.CreationDate = cmd.MaxCreationDate;

END;
$$ LANGUAGE plpgsql;
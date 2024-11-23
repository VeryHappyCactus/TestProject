
  \c "MyTestDB";

DROP FUNCTION IF EXISTS fn_GetCurrencyExchangeCourse;
CREATE FUNCTION fn_GetCurrencyExchangeCourse(IN searchDate DATE DEFAULT NULL)
RETURNS TABLE(CurrencyId UUID, CurrencyISOName CurrencyISONames, SaleValue NUMERIC, PurchaseValue NUMERIC, CreationDate TIMESTAMP)
AS
$$
BEGIN

	IF (searchDate IS NOT NULL) THEN

		RETURN QUERY WITH cteMaxDate AS
		(
			SELECT c.CurrencyId, c.CurrencyISOName, MAX(ces.CreationDate) as MaxCreationDate
			FROM CurrencyExchangeCourse ces
			INNER JOIN Currency c
				ON ces.CurrencyId = c.CurrencyId
			WHERE ces.CreationDate::date = searchDate
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
		
	END IF;

	IF (searchDate IS NULL) THEN
 		RETURN QUERY SELECT 
		    c.CurrencyId
			, c.CurrencyISOName
			, ces.SaleValue
			, ces.PurchaseValue
			, ces.CreationDate
		FROM CurrencyExchangeCourse ces
		INNER JOIN Currency c
			ON ces.CurrencyId = c.CurrencyId;
	END IF;
END;
$$ LANGUAGE plpgsql;

  \c "MyTestDB";

DROP FUNCTION IF EXISTS fn_GetCurrencyExchangeCourse;
CREATE FUNCTION fn_GetCurrencyExchangeCourse(IN searchDate DATE DEFAULT NULL)
RETURNS JSON
AS
$$
	DECLARE 
		res JSON = NULL;
	BEGIN
	
		IF (searchDate IS NOT NULL) THEN

			res:= 
			(
				WITH cteMaxDate AS
				(
					SELECT c.CurrencyId, c.CurrencyISOName, MAX(ces.CreationDate) as MaxCreationDate
					FROM CurrencyExchangeCourse ces
					INNER JOIN Currency c
						ON ces.CurrencyId = c.CurrencyId
					WHERE ces.CreationDate::date = searchDate
					GROUP BY c.CurrencyId, c.CurrencyISOName
				)
				 SELECT json_agg(t)
				 FROM 
				 (
					
				    SELECT 
						 c.CurrencyISOName
						, ces.SaleValue
						, ces.PurchaseValue
						, cmd.MaxCreationDate as CreationDate
					FROM CurrencyExchangeCourse ces
					INNER JOIN Currency c
						ON ces.CurrencyId = c.CurrencyId
					INNER JOIN cteMaxDate cmd
						ON c.CurrencyISOName = cmd.CurrencyISOName
					WHERE ces.CreationDate = cmd.MaxCreationDate
				 ) as t
			);
				
		END IF;
	
		IF (searchDate IS NULL) THEN
			res:= 
			(
				 SELECT json_agg(t)
				 FROM 
				 (
					SELECT 
						 c.CurrencyISOName
						, ces.SaleValue
						, ces.PurchaseValue
						, ces.CreationDate
					FROM CurrencyExchangeCourse ces
					INNER JOIN Currency c
						ON ces.CurrencyId = c.CurrencyId
				) as t
			);
		END IF;

		return res;
	END;
$$ LANGUAGE plpgsql;
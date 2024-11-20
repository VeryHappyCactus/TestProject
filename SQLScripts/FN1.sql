DROP FUNCTION IF EXISTS fn_GetStatusInfo;
CREATE FUNCTION fn_GetStatusInfo(IN msg JSONB)
RETURNS JSON
AS 
$$
	DECLARE
		res JSON = NULL;
	BEGIN

		IF (msg ? 'client_operation_id') THEN
			res := (
					WITH cte AS
					(
											SELECT 
						co.ClientOperationId          		
						, co.ClientOperationStatus    		
						, co.ClientOperationType      		
						, ca.AccountTotalAmount      		as CurrentTotalAmount
						, co.ClientAccountTotalAmountOld 	as TotalAamountOld
						, co.ClientAccountTotalAmountNew 	as TotalAmountNew
						, co.ClientOperationValue          as ClientOperationValue
						, c.CurrencyISOName					
						, ces.SaleValue						as CurrencyCourseSaleValue
						, ces.PurchaseValue					as CurrencyCoursePurchaseValue
						, (SELECT json_agg(t) 
							FROM (SELECT 
								CurrencyISOName 
								, SaleValue		
								, PurchaseValue	
								, CreationDate  
							 FROM fn_GetCurrentCurrencyExchangeCourse()) as t) as current_exchange_course
					FROM ClientOperation co
					INNER JOIN ClientAccount ca
						ON co.ClientAccountId = ca.ClientAccountId 
					LEFT JOIN CurrencyExchangeCourse ces
						ON co.CurrencyExchangeCourseId = ces.CurrencyExchangeCourseId
					LEFT JOIN Currency c
						ON c.CurrencyId = ces.CurrencyId
					WHERE co.ClientOperationId = (msg ->> 'client_operation_id')::uuid)

					SELECT to_json(c)
					FROM cte as c
			);

		END IF;

		IF (msg ? 'client_id' 
			AND msg ? 'department_address' 
			AND msg -> 'department_address' ? 'street_name' 
			AND msg -> 'department_address' ? 'building_number') THEN
				res := (
					WITH cte AS
					(
											SELECT 
						co.ClientOperationId          		
						, co.ClientOperationStatus    		
						, co.ClientOperationType      		
						, ca.AccountTotalAmount      		as CurrentTotalAmount
						, co.ClientAccountTotalAmountOld   as TotalAmountOld
						, co.ClientAccountTotalAmountNew   as TotalAmountNew
						, co.ClientOperationValue          
						, c.CurrencyISOName					
						, ces.SaleValue						as CurrencyCourseSaleValue
						, ces.PurchaseValue					as CurrencyCoursePurchaseValue
					FROM ClientAccount ca
					INNER JOIN ClientDepartment cd
						ON ca.ClientId = cd.Clientid
					INNER JOIN DepartmentAddress da
						ON da.DepartmentId = cd.DepartmentId 
						AND da.DepartmentAddressStreetName = (msg -> 'department_address' ->> 'street_name')
						AND da.departmentaddressBuildingNumber = (msg -> 'department_address' ->> 'building_number')
					INNER JOIN ClientOperation co
						ON co.ClientAccountId = ca.ClientAccountId 
					LEFT JOIN CurrencyExchangeCourse ces
						ON co.CurrencyExchangeCourseId = ces.CurrencyExchangeCourseId
					LEFT JOIN Currency c
						ON c.CurrencyId = ces.CurrencyId
					WHERE ca.ClientId = (msg ->> 'client_id')::uuid)

					SELECT json_agg(c.*) 
					FROM cte as c
			);

		END IF;

		RETURN res;

	END;
$$ LANGUAGE plpgsql;
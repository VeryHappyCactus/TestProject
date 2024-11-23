
  \c "MyTestDB";

DROP PROCEDURE IF EXISTS sp_Withdraw;
CREATE PROCEDURE sp_Withdraw(IN msg JSONB, INOUT rClientOperationId UUID DEFAULT NULL, INOUT rErrorCode INTEGER DEFAULT NULL)
LANGUAGE plpgsql
AS 
--DO
$$
DECLARE

	/*
	msg JSON := '{
		"client_id" : "784e626a-c7b7-400a-998d-861c4d646666", 
		"department_address": {
			"street_name":  "Приморская",
			"building_number": "6"
		},
		"amount": 10,
		"currency": "USD"
		}'::JSON;
		*/
	
	tClientId Client.ClientId%TYPE;	
	tClientAccountData RECORD;
	tClientAccountTotalAmountData RECORD;
	tCurrencyExchangeCourseData RECORD;
	tClientOperationStatus ClientOperationStatuses := 'SUCCESS'::ClientOperationStatuses;
	tClientOperationType ClientOperationTypes := 'WITHDRAW';
BEGIN

	--Errors
	--0x01 amount error, range is wrong
	--0x02 client is missing
	--0x03 we do no have any records in table for exchange rates

	--RAISE NOTICE 'msg: %', msg;
	--RAISE NOTICE 'clientId: %', (msg ->> 'client_id')::UUID;
	
	IF (((msg -> 'amount')::numeric < 100 AND (msg -> 'amount')::numeric > 100000)) THEN
		--RAISE NOTICE '0x01';
		rErrorCode := 0x01;
		RETURN;
	END IF;

	tClientId := 
	(
		SELECT DISTINCT c.ClientId
		FROM Client c
		INNER JOIN ClientDepartment cd
			ON c.ClientId = (msg ->> 'client_id')::UUID AND c.ClientId = cd.Clientid
		INNER JOIN DepartmentAddress da
			ON da.DepartmentId = cd.DepartmentId 
			AND da.DepartmentAddressStreetName = (msg -> 'department_address' ->> 'street_name')
			AND da.departmentaddressBuildingNumber = (msg -> 'department_address' ->> 'building_number')
		--FETCH FIRST ROW ONLY;
	);

	IF (tClientId IS NULL) THEN
		--RAISE NOTICE '0x02';
		rErrorCode := 0x02;
		RETURN;
	END IF;

	--RAISE NOTICE 'ClientId: %', tClientId;

	SELECT ca.ClientAccountId, c.CurrencyISOName, ca.AccountTotalAmount
	FROM ClientAccount ca
	INNER JOIN Currency c
		ON ca.CurrencyId = c.CurrencyId AND ca.ClientID = tClientId
	INTO tClientAccountData
	LIMIT 1; 
	

	RAISE NOTICE 'ClientAccountData: %', tClientAccountData;

	IF tClientAccountData.CurrencyISOName = (msg ->> 'currency')::CurrencyISONames THEN
		SELECT NULL::uuid as CurrencyExchangeCourseId, 1::numeric as PurchaseValue
		INTO tCurrencyExchangeCourseData;	
	ELSE 
		SELECT ces.CurrencyExchangeCourseId, ces.PurchaseValue
		FROM Currency c
		JOIN CurrencyExchangeCourse ces
			ON c.CurrencyId = ces.CurrencyId
		WHERE c.CurrencyISOName = (msg ->> 'currency')::CurrencyISONames 
		AND ces.CreationDate = 
		(
			SELECT MAX(tces.CreationDate)
			FROM CurrencyExchangeCourse tces
			WHERE tces.CurrencyId = c.CurrencyId AND tces.CreationDate::date <= CURRENT_DATE
		)
		INTO tCurrencyExchangeCourseData;	
	END IF;
	

	RAISE NOTICE 'CurrencyPurchaseValue: %', tCurrencyExchangeCourseData;

	IF (tCurrencyExchangeCourseData IS NULL OR tCurrencyExchangeCourseData.PurchaseValue IS NULL) THEN
		--RAISE NOTICE '0x03';
		rErrorCode := 0x03;
		RETURN;
	END IF;


	SELECT 
		ca.AccountTotalAmount - (msg ->> 'amount')::numeric * tCurrencyExchangeCourseData.PurchaseValue as AccountTotalAmountNew
		, (msg ->> 'amount')::numeric * tCurrencyExchangeCourseData.PurchaseValue as ClientOperationValue
	FROM ClientAccount ca
	INNER JOIN Currency c
		ON ca.CurrencyId = c.CurrencyId 
		AND ca.ClientID = tClientId
		AND c.CurrencyISOName = tClientAccountData.CurrencyISOName
	INTO tClientAccountTotalAmountData;

	RAISE NOTICE 'ClientAccountTotalAmountData %', tClientAccountTotalAmountData;

	IF (tClientAccountTotalAmountData IS NOT NULL AND tClientAccountTotalAmountData.AccountTotalAmountNew >= 0) THEN
		UPDATE ClientAccount ca
		SET AccountTotalAmount = tClientAccountTotalAmountData.AccountTotalAmountNew
		FROM Currency c
		WHERE ca.CurrencyId = c.CurrencyId AND ca.ClientID = tClientId AND c.CurrencyISOName = tClientAccountData.CurrencyISOName;
	ELSE
		tClientOperationStatus := 'DECLINE'::ClientOperationStatuses;
	END IF;


	INSERT INTO ClientOperation 
	(	
		ClientOperationId, 
		ClientAccountId, 
		CurrencyExchangeCourseId,
		ClientOperationType,
		ClientOperationStatus,
		ClientAccountTotalAmountOld,
		ClientOperationValue,
		ClientAccountTotalAmountNew,
		RawData,
		CreationDate
	) VALUES(
		gen_random_uuid(), 
		tClientAccountData.ClientAccountId, 
		tCurrencyExchangeCourseData.CurrencyExchangeCourseId,
		tClientOperationType, 
		tClientOperationStatus,
		tClientAccountData.AccountTotalAmount,
		tClientAccountTotalAmountData.ClientOperationValue,
		tClientAccountTotalAmountData.AccountTotalAmountNew,
		msg,
		current_timestamp)
	RETURNING ClientOperationId INTO rClientOperationId;

	
END;
$$;

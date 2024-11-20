
DO
$$
	DECLARE

		tTimestamp Timestamp := current_timestamp;
		
		tCountryId Country.CountryId%TYPE;
		tCityId City.CityId%TYPE;
		tDepartmentAddressId DepartmentAddress.DepartmentAddressId%TYPE;
		tDepartmentId Department.DepartmentId%TYPE;
		
		tClientId Client.ClientId%TYPE;

		tCurrencyUAHId Currency.CurrencyId%TYPE;
		tCurrencyUSDId Currency.CurrencyId%TYPE;
		tCurrencyEURId Currency.CurrencyId%TYPE;
		
	BEGIN
		INSERT INTO Country (CountryId, CountryName) 
			VALUES (gen_random_uuid(), 'Украина') RETURNING CountryId INTO tCountryId;
		
		INSERT INTO City (CityId, CityName, CountryId)
			VALUES (gen_random_uuid(), 'Одесса', tCountryId) RETURNING CityId INTO tCityId;
		
		INSERT INTO Department (DepartmentId, DepartmentName)
			VALUES (gen_random_uuid(), 'Департамент человека парохода') RETURNING DepartmentId INTO tDepartmentId;

		INSERT INTO DepartmentAddress (DepartmentAddressId, CityId, DepartmentId, DepartmentAddressStreetName, DepartmentAddressBuildingNumber, CreationDate)
			VALUES (gen_random_uuid(), tCityId, tDepartmentId, 'Приморская', '6', tTimestamp) RETURNING DepartmentAddressId INTO tDepartmentAddressId;
	
		INSERT INTO Client (ClientId, ClientFirstName, CLientLastName, ClientThirdName, CreationDate)
			VALUES (gen_random_uuid(), 'Иван', 'Крузенштерн', 'Федорович', tTimestamp) RETURNING ClientId INTO tClientId;
		
		INSERT INTO ClientDepartment (ClientDepartmentId, ClientId, DepartmentId, CreationDate)
			VALUES (gen_random_uuid(), tClientId, tDepartmentId, tTimestamp);

		INSERT INTO Currency (CurrencyId, CurrencyName, CurrencyISOName)
			VALUES (gen_random_uuid(), 'Гривна', 'UAH') RETURNING CurrencyId INTO tCurrencyUAHId;
		
		INSERT INTO Currency (CurrencyId, CurrencyName, CurrencyISOName)
			VALUES (gen_random_uuid(), 'Доллар', 'USD') RETURNING CurrencyId INTO tCurrencyUSDId;
		
		INSERT INTO Currency (CurrencyId, CurrencyName, CurrencyISOName)
			VALUES (gen_random_uuid(), 'Евро', 'EUR') RETURNING CurrencyId INTO tCurrencyEURId;

		INSERT INTO ClientAccount (ClientAccountId, ClientId, CurrencyId, AccountTotalAmount)
			VALUES (gen_random_uuid(), tClientId, tCurrencyUAHId, 5000);

		INSERT INTO CurrencyExchangeCourse (CurrencyExchangeCourseId, CurrencyId, SaleValue, PurchaseValue, CreationDate)
			VALUES (gen_random_uuid(), tCurrencyUSDId, 41.699, 41.237, tTimestamp);

		INSERT INTO CurrencyExchangeCourse (CurrencyExchangeCourseId, CurrencyId, SaleValue, PurchaseValue, CreationDate)
			VALUES (gen_random_uuid(), tCurrencyEURId, 45.115, 44.379, tTimestamp);

		INSERT INTO CurrencyExchangeCourse (CurrencyExchangeCourseId, CurrencyId, SaleValue, PurchaseValue, CreationDate)
			VALUES (gen_random_uuid(), tCurrencyUSDId, 41.670, 41.238, tTimestamp + INTERVAL '00:01');

		INSERT INTO CurrencyExchangeCourse (CurrencyExchangeCourseId, CurrencyId, SaleValue, PurchaseValue, CreationDate)
			VALUES (gen_random_uuid(), tCurrencyEURId, 45.116, 44.380, tTimestamp + INTERVAL '00:01');
		
	END;
$$;




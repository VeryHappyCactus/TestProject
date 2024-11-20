--Если надо, то можно создавать индексы на внешнии ключи. Я делать не стал, тестовое приложение, лишняя работа и нагрузка на базу. я индексы вообще создавать не стал
--Так же я не стал заниматься переводами, делать какие-то таблицы с дескрипшенами названиями под различные языки.
--так же не стал добавлять поля с временем обновления (CreationDate (добавил)/UpdateDate)
--не использовал очередь сообщений для postgresql и не реализовываол свою
--Тестовое задание, можно писать и писать...

DROP TYPE IF EXISTS CurrencyISONames;
CREATE TYPE CurrencyISONames AS ENUM ('USD', 'EUR', 'UAH');

DROP TYPE IF EXISTS ClientOperationTypes;
CREATE TYPE ClientOperationTypes AS ENUM ('WITHDRAW');

DROP TYPE IF EXISTS ClientOperationStatuses;
CREATE TYPE ClientOperationStatuses AS ENUM ('SUCCESS', 'DECLINE');


DROP TABLE IF EXISTS Country;
CREATE TABLE Country
(
	CountryId UUID PRIMARY KEY,
	CountryName VARCHAR(256) NOT NULL
);

DROP TABLE IF EXISTS City;
CREATE TABLE City
(
	CityId UUID PRIMARY KEY,
	CountryId UUID NOT NULL,
	CityName VARCHAR(256) NOT NULL,
	
	FOREIGN KEY (CountryId) REFERENCES Country (CountryId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

DROP TABLE IF EXISTS Department;
CREATE TABLE Department
(
	DepartmentId UUID PRIMARY KEY,
	DepartmentName VARCHAR(512) NOT NULL
	--PhoneNumber ....
);

--с адресами не совсем понятно, есть ли у нас доступ к полной базе адресов, чтобы сделать таблицу улиц или нет
DROP TABLE IF EXISTS DepartmentAddress;
CREATE TABLE DepartmentAddress
(
	DepartmentAddressId	UUID PRIMARY KEY,
	CityId UUID NOT NULL,
	DepartmentId UUID NOT NULL,
	DepartmentAddressStreetName VARCHAR(256) NOT NULL,
	DepartmentAddressBuildingNumber VARCHAR(16) NOT NULL,
	CreationDate Timestamp NOT NULL,
	
	FOREIGN KEY (CityId) REFERENCES City (CityId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,

	FOREIGN KEY (DepartmentId) REFERENCES Department (DepartmentId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);


DROP TABLE IF EXISTS Client;
CREATE TABLE Client
(
	ClientId UUID PRIMARY KEY,
	ClientFirstName VARCHAR(128) NOT NULL,
	ClientLastName VARCHAR(128) NOT NULL,
	ClientThirdName VARCHAR(128) NULL, -- Matronymic, Patronymic
	CreationDate Timestamp NOT NULL
);

DROP TABLE IF EXISTS ClientDepartment; 
CREATE TABLE ClientDepartment
(
	ClientDepartmentId UUID PRIMARY KEY,
	ClientId UUID NOT NULL,
	DepartmentId UUID NOT NULL,
	CreationDate Timestamp NOT NULL,
	
	FOREIGN KEY (DepartmentId) REFERENCES Department (DepartmentId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,

	FOREIGN KEY (ClientId) REFERENCES Client (ClientId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

DROP TABLE IF EXISTS Currency;
CREATE TABLE Currency
(
	CurrencyId UUID PRIMARY KEY,
	CurrencyName VARCHAR(128) NOT NULL,
	CurrencyISOName CurrencyISONames NOT NULL
	
);

DROP TABLE IF EXISTS CurrencyExchangeCourse;
CREATE TABLE CurrencyExchangeCourse
(
	CurrencyExchangeCourseId UUID PRIMARY KEY,
	CurrencyId UUID NOT NULL,
	SaleValue NUMERIC NOT NULL,
	PurchaseValue NUMERIC NOT NULL,
	CreationDate Timestamp NOT NULL,

	FOREIGN KEY (CurrencyId) REFERENCES Currency (CurrencyId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);

DROP TABLE IF EXISTS ClientAccount; -- есть счет организации или депортамента, есть счета клиентов. оставлю только клиентов.
CREATE TABLE ClientAccount
(
	ClientAccountId UUID PRIMARY KEY,
	ClientId UUID NOT NULL,
	CurrencyId UUID NOT NULL,
	AccountTotalAmount NUMERIC CONSTRAINT cstr_Positive_TotalAmount CHECK (AccountTotalAmount >= 0), -- не понятно, можно ли уходить в минус. не понятно, что делать с кредитом. поставил проверку.

	FOREIGN KEY (ClientId) REFERENCES Client (ClientId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION,

	FOREIGN KEY (CurrencyId) REFERENCES Currency (CurrencyId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);


DROP TABLE IF EXISTS ClientOperation;
CREATE TABLE ClientOperation
(
	ClientOperationId UUID PRIMARY KEY,
	ClientAccountId UUID NOT NULL,
	CurrencyExchangeCourseId UUID NULL,
	ClientOperationType ClientOperationTypes NOT NULL,
	ClientOperationStatus ClientOperationStatuses NOT NULL,
	ClientAccountTotalAmountOld NUMERIC NOT NULL,
	ClientOperationValue NUMERIC NOT NULL,
	ClientAccountTotalAmountNew NUMERIC NOT NULL,
	RawData JSONB NOT NULL,
	CreationDate Timestamp NOT NULL,
	
	FOREIGN KEY (ClientAccountId) REFERENCES ClientAccount (ClientAccountId)
	ON DELETE NO ACTION
	ON UPDATE NO ACTION
);




/*
DROP PROCEDURE IF EXISTS sp_Withdraw;
DROP FUNCTION IF EXISTS fn_GetStatusInfo;
DROP FUNCTION IF EXISTS fn_GetCurrentCurrencyExchangeCourse;

DROP TABLE IF EXISTS ClientOperation;
DROP TABLE IF EXISTS ClientAccount; -- есть счет организации или депортамента, есть счета клиентов. оставлю только клиентов.
DROP TABLE IF EXISTS CurrencyExchangeCourse;
DROP TABLE IF EXISTS Currency;
DROP TABLE IF EXISTS ClientDepartment; 
DROP TABLE IF EXISTS Client;
DROP TABLE IF EXISTS DepartmentAddress;
DROP TABLE IF EXISTS Department;
DROP TABLE IF EXISTS City;
DROP TABLE IF EXISTS Country;

DROP TYPE IF EXISTS ClientOperationTypes;
DROP TYPE IF EXISTS ClientOperationStatuses;
DROP TYPE IF EXISTS CurrencyISONames;
*/




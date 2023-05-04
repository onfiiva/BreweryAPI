set ansi_nulls on
go
set quoted_identifier on
go
set ansi_padding on
go

create database Brewery
go
use Brewery
go

create table [Brewery]
(
	[ID_Brewery] [int] not null identity(1,1) primary key,
	[Name_Brewery] [varchar] (50) not null,
	[Address_Brewery] [varchar] (200) not null,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Brewery] ([Name_Brewery], [Address_Brewery]) values
('Пивоварня на Нахиме', 'г. Москва, Нахимовский проспект, 21')
go

select * from [Brewery]
go

create table [Admin]
(
	[ID_Admin] [int] not null identity(1,1) primary key,
	[Phone_Admin] [varchar] (17) not null unique check([Phone_Admin] like ('+7([0-9][0-9][0-9])[0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]')),
	[Login_Admin] [varchar] (50) not null unique,
	[Password_Admin] [varchar] (50) not null,
	[Brewery_ID] [int] not null references [Brewery] ([ID_Brewery]) on delete cascade,
	[Password_Salt] [varchar] (256) null,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Admin] ([Phone_Admin], [Login_Admin], [Password_Admin], [Brewery_ID], [Password_Salt]) values
('+7(901)365-03-72', 'admin', 'admin1234', 1, '')
go

select * from [Admin]
go

create table [Suppliers]
(
	[ID_Supplier] [int] not null identity(1,1) primary key,
	[Name_Supplier] [varchar] (200) not null,
	[Phone_Supplier] [varchar] (17) not null unique check([Phone_Supplier] like ('+7([0-9][0-9][0-9])[0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]')),
	[Address_Supplier] [varchar] (200) not null unique,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Suppliers] ([Name_Supplier], [Phone_Supplier], [Address_Supplier]) values
('Зерновая "Арех"', '+7(926)724-71-81', 'г. Москва, Мясницкая, 71')
go

select * from [Suppliers]
go

create table [Ingridients_Type]
(
	[ID_Ingridient_Type] [int] not null identity(1,1) primary key,
	[Name_Ingridient_Type] [varchar] (50) not null unique,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Ingridients_Type] ([Name_Ingridient_Type]) values
('Зерно')
go

select * from [Ingridients_Type]
go

create table [Ingridients]
(
	[ID_Ingridient] [int] not null identity(1,1) primary key,
	[Name_Ingridient] [varchar] (50) not null unique,
	[Ingridient_Type_ID] [int] not null references [Ingridients_Type] ([ID_Ingridient_Type]) on delete cascade,
	[Admin_ID] [int] not null references [Admin] ([ID_Admin]) on delete cascade,
	[Supplier_ID] [int] not null references [Suppliers] ([ID_Supplier]) on delete cascade,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Ingridients] ([Name_Ingridient], [Ingridient_Type_ID], [Admin_ID], [Supplier_ID]) values
('Пшеница', 1, 1, 1)
go

select * from [Ingridients]
go

create table [Beer_Type]
(
	[ID_Beer_Type] [int] not null identity(1,1) primary key,
	[Name_Beer_Type] [varchar] (50) not null unique,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Beer_Type] ([Name_Beer_Type]) values
('Пшеничное')
go

select * from [Beer_Type]
go

create table [Beer]
(
	[ID_Beer] [int] not null identity(1,1) primary key,
	[Name_Beer] [varchar] (50) not null,
	[Production_Time] [datetime] not null,
	[Term] [datetime] not null,
	[Beer_Type_ID] [int] not null references [Beer_Type] ([ID_Beer_Type]) on delete cascade,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Beer] ([Name_Beer], [Production_Time], [Term], [Beer_Type_ID]) values
('"Козел Пшеничный"', '20230218 10:34:09 AM', '20240218 10:34:09 AM', 1)
go

select * from [Beer]
go

create table [Subscription]
(
	[ID_Subscription] [int] not null identity(1,1) primary key,
	[Name_Subscription] [varchar] (50) not null unique,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Subscription] ([Name_Subscription]) values
('Стандарт')
go

select * from [Subscription]
go

create table [User]
(
	[ID_User] [int] not null identity(1,1) primary key,
	[User_Phone] [varchar] (17) not null unique check([User_Phone] like ('+7([0-9][0-9][0-9])[0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]')),
	[Login_User] [varchar] (50) not null unique,
	[Password_User] [varchar] (50) not null,
	[Subscription_ID] [int] not null references [Subscription] ([ID_Subscription]) on delete cascade,
	[Password_Salt] [varchar] (256) null,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [User] ([User_Phone], [Login_User], [Password_User], [Subscription_ID], [Password_Salt]) values
('+7(916)947-30-55', 'oleg', '1234', 1, '*&#^4ikj2f')
go

select * from [User]
go

create table [Ingridients_Beer]
(
	[ID_Users_Beer] [int] not null identity(1,1) primary key,
	[Ingridient_ID] [int] not null references [Ingridients] ([ID_Ingridient]) on delete cascade,
	[Beer_ID] [int] not null references [Beer] ([ID_Beer]) on delete cascade,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Ingridients_Beer] ([Ingridient_ID], [Beer_ID]) values
(1, 1)
go

select * from [Ingridients_Beer]
go

create table [Cheque]
(
	[ID_Cheque] [int] not null identity(1,1) primary key,
	[User_ID] [int] not null references [User] ([ID_User]) on delete cascade,
	[Sum] [int] not null,
	[Time_Order] [datetime] not null,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Cheque] ([User_ID], [Sum], [Time_Order]) values
(1, 500, '20230218 10:34:09 AM')
go

select * from [Cheque]
go

create table [Beer_Cheque]
(
	[ID_Beer_Cheque] [int] not null identity(1,1) primary key,
	[Cheque_ID] [int] not null references [Cheque] ([ID_Cheque]) on delete cascade,
	[Beer_ID] [int] not null references [Beer] ([ID_Beer]) on delete cascade,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Beer_Cheque] ([Cheque_ID], [Beer_ID]) values
(1, 1)
go

select * from [Beer_Cheque]
go

create table [Brewery_Ingridients]
(
	[ID_Brewery_Ingridients] [int] not null identity(1,1) primary key,
	[Brewery_ID] [int] not null references [Brewery] ([ID_Brewery]) on delete cascade,
	[Ingridient_ID] [int] not null references [Ingridients] ([ID_Ingridient]) on delete no action,
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

insert into [Brewery_Ingridients] ([Brewery_ID], [Ingridient_ID]) values
(1, 1)
go

select * from [Brewery_Ingridients]
go

create table [Brewery_Beer]
(
	[ID_Brewery_Beer] [int] not null identity(1,1) primary key,
	[Brewery_ID] [int] not null references [Brewery] ([ID_Brewery]) on delete cascade,
	[Beer_ID] [int] not null references [Beer] ([ID_Beer]) on delete cascade,
	[Is_Deleted] BIT NOT NULL DEFAULT 0

)
go

insert into [Brewery_Beer] ([Brewery_ID], [Beer_ID]) values
(1, 1)
go

select * from [Brewery_Beer]
go

create table User_History
(
	[ID_User_History] [int] not null primary key,
	[User_History] [varchar] (max) null,
	[User_ID] [int] not null references [User] ([ID_User]) on delete cascade,
	[Create_Record] [datetime] null default(getdate()),
	[Change_Record] [datetime] null default(getdate()),
	[Is_Deleted] BIT NOT NULL DEFAULT 0
)
go

create table Token
(
	[ID_Token] [int] not null identity(1,1) primary key,
	[Token_Value] [varchar] (200) not null,
	[Token_DateTime] [datetime2] not null default(getdate())
)
go

--Триггеры--

CREATE OR ALTER TRIGGER [dbo].[User_History_Insert]
ON [User] AFTER INSERT
AS
	BEGIN
		INSERT INTO User_History ([ID_User_History],[User_History])
		SELECT CAST(NEWID() AS VARCHAR(36)), CONCAT('Номер чека: ', [ID_Cheque], ', Логин пользователя: ' + [Login_User], ', Сумма чека: ' + CAST([Sum] AS NVARCHAR(50)))
		FROM [dbo].[Cheque]
		INNER JOIN [dbo].[User] ON [ID_User] = [dbo].[Cheque].[User_ID]
		WHERE [ID_User] IN (SELECT [ID_User] FROM [inserted])
		PRINT 'Insert Record Complete!'
	END
GO

create or alter trigger [dbo].[User_History_Update]
on [User] after update
as
		begin
			update [dbo].[User_History] set
			[User_History] = (select CONCAT(' Номер чека: ',[ID_Cheque], ', Логин пользователя: ' + [Login_User], ', Сумма чека: ' + [Sum]) from [dbo].[Cheque]
			
			inner join [dbo].[User] on [ID_User] = [dbo].[Cheque].[User_ID]
			where [ID_User] = (select [ID_User] from [inserted])),

				[Change_Record] = GETDATE()
			print ('Update Record Complete!')
		end
go

create or alter trigger [dbo].[User_History_Delete]
on [dbo].[User] after delete
as
	begin
		begin
		update [dbo].[User_History] set
			[User_History] = (select CONCAT(' Номер чека: ',[ID_Cheque], ', Логин пользователя: ' + [Login_User], ', Сумма чека: ' + [Sum]) from [dbo].[Cheque]
			
			inner join [dbo].[User] on [ID_User] = [dbo].[Cheque].[User_ID]
			where [ID_User] = (select [ID_User] from [deleted])),

				[Change_Record] = GETDATE()
					print ('Remove Update Record Complete!')
		end
		begin
		delete from [dbo].[User_History]
		print ('Delete Record Complete!')
		end
	end
go

--Представления--

create or alter view [dbo].[Brewery_List] ("Информация о пивоварне")
as
	select 'Название: ' + [Name_Brewery] + ', Адрес: ' + [Address_Brewery] from [dbo].[Brewery]
go

select * from [dbo].[Brewery_List]
go

create or alter view [dbo].[Admin_List] ("Информация об админе")
as
	select 'Телефон: ' + [Phone_Admin] + ', Логин: ' + [Login_Admin] + ', Пароль: ' + [Password_Admin] from [dbo].[Admin]
go

select * from [dbo].[Admin_List]
go

create or alter view [dbo].[Suppliers_List] ("Информация о поставщиках")
as
	select 'Название: ' + [Name_Supplier] + ', Телефон: ' + [Phone_Supplier] + ', Адрес: ' + [Address_Supplier] from [dbo].[Suppliers]
go

select * from [dbo].[Suppliers_List]
go

create or alter view [dbo].[Ingridients_List] ("Информация об ингридиентах")
as
	select 'Название: ' + [Name_Ingridient] + ', Тип: ' + [Name_Ingridient_Type] + ', Номер админа: ' + [Phone_Admin] + ', Название поставщика: ' + [Name_Supplier] from [dbo].[Ingridients]
	inner join [dbo].[Ingridients_Type] on [ID_Ingridient_Type] = [Ingridient_Type_ID]
	inner join [dbo].[Admin] on [ID_Admin] = [Admin_ID]
	inner join [dbo].[Suppliers] on [ID_Supplier] = [Supplier_ID]
go

select * from [dbo].[Ingridients_List]
go

create or alter view [dbo].[Beer_List] ("Информация о пиве")
as
	select 'Название: ' + [Name_Beer] + ', Тип: ' + [Name_Beer_Type] + ', Время производства: ' + convert(varchar(50), [Production_Time], 120)  + ', Срок годности: ' + convert(varchar(50), [Term], 120) from [dbo].[Beer]
	inner join [dbo].[Beer_Type] on [ID_Beer_Type] = [Beer_Type_ID]
go

select * from [dbo].[Beer_List]
go

create or alter view [dbo].[Cheque_List] ("Информация о чеках")
as
	select 'Номер чека: ' + cast([ID_Cheque] as varchar(50)) + ', Клиент: ' + [User_Phone] + ', Сумма: ' + cast([Sum] as varchar(50))  + ', Время заказа: ' + convert(varchar(50), [Time_Order], 120) from [dbo].[Cheque]
	inner join [dbo].[User] on [ID_User] = [User_ID]
go

select * from [dbo].[Cheque_List]
go

create or alter view [dbo].[User_List] ("Информация о пользователе")
as
	select 'Телефон: ' + [User_Phone] + ', Логин: ' + [Login_User] + ', Пароль: ' + [Password_User] + ', Подписка: ' + [Name_Subscription] from [dbo].[User]
	inner join [dbo].[Subscription] on [ID_Subscription] = [Subscription_ID]
go

select * from [dbo].[User_List]
go

Create Procedure ValidateUser
    @Userlogin nvarchar(50)
    , @Password nvarchar(50)
As
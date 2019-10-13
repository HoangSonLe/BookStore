USE master
GO
CREATE DATABASE OrderFood
GO
USE OrderFood
GO
CREATE TABLE Menu (
	MenuID INT IDENTITY NOT NULL PRIMARY KEY,
	MenuName nvarchar(250),
	MenuContent nvarchar(250),
	--Link nvarchar(250),
	ParentID int FOREIGN KEY REFERENCES Menu(MenuID),
	IsActive int
)
GO
CREATE TABLE Slider (
	SlideID	INT IDENTITY NOT NULL PRIMARY KEY,
	SliderImage	nvarchar(250),
	Display	int	,--Status?
	--Link	nvarchar(250),
	Description	nvarchar(250),
	CreatedDate datetime,
	CreatedBy int,
	ModifiedDate datetime,
	ModifiedBy int ,
)
GO
CREATE TABLE About (
	AboutID INT IDENTITY NOT NULL PRIMARY KEY,
	Title nvarchar(250),
	Description	nvarchar(250),
	AboutImage	nvarchar(250),
	CreatedDate datetime,
	CreatedBy int,
	ModifiedDate datetime,
	ModifiedBy int ,
	Status int,
)
GO
CREATE TABLE Contact (
	ContactID INT IDENTITY NOT NULL PRIMARY KEY,
	Content	nvarchar(250),
	Status int,
)
GO
CREATE TABLE ProductCategory(
	CategoryID INT IDENTITY NOT NULL PRIMARY KEY,
	Name nvarchar(250),
	MetaTitle nvarchar(250),
	ParentID int,
	CreatedDate datetime,
	CreatedBy int,
	ModifiedDate datetime,
	ModifiedBy int ,
	Status int,
	FOREIGN KEY (ParentID) REFERENCES ProductCategory(CategoryID)
)
GO
CREATE TABLE Product (
	ProductID int IDENTITY NOT NULL PRIMARY KEY,
	ProductName nvarchar(50),
	MetaTitle varchar(250),
	Description nvarchar(500),
	ProductImage varchar(250),
	Price int,
	PromotionPrice int,
	IncludeVAT bit,
	Quantity int,
	CategoryID int,
	CreatedDate datetime,
	CreatedBy int,
	ModifiedDate datetime,
	ModifiedBy int ,
	Status int,
	ViewCounts int
	FOREIGN KEY (CategoryID) REFERENCES ProductCategory(CategoryID)
)
GO
CREATE TABLE ProductImages(
	ProductID int,
	ProductImage varchar(250),
	FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
)
	
GO
CREATE TABLE Roles(
	RoleID INT NOT NULL PRIMARY KEY,
	RoleName nvarchar(250)
)
GO
CREATE TABLE Customer (
	--BirthDate if people is employee
	--ManagerID if people is employee
	-- => Should has 2 tables ?
	CustomerID INT IDENTITY NOT NULL PRIMARY KEY,
	UserName varchar(250),
	Password varchar(250),
	FirstName nvarchar(50),
	LastName nvarchar(50),
	Address nvarchar(250),
	Email nvarchar(250),
	Phone nvarchar(250),
	CreatedDate datetime,
	CreatedBy int,
	ModifiedDate datetime,
	ModifiedBy int ,
	Status int
)
GO
CREATE TABLE Employee (
	EmployeeID INT IDENTITY NOT NULL PRIMARY KEY,
	UserName varchar(250),
	Password varchar(250),
	FirstName nvarchar(50),
	LastName nvarchar(50),
	Address nvarchar(250),
	Email nvarchar(250),
	Phone nvarchar(250),
	BirthDate datetime,
	Role int FOREIGN KEY REFERENCES Roles(RoleID),
	ManagerID int FOREIGN KEY REFERENCES Employee(EmployeeID),
	CreatedDate datetime,
	CreatedBy int,
	ModifiedDate datetime,
	ModifiedBy int ,
	Status int
)
GO

GO
CREATE TABLE Orders (
	OrderID INT IDENTITY NOT NULL PRIMARY KEY,
	CustomerID int FOREIGN KEY REFERENCES Customer(CustomerID),
	EmployeeID int FOREIGN KEY REFERENCES Employee(EmployeeID),
	Comment nvarchar(250),
	OrderStatus int, -- Order status: 1 = Pending; 2 = Processing; 3 = Rejected; 4 = Completed ???? Should we create a  Status table ?
	CreatedDate datetime,
	CreatedBy int,
	ModifiedDate datetime,
	ModifiedBy int ,
	ShipDate datetime
)
GO
CREATE TABLE OrderDetail (
	OrderID int FOREIGN KEY REFERENCES Orders(OrderID),
	ProductID int FOREIGN KEY REFERENCES Product(ProductID),
	Quantity int,
)
GO
--Comment in Product
CREATE TABLE Comment (
	CommentID int IDENTITY NOT NULL PRIMARY KEY,
	ProductID int FOREIGN KEY REFERENCES Product(ProductID),
	CustomerID int FOREIGN KEY REFERENCES Customer(CustomerID),
	EmployeeID int FOREIGN KEY REFERENCES Employee(EmployeeID),
	Context nvarchar(500),
	CreatedDate datetime,
	ModifiedDate datetime,
	Status int,
)
GO

--@(#) script.ddl

CREATE TABLE Address
(
	City varchar (255) NOT NULL,
	Street varchar (255) NOT NULL,
	ZipCode varchar (255) NOT NULL,
	HouseNumber varchar (255) NOT NULL,
	id_Address integer NOT NULL,
	PRIMARY KEY(id_Address)
);

CREATE TABLE EndUser
(
	PhoneNumber varchar (255) NOT NULL,
	id_EndUser integer NOT NULL,
	fk_Addressid_Address integer NOT NULL,
	PRIMARY KEY(id_EndUser),
	CONSTRAINT determinesThePlace FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE Terminal
(
	PhoneNumber varchar (255) NOT NULL,
	id_Terminal integer NOT NULL,
	fk_Addressid_Address integer NOT NULL,
	PRIMARY KEY(id_Terminal),
	UNIQUE(fk_Addressid_Address),
	CONSTRAINT determinesLocation FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE LoggedInUser
(
	Email varchar (255) NOT NULL,
	Password varchar (255) NOT NULL,
	Role char (9) NOT NULL,
	id_EndUser integer NOT NULL,
	CHECK(Role in ('Admin', 'Worker', 'Courier', 'Sender', 'Recipient', 'User')),
	PRIMARY KEY(id_EndUser),
	FOREIGN KEY(id_EndUser) REFERENCES EndUser (id_EndUser)
);

CREATE TABLE Package
(
	PutInTime date NULL,
	CollectionTime date NULL,
	Size char (1) NULL,
	PackageState char (15) NOT NULL,
	id_Package integer NOT NULL,
	fk_LoggedInUserid_EndUser integer NULL,
	fk_Terminalid_Terminal integer NULL,
	fk_EndUserid_EndUser integer NOT NULL,
	CHECK(Size in ('S', 'M', 'L')),
	CHECK(PackageState in ('Created', 'Activated', 'WaitsForCourier', 'InTerminal', 'WaitsForPickup', 'Delivered', 'EnRoute')),
	PRIMARY KEY(id_Package),
	CONSTRAINT sends FOREIGN KEY(fk_LoggedInUserid_EndUser) REFERENCES LoggedInUser (id_EndUser),
	CONSTRAINT has FOREIGN KEY(fk_Terminalid_Terminal) REFERENCES Terminal (id_Terminal),
	CONSTRAINT receives FOREIGN KEY(fk_EndUserid_EndUser) REFERENCES EndUser (id_EndUser)
);

CREATE TABLE PostMachine
(
	TurnedOn BIT NOT NULL,
	PostMachineState char (19) NOT NULL,
	id_PostMachine integer NOT NULL,
	fk_LoggedInUserid_EndUser integer NOT NULL,
	fk_LoggedInUserid_EndUser1 integer NOT NULL,
	fk_Addressid_Address integer NOT NULL,
	CHECK(PostMachineState in ('Operating', 'WaitsForMaintenance')),
	PRIMARY KEY(id_PostMachine),
	CONSTRAINT overseesPackages FOREIGN KEY(fk_LoggedInUserid_EndUser) REFERENCES LoggedInUser (id_EndUser),
	CONSTRAINT oversees FOREIGN KEY(fk_LoggedInUserid_EndUser1) REFERENCES LoggedInUser (id_EndUser),
	CONSTRAINT determinesPlace FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE Message
(
	Text varchar (255) NOT NULL,
	Sent BIT NOT NULL,
	id_Message integer NOT NULL,
	fk_Packageid_Package integer NOT NULL,
	fk_LoggedInUserid_EndUser integer NOT NULL,
	PRIMARY KEY(id_Message),
	CONSTRAINT notifies FOREIGN KEY(fk_Packageid_Package) REFERENCES Package (id_Package),
	CONSTRAINT gets FOREIGN KEY(fk_LoggedInUserid_EndUser) REFERENCES LoggedInUser (id_EndUser)
);

CREATE TABLE PostMachineBox
(
	IsOpen BIT DEFAULT 0 NOT NULL,
	Pin varchar (255) NULL,
	Size char (1) NOT NULL,
	id_PostMachineBox integer NOT NULL,
	fk_PostMachineid_PostMachine integer NOT NULL,
	fk_Packageid_Package integer NULL,
	CHECK(Size in ('S', 'M', 'L')),
	PRIMARY KEY(id_PostMachineBox),
	UNIQUE(fk_Packageid_Package),
	CONSTRAINT belongs FOREIGN KEY(fk_PostMachineid_PostMachine) REFERENCES PostMachine (id_PostMachine),
	CONSTRAINT holds FOREIGN KEY(fk_Packageid_Package) REFERENCES Package (id_Package)
);

--@(#) script.ddl

CREATE TABLE Address
(
	City varchar (255) NULL,
	Street varchar (255) NULL,
	ZipCode varchar (255) NULL,
	HouseNumber varchar (255) NULL,
	id_Address integer NULL,
	PRIMARY KEY(id_Address)
);

CREATE TABLE Terminal
(
	PhoneNumber varchar (255) NULL,
	id_Terminal integer NULL,
	fk_Addressid_Address integer NULL,
	PRIMARY KEY(id_Terminal),
	UNIQUE(fk_Addressid_Address),
	CONSTRAINT determines-place FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE User
(
	PhoneNumber varchar (255) NULL,
	id_User integer NULL,
	fk_Addressid_Address integer NULL,
	PRIMARY KEY(id_User),
	CONSTRAINT determines-place FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE LoggedInUser
(
	Email varchar (255) NULL,
	Password varchar (255) NULL,
	Role char (9) NULL,
	id_User integer NULL,
	CHECK(Role in ('Admin', 'Worker', 'Courier', 'Sender', 'Recipient', 'User')),
	PRIMARY KEY(id_User),
	FOREIGN KEY(id_User) REFERENCES User (id_User)
);

CREATE TABLE Package
(
	PutInTime date NULL,
	CollectionTime date NULL,
	Size char (1) NULL,
	PackageState char (15) NULL,
	id_Package integer NULL,
	fk_Terminalid_Terminal integer NULL,
	fk_Userid_User integer NULL,
	fk_LoggedInUserid_User integer NULL,
	CHECK(Size in ('S', 'M', 'L')),
	CHECK(PackageState in ('Created', 'Activated', 'WaitsForCourier', 'InTerminal', 'WaitsForPickup', 'Delivered', 'EnRoute')),
	PRIMARY KEY(id_Package),
	CONSTRAINT has FOREIGN KEY(fk_Terminalid_Terminal) REFERENCES Terminal (id_Terminal),
	CONSTRAINT gets FOREIGN KEY(fk_Userid_User) REFERENCES User (id_User),
	CONSTRAINT sends FOREIGN KEY(fk_LoggedInUserid_User) REFERENCES LoggedInUser (id_User)
);

CREATE TABLE PostMachine
(
	TurnedOn boolean NULL,
	PostMachineState char (19) NULL,
	id_PostMachine integer NULL,
	fk_LoggedInUserid_User integer NULL,
	fk_LoggedInUserid_User1 integer NULL,
	fk_Addressid_Address integer NULL,
	CHECK(PostMachineState in ('Operating', 'WaitsForMaintenance')),
	PRIMARY KEY(id_PostMachine),
	CONSTRAINT oversees FOREIGN KEY(fk_LoggedInUserid_User) REFERENCES LoggedInUser (id_User),
	CONSTRAINT oversees-packages FOREIGN KEY(fk_LoggedInUserid_User1) REFERENCES LoggedInUser (id_User),
	CONSTRAINT determines-place FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE Message
(
	Text varchar (255) NULL,
	Sent boolean NULL,
	id_Message integer NULL,
	fk_LoggedInUserid_User integer NULL,
	fk_Packageid_Package integer NULL,
	PRIMARY KEY(id_Message),
	CONSTRAINT gets FOREIGN KEY(fk_LoggedInUserid_User) REFERENCES LoggedInUser (id_User),
	CONSTRAINT notifies-about FOREIGN KEY(fk_Packageid_Package) REFERENCES Package (id_Package)
);

CREATE TABLE PostMachineBox
(
	Open boolean DEFAULT false,
	Pin varchar (255) NULL,
	Size char (1) NULL,
	id_PostMachineBox integer NULL,
	fk_Packageid_Package integer NULL,
	fk_PostMachineid_PostMachine integer NULL,
	CHECK(Size in ('S', 'M', 'L')),
	PRIMARY KEY(id_PostMachineBox),
	UNIQUE(fk_Packageid_Package),
	CONSTRAINT has FOREIGN KEY(fk_Packageid_Package) REFERENCES Package (id_Package),
	CONSTRAINT belongs FOREIGN KEY(fk_PostMachineid_PostMachine) REFERENCES PostMachine (id_PostMachine)
);

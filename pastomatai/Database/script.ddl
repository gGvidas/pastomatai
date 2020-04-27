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

CREATE TABLE Terminal
(
	PhoneNumber varchar (255) NOT NULL,
	id_Terminal integer NOT NULL,
	fk_Addressid_Address integer NOT NULL,
	PRIMARY KEY(id_Terminal),
	UNIQUE(fk_Addressid_Address),
	CONSTRAINT determines-location FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE User
(
	PhoneNumber varchar (255) NOT NULL,
	id_User integer NOT NULL,
	fk_Addressid_Address integer NOT NULL,
	PRIMARY KEY(id_User),
	CONSTRAINT determines-the-place FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE LoggedInUser
(
	Email varchar (255) NOT NULL,
	Password varchar (255) NOT NULL,
	Role char (9) NOT NULL,
	id_User integer NOT NULL,
	CHECK(Role in ('Admin', 'Worker', 'Courier', 'Sender', 'Recipient', 'User')),
	PRIMARY KEY(id_User),
	FOREIGN KEY(id_User) REFERENCES User (id_User)
);

CREATE TABLE Package
(
	PutInTime date NULL,
	CollectionTime date NULL,
	Size char (1) NULL,
	PackageState char (15) NOT NULL,
	id_Package integer NOT NULL,
	fk_LoggedInUserid_User integer NOT NULL,
	fk_Terminalid_Terminal integer NULL,
	fk_Userid_User integer NOT NULL,
	CHECK(Size in ('S', 'M', 'L')),
	CHECK(PackageState in ('Created', 'Activated', 'WaitsForCourier', 'InTerminal', 'WaitsForPickup', 'Delivered', 'EnRoute')),
	PRIMARY KEY(id_Package),
	CONSTRAINT sends FOREIGN KEY(fk_LoggedInUserid_User) REFERENCES LoggedInUser (id_User),
	CONSTRAINT has FOREIGN KEY(fk_Terminalid_Terminal) REFERENCES Terminal (id_Terminal),
	CONSTRAINT receives FOREIGN KEY(fk_Userid_User) REFERENCES User (id_User)
);

CREATE TABLE PostMachine
(
	TurnedOn boolean NOT NULL,
	PostMachineState char (19) NOT NULL,
	id_PostMachine integer NOT NULL,
	fk_LoggedInUserid_User integer NOT NULL,
	fk_LoggedInUserid_User1 integer NOT NULL,
	fk_Addressid_Address integer NOT NULL,
	CHECK(PostMachineState in ('Operating', 'WaitsForMaintenance')),
	PRIMARY KEY(id_PostMachine),
	CONSTRAINT oversees-packages FOREIGN KEY(fk_LoggedInUserid_User) REFERENCES LoggedInUser (id_User),
	CONSTRAINT oversees FOREIGN KEY(fk_LoggedInUserid_User1) REFERENCES LoggedInUser (id_User),
	CONSTRAINT determines-place FOREIGN KEY(fk_Addressid_Address) REFERENCES Address (id_Address)
);

CREATE TABLE Message
(
	Text varchar (255) NOT NULL,
	Sent boolean NOT NULL,
	id_Message integer NOT NULL,
	fk_Packageid_Package integer NOT NULL,
	fk_LoggedInUserid_User integer NOT NULL,
	PRIMARY KEY(id_Message),
	CONSTRAINT notifies-about FOREIGN KEY(fk_Packageid_Package) REFERENCES Package (id_Package),
	CONSTRAINT gets FOREIGN KEY(fk_LoggedInUserid_User) REFERENCES LoggedInUser (id_User)
);

CREATE TABLE PostMachineBox
(
	Open boolean DEFAULT false NOT NULL,
	Pin varchar (255) NULL,
	Size char (1) NOT NULL,
	id_PostMachineBox integer NOT NULL,
	fk_PostMachineid_PostMachine integer NOT NULL,
	fk_Packageid_Package integer NOT NULL,
	CHECK(Size in ('S', 'M', 'L')),
	PRIMARY KEY(id_PostMachineBox),
	UNIQUE(fk_Packageid_Package),
	CONSTRAINT belongs FOREIGN KEY(fk_PostMachineid_PostMachine) REFERENCES PostMachine (id_PostMachine),
	CONSTRAINT holds FOREIGN KEY(fk_Packageid_Package) REFERENCES Package (id_Package)
);

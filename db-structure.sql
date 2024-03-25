-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Dec 06, 2021 at 12:39 PM
-- Server version: 10.1.48-MariaDB-0+deb9u2
-- PHP Version: 7.4.24

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `SO-PM`
--

-- --------------------------------------------------------

--
-- Table structure for table `City`
--

CREATE TABLE `City` (
  `City_ID` int(11) NOT NULL,
  `City_Name` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Country_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `ClientContacts`
--

CREATE TABLE `ClientContacts` (
  `ClientContactID` int(11) NOT NULL,
  `ClientID` int(11) DEFAULT NULL,
  `ContactID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `Clients`
--

CREATE TABLE `Clients` (
  `Client_ID` int(11) NOT NULL,
  `ClientName` varchar(250) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Country_ID` int(11) DEFAULT NULL,
  `City_ID` int(11) DEFAULT NULL,
  `Date_Created` datetime DEFAULT NULL,
  `Created_By` int(11) DEFAULT NULL,
  `Date_Updated` datetime DEFAULT NULL,
  `Updated_By` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `ClientUser`
--

CREATE TABLE `ClientUser` (
  `ClientUser_ID` int(11) NOT NULL,
  `FirstName` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `LastName` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email` varchar(320) CHARACTER SET utf8mb4 DEFAULT NULL,
  `PhoneNumber` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Client_ID` int(11) DEFAULT NULL,
  `Role_ID` int(11) DEFAULT NULL,
  `Date_Created` datetime DEFAULT NULL,
  `Created_By` int(11) DEFAULT NULL,
  `Date_Updated` datetime DEFAULT NULL,
  `Updated_By` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `Contacts`
--

CREATE TABLE `Contacts` (
  `Contact_ID` int(11) NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Surname` varchar(100) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email1` varchar(320) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email2` varchar(320) CHARACTER SET utf8mb4 DEFAULT NULL,
  `PhoneNumber1` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `PhoneNumber2` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Role_ID` int(11) DEFAULT NULL,
  `Notes` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `County`
--

CREATE TABLE `County` (
  `Country_ID` int(11) NOT NULL,
  `County_Name` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `ProjectData`
--

CREATE TABLE `ProjectData` (
  `ProjectUser_ID` int(11) NOT NULL,
  `Project_ID` int(11) DEFAULT NULL,
  `Name` varchar(100) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email` varchar(320) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4,
  `URL` longtext CHARACTER SET utf8mb4,
  `User_Name` longtext CHARACTER SET utf8mb4,
  `Password` longtext CHARACTER SET utf8mb4,
  `Date_Created` datetime DEFAULT NULL,
  `CreatedBy` int(11) DEFAULT NULL,
  `Date_Updated` datetime DEFAULT NULL,
  `UpdatedBy` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `Projects`
--

CREATE TABLE `Projects` (
  `Project_ID` int(11) NOT NULL,
  `Project_Name` longtext CHARACTER SET utf8mb4,
  `Description` longtext CHARACTER SET utf8mb4,
  `Client_ID` int(11) DEFAULT NULL,
  `Date_Created` datetime DEFAULT NULL,
  `Created_By` int(11) DEFAULT NULL,
  `Date_Updated` datetime DEFAULT NULL,
  `Updated_By` int(11) DEFAULT NULL,
  `Provider_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `ProjectUser`
--

CREATE TABLE `ProjectUser` (
  `ProjectUser_ID` int(11) NOT NULL,
  `Project_ID` int(11) DEFAULT NULL,
  `SytemUser_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `ProviderContact`
--

CREATE TABLE `ProviderContact` (
  `ProviderContact_ID` int(11) NOT NULL,
  `Provider_ID` int(11) DEFAULT NULL,
  `ProviderContactDetail_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `ProviderContactDetail`
--

CREATE TABLE `ProviderContactDetail` (
  `ProviderContactDetail_ID` int(11) NOT NULL,
  `Name` char(10) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Surname` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email1` varchar(320) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email2` varchar(320) CHARACTER SET utf8mb4 DEFAULT NULL,
  `PhoneNumber1` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `PhoneNumber2` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Role_ID` int(11) DEFAULT NULL,
  `Notes` longtext CHARACTER SET utf8mb4
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `Providers`
--

CREATE TABLE `Providers` (
  `Provider_ID` int(11) NOT NULL,
  `ProviderName` varchar(250) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Country_ID` int(11) DEFAULT NULL,
  `City_ID` int(11) DEFAULT NULL,
  `Date_Created` datetime DEFAULT NULL,
  `Created_By` int(11) DEFAULT NULL,
  `Date_Updated` datetime DEFAULT NULL,
  `Updated_By` int(11) DEFAULT NULL,
  `BillingFullName` varchar(250) CHARACTER SET utf8mb4 DEFAULT NULL,
  `IdCard` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `FullAddress` longtext CHARACTER SET utf8mb4,
  `PostalCode` varchar(20) CHARACTER SET utf8mb4 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `SystemUser`
--

CREATE TABLE `SystemUser` (
  `SytemUser_ID` int(11) NOT NULL,
  `FirstName` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `LastName` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Email` varchar(320) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Password` longtext CHARACTER SET utf8mb4,
  `PhoneNumber` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL,
  `Role_ID` int(11) DEFAULT NULL,
  `IsFirstLogin` tinyint(1) DEFAULT NULL,
  `Status` tinyint(1) NOT NULL DEFAULT '0',
  `TwoStepVerification` tinyint(1) DEFAULT NULL,
  `VerificationCode` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `UserRole`
--

CREATE TABLE `UserRole` (
  `Role_ID` int(11) NOT NULL,
  `Role_Name` varchar(50) CHARACTER SET utf8mb4 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `City`
--
ALTER TABLE `City`
  CHANGE COLUMN `City_ID` `City_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`City_ID`),
  ADD KEY `FK_City_Country` (`Country_ID`);

--
-- Indexes for table `ClientContacts`
--
ALTER TABLE `ClientContacts`
  CHANGE COLUMN `ClientContactID` `ClientContactID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`ClientContactID`),
  ADD KEY `FK_ClientContacts_Clients` (`ClientID`),
  ADD KEY `FK_ClientContacts_Contacts` (`ContactID`);

--
-- Indexes for table `Clients`
--
ALTER TABLE `Clients`
  CHANGE COLUMN `Client_ID` `Client_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`Client_ID`),
  ADD KEY `FK_Clients_City` (`City_ID`),
  ADD KEY `FK_Clients_County` (`Country_ID`);

--
-- Indexes for table `ClientUser`
--
ALTER TABLE `ClientUser`
  CHANGE COLUMN `ClientUser_ID` `ClientUser_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`ClientUser_ID`),
  ADD KEY `FK_ClientUser_Clients` (`Client_ID`),
  ADD KEY `FK_ClientUser_UserRole` (`Role_ID`);

--
-- Indexes for table `Contacts`
--
ALTER TABLE `Contacts`
  CHANGE COLUMN `Contact_ID` `Contact_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`Contact_ID`);

--
-- Indexes for table `County`
--
ALTER TABLE `County`
  CHANGE COLUMN `Country_ID` `Country_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`Country_ID`);

--
-- Indexes for table `ProjectData`
--
ALTER TABLE `ProjectData`
  CHANGE COLUMN `ProjectUser_ID` `ProjectUser_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`ProjectUser_ID`),
  ADD KEY `FK_ProjectUser_Projects` (`Project_ID`);

--
-- Indexes for table `Projects`
--
ALTER TABLE `Projects`
  CHANGE COLUMN `Project_ID` `Project_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`Project_ID`);

--
-- Indexes for table `ProjectUser`
--
ALTER TABLE `ProjectUser`
  CHANGE COLUMN `ProjectUser_ID` `ProjectUser_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`ProjectUser_ID`),
  ADD KEY `FK_ProjectUser_Projects1` (`Project_ID`),
  ADD KEY `FK_ProjectUser_SystemUser` (`SytemUser_ID`);

--
-- Indexes for table `ProviderContact`
--
ALTER TABLE `ProviderContact`
  CHANGE COLUMN `ProviderContact_ID` `ProviderContact_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`ProviderContact_ID`),
  ADD KEY `FK_ProviderContact_ProviderContactDetail` (`ProviderContactDetail_ID`),
  ADD KEY `FK_ProviderContact_Providers` (`Provider_ID`);

--
-- Indexes for table `ProviderContactDetail`
--
ALTER TABLE `ProviderContactDetail`
  CHANGE COLUMN `ProviderContactDetail_ID` `ProviderContactDetail_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`ProviderContactDetail_ID`);

--
-- Indexes for table `Providers`
--
ALTER TABLE `Providers`
  CHANGE COLUMN `Provider_ID` `Provider_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`Provider_ID`);

--
-- Indexes for table `SystemUser`
--
ALTER TABLE `SystemUser`
  CHANGE COLUMN `SytemUser_ID` `SytemUser_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`SytemUser_ID`),
  ADD KEY `FK_SystemUser_UserRole` (`Role_ID`);

--
-- Indexes for table `UserRole`
--
ALTER TABLE `UserRole`
  CHANGE COLUMN `Role_ID` `Role_ID` INT NOT NULL AUTO_INCREMENT ,
  ADD PRIMARY KEY (`Role_ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `ProjectData`
--
ALTER TABLE `ProjectData`
  MODIFY `ProjectUser_ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `City`
--
ALTER TABLE `City`
  ADD CONSTRAINT `FK_City_Country` FOREIGN KEY (`Country_ID`) REFERENCES `County` (`Country_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `ClientContacts`
--
ALTER TABLE `ClientContacts`
  ADD CONSTRAINT `FK_ClientContacts_Clients` FOREIGN KEY (`ClientID`) REFERENCES `Clients` (`Client_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_ClientContacts_Contacts` FOREIGN KEY (`ContactID`) REFERENCES `Contacts` (`Contact_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `Clients`
--
ALTER TABLE `Clients`
  ADD CONSTRAINT `FK_Clients_City` FOREIGN KEY (`City_ID`) REFERENCES `City` (`City_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_Clients_County` FOREIGN KEY (`Country_ID`) REFERENCES `County` (`Country_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `ClientUser`
--
ALTER TABLE `ClientUser`
  ADD CONSTRAINT `FK_ClientUser_Clients` FOREIGN KEY (`Client_ID`) REFERENCES `Clients` (`Client_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_ClientUser_UserRole` FOREIGN KEY (`Role_ID`) REFERENCES `UserRole` (`Role_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `ProjectData`
--
ALTER TABLE `ProjectData`
  ADD CONSTRAINT `FK_ProjectUser_Projects` FOREIGN KEY (`Project_ID`) REFERENCES `Projects` (`Project_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `ProjectUser`
--
ALTER TABLE `ProjectUser`
  ADD CONSTRAINT `FK_ProjectUser_Projects1` FOREIGN KEY (`Project_ID`) REFERENCES `Projects` (`Project_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_ProjectUser_SystemUser` FOREIGN KEY (`SytemUser_ID`) REFERENCES `SystemUser` (`SytemUser_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `ProviderContact`
--
ALTER TABLE `ProviderContact`
  ADD CONSTRAINT `FK_ProviderContact_ProviderContactDetail` FOREIGN KEY (`ProviderContactDetail_ID`) REFERENCES `ProviderContactDetail` (`ProviderContactDetail_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_ProviderContact_Providers` FOREIGN KEY (`Provider_ID`) REFERENCES `Providers` (`Provider_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `SystemUser`
--
ALTER TABLE `SystemUser`
  ADD CONSTRAINT `FK_SystemUser_UserRole` FOREIGN KEY (`Role_ID`) REFERENCES `UserRole` (`Role_ID`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

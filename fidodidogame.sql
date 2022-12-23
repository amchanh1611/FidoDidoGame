-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 23, 2022 at 07:57 AM
-- Server version: 10.4.25-MariaDB
-- PHP Version: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `fidodidogame`
--

-- --------------------------------------------------------

--
-- Table structure for table `dido`
--

CREATE TABLE `dido` (
  `Id` int(11) NOT NULL,
  `Name` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `dido`
--

INSERT INTO `dido` (`Id`, `Name`) VALUES
(1, 'Fido'),
(2, '7Up'),
(3, 'Lemon'),
(4, 'Shades'),
(5, 'Lens'),
(6, 'Skis'),
(7, 'Hammock'),
(8, 'RGB');

-- --------------------------------------------------------

--
-- Table structure for table `fido`
--

CREATE TABLE `fido` (
  `Id` int(11) NOT NULL,
  `Name` text NOT NULL,
  `Percent` int(11) NOT NULL,
  `PercentRand` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `fido`
--

INSERT INTO `fido` (`Id`, `Name`, `Percent`, `PercentRand`) VALUES
(1, 'BigFido', 20, 20),
(2, 'MediumFido', 30, 50),
(3, 'SmallFido', 50, 100);

-- --------------------------------------------------------

--
-- Table structure for table `fido_dido`
--

CREATE TABLE `fido_dido` (
  `FidoId` int(11) NOT NULL,
  `DidoId` int(11) NOT NULL,
  `Percent` int(11) NOT NULL,
  `Point` char(9) NOT NULL,
  `PercentRand` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `fido_dido`
--

INSERT INTO `fido_dido` (`FidoId`, `DidoId`, `Percent`, `Point`, `PercentRand`) VALUES
(1, 1, 20, '777', 20),
(1, 2, 25, '390', 45),
(1, 3, 10, '200', 75),
(1, 4, 5, 'x2', 80),
(1, 5, 5, 'auto', 85),
(1, 6, 5, 'heal', 90),
(1, 7, 10, 'ban', 100),
(2, 2, 35, '390', 35),
(2, 3, 45, '200', 80),
(2, 4, 5, 'x2', 85),
(2, 5, 5, 'auto', 90),
(2, 7, 10, 'ban', 100),
(3, 3, 65, '200', 65),
(3, 4, 10, 'x2', 75),
(3, 8, 25, '390', 100);

-- --------------------------------------------------------

--
-- Table structure for table `point_detail`
--

CREATE TABLE `point_detail` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Point` int(11) NOT NULL,
  `Date` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `point_of_day`
--

CREATE TABLE `point_of_day` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Point` int(11) NOT NULL,
  `Date` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `status`
--

CREATE TABLE `status` (
  `StatusCode` char(9) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `status`
--

INSERT INTO `status` (`StatusCode`) VALUES
('auto'),
('ban'),
('heal'),
('normal'),
('x2');

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `Id` int(11) NOT NULL,
  `Name` text NOT NULL,
  `NickName` text NOT NULL,
  `Phone` char(12) DEFAULT NULL,
  `Address` text DEFAULT NULL,
  `Male` tinyint(4) DEFAULT NULL,
  `Avatar` text DEFAULT NULL,
  `FidoId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`Id`, `Name`, `NickName`, `Phone`, `Address`, `Male`, `Avatar`, `FidoId`) VALUES
(1, 'MinhChanh', 'Chanh', NULL, NULL, 0, NULL, NULL),
(2, 'Chanh', 'ChanhAu', NULL, NULL, NULL, NULL, 3),
(3, 'ChanhMinh', 'Minh', NULL, NULL, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `user_status`
--

CREATE TABLE `user_status` (
  `UserId` int(11) NOT NULL,
  `StatusCode` char(9) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `user_status`
--

INSERT INTO `user_status` (`UserId`, `StatusCode`) VALUES
(1, 'normal'),
(2, 'normal'),
(3, 'normal');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20221222074949_FirstTimeCreateModels', '6.0.11'),
('20221222085502_ChangeTypeofPointToString', '6.0.11'),
('20221222151030_AddPercentToFido', '6.0.11'),
('20221223023545_EditStructureDatabase', '6.0.11'),
('20221223051005_AddPercentRandToModel', '6.0.11');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `dido`
--
ALTER TABLE `dido`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `fido`
--
ALTER TABLE `fido`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `fido_dido`
--
ALTER TABLE `fido_dido`
  ADD PRIMARY KEY (`FidoId`,`DidoId`),
  ADD KEY `IX_fido_dido_DidoId` (`DidoId`);

--
-- Indexes for table `point_detail`
--
ALTER TABLE `point_detail`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_point_detail_UserId` (`UserId`);

--
-- Indexes for table `point_of_day`
--
ALTER TABLE `point_of_day`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_point_of_day_UserId` (`UserId`);

--
-- Indexes for table `status`
--
ALTER TABLE `status`
  ADD PRIMARY KEY (`StatusCode`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_user_FidoId` (`FidoId`);

--
-- Indexes for table `user_status`
--
ALTER TABLE `user_status`
  ADD PRIMARY KEY (`UserId`,`StatusCode`),
  ADD KEY `IX_user_status_StatusCode` (`StatusCode`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `dido`
--
ALTER TABLE `dido`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `fido`
--
ALTER TABLE `fido`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `point_detail`
--
ALTER TABLE `point_detail`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `point_of_day`
--
ALTER TABLE `point_of_day`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `fido_dido`
--
ALTER TABLE `fido_dido`
  ADD CONSTRAINT `FK_fido_dido_dido_DidoId` FOREIGN KEY (`DidoId`) REFERENCES `dido` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_fido_dido_fido_FidoId` FOREIGN KEY (`FidoId`) REFERENCES `fido` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `point_detail`
--
ALTER TABLE `point_detail`
  ADD CONSTRAINT `FK_point_detail_user_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `point_of_day`
--
ALTER TABLE `point_of_day`
  ADD CONSTRAINT `FK_point_of_day_user_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `FK_user_fido_FidoId` FOREIGN KEY (`FidoId`) REFERENCES `fido` (`Id`);

--
-- Constraints for table `user_status`
--
ALTER TABLE `user_status`
  ADD CONSTRAINT `FK_user_status_status_StatusCode` FOREIGN KEY (`StatusCode`) REFERENCES `status` (`StatusCode`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_user_status_user_UserId` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

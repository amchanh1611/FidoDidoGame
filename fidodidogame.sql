-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 28, 2022 at 02:11 AM
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

--
-- Dumping data for table `fido`
--

INSERT INTO `fido` (`Id`, `Name`, `Percent`, `PercentRand`) VALUES
(1, 'BigFido', 20, 20),
(2, 'MediumFido', 30, 50),
(3, 'SmallFido', 50, 100);

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

--
-- Dumping data for table `point_detail`
--

INSERT INTO `point_detail` (`Id`, `UserId`, `Point`, `Date`, `IsX2`) VALUES
(1, 5, '200', '2022-12-25 05:49:46.160751', ''),
(3, 5, '390', '2022-12-25 06:12:03.368239', ''),
(4, 5, '200', '2022-12-26 12:48:31.302088', ''),
(5, 5, 'heal', '2022-12-27 06:04:31.932308', 'heal'),
(6, 5, 'heal', '2022-12-27 07:30:03.476220', 'heal'),
(7, 5, '200', '2022-12-27 07:32:59.765864', '200'),
(8, 5, '200', '2022-12-27 07:34:21.311448', '400'),
(9, 5, 'auto', '2022-12-27 07:39:11.025389', 'auto'),
(10, 5, '390', '2022-12-27 07:40:35.382220', '780'),
(11, 5, '390', '2022-12-27 08:04:58.232992', '780');

--
-- Dumping data for table `point_of_day`
--

INSERT INTO `point_of_day` (`Id`, `UserId`, `Point`, `Date`) VALUES
(1, 5, 590, '2022-12-25 06:12:03.368239'),
(2, 5, 200, '2022-12-26 12:48:31.302088'),
(3, 5, 980, '2022-12-27 08:04:58.232992');

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`Id`, `Name`, `NickName`, `Phone`, `Address`, `Male`, `Avatar`, `FidoId`, `Status`) VALUES
(5, 'minhminh', 'minhminh', NULL, NULL, NULL, NULL, NULL, '[1,2]');

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20221222074949_FirstTimeCreateModels', '6.0.11'),
('20221222085502_ChangeTypeofPointToString', '6.0.11'),
('20221222151030_AddPercentToFido', '6.0.11'),
('20221223023545_EditStructureDatabase', '6.0.11'),
('20221223051005_AddPercentRandToModel', '6.0.11'),
('20221224051950_MergeUserStatusToUser', '6.0.11'),
('20221224052111_MergeUserStatusToUserAndEditColumnType', '6.0.11'),
('20221226033442_ChangeTypePointDetailToString', '6.0.11'),
('20221227053040_AddCollumnIsX2InPointDetail', '6.0.11');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

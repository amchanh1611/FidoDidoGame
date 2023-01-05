-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 05, 2023 at 03:12 PM
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

INSERT INTO `fido_dido` (`FidoId`, `DidoId`, `Percent`, `Point`, `PercentRand`, `SpecialStatus`) VALUES
(1, 1, 20, 777, 20, 6),
(1, 2, 25, 390, 45, 6),
(1, 3, 10, 200, 75, 6),
(1, 4, 5, 0, 80, 2),
(1, 5, 5, 0, 85, 3),
(1, 6, 5, 0, 90, 4),
(1, 7, 10, 0, 100, 5),
(2, 2, 35, 390, 35, 6),
(2, 3, 45, 200, 80, 6),
(2, 4, 5, 0, 85, 2),
(2, 5, 5, 0, 90, 3),
(2, 7, 10, 0, 100, 5),
(3, 3, 65, 200, 65, 6),
(3, 4, 10, 0, 75, 2),
(3, 8, 25, 390, 100, 6);

--
-- Dumping data for table `point_detail`
--

INSERT INTO `point_detail` (`Id`, `UserId`, `Point`, `Date`, `IsX2`, `SpecialStatus`) VALUES
(1, 1, '390', '2022-12-29 13:05:24.064871', '390', 6),
(2, 2, '390', '2022-12-29 13:05:51.851950', '390', 6),
(3, 3, '390', '2022-12-29 13:08:09.217308', '390', 6),
(4, 1, '200', '2022-12-29 14:43:21.822202', '200', 6),
(5, 1, '777', '2022-12-29 16:15:35.057884', '777', 6),
(6, 1, '390', '2022-12-29 16:22:35.705008', '390', 6),
(7, 1, '200', '2022-12-29 16:22:41.059604', '200', 6),
(8, 2, '200', '2022-12-29 16:33:17.131096', '200', 6),
(9, 2, '200', '2022-12-29 16:33:28.055222', '200', 6),
(10, 2, '200', '2022-12-29 16:33:39.357461', '200', 6),
(11, 2, '200', '2022-12-29 16:33:43.775303', '200', 6),
(12, 2, '200', '2022-12-29 16:33:53.242811', '200', 6),
(13, 2, '200', '2022-12-29 16:33:57.147564', '200', 6),
(14, 3, '200', '2022-12-29 16:45:13.252422', '200', 6),
(15, 3, '200', '2022-12-29 16:47:18.652846', '200', 6),
(16, 3, '200', '2022-12-29 16:47:27.430962', '200', 6),
(17, 3, '777', '2022-12-29 16:47:51.973251', '777', 6),
(18, 3, '200', '2022-12-29 16:47:59.429797', '200', 6),
(19, 3, '200', '2022-12-29 16:48:04.537620', '200', 6),
(20, 3, '200', '2022-12-29 16:48:09.400312', '200', 6),
(21, 3, '777', '2022-12-29 16:48:14.567383', '777', 6),
(22, 3, '390', '2022-12-29 16:48:21.796851', '390', 6),
(23, 3, '390', '2022-12-29 16:48:27.943514', '390', 6),
(24, 3, '200', '2022-12-29 16:48:38.653564', '200', 6),
(25, 3, '777', '2022-12-29 16:48:45.539450', '777', 6),
(26, 3, '200', '2022-12-29 16:48:50.263206', '200', 6),
(27, 3, '390', '2022-12-29 16:48:55.216857', '390', 6),
(28, 3, '200', '2022-12-29 16:48:59.640188', '200', 6),
(29, 3, '390', '2022-12-29 16:49:04.155383', '390', 6),
(30, 2, '390', '2022-12-29 16:49:17.664066', '390', 6),
(31, 2, '200', '2022-12-29 16:49:24.922596', '200', 6),
(32, 2, '200', '2022-12-29 16:49:30.348809', '200', 6),
(33, 2, '390', '2022-12-29 16:49:36.525465', '390', 6),
(34, 2, '0', '2022-12-29 16:49:39.886713', '0', 2),
(35, 2, '200', '2022-12-29 16:50:03.153370', '400', 6);

--
-- Dumping data for table `point_of_day`
--

INSERT INTO `point_of_day` (`Id`, `UserId`, `Point`, `Date`) VALUES
(1, 1, 1957, '2022-12-29 16:22:41.059604'),
(2, 2, 2970, '2022-12-29 16:50:03.153370'),
(3, 3, 6081, '2022-12-29 16:49:04.155383');

--
-- Dumping data for table `server`
--

INSERT INTO `server` (`Id`, `Data`, `LastHeartbeat`) VALUES
('55c1de92-f2c6-417a-82ac-8cdc26824cf9', '{\"WorkerCount\":5,\"Queues\":[\"default\",\"default\",\"notdefault\"],\"StartedAt\":\"2023-01-04T13:49:44.5603464Z\"}', '2023-01-04 13:50:45.185739');

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`Id`, `Name`, `NickName`, `Phone`, `Address`, `Male`, `Avatar`, `FidoId`, `IdCard`, `RefreshToken`) VALUES
(1, 'Âu Minh Chánh', 'MinhChanh', NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(2, 'Âu Minh', 'Minh', NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(3, 'Âu Chanh', 'ChanhChanh', NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(177966851518295, 'Minh Chánh', 'Minh Chánh', NULL, NULL, NULL, 'https://platform-lookaside.fbsbx.com/platform/profilepic/?asid=177966851518295&height=720&ext=1675432202&hash=AeQ7m7Ow3Ms3gzxO7Us', NULL, NULL, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzI4NDAyMjIsImV4cCI6MTY3MzQ0NTAyMiwiaWF0IjoxNjcyODQwMjIyLCJpc3MiOiJodHRwczovL0ZpZG9kaWRvZ2FtZS5jb20ifQ.2pmxXU6K_q20ALb7ivVSCB63Pag9GwZGE-PeNVloSGI');

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20221223023545_EditStructureDatabase', '6.0.11'),
('20221223051005_AddPercentRandToModel', '6.0.11'),
('20221224051950_MergeUserStatusToUser', '6.0.11'),
('20221224052111_MergeUserStatusToUserAndEditColumnType', '6.0.11'),
('20221226033442_ChangeTypePointDetailToString', '6.0.11'),
('20221227053040_AddCollumnIsX2InPointDetail', '6.0.11'),
('20221229050348_EditModelFidoDido', '6.0.11'),
('20230104040557_DisableAutoIncrementUserId', '6.0.11'),
('20230104050232_ChangeTypeUserId', '6.0.11'),
('20230104134634_AddColumnRefreshTokenInUser', '6.0.11'),
('20230104134803_ChangeTypeColumnRefreshTokenInUser', '6.0.11'),
('20230104134827_ChangeTypeColumnRefreshTokenInUser1', '6.0.11'),
('20230105135151_AddModelReward', '6.0.11');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

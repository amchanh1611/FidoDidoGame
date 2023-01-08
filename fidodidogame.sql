-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 08, 2023 at 09:59 AM
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
-- Dumping data for table `event`
--

INSERT INTO `event` (`Id`, `Round`, `DateStart`, `DateEnd`) VALUES
(1, 1, '2023-01-07 00:00:00.000000', '2023-01-08 23:59:59.000000'),
(2, 2, '2023-01-09 00:00:00.000000', '2023-01-15 23:59:59.000000');

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

INSERT INTO `fido_dido` (`FidoId`, `DidoId`, `Percent`, `PercentRand`, `SpecialStatus`, `Point`) VALUES
(1, 1, 20, 20, 6, 777),
(1, 2, 25, 45, 6, 390),
(1, 3, 10, 75, 6, 200),
(1, 4, 5, 80, 2, 0),
(1, 5, 5, 85, 3, 0),
(1, 6, 5, 90, 4, 0),
(1, 7, 10, 100, 5, 0),
(2, 2, 35, 35, 6, 390),
(2, 3, 45, 80, 6, 200),
(2, 4, 5, 85, 2, 0),
(2, 5, 5, 90, 3, 0),
(2, 7, 10, 100, 5, 0),
(3, 3, 65, 65, 6, 200),
(3, 4, 10, 75, 2, 0),
(3, 8, 25, 100, 6, 390);

--
-- Dumping data for table `point_detail`
--

INSERT INTO `point_detail` (`Id`, `UserId`, `SpecialStatus`, `Point`, `Date`, `IsX2`) VALUES
(10, 177966851518295, 6, '390', '2023-01-07 20:57:25.133768', '390'),
(11, 177966851518295, 6, '200', '2023-01-07 20:57:38.354099', '200'),
(12, 177966851518295, 6, '200', '2023-01-10 20:59:19.999037', '200'),
(13, 177966851518295, 6, '390', '2023-01-10 20:59:25.661595', '390');

--
-- Dumping data for table `point_of_round`
--

INSERT INTO `point_of_round` (`Id`, `UserId`, `Point`, `Date`, `EventId`) VALUES
(1, 177966851518295, 590, '2023-01-07 20:57:38.354099', 1),
(2, 177966851518295, 590, '2023-01-10 20:59:25.661595', 2),
(3, 1, 0, '2023-01-08 15:06:08.829883', 1),
(4, 2, 0, '2023-01-08 15:06:50.843699', 1),
(5, 118008904500007, 0, '2023-01-08 15:58:03.691450', 1);

--
-- Dumping data for table `role`
--

INSERT INTO `role` (`Id`, `Name`) VALUES
(1, 'Admin'),
(2, 'Develop'),
(3, 'User'),
(4, 'Test');

--
-- Dumping data for table `server`
--

INSERT INTO `server` (`Id`, `Data`, `LastHeartbeat`) VALUES
('97e0122d-a0cf-4f9e-9f22-b29c7586dc89', '{\"WorkerCount\":5,\"Queues\":[\"default\",\"default\",\"notdefault\"],\"StartedAt\":\"2023-01-08T08:53:17.6553508Z\"}', '2023-01-08 08:58:18.229023');

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`Id`, `Name`, `NickName`, `Phone`, `Address`, `IdCard`, `Male`, `Avatar`, `FidoId`, `RefreshToken`, `RoleId`, `Password`) VALUES
(1, 'Admin', 'Admin', NULL, NULL, NULL, NULL, 'string', NULL, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzMxNjYyNzksImV4cCI6MTY3Mzc3MTA3OSwiaWF0IjoxNjczMTY2Mjc5LCJpc3MiOiJodHRwczovL0ZpZG9kaWRvZ2FtZS5jb20ifQ.eQ_co7wMKcVrGFsdz5WR6pEEUuaMTP7oAcOIGQnLP98', 1, 'admin'),
(2, 'Develop', 'Develop', NULL, NULL, NULL, NULL, 'string', NULL, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzMxNjYyMDIsImV4cCI6MTY3Mzc3MTAwMiwiaWF0IjoxNjczMTY2MjAyLCJpc3MiOiJodHRwczovL0ZpZG9kaWRvZ2FtZS5jb20ifQ.bfxWBMjLnzbaOecDu7ocCCX7KfU_wB7gCgqWtSn9RRs', 2, 'develop'),
(106155052368448, 'Carol Alhibahfjbage Bharambestein', 'Carol Alhibahfjbage Bharambestein', NULL, NULL, NULL, NULL, 'https://scontent.fsgn2-5.fna.fbcdn.net/v/t1.30497-1/84628273_176159830277856_972693363922829312_n.jpg?stp=dst-jpg_p720x720&_nc_cat=1&ccb=1-7&_nc_sid=12b3be&_nc_ohc=oSvaGOjAge0AX-3O2Ya&_nc_ht=scontent.fsgn2-5.fna&edm=AHgPADgEAAAA&oh=00_AfAE5gkA2bejcOFOS42NX_Nw7XL2luND4A9vgBjH4JK7mw&oe=63DF3BD9', NULL, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzMwNzIyMTQsImV4cCI6MTY3MzY3NzAxNCwiaWF0IjoxNjczMDcyMjE0LCJpc3MiOiJodHRwczovL0ZpZG9kaWRvZ2FtZS5jb20ifQ.E-s1xjqztPEOAgZjJnTk1iS5FWKxZMcl0F405ovSw6I', 3, NULL),
(118008904500007, 'Open Graph Test User', 'Open Graph Test User', NULL, NULL, NULL, NULL, 'https://z-p3-scontent.fsgn5-9.fna.fbcdn.net/v/t1.30497-1/84628273_176159830277856_972693363922829312_n.jpg?stp=dst-jpg_p720x720&_nc_cat=1&ccb=1-7&_nc_sid=12b3be&_nc_ohc=qIPCd3XtmYYAX_oZBVj&_nc_ht=z-p3-scontent.fsgn5-9.fna&edm=AHgPADgEAAAA&oh=00_AfBX_R4t70YU0NjIELugDsJhmJe1mWPqI4bXDvqSsxNNJQ&oe=63E1DED9', NULL, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzMxNjgyODMsImV4cCI6MTY3Mzc3MzA4MywiaWF0IjoxNjczMTY4MjgzLCJpc3MiOiJodHRwczovL0ZpZG9kaWRvZ2FtZS5jb20ifQ.5onwjTbNwrUq_Vkcaf5XqL4mzfiwrxXuTEoVC6yrm8w', 3, NULL),
(177966851518295, 'Minh Chánh', 'Minh Chánh', NULL, NULL, NULL, NULL, 'https://platform-lookaside.fbsbx.com/platform/profilepic/?asid=177966851518295&height=720&ext=1675566887&hash=AeRz3SgRxhlkR5Qy_U8', NULL, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NzI5NzQ4OTIsImV4cCI6MTY3MzU3OTY5MiwiaWF0IjoxNjcyOTc0ODkyLCJpc3MiOiJodHRwczovL0ZpZG9kaWRvZ2FtZS5jb20ifQ.1W8hFW6hha3lxO3KN2nT0GkFWn7ooPjnUZZw59RHsuk', 3, NULL);

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20230108074949_Init', '6.0.11'),
('20230108075700_AddModelsRole', '6.0.11'),
('20230108081314_AddColumnPassword', '6.0.11');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

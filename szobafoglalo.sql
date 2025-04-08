DROP DATABASE IF EXISTS szobafoglalo;

CREATE DATABASE szobafoglalo
DEFAULT CHARACTER SET utf8
COLLATE utf8_hungarian_ci;

USE szobafoglalo;

-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Ápr 02. 09:01
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `szobafoglalo`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `foglalas`
--

CREATE TABLE `foglalas` (
  `foglalasId` int(11) NOT NULL,
  `szobaszam` int(11) NOT NULL,
  `szemszam` varchar(8) NOT NULL,
  `ellatas` int(11) NOT NULL,
  `szemelyekSzama` int(11) NOT NULL,
  `checkInDatum` date NOT NULL,
  `checkOutDatum` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `foglalas`
--

INSERT INTO `foglalas` (`foglalasId`, `szobaszam`, `szemszam`, `ellatas`, `szemelyekSzama`, `checkInDatum`, `checkOutDatum`) VALUES
(1, 101, 'AABBCC12', 1, 2, '2024-12-10', '2024-12-15'),
(2, 102, 'ABCDEF12', 3, 4, '2024-12-26', '2024-12-29'),
(3, 103, 'CIXNSA72', 0, 2, '2025-02-20', '2025-02-25'),
(4, 202, 'NUTMNJ13', 2, 3, '2025-04-18', '2025-04-22'),
(5, 203, 'LGLMBX16', 3, 5, '2025-04-18', '2025-04-20'),
(6, 201, 'TGLICG59', 3, 5, '2025-04-05', '2025-04-20'),
(7, 201, 'WACHNC00', 3, 6, '2025-04-23', '2025-04-30'),
(8, 202, 'AABBCC12', 2, 6, '2025-04-23', '2025-04-26');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `foglalo`
--

CREATE TABLE `foglalo` (
  `szemSzam` varchar(8) NOT NULL,
  `nev` varchar(64) NOT NULL,
  `iranyitoszam` int(11) NOT NULL,
  `email` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `foglalo`
--

INSERT INTO `foglalo` (`szemSzam`, `nev`, `iranyitoszam`, `email`) VALUES
('AABBCC12', 'Kovács János', 1011, 'janos.kovacs@gmail.com'),
('ABCDEF12', 'Szabó Erika', 1020, 'erika.szabo@gmail.com'),
('CIXNSA72', 'Tóth Béla', 1117, 'bela.toth@gmail.com'),
('DZMWYV20', 'Molnár István', 1032, 'istvan.molnar@gmail.com'),
('LGLMBX16', 'Nagy Lajos', 3100, 'lajos.nagy@gmail.com'),
('NUTMNJ13', 'Nagy Mária', 2034, 'maria.nagy@gmail.com'),
('PIJYCV83', 'Szilágyi Ádám', 6200, 'adam.szilagyi@gmail.com'),
('TGLICG59', 'Kiss Péter', 3100, 'peter.kiss@gmail.com'),
('WACHNC00', 'Balogh Ferenc', 4100, 'ferenc.balogh@gmail.com');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `szoba`
--

CREATE TABLE `szoba` (
  `szobaszam` int(11) NOT NULL,
  `sztId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `szoba`
--

INSERT INTO `szoba` (`szobaszam`, `sztId`) VALUES
(101, 1),
(102, 1),
(103, 2),
(104, 2),
(105, 3),
(201, 4),
(202, 4),
(203, 4);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `szobatipus`
--

CREATE TABLE `szobatipus` (
  `sztId` int(11) NOT NULL,
  `ferohelyek` int(11) NOT NULL,
  `alaprajzKep` varchar(64) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- A tábla adatainak kiíratása `szobatipus`
--

INSERT INTO `szobatipus` (`sztId`, `ferohelyek`, `alaprajzKep`) VALUES
(1, 2, '../../Assets/Images/alaprajz1.png'),
(2, 3, '../../Assets/Images/alaprajz2.png'),
(3, 4, '../../Assets/Images/alaprajz3.png'),
(4, 6, '../../Assets/Images/alaprajz4.png');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `foglalas`
--
ALTER TABLE `foglalas`
  ADD PRIMARY KEY (`foglalasId`),
  ADD KEY `szobaszam` (`szobaszam`),
  ADD KEY `szemszam` (`szemszam`);

--
-- A tábla indexei `foglalo`
--
ALTER TABLE `foglalo`
  ADD PRIMARY KEY (`szemSzam`);

--
-- A tábla indexei `szoba`
--
ALTER TABLE `szoba`
  ADD PRIMARY KEY (`szobaszam`),
  ADD KEY `sztId` (`sztId`);

--
-- A tábla indexei `szobatipus`
--
ALTER TABLE `szobatipus`
  ADD PRIMARY KEY (`sztId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `foglalas`
--
ALTER TABLE `foglalas`
  MODIFY `foglalasId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT a táblához `szobatipus`
--
ALTER TABLE `szobatipus`
  MODIFY `sztId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `foglalas`
--
ALTER TABLE `foglalas`
  ADD CONSTRAINT `foglalas_ibfk_1` FOREIGN KEY (`szobaszam`) REFERENCES `szoba` (`szobaszam`),
  ADD CONSTRAINT `foglalas_ibfk_2` FOREIGN KEY (`szemszam`) REFERENCES `foglalo` (`szemSzam`);

--
-- Megkötések a táblához `szoba`
--
ALTER TABLE `szoba`
  ADD CONSTRAINT `szoba_ibfk_1` FOREIGN KEY (`sztId`) REFERENCES `szobatipus` (`sztId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

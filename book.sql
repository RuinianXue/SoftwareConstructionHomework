
DROP DATABASE IF EXISTS `book`;
CREATE DATABASE  `book`;
USE book;

--
-- Table structure for table `authors`
--

DROP TABLE IF EXISTS `authors`;
CREATE TABLE `authors` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `authors`
--

LOCK TABLES `authors` WRITE;
INSERT INTO `authors` VALUES (1,'Jack London'),(2,'Honore de Balzac'),(3,'Lion Feuchtwanger'),(5,'Truman Capote'),(6,'Trygve Gulbranssen'),(23,'li'),(24,'jia');
UNLOCK TABLES;

--
-- Table structure for table `books`
--

DROP TABLE IF EXISTS `books`;
CREATE TABLE `books` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AuthorId` int(11) DEFAULT NULL,
  `Title` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `AuthorId` (`AuthorId`),
  CONSTRAINT `books_ibfk_1` FOREIGN KEY (`AuthorId`) REFERENCES `authors` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `books`
--

LOCK TABLES `books` WRITE;
INSERT INTO `books` VALUES (1,1,'Call of the Wild'),(2,1,'Martin Eden'),(3,2,'Old Goriot'),(4,2,'Cousin Bette'),(5,3,'Jew Suess'),(8,5,'In Cold blood'),(9,5,'Breakfast at Tiffany'),(21,1,'New Book'),(22,1,'New Book'),(23,1,'New Book'),(24,1,'New Book'),(25,1,'New Book');
UNLOCK TABLES;

-- Dump completed on 2020-04-26 16:04:36

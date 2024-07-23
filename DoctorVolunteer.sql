-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: doctor
-- ------------------------------------------------------
-- Server version	8.0.36

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20240513060812_First','8.0.4');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account` (
  `Account_id` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `Member_id` int DEFAULT NULL,
  PRIMARY KEY (`Account_id`),
  KEY `Member_id` (`Member_id`),
  CONSTRAINT `account_ibfk_1` FOREIGN KEY (`Member_id`) REFERENCES `members` (`Member_id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` VALUES (4,'maingocduy11','AQAAAAIAAYagAAAAEFelakb/SznsRmWTyX3UU/es1fRq2SNYdwCChV56e0yCTo9CkHL7UlOxvWhZPqUgEg==',10),(24,'maingocduy12','AQAAAAIAAYagAAAAEMWOlyvTEVE2UiwkpOHgWv1Egn74VRgcq7FOOVa0CpqN3LzmFP8KnC6xAwWbkXld3w==',39);
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES ('42d8638a-54a1-421e-8d3c-da153c993384','Manager','MANAGER',NULL),('58ba5132-ccc8-4bb2-b258-4f614611bf73','User','USER',NULL),('980a42f6-6dc6-4644-8698-55d50bec11b3','Admin','ADMIN',NULL);
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES ('4485b085-17a1-435c-9c0e-f03ff75193ef','58ba5132-ccc8-4bb2-b258-4f614611bf73'),('c5d3a078-8c97-4a4e-a7e5-5a7bd5e03c86','980a42f6-6dc6-4644-8698-55d50bec11b3');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('4485b085-17a1-435c-9c0e-f03ff75193ef','maingocduy12','MAINGOCDUY12','maingocduy103@gmail.com','MAINGOCDUY103@GMAIL.COM',1,'AQAAAAIAAYagAAAAEMWOlyvTEVE2UiwkpOHgWv1Egn74VRgcq7FOOVa0CpqN3LzmFP8KnC6xAwWbkXld3w==','EVQESWDCX7SDS5FJQOCIDNQEN3K3SWAS','abb90378-ec1c-47c5-8054-53491b721f3a',NULL,0,0,NULL,1,0),('c5d3a078-8c97-4a4e-a7e5-5a7bd5e03c86','maingocduy11','MAINGOCDUY11','maingocduy11@gmail.com','MAINGOCDUY11@GMAIL.COM',1,'AQAAAAIAAYagAAAAEFelakb/SznsRmWTyX3UU/es1fRq2SNYdwCChV56e0yCTo9CkHL7UlOxvWhZPqUgEg==','TZWNBGHZUUKMDV2GUGXSJGBGJN5SLFQY','f57c1690-96d4-4c42-acf0-682f10896f6a',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `blog`
--

DROP TABLE IF EXISTS `blog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `blog` (
  `Blog_id` int NOT NULL AUTO_INCREMENT,
  `Account_id` int DEFAULT NULL,
  `Title` varchar(255) DEFAULT NULL,
  `Content` text,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `Approved` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`Blog_id`),
  KEY `Account_id` (`Account_id`),
  CONSTRAINT `blog_ibfk_1` FOREIGN KEY (`Account_id`) REFERENCES `account` (`Account_id`)
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `blog`
--

LOCK TABLES `blog` WRITE;
/*!40000 ALTER TABLE `blog` DISABLE KEYS */;
INSERT INTO `blog` VALUES (7,4,'Cách chăm sóc vết thương khi bị tai nạn','<h6><span style=\"font-size: 12pt;\">Việc chăm s&oacute;c vết thương sau khi bị tai nạn l&agrave; rất quan trọng để đảm bảo vết thương kh&ocirc;ng nặng hơn v&agrave; để nhanh ch&oacute;ng hồi phục. Dưới đ&acirc;y l&agrave; một số bước cơ bản để chăm s&oacute;c vết thương:</span></h6>\n<ol>\n<li>\n<p><strong>Kiểm so&aacute;t chảy m&aacute;u (nếu c&oacute;)</strong>:</p>\n<ul>\n<li>Sử dụng tay để b&oacute;p vết thương nhẹ nh&agrave;ng để kiểm so&aacute;t lượng m&aacute;u chảy ra.</li>\n<li>Nếu m&aacute;u chảy nhiều, n&ecirc;n n&eacute;n vết thương bằng tay hoặc d&ugrave;ng băng vải sạch để b&oacute; bột bốt.<img src=\"https://res.cloudinary.com/daqk0vumb/image/upload/v1718778712/8267516d-47bf-4957-859d-6594255faed3.jpg\" alt=\"Sơ cứu khi bị chảy m&aacute;u nghi&ecirc;m trọng | Vinmec\" width=\"519\" height=\"372\"></li>\n</ul>\n</li>\n<li>\n<p><strong>Rửa sạch vết thương</strong>:</p>\n<p><img src=\"https://res.cloudinary.com/daqk0vumb/image/upload/v1718778762/7768c757-91ae-40f5-a641-7a7f9d8a2e15.jpg\" alt=\"N&ecirc;n rửa vết thương ng&agrave;y mấy lần? Rửa sao cho đ&uacute;ng?\" width=\"557\" height=\"391\"></p>\n<ul>\n<li>Sử dụng nước sạch v&agrave; x&agrave; ph&ograve;ng nhẹ nh&agrave;ng rửa vết thương để loại bỏ bụi bẩn, vi khuẩn.</li>\n<li>Đừng d&ugrave;ng cồn hoặc c&aacute;c chất tẩy rửa kh&aacute;c v&igrave; ch&uacute;ng c&oacute; thể l&agrave;m tổn thương da.</li>\n</ul>\n</li>\n<li>\n<p><strong>S&aacute;t tr&ugrave;ng vết thương</strong>:</p>\n<p><img src=\"https://res.cloudinary.com/daqk0vumb/image/upload/v1718778813/3cb84f54-3368-440f-92b6-b558332f91e5.jpg\" alt=\"C&aacute;ch s&aacute;t tr&ugrave;ng vết thương hở hiệu quả chỉ với 7 bước\" width=\"548\" height=\"411\"></p>\n<ul>\n<li>Sử dụng dung dịch s&aacute;t tr&ugrave;ng như nước muối sinh l&yacute; để rửa sạch vết thương.</li>\n<li>Nếu c&oacute; thuốc s&aacute;t tr&ugrave;ng, bạn c&oacute; thể sử dụng để b&ocirc;i l&ecirc;n vết thương sau khi đ&atilde; rửa sạch.</li>\n</ul>\n</li>\n<li>\n<p><strong>Băng b&oacute; vết thương</strong>:</p>\n<p><img src=\"https://res.cloudinary.com/daqk0vumb/image/upload/v1718778878/795e22bf-ecc5-4946-9b68-41cfc55106c2.jpg\" alt=\"Kỹ thuật cơ bản băng b&oacute; c&aacute;c vết thương\" width=\"551\" height=\"390\"></p>\n<ul>\n<li>Sử dụng băng vải y tế sạch để băng b&oacute; vết thương, đảm bảo băng vải kh&ocirc;ng qu&aacute; chặt v&agrave; kh&ocirc;ng cản trở tuần ho&agrave;n m&aacute;u.</li>\n<li>Thay băng vải thường xuy&ecirc;n, đặc biệt l&agrave; khi băng vải bị ướt hoặc bẩn.</li>\n</ul>\n</li>\n<li>\n<p><strong>Theo d&otilde;i v&agrave; chăm s&oacute;c th&ecirc;m</strong>:</p>\n<ul>\n<li>Theo d&otilde;i t&igrave;nh trạng của vết thương, nếu c&oacute; dấu hiệu nhiễm tr&ugrave;ng như sưng tấy, đỏ v&agrave; nổi mủ, cần tham khảo ngay với b&aacute;c sĩ.</li>\n<li>Nếu vết thương qu&aacute; lớn, s&acirc;u hoặc g&acirc;y ra nghi ngại, cần điều trị tại bệnh viện để đảm bảo chăm s&oacute;c tốt nhất.</li>\n</ul>\n</li>\n<li>\n<p><strong>Giảm đau v&agrave; vi&ecirc;m</strong>:</p>\n<ul>\n<li>Nếu cần thiết, bạn c&oacute; thể sử dụng thuốc giảm đau nhẹ như paracetamol để giảm đau v&agrave; c&aacute;c thuốc kh&aacute;ng vi&ecirc;m để giảm vi&ecirc;m.</li>\n</ul>\n</li>\n<li>\n<p><strong>Bảo vệ vết thương</strong>:</p>\n<ul>\n<li>Tr&aacute;nh để vết thương tiếp x&uacute;c với bụi bẩn, nước bẩn hoặc c&aacute;c chất g&acirc;y k&iacute;ch ứng kh&aacute;c bằng c&aacute;ch băng b&oacute; hoặc đeo băng bao bọc vết thương khi cần thiết.</li>\n</ul>\n</li>\n</ol>\n<div id=\"eJOY__extension_root\" class=\"eJOY__extension_root_class\" style=\"all: unset;\"></div>','2024-06-19 13:35:51',1);
/*!40000 ALTER TABLE `blog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groups`
--

DROP TABLE IF EXISTS `groups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groups` (
  `Group_id` int NOT NULL AUTO_INCREMENT,
  `group_name` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Group_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groups`
--

LOCK TABLES `groups` WRITE;
/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
INSERT INTO `groups` VALUES (1,'Bác sĩ'),(2,'Y tá'),(3,'Dược sĩ'),(4,'Điều dưỡng'),(5,'Sinh viên'),(6,'Tình nguyện viên');
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `memberprojects`
--

DROP TABLE IF EXISTS `memberprojects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `memberprojects` (
  `Member_id` int NOT NULL,
  `Project_id` int NOT NULL,
  PRIMARY KEY (`Member_id`,`Project_id`),
  KEY `Project_id` (`Project_id`),
  CONSTRAINT `memberprojects_ibfk_1` FOREIGN KEY (`Member_id`) REFERENCES `members` (`Member_id`),
  CONSTRAINT `memberprojects_ibfk_2` FOREIGN KEY (`Project_id`) REFERENCES `projects` (`Project_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `memberprojects`
--

LOCK TABLES `memberprojects` WRITE;
/*!40000 ALTER TABLE `memberprojects` DISABLE KEYS */;
INSERT INTO `memberprojects` VALUES (10,5),(10,15),(39,15);
/*!40000 ALTER TABLE `memberprojects` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `members`
--

DROP TABLE IF EXISTS `members`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `members` (
  `Member_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `phone` int DEFAULT NULL,
  `Group_id` int DEFAULT NULL,
  PRIMARY KEY (`Member_id`),
  KEY `Group_id` (`Group_id`),
  CONSTRAINT `members_ibfk_1` FOREIGN KEY (`Group_id`) REFERENCES `groups` (`Group_id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `members`
--

LOCK TABLES `members` WRITE;
/*!40000 ALTER TABLE `members` DISABLE KEYS */;
INSERT INTO `members` VALUES (10,'Mai Dụy','maingocduy11@gmail.com',912345678,2),(39,'trường','maingocduy103@gmail.com',919528610,3);
/*!40000 ALTER TABLE `members` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `otp_member`
--

DROP TABLE IF EXISTS `otp_member`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `otp_member` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Member_id` int NOT NULL,
  `otp_code` varchar(6) NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `expires_at` timestamp NOT NULL,
  `IsVerified` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `Member_id` (`Member_id`),
  CONSTRAINT `otp_member_ibfk_1` FOREIGN KEY (`Member_id`) REFERENCES `members` (`Member_id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `otp_member`
--

LOCK TABLES `otp_member` WRITE;
/*!40000 ALTER TABLE `otp_member` DISABLE KEYS */;
INSERT INTO `otp_member` VALUES (16,10,'142266','2024-07-03 06:59:31','2024-07-03 07:04:31',1),(17,10,'860847','2024-07-03 08:43:57','2024-07-03 08:48:57',1),(18,10,'594372','2024-07-03 08:53:12','2024-07-03 08:58:12',1),(19,10,'227216','2024-07-03 08:55:24','2024-07-03 09:00:24',1),(20,10,'899516','2024-07-03 08:58:45','2024-07-03 09:03:45',1),(21,10,'443128','2024-07-03 09:02:11','2024-07-03 09:07:11',1),(22,10,'745915','2024-07-03 09:14:01','2024-07-03 09:19:01',1),(23,10,'661160','2024-07-03 09:20:07','2024-07-03 09:25:07',1),(24,10,'554020','2024-07-03 09:20:38','2024-07-03 09:25:38',1),(25,10,'639786','2024-07-03 09:21:42','2024-07-03 09:26:42',1),(28,10,'661237','2024-07-15 12:59:20','2024-07-15 13:04:20',0),(30,39,'285566','2024-07-15 16:28:52','2024-07-15 16:33:52',1);
/*!40000 ALTER TABLE `otp_member` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `otp_table`
--

DROP TABLE IF EXISTS `otp_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `otp_table` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Account_id` int NOT NULL,
  `otp_code` varchar(6) NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `expires_at` timestamp NOT NULL,
  `IsVerified` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `Account_id` (`Account_id`),
  CONSTRAINT `otp_table_ibfk_1` FOREIGN KEY (`Account_id`) REFERENCES `account` (`Account_id`)
) ENGINE=InnoDB AUTO_INCREMENT=99 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `otp_table`
--

LOCK TABLES `otp_table` WRITE;
/*!40000 ALTER TABLE `otp_table` DISABLE KEYS */;
INSERT INTO `otp_table` VALUES (33,4,'985264','2024-06-12 08:06:22','2024-06-12 08:11:22',1),(34,4,'383284','2024-06-12 08:54:55','2024-06-12 08:59:55',1),(35,4,'683033','2024-06-12 09:05:17','2024-06-12 09:10:17',1),(36,4,'172440','2024-06-12 09:07:59','2024-06-12 09:12:59',1),(37,4,'481163','2024-06-12 09:10:45','2024-06-12 09:15:45',1),(38,4,'119030','2024-06-12 09:29:44','2024-06-12 09:34:44',1),(39,4,'138715','2024-06-12 09:39:46','2024-06-12 09:44:46',1),(40,4,'489365','2024-06-12 09:43:16','2024-06-12 09:48:16',1),(41,4,'860587','2024-06-12 09:46:23','2024-06-12 09:51:23',1),(42,4,'415513','2024-06-12 09:49:36','2024-06-12 09:54:36',1),(43,4,'486132','2024-06-12 09:51:07','2024-06-12 09:56:07',1),(44,4,'434195','2024-06-12 09:55:12','2024-06-12 10:00:12',1),(45,4,'691671','2024-06-12 09:57:23','2024-06-12 10:02:23',1),(46,4,'857014','2024-06-12 10:04:38','2024-06-12 10:09:38',1),(47,4,'818909','2024-06-12 10:11:09','2024-06-12 10:16:09',1),(48,4,'220388','2024-06-12 10:13:14','2024-06-12 10:18:14',1),(49,4,'082549','2024-06-12 10:21:41','2024-06-12 10:26:41',1),(50,4,'321202','2024-06-12 10:24:25','2024-06-12 10:29:25',1),(51,4,'546303','2024-06-12 10:32:53','2024-06-12 10:37:53',1),(52,4,'442829','2024-06-13 08:57:03','2024-06-13 09:02:03',1),(53,4,'118216','2024-06-13 09:11:17','2024-06-13 09:16:17',1),(54,4,'622828','2024-06-13 09:13:53','2024-06-13 09:18:53',1),(55,4,'234236','2024-06-13 09:15:17','2024-06-13 09:20:17',1),(56,4,'626118','2024-06-13 09:18:17','2024-06-13 09:23:17',1),(57,4,'632553','2024-06-13 09:18:44','2024-06-13 09:23:44',1),(58,4,'474067','2024-06-13 10:07:31','2024-06-13 10:12:31',1),(59,4,'654376','2024-06-13 10:11:16','2024-06-13 10:16:16',1),(60,4,'003336','2024-06-13 10:11:26','2024-06-13 10:16:26',1),(61,4,'338934','2024-06-13 10:11:42','2024-06-13 10:16:42',1),(62,4,'261094','2024-06-13 10:12:30','2024-06-13 10:17:30',1),(63,4,'850423','2024-06-13 10:17:30','2024-06-13 10:22:30',1),(64,4,'342305','2024-06-13 10:20:40','2024-06-13 10:25:40',1),(65,4,'749764','2024-06-13 10:21:42','2024-06-13 10:26:42',1),(66,4,'134546','2024-06-13 10:23:15','2024-06-13 10:28:15',1),(67,4,'681937','2024-06-13 10:26:29','2024-06-13 10:31:29',1),(68,4,'678020','2024-06-13 10:29:31','2024-06-13 10:34:31',1),(69,4,'138102','2024-06-13 10:30:41','2024-06-13 10:35:41',1),(70,4,'720563','2024-06-13 10:32:02','2024-06-13 10:37:02',1),(71,4,'048489','2024-06-14 04:10:34','2024-06-14 04:15:34',1),(72,4,'133708','2024-06-14 04:12:05','2024-06-14 04:17:05',1),(73,4,'213480','2024-06-14 04:20:29','2024-06-14 04:25:29',1),(74,4,'482315','2024-06-14 04:30:00','2024-06-14 04:35:00',1),(75,4,'717403','2024-06-14 04:48:23','2024-06-14 04:53:23',1),(76,4,'820690','2024-06-17 06:37:36','2024-06-17 06:42:36',1),(77,4,'331681','2024-06-17 06:44:05','2024-06-17 06:49:05',1),(78,4,'646332','2024-06-17 07:18:26','2024-06-17 07:23:26',1),(79,4,'952314','2024-06-17 07:30:13','2024-06-17 07:35:13',1),(80,4,'075168','2024-06-17 07:40:37','2024-06-17 07:45:37',1),(81,4,'147653','2024-06-17 07:42:57','2024-06-17 07:47:57',1),(82,4,'635849','2024-06-17 07:46:20','2024-06-17 07:51:20',1),(83,4,'592610','2024-06-24 10:27:14','2024-06-24 10:32:14',1),(84,4,'179418','2024-06-26 02:11:16','2024-06-26 02:16:16',1),(88,4,'618717','2024-07-01 09:36:02','2024-07-01 09:41:02',1),(89,4,'099854','2024-07-02 09:25:06','2024-07-02 09:30:06',1),(90,4,'195315','2024-07-03 03:03:55','2024-07-03 03:08:55',1),(91,4,'868196','2024-07-03 03:05:17','2024-07-03 03:10:17',1),(92,4,'733687','2024-07-03 03:07:43','2024-07-03 03:12:43',1),(93,4,'905646','2024-07-03 07:21:55','2024-07-03 07:26:55',1),(94,4,'267800','2024-07-03 09:35:14','2024-07-03 09:40:14',1),(97,4,'076820','2024-07-09 03:03:22','2024-07-09 03:08:22',1),(98,4,'947938','2024-07-15 16:01:00','2024-07-15 16:06:00',0);
/*!40000 ALTER TABLE `otp_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `project_image`
--

DROP TABLE IF EXISTS `project_image`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `project_image` (
  `image_id` varchar(255) NOT NULL,
  `image_url` varchar(255) DEFAULT NULL,
  `image_content` varchar(255) DEFAULT NULL,
  `Project_id` int DEFAULT NULL,
  PRIMARY KEY (`image_id`),
  KEY `Project_id` (`Project_id`),
  CONSTRAINT `project_image_ibfk_1` FOREIGN KEY (`Project_id`) REFERENCES `projects` (`Project_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `project_image`
--

LOCK TABLES `project_image` WRITE;
/*!40000 ALTER TABLE `project_image` DISABLE KEYS */;
INSERT INTO `project_image` VALUES ('56e94d2c-45b6-4787-a15c-94b92aff1a08','https://res.cloudinary.com/daqk0vumb/image/upload/v1718952544/56e94d2c-45b6-4787-a15c-94b92aff1a08.jpg',NULL,5),('5ee9bc5c-0878-4096-aded-618b2020651f','https://res.cloudinary.com/daqk0vumb/image/upload/v1718952541/5ee9bc5c-0878-4096-aded-618b2020651f.jpg',NULL,5),('787cbbd9-ff19-43ac-8799-28624ef435bb','https://res.cloudinary.com/daqk0vumb/image/upload/v1719385819/787cbbd9-ff19-43ac-8799-28624ef435bb.png',NULL,6),('9041d66f-a041-4da3-be07-4d8ea8500167','https://res.cloudinary.com/daqk0vumb/image/upload/v1719385806/9041d66f-a041-4da3-be07-4d8ea8500167.png',NULL,6),('ee9d6cd3-b406-446e-b0d1-041f3d5dafff','https://res.cloudinary.com/daqk0vumb/image/upload/v1718952541/ee9d6cd3-b406-446e-b0d1-041f3d5dafff.jpg',NULL,5);
/*!40000 ALTER TABLE `project_image` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `projects`
--

DROP TABLE IF EXISTS `projects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `projects` (
  `Project_id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Budget` decimal(15,2) DEFAULT NULL,
  `Description` text,
  `StartDate` date DEFAULT NULL,
  `EndDate` date DEFAULT NULL,
  `Contributions` decimal(15,2) DEFAULT '0.00',
  `status` tinyint DEFAULT '0',
  PRIMARY KEY (`Project_id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projects`
--

LOCK TABLES `projects` WRITE;
/*!40000 ALTER TABLE `projects` DISABLE KEYS */;
INSERT INTO `projects` VALUES (1,'Project A',50000.00,'This is the description for Project A.','2024-05-01','2024-12-31',1700000.00,1),(5,'Dự án B',1000000.00,'<p>Dự &aacute;n \"B&aacute;c sĩ T&igrave;nh nguyện: Chăm s&oacute;c sức khỏe cho cộng đồng\" l&agrave; một s&aacute;ng kiến nổi bật của Hội B&aacute;c sĩ T&igrave;nh nguyện nhằm cung cấp c&aacute;c dịch vụ y tế miễn ph&iacute; cho những người d&acirc;n c&oacute; ho&agrave;n cảnh kh&oacute; khăn tại c&aacute;c khu vực v&ugrave;ng s&acirc;u v&ugrave;ng xa.</p>\n<p>Dự &aacute;n tập trung v&agrave;o việc tổ chức c&aacute;c buổi kh&aacute;m v&agrave; điều trị bệnh miễn ph&iacute;, đặc biệt l&agrave; cho những đối tượng kh&ocirc;ng c&oacute; điều kiện t&agrave;i ch&iacute;nh để truy cập dịch vụ y tế. Những bệnh nh&acirc;n tại đ&acirc;y kh&ocirc;ng chỉ được kh&aacute;m chữa bệnh m&agrave; c&ograve;n được tư vấn về c&aacute;ch ph&ograve;ng tr&aacute;nh bệnh tật v&agrave; cải thiện chế độ dinh dưỡng.</p>\n<p>Ngo&agrave;i ra, dự &aacute;n c&ograve;n th&uacute;c đẩy hoạt động gi&aacute;o dục sức khỏe như c&aacute;c buổi tư vấn về bệnh l&yacute; phổ biến v&agrave; c&aacute;c biện ph&aacute;p ph&ograve;ng ngừa bệnh tật trong cộng đồng. Điều n&agrave;y gi&uacute;p n&acirc;ng cao nhận thức về sức khỏe cho người d&acirc;n v&agrave; khuyến kh&iacute;ch họ tham gia v&agrave;o c&aacute;c chương tr&igrave;nh chăm s&oacute;c sức khỏe định kỳ.</p>\n<p>Mỗi dự &aacute;n được tổ chức bởi Hội B&aacute;c sĩ T&igrave;nh nguyện đều mang lại những kết quả đ&aacute;ng kể như sự h&agrave;i l&ograve;ng v&agrave; niềm tin từ ph&iacute;a cộng đồng, đồng thời gi&uacute;p cải thiện chất lượng cuộc sống v&agrave; sức khỏe cho những người d&acirc;n gặp kh&oacute; khăn. Điều n&agrave;y chứng tỏ sự hiệu quả v&agrave; tầm quan trọng của việc hỗ trợ y tế miễn ph&iacute; trong x&atilde; hội hiện nay.</p>\n<div id=\"eJOY__extension_root\" class=\"eJOY__extension_root_class\" style=\"all: unset;\"></div>','2024-06-28','2024-06-30',2147493647.00,0),(6,'Dự án c',1000000.00,'<p><strong>Mục ti&ecirc;u:</strong></p>\n<ul>\n<li>Cung cấp dịch vụ y tế miễn ph&iacute; cho c&aacute;c cộng đồng ngh&egrave;o, v&ugrave;ng s&acirc;u v&ugrave;ng xa.</li>\n<li>N&acirc;ng cao nhận thức về chăm s&oacute;c sức khỏe v&agrave; ph&ograve;ng ngừa bệnh tật trong cộng đồng.</li>\n<li>Hỗ trợ đ&agrave;o tạo v&agrave; n&acirc;ng cao năng lực cho c&aacute;n bộ y tế địa phương.</li>\n</ul>\n<p><strong>Phạm vi dự &aacute;n:</strong></p>\n<ul>\n<li>Khu vực triển khai: C&aacute;c v&ugrave;ng n&ocirc;ng th&ocirc;n, miền n&uacute;i, hải đảo kh&oacute; khăn.</li>\n<li>Đối tượng thụ hưởng: Người d&acirc;n c&oacute; ho&agrave;n cảnh kh&oacute; khăn, đặc biệt l&agrave; trẻ em, người gi&agrave;, phụ nữ mang thai, v&agrave; người khuyết tật.</li>\n</ul>\n<p><strong>C&aacute;c hoạt động ch&iacute;nh:</strong></p>\n<ol>\n<li>\n<p><strong>Kh&aacute;m chữa bệnh miễn ph&iacute;:</strong></p>\n<ul>\n<li>Tổ chức c&aacute;c đợt kh&aacute;m chữa bệnh định kỳ tại c&aacute;c địa phương.</li>\n<li>Cung cấp thuốc miễn ph&iacute; v&agrave; hướng dẫn sử dụng.</li>\n<li>Tư vấn v&agrave; điều trị bệnh l&yacute; phổ biến.</li>\n</ul>\n</li>\n<li>\n<p><strong>Tư vấn v&agrave; gi&aacute;o dục sức khỏe:</strong></p>\n<ul>\n<li>Tổ chức c&aacute;c buổi hội thảo, lớp học về chăm s&oacute;c sức khỏe.</li>\n<li>Ph&aacute;t h&agrave;nh t&agrave;i liệu, tờ rơi về ph&ograve;ng ngừa bệnh tật v&agrave; chăm s&oacute;c sức khỏe.</li>\n<li>Tư vấn dinh dưỡng v&agrave; vệ sinh c&aacute; nh&acirc;n.</li>\n</ul>\n</li>\n<li>\n<p><strong>Đ&agrave;o tạo v&agrave; n&acirc;ng cao năng lực:</strong></p>\n<ul>\n<li>Tập huấn v&agrave; hỗ trợ chuy&ecirc;n m&ocirc;n cho c&aacute;n bộ y tế địa phương.</li>\n<li>Chuyển giao kỹ thuật y tế mới v&agrave; ti&ecirc;n tiến.</li>\n<li>X&acirc;y dựng mạng lưới hỗ trợ y tế từ xa.</li>\n</ul>\n</li>\n<li>\n<p><strong>Hoạt động cộng đồng:</strong></p>\n<ul>\n<li>Tổ chức c&aacute;c chiến dịch ti&ecirc;m chủng v&agrave; ph&ograve;ng chống dịch bệnh.</li>\n<li>Hỗ trợ x&acirc;y dựng cơ sở vật chất y tế như ph&ograve;ng kh&aacute;m, nh&agrave; vệ sinh, hệ thống nước sạch.</li>\n<li>Phối hợp với c&aacute;c tổ chức địa phương để triển khai c&aacute;c hoạt động v&igrave; cộng đồng.</li>\n</ul>\n</li>\n</ol>\n<p><strong>Thời gian thực hiện:</strong></p>\n<ul>\n<li>Dự &aacute;n được triển khai trong giai đoạn 2024 - 2025, với c&aacute;c hoạt động định kỳ v&agrave; chiến dịch đặc biệt theo từng thời điểm.</li>\n</ul>\n<p><strong>Đội ngũ thực hiện:</strong></p>\n<ul>\n<li>C&aacute;c b&aacute;c sĩ, y t&aacute;, v&agrave; t&igrave;nh nguyện vi&ecirc;n y tế c&oacute; kinh nghiệm v&agrave; t&acirc;m huyết.</li>\n<li>Sự hợp t&aacute;c từ c&aacute;c tổ chức phi ch&iacute;nh phủ, c&aacute;c doanh nghiệp v&agrave; cộng đồng địa phương.</li>\n</ul>\n<p><strong>Kết quả mong đợi:</strong></p>\n<ul>\n<li>Cải thiện chất lượng sức khỏe cho h&agrave;ng ng&agrave;n người d&acirc;n ở c&aacute;c v&ugrave;ng kh&oacute; khăn.</li>\n<li>Tăng cường nhận thức v&agrave; kiến thức về chăm s&oacute;c sức khỏe trong cộng đồng.</li>\n<li>X&acirc;y dựng m&ocirc; h&igrave;nh chăm s&oacute;c sức khỏe bền vững v&agrave; hiệu quả.</li>\n</ul>\n<p><strong>Kinh ph&iacute; v&agrave; nguồn lực:</strong></p>\n<ul>\n<li>T&agrave;i trợ từ c&aacute;c tổ chức phi ch&iacute;nh phủ, doanh nghiệp, v&agrave; c&aacute;c nh&agrave; hảo t&acirc;m.</li>\n<li>Đ&oacute;ng g&oacute;p từ cộng đồng v&agrave; c&aacute;c t&igrave;nh nguyện vi&ecirc;n.</li>\n</ul>\n<p><strong>Li&ecirc;n hệ:</strong></p>\n<ul>\n<li>Email: <a rel=\"noreferrer\">info@suckhoecongdong.org</a></li>\n<li>Điện thoại: (+84) 123 456 789</li>\n<li>Địa chỉ: 123 Đường T&igrave;nh Nguyện, Quận Y Tế, Th&agrave;nh Phố Y&ecirc;u Thương, Việt Nam</li>\n</ul>\n<p>Ch&uacute;ng t&ocirc;i hy vọng dự &aacute;n sẽ nhận được sự quan t&acirc;m v&agrave; ủng hộ từ cộng đồng để c&oacute; thể lan tỏa y&ecirc;u thương v&agrave; chăm s&oacute;c sức khỏe đến mọi người, mọi nh&agrave;.</p>\n<div id=\"eJOY__extension_root\" class=\"eJOY__extension_root_class\" style=\"all: unset;\"></div>','2024-07-03','2024-07-04',120000.00,0),(7,'Dự án D',1000000.00,'<p>Dự &aacute;n n&agrave;y l&agrave; để d&agrave;nh cho lịch tr&igrave;nh đi kh&aacute;m bệnh v&ugrave;ng s&acirc;u v&ugrave;ng xa</p>\n<div id=\"eJOY__extension_root\" class=\"eJOY__extension_root_class\" style=\"all: unset;\"></div>','2024-07-05','2024-07-06',0.00,0),(15,'Dự án F',100000.00,'<p>aaa</p>','2024-07-20','2024-07-21',90000.00,0);
/*!40000 ALTER TABLE `projects` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `projectsponsor`
--

DROP TABLE IF EXISTS `projectsponsor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `projectsponsor` (
  `Project_id` int NOT NULL,
  `Sponsor_id` int NOT NULL,
  PRIMARY KEY (`Project_id`,`Sponsor_id`),
  KEY `Sponsor_id` (`Sponsor_id`),
  CONSTRAINT `projectsponsor_ibfk_1` FOREIGN KEY (`Project_id`) REFERENCES `projects` (`Project_id`),
  CONSTRAINT `projectsponsor_ibfk_2` FOREIGN KEY (`Sponsor_id`) REFERENCES `sponsor` (`Sponsor_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projectsponsor`
--

LOCK TABLES `projectsponsor` WRITE;
/*!40000 ALTER TABLE `projectsponsor` DISABLE KEYS */;
INSERT INTO `projectsponsor` VALUES (1,1),(1,2),(1,3),(1,4),(5,6),(6,7),(6,8),(6,9),(15,10),(15,11),(15,12),(15,13),(15,14),(15,15),(15,16),(15,17),(15,18);
/*!40000 ALTER TABLE `projectsponsor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sponsor`
--

DROP TABLE IF EXISTS `sponsor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sponsor` (
  `Sponsor_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Contact` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Address` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `ContributionAmount` decimal(15,2) DEFAULT NULL,
  `time_create` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Sponsor_id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sponsor`
--

LOCK TABLES `sponsor` WRITE;
/*!40000 ALTER TABLE `sponsor` DISABLE KEYS */;
INSERT INTO `sponsor` VALUES (1,'Tan','cheddar4904@msgsafe.io','tran quoc toan',100000.00,'2024-06-26 04:00:48'),(2,'mai duy','maingocduy11@gmail.com','phạm ngũ lão',100000.00,'2024-06-26 04:00:48'),(3,'maingocduy11','maingocduy11@yahoo.com','tran quoc toan',1000000.00,'2024-06-26 04:00:48'),(4,'Trường','cheddar4904@msgsafe.io','tran quoc toan',500000.00,'2024-06-26 04:00:48'),(6,'Tấn','cheddar4904@msgsafe.io','tran quoc toan',10000.00,'2024-06-26 04:00:48'),(7,'tan','maingocduy103@gmail.com','',100000.00,'2024-06-27 04:24:10'),(8,'tan','maingocduy103@gmail.com','tran quoc toan',10000.00,'2024-07-02 10:24:39'),(9,'maingocduy11','maingocduy11@yahoo.com','tran quoc toan',10000.00,'2024-07-03 07:12:54'),(10,'Tan','maingocduy11@yahoo.com','tran quoc toan',10000.00,'2024-07-09 02:39:47'),(11,'trường','maingocduy11@yahoo.com','tran quoc toan',10000.00,'2024-07-15 14:23:37'),(12,'Trần quốc tấn','maingocduy11@yahoo.com','tran quoc toan',10000.00,'2024-07-15 14:24:46'),(13,'trường','maingocduy11@gmail.com','tran quoc toan',10000.00,'2024-07-15 14:28:56'),(14,'trường','maingocduy11@yahoo.com','tran quoc toan',10000.00,'2024-07-15 14:30:41'),(15,'Tr??ng','maingocduy11@yahoo.com','tran quoc toan',10000.00,'2024-07-15 14:33:55'),(16,'trường','namhoangheller@gmail.com','tran quoc toan',10000.00,'2024-07-15 14:39:40'),(17,'Trường','maingocduy11@yahoo.com','',10000.00,'2024-07-15 14:41:32'),(18,'Trường','maingocduy11@yahoo.com','tran quoc toan',10000.00,'2024-07-15 14:43:48');
/*!40000 ALTER TABLE `sponsor` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-07-15 23:33:10

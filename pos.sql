CREATE DATABASE  IF NOT EXISTS `pos` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `pos`;
-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: pos
-- ------------------------------------------------------
-- Server version	8.0.40

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
-- Table structure for table `tbl_coupon`
--

DROP TABLE IF EXISTS `tbl_coupon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_coupon` (
  `CouponId` varchar(50) NOT NULL,
  `CouponCode` varchar(255) NOT NULL,
  `CouponName` varchar(255) NOT NULL,
  `DiscountAmount` decimal(10,2) NOT NULL,
  `AvailableQuantity` int NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `CouponQrFilePath` varchar(100) NOT NULL,
  `CreatedUserId` varchar(50) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `ModifiedUserId` varchar(50) DEFAULT NULL,
  `ModifiedDateTime` datetime DEFAULT NULL,
  `DelFlag` bit(1) NOT NULL,
  PRIMARY KEY (`CouponId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_coupon`
--

LOCK TABLES `tbl_coupon` WRITE;
/*!40000 ALTER TABLE `tbl_coupon` DISABLE KEYS */;
INSERT INTO `tbl_coupon` VALUES ('a3ef3850-92a4-11ef-a32c-00090ffe0001','C000001','One Dollar',1.00,5,'2024-10-25 14:12:09','2024-11-24 14:12:09','','95a5fa04-941d-11ef-9bd9-00090ffe0001','2024-10-27 00:00:00',NULL,NULL,_binary '\0'),('a3fddb91-92a4-11ef-a32c-00090ffe0001','C000002','Two Dollar',2.00,10,'2024-10-25 14:12:09','2024-11-24 14:12:09','','95a5fa04-941d-11ef-9bd9-00090ffe0001','2024-10-27 00:00:00',NULL,NULL,_binary '\0'),('e3204d0e-92a4-11ef-a32c-00090ffe0001','C000003','Three Dollar',3.00,15,'2024-10-25 14:13:55','2024-11-24 14:13:55','','95a5fa04-941d-11ef-9bd9-00090ffe0001','2024-10-27 00:00:00',NULL,NULL,_binary '\0');
/*!40000 ALTER TABLE `tbl_coupon` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbl_member`
--

DROP TABLE IF EXISTS `tbl_member`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_member` (
  `MemberId` varchar(50) DEFAULT NULL,
  `MemberCode` varchar(255) DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `PhoneNo` varchar(15) DEFAULT NULL,
  `TotalPoints` int DEFAULT NULL,
  `TotalPurchasedAmount` decimal(10,2) DEFAULT NULL,
  `MemberQrFilePath` varchar(100) DEFAULT NULL,
  `CreatedUserId` varchar(50) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `ModifiedUserId` varchar(50) DEFAULT NULL,
  `ModifiedDateTime` datetime DEFAULT NULL,
  `DelFlag` bit(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_member`
--

LOCK TABLES `tbl_member` WRITE;
/*!40000 ALTER TABLE `tbl_member` DISABLE KEYS */;
INSERT INTO `tbl_member` VALUES ('1180bf3a-92a5-11ef-a32c-00090ffe0001','P000002','Clox','09955949712',0,0.00,'','95a5fa04-941d-11ef-9bd9-00090ffe0001','2024-10-27 00:00:00',NULL,NULL,_binary '\0'),('96723736-929a-11ef-9879-00090ffe0001','P000001','Yaho','095130453',0,0.00,'','95a5fa04-941d-11ef-9bd9-00090ffe0001','2024-10-27 00:00:00',NULL,NULL,_binary '\0');
/*!40000 ALTER TABLE `tbl_member` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbl_purchasehistory`
--

DROP TABLE IF EXISTS `tbl_purchasehistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_purchasehistory` (
  `PurchaseHistoryId` varchar(50) DEFAULT NULL,
  `MemberId` varchar(50) DEFAULT NULL,
  `TotalPoint` int DEFAULT NULL,
  `TotalPrice` decimal(10,2) DEFAULT NULL,
  `TranDate` datetime DEFAULT NULL,
  `CreatedUserId` varchar(50) DEFAULT NULL,
  `CreatedDateTime` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_purchasehistory`
--

LOCK TABLES `tbl_purchasehistory` WRITE;
/*!40000 ALTER TABLE `tbl_purchasehistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbl_purchasehistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbl_purchasehistorydetail`
--

DROP TABLE IF EXISTS `tbl_purchasehistorydetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_purchasehistorydetail` (
  `PurchaseHistoryDetailId` varchar(50) DEFAULT NULL,
  `PurchaseHistoryId` varchar(50) DEFAULT NULL,
  `ItemDescription` varchar(100) DEFAULT NULL,
  `AlcoholFree` bit(1) DEFAULT NULL,
  `Price` decimal(10,2) DEFAULT NULL,
  `Quantity` int DEFAULT NULL,
  `TotalPrice` decimal(10,2) DEFAULT NULL,
  `CreatedUserId` varchar(50) DEFAULT NULL,
  `CreatedDateTime` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_purchasehistorydetail`
--

LOCK TABLES `tbl_purchasehistorydetail` WRITE;
/*!40000 ALTER TABLE `tbl_purchasehistorydetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `tbl_purchasehistorydetail` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-10-27 12:27:30

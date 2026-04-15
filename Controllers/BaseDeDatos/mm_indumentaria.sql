-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: mm_indumentaria
-- ------------------------------------------------------
-- Server version	8.0.39

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
-- Table structure for table `categorias`
--

DROP TABLE IF EXISTS `categorias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categorias` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) DEFAULT NULL,
  `Imagen` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Nombre` (`Nombre`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categorias`
--

LOCK TABLES `categorias` WRITE;
/*!40000 ALTER TABLE `categorias` DISABLE KEYS */;
INSERT INTO `categorias` VALUES (1,'Remeras','remeragris.jpeg'),(2,'Abrigos','sweaterAzul.png'),(3,'Pantalones','pantalonBaggy.png'),(4,'Cortos','shortDeportivo.jpg'),(5,'Accesorios','riñoneras.png');
/*!40000 ALTER TABLE `categorias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `productos`
--

DROP TABLE IF EXISTS `productos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) DEFAULT NULL,
  `Precio` double DEFAULT NULL,
  `Stock` int DEFAULT NULL,
  `Categoria` varchar(50) DEFAULT NULL,
  `Tipo` varchar(50) DEFAULT NULL,
  `Imagen` varchar(100) DEFAULT NULL,
  `Codigo` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productos`
--

LOCK TABLES `productos` WRITE;
/*!40000 ALTER TABLE `productos` DISABLE KEYS */;
INSERT INTO `productos` VALUES (1,'Remera blanca',8000,4,'Remeras','Remeras lisas','remerablanca.jpeg','PROD001'),(2,'Remera azul estampada',8500,3,'Remeras','Remeras estampadas','remeraazul.jpeg','PROD002'),(3,'Remera Gris Estampada',8500,1,'Remeras','Remeras estampadas','remeragris.jpeg','PROD003'),(4,'Sweater Bremer Azul Marino',12000,3,'Abrigos','Sweaters','sweaterAzul.png','PROD004'),(5,'Sweater Bremer Bordo',12000,2,'Abrigos','Sweaters','sweaterBordo.png','PROD005'),(6,'Buzo Overside Dados',9000,2,'Abrigos','Buzos','buzoOverside1.png','PROD006'),(7,'Campera Impermeable Reversible 1',10000,4,'Abrigos','Camperas','campera1.jpeg','PROD007'),(8,'Buzo Overside Cherries',9000,3,'Abrigos','Buzos','buzoOverside2.png','PROD008'),(9,'Campera Impermeable Reversible 2',10000,3,'Abrigos','Camperas','campera2.jpeg','PROD009'),(10,'Pantalón Baggy',12000,2,'Pantalones','Jeans','pantalonBaggy.png','PROD010'),(11,'Cupín Elastizado',14000,5,'Pantalones','Jeans','chupinElastizado.png','PROD011'),(12,'Joggins Con Elástico',10000,6,'Pantalones','Joggins','joggins.png','PROD012'),(13,'Pantalón Deportivo',11000,7,'Pantalones','Deportivos','deportivo.png','PROD013'),(14,'Pantalón Camuflado',9500,8,'Pantalones','Cargo','cargoCamuflado.png','PROD014'),(15,'Short Deportivo Varios Colores',6000,6,'Cortos','Shorts','shortDeportivo.jpg','PROD015'),(16,'Bermuda Camuflada',8500,4,'Cortos','Bermudas','bermudasCamufladas.png','PROD016');
/*!40000 ALTER TABLE `productos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subcategorias`
--

DROP TABLE IF EXISTS `subcategorias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subcategorias` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) DEFAULT NULL,
  `Categoria` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Nombre` (`Nombre`,`Categoria`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subcategorias`
--

LOCK TABLES `subcategorias` WRITE;
/*!40000 ALTER TABLE `subcategorias` DISABLE KEYS */;
INSERT INTO `subcategorias` VALUES (10,'Bermudas','Cortos'),(5,'Buzos','Abrigos'),(4,'Camperas','Abrigos'),(9,'Cargo','Pantalones'),(8,'Deportivos','Pantalones'),(6,'Jeans','Pantalones'),(7,'Joggins','Pantalones'),(2,'Remeras estampadas','Remeras'),(1,'Remeras lisas','Remeras'),(11,'Shorts','Cortos'),(3,'Sweaters','Abrigos');
/*!40000 ALTER TABLE `subcategorias` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-04-15 20:12:10

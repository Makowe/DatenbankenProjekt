-- MariaDB dump 10.19  Distrib 10.4.18-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: project_9275184
-- ------------------------------------------------------
-- Server version	10.4.18-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `component`
--

DROP TABLE IF EXISTS `component`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `component` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=183 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `component`
--

LOCK TABLES `component` WRITE;
/*!40000 ALTER TABLE `component` DISABLE KEYS */;
INSERT INTO `component` VALUES (0,'Gelöschte Zutat'),(6,'Kartoffeln (festkochend)'),(7,'Zwiebel'),(8,'Paprikagewürz'),(9,'Eier'),(10,'Zucker'),(13,'Fischstäbchen'),(16,'Wasser'),(17,'Mehl'),(18,'Rosmarin'),(19,'Haferflocken'),(20,'Knoblauch'),(22,'Zucchini'),(23,'Apfel'),(27,'Speck'),(28,'Milch'),(29,'Schlagsahne'),(31,'Käse'),(32,'Frischkäse'),(33,'Salz'),(34,'Tomaten'),(35,'Rinderhackfleisch'),(40,'Pfeffer'),(41,'Paniermehl'),(42,'Pommes'),(43,'Schweineschnitzel'),(44,'Mozarella'),(45,'Oregano'),(46,'Thymian'),(48,'Tomaten (stückig)'),(49,'Jodsalz'),(50,'Hackfleisch'),(51,'Cayenne Pfeffer'),(52,'Olivenöl'),(53,'Zimt'),(54,'Hanföl'),(55,'Leinsamen'),(56,'Hafermilch'),(57,'Eiswürfel'),(59,'Tonic Water'),(60,'Gin'),(61,'Leinöl'),(62,'Heidelbeeren'),(63,'Karotte'),(64,'Salat'),(65,'Currypulver'),(66,'Kartoffeln'),(67,'Seelachs'),(68,'Paprika'),(69,'Basilikum Kräuter'),(70,'Tabasco'),(71,'Tomatensaft'),(72,'Vodka'),(80,'Brokkoli'),(81,'Aubergine');
/*!40000 ALTER TABLE `component` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `component_in_recipe`
--

DROP TABLE IF EXISTS `component_in_recipe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `component_in_recipe` (
  `component` int(11) NOT NULL,
  `recipe` int(11) NOT NULL,
  `amount` double(10,2) DEFAULT NULL,
  `unit` int(11) DEFAULT NULL,
  KEY `recipe` (`recipe`),
  KEY `unit` (`unit`),
  CONSTRAINT `component_in_recipe_ibfk_1` FOREIGN KEY (`component`) REFERENCES `component` (`id`),
  CONSTRAINT `component_in_recipe_ibfk_2` FOREIGN KEY (`recipe`) REFERENCES `recipe` (`id`),
  CONSTRAINT `component_in_recipe_ibfk_3` FOREIGN KEY (`unit`) REFERENCES `unit` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `component_in_recipe`
--

LOCK TABLES `component_in_recipe` WRITE;
/*!40000 ALTER TABLE `component_in_recipe` DISABLE KEYS */;
INSERT INTO `component_in_recipe` VALUES (35,11,500.00,1),(22,11,1200.00,1),(7,11,1.00,2),(20,11,2.00,2),(34,11,400.00,1),(33,11,1.00,6),(32,11,200.00,1),(29,11,100.00,3),(31,11,100.00,1),(49,2,10.00,1),(6,2,2.00,2),(7,2,2.00,2),(43,1,1000.00,1),(28,1,0.00,1),(41,1,0.00,1),(33,1,1.00,6),(40,1,1.00,6),(48,20,400.00,1),(50,20,200.00,1),(18,20,2.00,4),(46,20,2.00,4),(45,20,2.00,4),(7,20,2.00,2),(20,20,1.00,2),(19,24,120.00,1),(56,24,350.00,3),(55,24,3.00,4),(54,24,1.00,5),(53,24,1.00,6),(60,27,40.00,3),(59,27,200.00,3),(0,27,20.00,1),(57,27,2.00,2),(67,29,250.00,1),(66,29,500.00,1),(29,29,50.00,3),(28,29,150.00,3),(65,29,3.00,4),(72,30,10.00,3),(71,30,10.00,3),(70,30,1.00,5),(68,31,1.00,2),(66,31,4.00,2),(29,31,100.00,3),(65,31,1.00,4),(16,31,200.00,3),(80,31,100.00,1),(81,31,1.00,2);
/*!40000 ALTER TABLE `component_in_recipe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `instruction`
--

DROP TABLE IF EXISTS `instruction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `instruction` (
  `recipe` int(11) NOT NULL,
  `step` int(11) NOT NULL,
  `description` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`recipe`,`step`),
  CONSTRAINT `instruction_ibfk_1` FOREIGN KEY (`recipe`) REFERENCES `recipe` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `instruction`
--

LOCK TABLES `instruction` WRITE;
/*!40000 ALTER TABLE `instruction` DISABLE KEYS */;
INSERT INTO `instruction` VALUES (1,1,'Schnitzel klopfen, würzen und panieren'),(1,2,'Pommes fritieren'),(1,3,'Schnitzel anbraten'),(2,1,'Kartoffeln reiben'),(2,2,'Zwiebeln hacken'),(2,3,'Kartoffeln und Zwiebeln vermischen und würzen'),(2,4,'Die Masse braten'),(11,1,'Zucchini in Streifen schneiden und in Backofen garen'),(11,2,'Zwiebel, Knoblauch, Hackfleisch braten. Danach Tomaten dazugeben'),(11,3,'Sahne und Frischkäse vermischen'),(11,4,'3-4 Schichten Lasagne. Jeweils Zucchini, Tomatensoße und Sahnesoße. Zum Schluss Käse darüber geben'),(11,5,'Bei 200° für 20-30 Minuten in Backofen'),(20,1,'Zwiebel, Knoblauch und Hackfleisch braten'),(20,2,'Tomaten und Gewürze dazu geben'),(20,3,'20 - 25 Minuten köcheln lassen'),(24,1,'Haferflocken, Milch und Leinsamen vermischen'),(24,2,'2 Minuten In Mikrowelle erhitzen '),(24,3,'Öl und Zimt dazu geben'),(29,1,'Fisch in Backofen garen (10 Minuten Umluft, 200°)'),(29,2,'Kartoffeln kochen und schälen'),(29,3,'Sahne, Milch und Curry vermischen, rühren und aufkochen'),(31,1,'Das Gemüse garen'),(31,2,'Gemüse zusammen mit dem Wasser und den Gewürzen mixen'),(31,3,'Die Suppe im Thermomix oder Topf erhitzen'),(31,4,'Die Sahne dazugeben und noch etwas köcheln lassen');
/*!40000 ALTER TABLE `instruction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipe`
--

DROP TABLE IF EXISTS `recipe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `people` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipe`
--

LOCK TABLES `recipe` WRITE;
/*!40000 ALTER TABLE `recipe` DISABLE KEYS */;
INSERT INTO `recipe` VALUES (1,'Paniertes Schnitzel mit Pommes',3),(2,'Rösti',4),(11,'Zucchini Lasagne',4),(20,'Tomatensoße',4),(24,'Hafer Porridge',2),(27,'Gin Tonic',2),(29,'Seelachs mit Kartoffeln und Sahnesoße',2),(30,'Mexikaner Shot',2),(31,'Gemüsecremesuppe',4),(53,'Rezept1 Unittest',2);
/*!40000 ALTER TABLE `recipe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tag`
--

DROP TABLE IF EXISTS `tag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tag` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tag`
--

LOCK TABLES `tag` WRITE;
/*!40000 ALTER TABLE `tag` DISABLE KEYS */;
INSERT INTO `tag` VALUES (1,'Low Carb'),(3,'Snack'),(4,'Dessert'),(5,'Glutenfrei'),(6,'Vegetarisch'),(7,'Vegan'),(8,'Für unterwegs'),(9,'Fingerfood'),(10,'Laktosefrei'),(11,'High Protein'),(12,'Beilage'),(13,'Getränk'),(14,'Alkoholisch');
/*!40000 ALTER TABLE `tag` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tag_in_recipe`
--

DROP TABLE IF EXISTS `tag_in_recipe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tag_in_recipe` (
  `recipe` int(11) DEFAULT NULL,
  `tag` int(11) DEFAULT NULL,
  KEY `recipe` (`recipe`),
  KEY `tag` (`tag`),
  CONSTRAINT `tag_in_recipe_ibfk_1` FOREIGN KEY (`recipe`) REFERENCES `recipe` (`id`),
  CONSTRAINT `tag_in_recipe_ibfk_2` FOREIGN KEY (`tag`) REFERENCES `tag` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tag_in_recipe`
--

LOCK TABLES `tag_in_recipe` WRITE;
/*!40000 ALTER TABLE `tag_in_recipe` DISABLE KEYS */;
INSERT INTO `tag_in_recipe` VALUES (11,1),(2,10),(2,5),(2,7),(20,10),(20,5),(20,12),(24,7),(24,5),(27,14),(27,13),(29,11),(30,14),(31,6),(31,12),(31,1);
/*!40000 ALTER TABLE `tag_in_recipe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unit`
--

DROP TABLE IF EXISTS `unit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `unit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) DEFAULT NULL,
  `shortname` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unit`
--

LOCK TABLES `unit` WRITE;
/*!40000 ALTER TABLE `unit` DISABLE KEYS */;
INSERT INTO `unit` VALUES (1,'Gramm','g'),(2,'Stück','St.'),(3,'Milliliter','ml'),(4,'Esslöffel','El.'),(5,'Teelöffel','Tl.'),(6,'Prise','Pr.');
/*!40000 ALTER TABLE `unit` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-05-21 14:33:07

/*M!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19  Distrib 10.11.11-MariaDB, for Linux (x86_64)
--
-- Host: 127.0.0.1    Database: joymodels-db
-- ------------------------------------------------------
-- Server version	11.8.2-MariaDB-ubu2404

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
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `uuid` char(36) NOT NULL,
  `category_name` varchar(50) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `category_name` (`category_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `community_post_pictures`
--

DROP TABLE IF EXISTS `community_post_pictures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `community_post_pictures` (
  `uuid` char(36) NOT NULL,
  `community_post_uuid` char(36) NOT NULL,
  `picture_location` varchar(254) NOT NULL,
  `picture_width` int(11) NOT NULL,
  `picture_height` int(11) NOT NULL,
  `created_at` datetime NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_community_post_picture` (`community_post_uuid`,`picture_location`),
  CONSTRAINT `community_post_pictures_ibfk_1` FOREIGN KEY (`community_post_uuid`) REFERENCES `community_posts` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`picture_width` > 0 and `picture_height` > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `community_post_pictures`
--

LOCK TABLES `community_post_pictures` WRITE;
/*!40000 ALTER TABLE `community_post_pictures` DISABLE KEYS */;
/*!40000 ALTER TABLE `community_post_pictures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `community_post_question_section`
--

DROP TABLE IF EXISTS `community_post_question_section`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `community_post_question_section` (
  `uuid` char(36) NOT NULL,
  `user_origin_uuid` char(36) NOT NULL,
  `user_target_uuid` char(36) DEFAULT NULL,
  `community_post_uuid` char(36) NOT NULL,
  `message_type_uuid` char(36) NOT NULL,
  `message_text` text NOT NULL,
  `created_at` datetime NOT NULL,
  PRIMARY KEY (`uuid`),
  KEY `user_origin_uuid` (`user_origin_uuid`),
  KEY `user_target_uuid` (`user_target_uuid`),
  KEY `community_post_uuid` (`community_post_uuid`),
  KEY `message_type_uuid` (`message_type_uuid`),
  CONSTRAINT `community_post_question_section_ibfk_1` FOREIGN KEY (`user_origin_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `community_post_question_section_ibfk_2` FOREIGN KEY (`user_target_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `community_post_question_section_ibfk_3` FOREIGN KEY (`community_post_uuid`) REFERENCES `community_posts` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `community_post_question_section_ibfk_4` FOREIGN KEY (`message_type_uuid`) REFERENCES `message_types` (`uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `community_post_question_section`
--

LOCK TABLES `community_post_question_section` WRITE;
/*!40000 ALTER TABLE `community_post_question_section` DISABLE KEYS */;
/*!40000 ALTER TABLE `community_post_question_section` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `community_post_review_types`
--

DROP TABLE IF EXISTS `community_post_review_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `community_post_review_types` (
  `uuid` char(36) NOT NULL,
  `review_name` varchar(50) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `review_name` (`review_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `community_post_review_types`
--

LOCK TABLES `community_post_review_types` WRITE;
/*!40000 ALTER TABLE `community_post_review_types` DISABLE KEYS */;
/*!40000 ALTER TABLE `community_post_review_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `community_post_types`
--

DROP TABLE IF EXISTS `community_post_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `community_post_types` (
  `uuid` char(36) NOT NULL,
  `community_post_name` varchar(50) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `community_post_name` (`community_post_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `community_post_types`
--

LOCK TABLES `community_post_types` WRITE;
/*!40000 ALTER TABLE `community_post_types` DISABLE KEYS */;
/*!40000 ALTER TABLE `community_post_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `community_post_user_reviews`
--

DROP TABLE IF EXISTS `community_post_user_reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `community_post_user_reviews` (
  `uuid` char(36) NOT NULL,
  `user_uuid` char(36) NOT NULL,
  `community_post_uuid` char(36) NOT NULL,
  `review_type_uuid` char(36) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_user_community_post` (`user_uuid`,`community_post_uuid`),
  KEY `community_post_uuid` (`community_post_uuid`),
  KEY `review_type_uuid` (`review_type_uuid`),
  CONSTRAINT `community_post_user_reviews_ibfk_1` FOREIGN KEY (`user_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `community_post_user_reviews_ibfk_2` FOREIGN KEY (`community_post_uuid`) REFERENCES `community_posts` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `community_post_user_reviews_ibfk_3` FOREIGN KEY (`review_type_uuid`) REFERENCES `community_post_review_types` (`uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `community_post_user_reviews`
--

LOCK TABLES `community_post_user_reviews` WRITE;
/*!40000 ALTER TABLE `community_post_user_reviews` DISABLE KEYS */;
/*!40000 ALTER TABLE `community_post_user_reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `community_posts`
--

DROP TABLE IF EXISTS `community_posts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `community_posts` (
  `uuid` char(36) NOT NULL,
  `user_uuid` char(36) NOT NULL,
  `title` varchar(100) NOT NULL,
  `description` text NOT NULL,
  `post_type_uuid` char(36) NOT NULL,
  `youtube_video_link` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_user_title` (`user_uuid`,`title`),
  KEY `post_type_uuid` (`post_type_uuid`),
  CONSTRAINT `community_posts_ibfk_1` FOREIGN KEY (`user_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `community_posts_ibfk_2` FOREIGN KEY (`post_type_uuid`) REFERENCES `community_post_types` (`uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `community_posts`
--

LOCK TABLES `community_posts` WRITE;
/*!40000 ALTER TABLE `community_posts` DISABLE KEYS */;
/*!40000 ALTER TABLE `community_posts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `message_types`
--

DROP TABLE IF EXISTS `message_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `message_types` (
  `uuid` char(36) NOT NULL,
  `message_name` varchar(50) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `message_name` (`message_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `message_types`
--

LOCK TABLES `message_types` WRITE;
/*!40000 ALTER TABLE `message_types` DISABLE KEYS */;
/*!40000 ALTER TABLE `message_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `model_availability`
--

DROP TABLE IF EXISTS `model_availability`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `model_availability` (
  `uuid` char(36) NOT NULL,
  `availability_name` varchar(50) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `availability_name` (`availability_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `model_availability`
--

LOCK TABLES `model_availability` WRITE;
/*!40000 ALTER TABLE `model_availability` DISABLE KEYS */;
/*!40000 ALTER TABLE `model_availability` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `model_categories`
--

DROP TABLE IF EXISTS `model_categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `model_categories` (
  `uuid` char(36) NOT NULL,
  `model_uuid` char(36) NOT NULL,
  `category_uuid` char(36) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_model_category` (`model_uuid`,`category_uuid`),
  KEY `category_uuid` (`category_uuid`),
  CONSTRAINT `model_categories_ibfk_1` FOREIGN KEY (`model_uuid`) REFERENCES `models` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `model_categories_ibfk_2` FOREIGN KEY (`category_uuid`) REFERENCES `categories` (`uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `model_categories`
--

LOCK TABLES `model_categories` WRITE;
/*!40000 ALTER TABLE `model_categories` DISABLE KEYS */;
/*!40000 ALTER TABLE `model_categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `model_faq_section`
--

DROP TABLE IF EXISTS `model_faq_section`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `model_faq_section` (
  `uuid` char(36) NOT NULL,
  `user_origin_uuid` char(36) NOT NULL,
  `user_target_uuid` char(36) DEFAULT NULL,
  `model_uuid` char(36) NOT NULL,
  `message_type_uuid` char(36) NOT NULL,
  `message_text` text NOT NULL,
  `created_at` datetime NOT NULL,
  PRIMARY KEY (`uuid`),
  KEY `user_origin_uuid` (`user_origin_uuid`),
  KEY `user_target_uuid` (`user_target_uuid`),
  KEY `model_uuid` (`model_uuid`),
  KEY `message_type_uuid` (`message_type_uuid`),
  CONSTRAINT `model_faq_section_ibfk_1` FOREIGN KEY (`user_origin_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `model_faq_section_ibfk_2` FOREIGN KEY (`user_target_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `model_faq_section_ibfk_3` FOREIGN KEY (`model_uuid`) REFERENCES `models` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `model_faq_section_ibfk_4` FOREIGN KEY (`message_type_uuid`) REFERENCES `message_types` (`uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `model_faq_section`
--

LOCK TABLES `model_faq_section` WRITE;
/*!40000 ALTER TABLE `model_faq_section` DISABLE KEYS */;
/*!40000 ALTER TABLE `model_faq_section` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `model_pictures`
--

DROP TABLE IF EXISTS `model_pictures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `model_pictures` (
  `uuid` char(36) NOT NULL,
  `model_uuid` char(36) NOT NULL,
  `picture_location` varchar(254) NOT NULL,
  `picture_width` int(11) NOT NULL,
  `picture_height` int(11) NOT NULL,
  `created_at` datetime NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_model_picture` (`model_uuid`,`picture_location`),
  CONSTRAINT `model_pictures_ibfk_1` FOREIGN KEY (`model_uuid`) REFERENCES `models` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`picture_width` > 0 and `picture_height` > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `model_pictures`
--

LOCK TABLES `model_pictures` WRITE;
/*!40000 ALTER TABLE `model_pictures` DISABLE KEYS */;
/*!40000 ALTER TABLE `model_pictures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `model_review_types`
--

DROP TABLE IF EXISTS `model_review_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `model_review_types` (
  `uuid` char(36) NOT NULL,
  `review_name` varchar(50) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `review_name` (`review_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `model_review_types`
--

LOCK TABLES `model_review_types` WRITE;
/*!40000 ALTER TABLE `model_review_types` DISABLE KEYS */;
/*!40000 ALTER TABLE `model_review_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `model_reviews`
--

DROP TABLE IF EXISTS `model_reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `model_reviews` (
  `uuid` char(36) NOT NULL,
  `model_uuid` char(36) NOT NULL,
  `user_uuid` char(36) NOT NULL,
  `review_type_uuid` char(36) NOT NULL,
  `review_text` text NOT NULL,
  `created_at` datetime NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_user_model_review` (`user_uuid`,`model_uuid`),
  KEY `model_uuid` (`model_uuid`),
  KEY `review_type_uuid` (`review_type_uuid`),
  CONSTRAINT `model_reviews_ibfk_1` FOREIGN KEY (`model_uuid`) REFERENCES `models` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `model_reviews_ibfk_2` FOREIGN KEY (`user_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `model_reviews_ibfk_3` FOREIGN KEY (`review_type_uuid`) REFERENCES `model_review_types` (`uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `model_reviews`
--

LOCK TABLES `model_reviews` WRITE;
/*!40000 ALTER TABLE `model_reviews` DISABLE KEYS */;
/*!40000 ALTER TABLE `model_reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `models`
--

DROP TABLE IF EXISTS `models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `models` (
  `uuid` char(36) NOT NULL,
  `name` varchar(100) NOT NULL,
  `user_uuid` char(36) NOT NULL,
  `created_at` datetime NOT NULL,
  `description` text NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `model_availability_uuid` char(36) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `name` (`name`),
  KEY `user_uuid` (`user_uuid`),
  KEY `model_availability_uuid` (`model_availability_uuid`),
  CONSTRAINT `models_ibfk_1` FOREIGN KEY (`user_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `models_ibfk_2` FOREIGN KEY (`model_availability_uuid`) REFERENCES `model_availability` (`uuid`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`price` > 0)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `models`
--

LOCK TABLES `models` WRITE;
/*!40000 ALTER TABLE `models` DISABLE KEYS */;
/*!40000 ALTER TABLE `models` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pending_users`
--

DROP TABLE IF EXISTS `pending_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `pending_users` (
  `uuid` char(36) NOT NULL,
  `user_uuid` char(36) NOT NULL,
  `otp_code` char(8) NOT NULL,
  `otp_created_at` datetime NOT NULL,
  `otp_expiration_date` datetime NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `otp_code` (`otp_code`),
  KEY `user_uuid` (`user_uuid`),
  CONSTRAINT `pending_users_ibfk_1` FOREIGN KEY (`user_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`otp_expiration_date` > `otp_created_at`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pending_users`
--

LOCK TABLES `pending_users` WRITE;
/*!40000 ALTER TABLE `pending_users` DISABLE KEYS */;
/*!40000 ALTER TABLE `pending_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_devices`
--

DROP TABLE IF EXISTS `user_devices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_devices` (
  `uuid` char(36) NOT NULL,
  `device_name` varchar(50) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `device_name` (`device_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_devices`
--

LOCK TABLES `user_devices` WRITE;
/*!40000 ALTER TABLE `user_devices` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_devices` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_followers`
--

DROP TABLE IF EXISTS `user_followers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_followers` (
  `uuid` char(36) NOT NULL,
  `user_origin_uuid` char(36) NOT NULL,
  `user_target_uuid` char(36) NOT NULL,
  `followed_at` datetime NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_unique_follow` (`user_origin_uuid`,`user_target_uuid`),
  KEY `user_target_uuid` (`user_target_uuid`),
  CONSTRAINT `user_followers_ibfk_1` FOREIGN KEY (`user_origin_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `user_followers_ibfk_2` FOREIGN KEY (`user_target_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `chk_no_self_follow` CHECK (`user_origin_uuid` <> `user_target_uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_followers`
--

LOCK TABLES `user_followers` WRITE;
/*!40000 ALTER TABLE `user_followers` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_followers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_model_likes`
--

DROP TABLE IF EXISTS `user_model_likes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_model_likes` (
  `uuid` char(36) NOT NULL,
  `user_uuid` char(36) NOT NULL,
  `model_uuid` char(36) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_user_model_like` (`user_uuid`,`model_uuid`),
  KEY `model_uuid` (`model_uuid`),
  CONSTRAINT `user_model_likes_ibfk_1` FOREIGN KEY (`user_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `user_model_likes_ibfk_2` FOREIGN KEY (`model_uuid`) REFERENCES `models` (`uuid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_model_likes`
--

LOCK TABLES `user_model_likes` WRITE;
/*!40000 ALTER TABLE `user_model_likes` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_model_likes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_roles`
--

DROP TABLE IF EXISTS `user_roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_roles` (
  `uuid` char(36) NOT NULL,
  `role_name` varchar(50) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `role_name` (`role_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_roles`
--

LOCK TABLES `user_roles` WRITE;
/*!40000 ALTER TABLE `user_roles` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_tokens`
--

DROP TABLE IF EXISTS `user_tokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_tokens` (
  `uuid` char(36) NOT NULL,
  `user_uuid` char(36) NOT NULL,
  `user_device_uuid` char(36) NOT NULL,
  `refresh_token` varchar(255) NOT NULL,
  `token_created_at` datetime NOT NULL,
  `token_expiration_date` datetime NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `uq_user_refresh_token` (`user_uuid`,`refresh_token`),
  KEY `user_device_uuid` (`user_device_uuid`),
  CONSTRAINT `user_tokens_ibfk_1` FOREIGN KEY (`user_uuid`) REFERENCES `users` (`uuid`) ON DELETE CASCADE,
  CONSTRAINT `user_tokens_ibfk_2` FOREIGN KEY (`user_device_uuid`) REFERENCES `user_devices` (`uuid`),
  CONSTRAINT `CONSTRAINT_1` CHECK (`token_expiration_date` > `token_created_at`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_tokens`
--

LOCK TABLES `user_tokens` WRITE;
/*!40000 ALTER TABLE `user_tokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_tokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `uuid` char(36) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(100) DEFAULT NULL,
  `nick_name` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password_salt` varchar(255) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `created_at` datetime NOT NULL,
  `user_role_uuid` char(36) NOT NULL,
  PRIMARY KEY (`uuid`),
  UNIQUE KEY `nick_name` (`nick_name`),
  UNIQUE KEY `email` (`email`),
  KEY `user_role_uuid` (`user_role_uuid`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`user_role_uuid`) REFERENCES `user_roles` (`uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-04  0:29:54

CREATE DATABASE IF NOT EXISTS education_centre
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE education_centre;

CREATE TABLE IF NOT EXISTS People (
  PersonId INT NOT NULL AUTO_INCREMENT,
  Role VARCHAR(16) NOT NULL,
  Name VARCHAR(100) NOT NULL,
  Telephone VARCHAR(30) NULL,
  Email VARCHAR(255) NOT NULL,

  StudentSubject1 VARCHAR(100) NULL,
  StudentSubject2 VARCHAR(100) NULL,
  StudentSubject3 VARCHAR(100) NULL,

  TeacherSalary DECIMAL(12,2) NULL,
  TeacherSubject1 VARCHAR(100) NULL,
  TeacherSubject2 VARCHAR(100) NULL,

  AdminSalary DECIMAL(12,2) NULL,
  AdminFullTimeOrPartTime VARCHAR(20) NULL,
  AdminWorkingHours INT NULL,

  PRIMARY KEY (PersonId),
  UNIQUE KEY UQ_People_Email (Email)
);

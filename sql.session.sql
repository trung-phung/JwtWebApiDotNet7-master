CREATE DATABASE PROJECT3_BOUQUET
CREATE TABLE PROJECT3_BOUQUET.USER (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    USER_ROLE CHAR(10),
    F_Name Varchar(20),
    L_Name Varchar(20),
    UserName CHAR(20) UNIQUE,
    DOB DATE,
    Gender CHAR(20),
    P_No INT,
    Address VARCHAR(5000),
    Password CHAR(255)
)
CREATE TABLE PROJECT3_BOUQUET.BOUQUET (
    Bouquet_ID INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(100) UNIQUE,
    Price INT,
    Photo BLOB(3000)
)
CREATE TABLE PROJECT3_BOUQUET.MESSAGES (
    Occasion_ID INT PRIMARY KEY AUTO_INCREMENT,
    Message VARCHAR(5000)
)
CREATE TABLE PROJECT3_BOUQUET.ODERS (
    ID INT PRIMARY KEY AUTO_INCREMENT,
    Receiver_Name VARCHAR(255),
    Delivery_Address VARCHAR(5000),
    Phone INT(10),
    Date DATE,
    User_ID INT,
    Occasion_ID INT,
    Custom_Message VARCHAR(1000),
    Status VARCHAR(20),
    FOREIGN KEY (User_ID) REFERENCES PROJECT3_BOUQUET.USER(ID),
    FOREIGN KEY (Occasion_ID) REFERENCES PROJECT3_BOUQUET.MESSAGES(Occasion_ID)
)


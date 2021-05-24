CREATE TABLE Books (
  Id bigint NOT NULL identity,
  Author varchar(Max) NULL,
  Launch_Date datetime NOT NULL,
  Price decimal NOT NULL,
  Title varchar(max) NULL,
  PRIMARY KEY (id)
) 
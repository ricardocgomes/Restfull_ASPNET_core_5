CREATE TABLE person (
  id bigint NOT NULL identity,
  first_name varchar(80) NOT NULL,
  last_name varchar(80) NOT NULL,
  address varchar(100) NOT NULL,
  gender varchar(20) NOT NULL,
  PRIMARY KEY (id)
) 
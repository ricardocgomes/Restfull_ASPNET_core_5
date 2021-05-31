CREATE TABLE users (
	id bigint NOT NULL identity,
	user_name VARCHAR(50) NOT NULL unique,
	password VARCHAR(130) NOT NULL,
	full_name VARCHAR(120) NOT NULL,
	refresh_token VARCHAR(500) NULL,
	refresh_token_expiry_time DATETIME NULL DEFAULT NULL,
	PRIMARY KEY (id),
)

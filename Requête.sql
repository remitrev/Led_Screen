CREATE USER 'visualstudio'@'localhost' IDENTIFIED BY 'visualstudio';

CREATE DATABASE led_screen;

GRANT ALL PRIVILEGES ON led_screen.* TO 'visualstudio'@'localhost' IDENTIFIED BY 'visualstudio';


CREATE TABLE led_screen.message (
  id CHAR(36) NOT NULL,
  content VARCHAR(255) NOT NULL,
  tag VARCHAR(50) NOT NULL,
  createdDate DATETIME NOT NULL,
  lastUsedDate DATETIME NOT NULL,
  PRIMARY KEY (id),
  UNIQUE KEY unique_content_tag (content, tag)
);

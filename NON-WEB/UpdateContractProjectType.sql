USE CityController_PROD;

UPDATE contracts
  SET Service=72 WHERE Service=73;
UPDATE contracts
  SET Service=74 WHERE Service=75;
DELETE FROM tlk_service WHERE ID=73 OR ID=75;
INSERT INTO tlk_service (ServiceName) VALUES ('LICENSE AGREEMENT');

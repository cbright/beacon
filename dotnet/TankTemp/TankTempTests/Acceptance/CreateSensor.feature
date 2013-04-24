Feature: Create Sensor
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Create a new sensor
	Given I have a sensor as
		| NetworkId | SerialNumber | Description   | Unit | Type              |
		| 1         | 28d702b40043 | TankTempature | C    | TemperatureSensor |
	When I POST to "/api/v1/sensor"
	Then the result HTTP Status code should be "201"
	And the result should have "location" header "/api/v1/sensor/1"

Scenario: Create a new sensor for unknown network
	Given I have a sensor as
		| NetworkId | SerialNumber | Description | Unit | Type              |
		| 500       | 28d702b40043 | TankTemp    | C    | TemperatureSensor |
	When I POST to "/api/v1/sensor"
	Then the result HTTP Status code should be "404"

Scenario: Create new sensor of uknown type
	Given I have a sensor as
		| NetworkId | SerialNumber | Description   | Unit | Type        |
		| 1         | 28d702b40043 | TankTempature | C    | FloatSensor |
	When I POST to "/api/v1/sensor"
	Then the result HTTP Status code should be "400"

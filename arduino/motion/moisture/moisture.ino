#include <Dhcp.h>
#include <Dns.h>
#include <SPI.h>
#include <Ethernet.h>
#include <EthernetServer.h>
#include <EthernetUdp.h>
#include #Timer.h


byte mac[] = {0x00,0xE0,0x36,0xEF,0x5E,0xFE};
unsigned int localPort = 3968;
byte broadcast[] = {0xFF,0xFF,0xFF,0xFF};
EthernetUDP Udp;

int clayPin = A0; //pin for the container with Burleson clay (test)
int pottingPin = A1; //pin for potting soil (control)
double clayVoltage = 0;
double pottingVoltage = 0;
char clayVoltageStr[10];
char pottingVoltageStr[10];
char packetBuffer[UDP_TX_PACKET_MAX_SIZE];

void setup() {  
  Serial.begin(9600);
  Serial.println("Executing Setup");
   
   if(Ethernet.begin(mac) == 0)
   {
      Serial.println("Failed to configure Ethernet using DHCP");
      for(;;)
      {
        ; 
      }
   }
   
   Serial.println("Ethernet configured using DHCP successfully.");
   Serial.println("Local IP: " + Ethernet.localIP());
   
   Udp.begin(localPort);
}

void loop() {
  // read the value from the sensor:
  Serial.println("Reading...");
  
  clayVoltage = analogRead(clayPin) * (5.0 / 1023.0);
  pottingVoltage = analogRead(pottingPin) * (5.0 / 1023.0);
  
  dtostrf(clayVoltage,2,3,clayVoltageStr);
  dtostrf(pottingVoltage,2,3,pottingVoltageStr);
  
 
  
  
  String json = String("[{\"Id\":\"MOISTURE0001\",\"VOLTAGE\":" + String(clayVoltageStr) + "},{\"Id\":\"MOISTURE0002\",\"VOLTAGE\":" + String(pottingVoltageStr) + "}]");
  json.toCharArray(packetBuffer,100);
  
  Udp.beginPacket(broadcast,localPort);
  Udp.write(packetBuffer);
  Udp.endPacket();
  
  delay(30000);
}

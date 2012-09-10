#include <Event.h>
#include <Timer.h>

#include <OneWire.h>



#include <SPI.h>
#include <Dhcp.h>
#include <Dns.h>
#include <Ethernet.h>
#include <EthernetServer.h>
#include <EthernetUdp.h>
#include <util.h>


byte mac[] = {0x00,0xE0,0x36,0xEF,0x5E,0xFE};
unsigned int localPort = 3968;
byte broadcast[] = {0xFF,0xFF,0xFF,0xFF};
EthernetUDP Udp;
const int pir = 2;


OneWire ds(2);//DS18B20 on pin 2

Timer t;

void setup()
{  
   Serial.begin(9600);
   Serial.println("Starting");
   
   //t.every(60000, measureTemperature);
   
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

void loop(){
  
  //t.update();
  measureTemperature();
}

void measureTemperature(){
    int HighByte, LowByte, TReading, SignBit, Tc_100, Whole, Fract;
  
  byte i;
  byte present = 0;
  byte data[12];
  byte addr[8];
  String address;
  char buffer[50];
  
  if ( !ds.search(addr)) {
    Serial.print("No more addresses.\n");
    ds.reset_search();
    return;
  }

  for( i = 0; i < 8; i++) {
    address += String(addr[i],HEX);
  }

  if ( OneWire::crc8( addr, 7) != addr[7]) {
      Serial.print("CRC is not valid!\n");
      return;
  }
  
  if(addr[0] != 0x28) {
      Serial.print("Device family is not recognized: 0x");
      Serial.println(addr[0],HEX);
      return;
  }

  ds.reset();
  ds.select(addr);
  ds.write(0x44,1);         // start conversion, with parasite power on at the end

  delay(1000);     // maybe 750ms is enough, maybe not
  // we might do a ds.depower() here, but the reset will take care of it.

  present = ds.reset();
  ds.select(addr);    
  ds.write(0xBE);         // Read Scratchpad


  for ( i = 0; i < 9; i++) {           // we need 9 bytes
    data[i] = ds.read();
  }
  
  LowByte = data[0];
  HighByte = data[1];
  TReading = (HighByte << 8) + LowByte;
  SignBit = TReading & 0x8000;  // test most sig bit
  if (SignBit) // negative
  {
    TReading = (TReading ^ 0xffff) + 1; // 2's comp
  }
  Tc_100 = (6 * TReading) + TReading / 4;    // multiply by (100 * 0.0625) or 6.25

  Whole = Tc_100 / 100;  // separate off the whole and fractional portions
  Fract = Tc_100 % 100;
  
  String tempature = "";
  
  if (SignBit) // If its negative
  {
    tempature = tempature +  "-";
  }
  
  tempature = tempature + Whole + ".";
  if (Fract < 10)
  {
    tempature = tempature + "0";
  }
  tempature = tempature + Fract;
  
  String json = String("{\"Id\":\"" + address + "\",\"Tempature\":" + tempature + "}");
  json.toCharArray(buffer,50);
  
  Udp.beginPacket(broadcast,localPort);
  Udp.write(buffer);
  Udp.endPacket();
}




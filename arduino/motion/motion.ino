#include <SPI.h>
#include <Dhcp.h>
#include <Dns.h>
#include <Ethernet.h>
#include <EthernetClient.h>
#include <EthernetServer.h>
#include <EthernetUdp.h>
#include <util.h>


int motionDetector = 2;
int onboardLed = 13;
byte mac[] = {0x00,0xE0,0x36,0xEF,0x5E,0xFE};
IPAddress server(192,168,1,8);
boolean latch = false;

EthernetClient client;

void setup()
{
   pinMode(motionDetector,INPUT);
   pinMode(onboardLed,OUTPUT);
   
   Serial.begin(9600);
   Serial.println("Starting");
   
   if(Ethernet.begin(mac) == 0)
   {
      Serial.println("Failed to configure Ethernet using DHCP");
     for(;;)
      ; 
   }
   
   delay(1000);
   
    if(client.connect(server,1337))
    {
       Serial.println("connected");
    }else{
       Serial.println("connection failed"); 
    }
   
   Serial.println("startup complete.");
}

void loop()
{
  if(digitalRead(motionDetector) == HIGH && !latch)
  {
    if(client.connected())
    {
       client.println("POST /bus HTTP/1.0");
       client.println("channel: echo-create");
       client.println("message: Testing"); 
    }
    
    digitalWrite(onboardLed,HIGH);
    if (client.available()) {
      char c = client.read();
      Serial.print(c);
    }
    
    while(client.available() > 0)
    {
      char c = client.read();
      Serial.print(c);
    }
   
    latch = true;
  }
  
  if(!client.connected()){
   client.stop();
   Serial.println("disconnected"); 
  }
}




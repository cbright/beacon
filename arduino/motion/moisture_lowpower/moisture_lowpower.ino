


//Timer Variables
unsigned long freqCounter = 0;
#define PERIOD 30 //Do something every PERIOD seconds currently at 10 minutes

void setup() {  
   Serial.begin(9600); 
   
   Serial.println("Executing Setup");
  
   Serial.println("Enabling 10 minute timer");
   noInterrupts();           // disable all interrupts
   TCCR2A = 0;
   TCCR2B = 0;
   TCNT1  = 0;
   
   OCR1A = 62500;            // compare match register 16MHz/256/2Hz
   TCCR2A |= (1 << WGM12);   // CTC mode
   TCCR2B |= (1 << CS12);    // 256 prescaler 
   TIMSK2 |= (1 << OCIE1A);  // enable timer compare interrupt
   interrupts();             // enable all interrupts
}

ISR(TIMER1_COMPA_vect)          // timer compare interrupt service routine
{
  Serial.println("Waking up")
  
  
  if(freqCounter < PERIOD){
     freqCounter++; 
  }else{
    freqCounter = 0;
    Serial.println("I work up and need to do somthing")
  }
}


void loop() {
  Serial.println("Going into seep mode...")
  SMCR |=
}

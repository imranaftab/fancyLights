#include <SoftwareSerial.h>// import the serial library
SoftwareSerial Genotronex(10, 11); // RX, TX

const byte OnAutoCommand = 0;
const byte SetRGBCommand = 1;
const byte AlwaysOn = 0;
const byte Auto = 1;

int redPin = 9;
int greenPin = 5;
int bluePin = 6;

int inputPin = 2;               // choose the input pin (for PIR sensor)
int pirState = LOW;             // we start, assuming no motion detected
int val = 0;                    // variable for reading the pin status

int redValue = 0;
int greenValue = 0;
int blueValue = 0;
byte payload[3];

int lightOnTrigger = 0;// By default light on trigger is AlwaysOn

void setup() {
  // put your setup code here, to run once:
  pinMode(inputPin, INPUT);     // declare sensor as input
  pinMode(redPin, OUTPUT);
  pinMode(greenPin, OUTPUT);
  pinMode(bluePin, OUTPUT);  
  Genotronex.begin(9600);
  Serial.begin(9600);
}
void loop() {
   val = digitalRead(inputPin);  // read input value
   PIRUpdate(val);
   if (Genotronex.available())
   {
      byte cmd = Genotronex.read();
      Serial.println(cmd);
        
      
      if( cmd == SetRGBCommand)
      {
        Serial.println("SetRGBColor");
        
        Genotronex.readBytes(payload, 3);
        Serial.println(payload[0]);
        Serial.println(payload[1]);
        Serial.println(payload[2]);
        redValue = payload[0];
        greenValue = payload[1];
        blueValue = payload[2];
        setColor(redValue, greenValue, blueValue);
      }
      else if(cmd == OnAutoCommand)
      {
        Serial.println("Set LightOn Trigger");
        lightOnTrigger = Genotronex.read();
        setLightOnTrigger();
      }
   }//end if Genotronex.available()
   delay(100);
}
void setLightOnTrigger()
{
    if(lightOnTrigger == AlwaysOn)
    {
      setColor(redValue, greenValue, blueValue);
    }
    else if(lightOnTrigger == Auto)
    {
      analogWrite(redPin, LOW);
      analogWrite(greenPin, LOW);
      analogWrite(bluePin, LOW);
    }
}
void PIRUpdate(int val)
{
    if(lightOnTrigger == AlwaysOn)
      return;// Light is Always On
      
    if (val == HIGH) {            // check if the input is HIGH
      /// turn LED ON
      setColor(redValue, greenValue, blueValue);
      if (pirState == LOW) 
      {
        Serial.println("Motion detected!");
        pirState = HIGH;
      }
    }else 
    {
      //turn LED OFF
      analogWrite(redPin, LOW);
      analogWrite(greenPin, LOW);
      analogWrite(bluePin, LOW);
      if (pirState == HIGH)
      {
        Serial.println("Motion ended!");
        pirState = LOW;
      }
    }  
}

void setColor(int red, int green, int blue)
{
    redValue = 255 - redValue;
    greenValue = 255 - greenValue;
    blueValue = 255 - blueValue;
  analogWrite(redPin, red);
  analogWrite(greenPin, green);
  analogWrite(bluePin, blue);  
}

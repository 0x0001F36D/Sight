#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266HTTPClient.h>
#include "config.h"
#define GPIO 0
#define ENABLE HIGH
#define DELAY_MS 1000

#define RESX (String("http://")+HOST +":"+PORT +"/data/state/")

ESP8266WiFiMulti WiFiMulti;

void setup()
{
  Serial.begin(9600);
  Serial.println("init");
  delay(10);
  WiFi.mode(WIFI_STA);
  WiFi.begin(WIFI_SSID , WIFI_PASSWORD );
  while (WiFi.status() != WL_CONNECTED)
    delay(500);
  pinMode(GPIO, OUTPUT);
  digitalWrite(GPIO, ENABLE);

}

void loop() {
  if ((WiFiMulti.run() == WL_CONNECTED)) {

    HTTPClient http;
    http.begin(RESX);

    int httpCode = http.GET();

    if (httpCode > 0) {
      if (httpCode == HTTP_CODE_OK) {
        String payload = http.getString();
        if (payload.indexOf("on") != -1)
        {
          digitalWrite(GPIO, !ENABLE);
        }
        else if (payload.indexOf("off") != -1)
        {
          digitalWrite(GPIO, ENABLE);
        }
      }
    }
    else
    {
      //fail
    }

    http.end();
  }

  delay(DELAY_MS);
}

#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>
#include <DHT_U.h>

#include "config.h"

#define DHT_PIN  2
 

const uint32_t DELAY_MS = 1000;

ESP8266WiFiMulti WiFiMulti;
DHT_Unified dht(DHT_PIN, DHT11);

String build_url(float t, float h)
{
  return String("http://")+HOST+":"+PORT+"/data/"+ "p="+ PASSWORD + "&t=" + t + "&h=" + h;
}



void setup()
{
  dht.begin();
  delay(10);
  WiFi.mode(WIFI_STA);
  WiFiMulti.addAP(WIFI_SSID, WIFI_PASSWORD);

}

void loop() {
  if ((WiFiMulti.run() == WL_CONNECTED)) {

    HTTPClient http;
    sensors_event_t event;

    dht.temperature().getEvent(&event);
    float t = isnan(event.temperature) ? 0 : event.temperature;

    dht.humidity().getEvent(&event);
    float h = isnan(event.relative_humidity) ? 0 : event.relative_humidity;
    
    http.begin(build_url(t, h));

    int httpCode = http.GET();

    if (httpCode > 0) {
      if (httpCode == HTTP_CODE_OK) {
        String payload = http.getString();
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

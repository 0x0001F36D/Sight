# Sight 

## Introduction
A lightweight RESTful http server for IoT sensors development project.

## Todo
- [x] RESTful Server
- [x] Sensor
- [ ] Endpoint

## Server Startup Arguments
|Short Name|Full Name|Verbose|Required|Summary|
|:---:|:---:|:---:|:---:|:---:|
|-l|--logpath|Sets the log file path.|✅|  |
|-v|--password|Sets the password.|✅|Only supported characters and numerics.|
|-p|--port|Sets the server binding port.|✅|This value can only be 0 to 65535.
||--help|Display the help screen.|❎|
||--version|Display version infomation|❎|


## RESTful Routing

| Query String | Http Method | Arguments | Features |
|:---|:---:|:---:|:---:|
|/|POST, GET|  N/A | Will be redirect to '/interval=300'. |
|/interval=\{seconds}|POST, GET|  seconds | Gets the latest log every \{seconds} seconds, Suggests value is '300'. |
|/q|GET| N/A |Gets all of the logs. This uri will be redirect to '/q/from=0'.|
|/q/from=\{index} |GET| index |Gets all of the logs from \{index}.
|/q/from=\{index}&count=\{amount} |GET| index, amount |Gets \{amount} logs from \{index}.
|/data/p=\{password}&t=\{temperature}&h=\{humidity}|PUT|temperature , humidity|Updates temperature and humidity when verification passed.

## Packages & Tools

- Packages
  1. NancyFx
  2. CommandLineParser
  3. BouncyCastle
  4. Costura.Fody

- Tools
  1. Postman
  2. Cmder

## Other
Dependency on Windows .NET framework 4.7.1

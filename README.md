## Overview
This project is a weather information service that fetches data from the api.open-meteo.com API and stores it in a database. The architecture may seem overly complex for only four endpoints, but it was designed to demonstrate various concepts and to easily swap service implementations without major issues.

## Key Features:
Fetches and stores weather data for specific locations.
Provides API endpoints to manage weather locations.
## Technologies Used:
Entity Framework
NUnit
FluentAssertions
AutoMapper
Moq
Newtonsoft.Json
Swagger
## Getting Started
To run the application, use the following Docker command:
```bash
docker-compose up --build
```
## Endpoints
- **GET** `/Weather/{locationId}`: Retrieve weather data for a specific location.
- **GET** `/Weather/location`: List all locations.
- **POST** `/Weather/location`: Add a new location.
- **DELETE** `/Weather/location/{locationId}`: Remove a location.
## Project Limitations (known issues)
Due to the limited implementation time (one evening), the project has the following limitations:

Lack of comprehensive unit tests.
- No handling for database downtimes.
- No error handling for faults returned by the HTTP client.
- Not hosted on Azure cloud.
These issues will be addressed in the next version of the application, following technical discussions.

## Notes
The project architecture is more elaborate than necessary for just four endpoints. This "overengineering" was intentional to showcase different architectural concepts. In a larger project, I might add a Repository layer and a Repository.Abstraction layer, but for this project, Entity Framework provides sufficient database abstraction.

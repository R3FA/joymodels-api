# JoyModels API
REST API service for a Flutter 3D model marketplace application. It is built with .NET 8.0 and utilizes MariaDB and RabbitMQ.

- [JoyModels API](#joymodels-api)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Initial Setup](#initial-setup)
    - [Running with Docker](#running-with-docker)
    - [Credentials](#credentials)

## Getting Started

### Prerequisites
Before using the source code, ensure you have the following installed and configured
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (Includes the ASP.NET Core Runtime)
  
Furthermore, you should have the following services running
- [MariaDB Server](https://mariadb.org/download/?t=mariadb&p=mariadb&r=12.1.2&os=Linux&cpu=x86_64&pkg=tar_gz&i=systemd&mirror=bme)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Docker](https://docs.docker.com/engine/) (if you intend to run this inside of a container)

### Initial Setup
Clone the repository
```bash
git clone https://github.com/R3FA/joymodels-api.git
```

### Running with Docker
To build the necessary images and run the provided [compose.yml](compose.yml), simply run the following command
```bash
docker compose up
```

### Credentials

For test purposes, the API backend provides some test accounts for the client applications.

```bash
# Root account
Username: root1
Password: strinG1!

#Another root account
Username: root2
Password: strinG1!

# Admin account
Username: admin1
Password: strinG1!

# Another admin account
Username: admin2
Password: strinG1!

# Normal user account
Username: user1
Password: strinG1!

# Another normal user account
Username: user2
Password: strinG1!
```

**Note:** Check [compose.yml](compose.yaml) to see if you need to set any environment variables explicitly!

**Note:** Check the appsettings.json file of each app to see if you need to set any environment variables explicitly!

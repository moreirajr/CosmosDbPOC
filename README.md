# CosmosDbPOC
A simple POC using Cosmos Db.

## Running the API in a docker container

```shell
   $ docker compose -f docker-compose.yml up -d --build
```

Then navigate to the below URL to view the API definition.

```
    http://localhost:5000/swagger/index.html
```

## Cosmos Db Emulator
Navigate to the URL below:

```
	https://localhost:8081/_explorer/index.html
```

Create 'CosmosPoc' database and 'Products' container, with "id" as partition key (case sensitive).
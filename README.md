This API can be run using Docker:

```bash
git clone https://github.com/samplebug/FibonacciApi
cd FibonacciApi
docker build -t fibonacci-api .
docker run -p 8080:8080 fibonacci-api
```

After launch:

- UI is accessible at `http://localhost:8080/fibonacci`
- API directly accessible at `http://localhost:8080/api/fibonacci?n=1`

Docker was not detached so console will display execution information until closed.

### CI/CD

Project utilizes Github Actions.
Configuration is saved to file `.github/workflows/dotnet.yml`
Current pipeline only tests Ubuntu, but project also successfully ran on CentOS and Windows 10 during development.

### Scaling

Would be best to scale with Kubernetes but no config ready yet.
There's a Docker compose file that can be used to deploy multiple instances for testing.

Configuration file sets up three instances `./FibonacciApi/compose.yaml`
Use this to fire them up:

```bash
docker compose up
```

All instances will run on localhost as before but check what ports they got assigned. :

```bash
docker ps
```

### Cache

Current version uses distributed cache as a precursor to Redis later on.
`Program.cs` contains a commented section for Redis but it cannot work without further setup.

### Logging

Current version uses Serilog. Log files are created as `./FibonacciApi/logs/fibonacciYYYYMMDD.log`
Only console and file sinks implemented right now with possibility to extend to ELK later on.

Log files for running containers can be accessed like this:

```bash
docker exec -it <ContainerID> /bin/bash
cd logs
```

### Monitoring

Application ships with Prometheus, metrics are mapped at /metrics endpoint `http://localhost:8080/metrics`

### Testing

Docker can be used to test locally:

```bash
docker build -t fibonacci-api-tests -f Dockerfile.Tests .
docker run --rm fibonacci-api-tests
```

Testing is also part of CI/CD process and happens automatically with each repository update per configuration in `.github/workflows/dotnet.yml`
# PasswordVault-API
An API with Swagger UI that stores passwords in SQLite DB securely using AES-256.

Add a category

<br/><br/>

## How to run the API using Docker
1. How to run the API using Docker

```bash
docker pull davida01/passwordvault:latest
```

1. Download the Docker image
 
```bash
docker pull davida01/passwordvault:latest
```

2. Run the docker image

```bash
sudo docker run -d \
  -p 8080:8080 \
  --name vault-api \
  -v ~/vault.db:/app/vault.db \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e VAULT_MASTER_KEY="your-super-secret-32-chars-here!" \
  davida01/passwordvault:latest
```
3. Access the API Swagger UI at: http://localhost:8080/swagger/index.html 


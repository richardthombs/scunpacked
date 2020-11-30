docker build -t scdb-api .
docker stop scdb-api
docker run --name scdb-api --rm -p 8080:80 scdb-api

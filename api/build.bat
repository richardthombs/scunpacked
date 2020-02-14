set name=gearstone/scdb-api
set latesttag=%name%:latest
set versiontag=%name%:%npm_package_version%

docker build -t %versiontag% .

docker tag %versiontag% %latesttag%
docker push %versiontag%
docker push %latesttag%

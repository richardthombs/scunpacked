set name=gearstone/scdb-website
set latesttag=%name%:latest
set versiontag=%name%:%npm_package_version%

call ng build --prod
docker build -t %versiontag% .

docker tag %versiontag% %latesttag%
docker push %versiontag%
docker push %latesttag%

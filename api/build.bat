set name=gearstone/scdb-api
set latesttag=%name%:latest
set versiontag=%name%:%npm_package_version%

rmdir dist /s /q
mkdir .\dist\json
xcopy ..\Loader\json .\dist\json /s/e/q/y

docker build -t %versiontag% .

docker tag %versiontag% %latesttag%
docker push %versiontag%
docker push %latesttag%

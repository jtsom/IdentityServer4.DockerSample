
# Docker sample for Identity server 4

This LAB uses IdentityServer4, a MVC client and a MVC Web API in 3 docker containers for a LAB envirionment.
First create a docker machine. In my lab I use docker toolbox on windows with virtual box. Create a Linux docker host using docker-machine.

List you docker hosts virtal machines, start or create the docker host machine: idsrv-host
```
docker-machine ls

docker-machine start idsrv-demo
or
docker-machine create --driver virtualbox idsrv-demo
```

Start a shell (cmd) and connect the docker client to talk to the docker daemon in the docker host call idsrv-demo. 
```
@FOR /f "tokens=*" %i IN ('docker-machine env idsrv-demo') DO @%i

Powershell:
docker-machine.exe env --shell=powershell idsrv-demo | Invoke-Expression
```

Build the container for the host, tag the image as idsrvhost
```
cd C:\src\IdentityServer4.DockerSample2\src\Host
dotnet restore
dotnet publish --framework netcoreapp1.0 --configuration Debug
docker build -t idsrvhost .
```

Some powershell scripts are included to automate the redeploy of container. The script:
* These scripts remove all contrainers based in the current image. 
* Remove the current image
* Rebuild & publish the project
* Build a new docker image
* Start a container from the new image
```
\src\Host\redeploy.ps1
\src\SampleApi\redeploy.ps1
\src\AspNetCoreAuthrntication\redeploy.ps1

Run all at once:
\src\RebuildAll.ps1
```

Once the images are created the LAB environment can be restrated using:
```
\src\StartAll.ps1
```

## Troubleshooting
```
run a shell instead of the app. Run the entrypoint manually to see whats going on
docker run -it  --entrypoint /bin/bash idsrvhost

run a shell inside a running container
docker exec -it <containerid> /bin/bash

To follow the STDOUT of a container api1
docker logs -f api1
```

## Pushing docker images to (private) docker hub
```
docker login

docker tag 72ee3f29dc28 evertmulder/evertrepo:idsrvhost
docker push evertmulder/evertrepo:idsrvhost
```

## Resources
Flow comparison: <https://www.scottbrady91.com/OpenID-Connect/OpenID-Connect-Flows>  
An Introduction to Open ID Connect: <http://wso2.com/library/articles/2014/06/open-id-connect/>  
Hybride flow: <https://leastprivilege.com/2014/10/10/openid-connect-hybrid-flow-and-identityserver-v3/>  

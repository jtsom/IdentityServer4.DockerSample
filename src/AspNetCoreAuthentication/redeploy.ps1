$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
Set-Location $scriptDir

docker-machine.exe env --shell=powershell idsrv-demo | Invoke-Expression

& "$scriptDir\..\SetEnv.ps1"

Write-Host "`nREMOVE CURRENT CONTAINERS" -ForegroundColor Green

$imageName = "mvc_hybride"
$runningDockersStr = docker ps -f ancestor=$imageName -a -q | Out-String
foreach ($dockerId in ($runningDockersStr -split "`n")) {
    if([string]::IsNullOrEmpty($dockerId) -eq $false){
        Write-Host "Removing contrainer. docker rm -f $dockerId"
        $expression = "docker rm -f " + $dockerId
        Invoke-Expression $expression
    }
}

Write-Host "`nREMOVE CURRENT IMAGES" -ForegroundColor Green

$existingImagesStr = docker images -q $imageName  | Out-String
foreach ($imgId in ($existingImagesStr -split "`n")) {
    if([string]::IsNullOrEmpty($imgId) -eq $false){
        Write-Host "Removing image $imageNamem ID $imgId"
        $expression = "docker rmi " + $imgId
        Invoke-Expression $expression
    }
}

Write-Host "`nDOTNET RESTORE" -ForegroundColor Green
dotnet restore
Write-Host "`nDOTNET PUBLISH" -ForegroundColor Green
dotnet publish --framework netcoreapp1.0 --configuration Debug
Write-Host "`nDOCKER BUILD NEW IMAGE" -ForegroundColor Green
docker build -t $imageName .
Write-Host "`nDOCKER RUN CONTAINER FROM NEW IMAGE" -ForegroundColor Green
docker run -d -p 80:3308 -e IDSRVSAMPLE_ENV_IDSRVHOST -e IDSRVSAMPLE_ENV_POSTLOGOUTREDIRECTURI -e IDSRVSAMPLE_ENV_APIURL --name mvc_hybride1 --restart=unless-stopped $imageName
#docker run -it -p 80:3308 -e IDSRVSAMPLE_ENV_IDSRVHOST -e IDSRVSAMPLE_ENV_POSTLOGOUTREDIRECTURI --name mvc_hybride1  --entrypoint /bin/bash $imageName

Write-Host "`nDone. Showing docker ps" -ForegroundColor Green

docker ps -a

$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
docker-machine start idsrv-demo
$dockerHostIp = docker-machine.exe ip idsrv-demo
Write-Host "Docker host is running on http://$dockerHostIp/"

docker-machine.exe env --shell=powershell idsrv-demo | Invoke-Expression

& "$scriptDir\SetEnv.ps1"

docker start sampleapi1
docker start mvc_hybride1
docker start idsrvhost1


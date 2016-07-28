$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$dockerHostIp = docker-machine.exe ip idsrv-demo
Write-Host "Open http://$dockerHostIp/"

& "$scriptDir\AspNetCoreAuthentication\redeploy.ps1"
& "$scriptDir\Host\redeploy.ps1"
& "$scriptDir\SampleApi\redeploy.ps1"

Write-Host "Open http://$dockerHostIp/"
Start-Process -Path "http://$dockerHostIp/"
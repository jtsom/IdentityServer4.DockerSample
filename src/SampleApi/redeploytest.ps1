
Write-Host "InvocationName:" $MyInvocation.InvocationName
Write-Host "Path:" $MyInvocation.MyCommand.Path

$scriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
Write-Host "Path: $scriptDir"
docker-machine.exe env --shell=powershell idsrv-demo | Invoke-Expression


& "$scriptDir\..\SetEnv.ps1"

Write-Host "$env:IDSRVSAMPLE_ENV_REDIRECTURI" -ForegroundColor Green

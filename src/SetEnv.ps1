Write-Host "`nSetting environment" -ForegroundColor Green

$dockerHostIp = docker-machine.exe ip idsrv-demo
$env:IDSRVSAMPLE_ENV_IDSRVHOST="http://$($dockerHostIp):1941/" 
$env:IDSRVSAMPLE_ENV_POSTLOGOUTREDIRECTURI="http://$($dockerHostIp)/" 
$env:IDSRVSAMPLE_ENV_REDIRECTURI="http://$($dockerHostIp)/signin-oidc" 
$env:IDSRVSAMPLE_ENV_APIURL="http://$($dockerHostIp):1773/Identity" 
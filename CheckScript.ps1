Param(
	[String]$baseUrl,
	[String]$authenTokenScope,
	[String]$serviceBaseUrl,
	[String]$authTokenUrl,
	[String]$authTokenRedirectUrl,
	[String]$clientId,
	[String]$clientSecret,
	[String]$timeoutSeconds,
	[String]$retryCount,
	[String]$apiResouce,
	[String]$graphAPIResourceUrl,
	[String]$tokenEndpointUrl,
	[String]$sapBaseUrl,
	[String]$devopsPatToken

)

Write-Host "line 1"

if (-Not $baseUrl) {
	Write-Host "no base url set"
	exit 1
}
if (-Not $clientId) {
	Write-Host "no client id set"
	exit 1
}
if (-Not $clientSecret) {
	Write-Host "no client secret set"
	exit 1
}

if(-Not $timeoutSeconds){
	$timeoutSeconds = "30"
}

if(-Not $retryCount){
	$retryCount = "2"
}

Write-Host "line 2"
$currentLocation = Get-Location

Write-Host $currentLocation
  
$configfile = Get-Item -Path 'config.json' -Force

$configJson = Get-Content $configfile -raw | ConvertFrom-Json

Write-Host "Original Config File"
Write-Host "++++++++++++++++++++++"
Get-Content $configfile

$configJson | % {
	$_.BaseUrl=$baseUrl
	$_.AuthenTokenScope=$authenTokenScope
	$_.ServiceBaseUrl=$serviceBaseUrl
	$_.AuthTokenUrl=$authTokenUrl
	$_.AuthTokenRedirectUrl=$authTokenRedirectUrl
	$_.ClientId=$clientId
	$_.ClientSecret=$clientSecret
	$_.TimeoutSeconds=$timeoutSeconds
	$_.RetryCount=$retryCount
	$_.ApiResource=$apiResouce
	$_.GraphAPIResourceUrl=$graphAPIResourceUrl
	$_.TokenEndpointUrl=$tokenEndpointUrl
	$_.SapBaseUrl=$sapBaseUrl
	$_.DevopsPatToken=$devopsPatToken
	
	
}
$configJson | ConvertTo-Json -depth 32| set-content $configfile

Write-Host "                     "
Write-Host "Updated Config File"
Write-Host "++++++++++++++++++++++"
Get-Content $configfile

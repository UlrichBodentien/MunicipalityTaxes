$ServiceName = "MunicipalityTaxService"

Write-Host "Stopping service"
sc.exe stop $ServiceName
Write-Host "Removing service"
sc.exe delete $ServiceName
Write-Host "Removed service"
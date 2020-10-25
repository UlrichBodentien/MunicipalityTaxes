dotnet publish -c Release -r win-x64 --self-contained

$ServiceName = "MunicipalityTaxService"
$OutputBinary = Resolve-Path -Path "MunicipalityTaxes.Producer\bin\Release\netcoreapp3.1\win-x64\MunicipalityTaxes.Producer.exe"

Write-Host "Creating service"
New-Service -Name $ServiceName -BinaryPathName $OutputBinary
Write-Host "Starting service"
Start-Service -Name $ServiceName
Write-Host "Started service"
$registryPath = "HKEY_CLASSES_ROOT\.csv\shell\"
$registryName = "GenerateKql"

New-Item –Path "HKCU:\dummy" –Name NetwrixKey

$Name = "(Default)"
$value = "Generate KQL"
New-ItemProperty -Path $registryPath -Name $name -Value $value  -PropertyType ExpandString -Force | Out-Null
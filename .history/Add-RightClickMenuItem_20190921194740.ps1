$registryPath = "HKEY_CLASSES_ROOT\.csv\shell\"
$registryName = "GenerateKql"

New-Item -Path $registryPath -Name GenerateKql

$Name = "(Default)"
$value = "Generate KQL"
New-ItemProperty -Path "$registryPath\$registryName" -Name $name -Value $value  -PropertyType ExpandString -Force | Out-Null
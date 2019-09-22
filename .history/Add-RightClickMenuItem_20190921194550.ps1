$registryPath = "HKEY_CLASSES_ROOT\.csv\shell\"
$registryName = "GenerateKql"

New-Item –Path $registryPath –Name $registryName

$Name = "(Default)"
$value = "Generate KQL"
New-ItemProperty -Path $registryPath -Name $name -Value $value  -PropertyType ExpandString -Force | Out-Null
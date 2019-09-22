$registryPath = "HKEY_CLASSES_ROOT\.csv\shell\GenerateKql"

$Name = "(Default)"

$value = "1"

New-ItemProperty -Path $registryPath -Name $name -Value $value  -PropertyType ExpandString -Force | Out-Null
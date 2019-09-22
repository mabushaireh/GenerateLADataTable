$extensionPath = "shell"
$registryPath = "HKEY_CLASSES_ROOT\.csv"
New-Item -Path $registryPath -Name $extensionPath


$registryName = "GenerateKql"

New-Item -Path "$registryPath\$extensionPath" -Name $registryName

$Name = "(Default)"
$value = "Generate KQL"
New-ItemProperty -Path "$registryPath\$registryName" -Name $name -Value $value  -PropertyType ExpandString -Force | Out-Null
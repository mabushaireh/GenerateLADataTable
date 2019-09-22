New-PSDrive -Name HKCR -PSProvider Registry -Root HKEY_CLASSES_ROOT
$extensionPath = "shell"
$registryPath = "HKLM:\.csv"
New-Item -Path $registryPath -Name $extensionPath


$registryName = "GenerateKql"

New-Item -Path "$registryPath\$extensionPath" -Name $registryName

$Name = "(Default)"
$value = "Generate KQL"
New-ItemProperty -Path "$registryPath\$registryName" -Name $name -Value $value  -PropertyType ExpandString -Force | Out-Null
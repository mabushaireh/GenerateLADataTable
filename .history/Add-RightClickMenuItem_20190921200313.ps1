New-PSDrive -Name HKCR -PSProvider Registry -Root HKEY_CLASSES_ROOT
$shellPath = "shell"
$registryPath = "HKCR:\.csv"
New-Item -Path $registryPath -Name $shellPath


$registryName = "GenerateKql"

New-Item -Path "$registryPath\$shellPath" -Name $registryName

$Name = "(Default)"
$value = "Generate KQL"
New-ItemProperty -Path "$registryPath\$shellPath\$registryName" -Name $name -Value $value  -PropertyType ExpandString -Force | Out-Null

New-Item -Path "$registryPath\$shellPath" -Name "command"

New-ItemProperty -Path "$registryPath\$shellPath\$registryName\command" -Name $Name -Value 'C:\Program Files\Microsoft VS Code\Code.exe" "%1"'  -PropertyType ExpandString -Force | Out-Null


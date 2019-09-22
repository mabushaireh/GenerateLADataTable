$registryPath = "HKEY_CLASSES_ROOT\.csv\shell\ContextMenuHandlers"

$Name = "Version"

$value = "1"

New-ItemProperty -Path $registryPath -Name $name -Value $value `

    -PropertyType DWORD -Force | Out-Null
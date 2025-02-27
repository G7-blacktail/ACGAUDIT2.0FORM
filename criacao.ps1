# Coletar informações do sistema
$uuid = (Get-CimInstance -ClassName Win32_ComputerSystemProduct).UUID
$serialNumber = (Get-CimInstance -ClassName Win32_BIOS).SerialNumber
$hostname = $env:COMPUTERNAME

# Combinar os valores
$combinedString = $uuid + $serialNumber + $hostname

# Criar hash SHA256
$sha256 = [System.Security.Cryptography.SHA256]::Create()
try {
    $hashBytes = $sha256.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($combinedString))
    $hashString = [BitConverter]::ToString($hashBytes) -replace '-'
}
finally {
    $sha256.Dispose()
}

# Definir caminho e garantir que o diretório existe
$outputPath = "$env:APPDATA\ACG Audit\acg audit files\Etiqueta.txt"
$directory = [System.IO.Path]::GetDirectoryName($outputPath)
if (-not (Test-Path -Path $directory)) {
    New-Item -Path $directory -ItemType Directory -Force | Out-Null
}

# Salvar o hash em arquivo
$hashString | Out-File -FilePath $outputPath -Encoding utf8

Write-Host "Hash gerado e salvo em: $outputPath"
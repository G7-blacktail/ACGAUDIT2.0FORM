# # Este script gera um UUID para o acg audit de forma que não colida com nenhum outro UUID

# # Obter o número de série do disco rígido
# $serial = (Get-WmiObject Win32_DiskDrive | Select-Object -ExpandProperty SerialNumber).TrimEnd('.') -join ""
# # Write-Host "Serial: $serial"

# # Obter o nome do computador
# $hostname = $env:COMPUTERNAME
# # Write-Host "HOST: $hostname"

# # Obter o UUID do computador
# $uuid = (Get-WmiObject Win32_ComputerSystemProduct | Select-Object -ExpandProperty UUID).Trim()
# # Write-Host "UUID: $uuid"

# # Concatenar os valores
# $concatenatedInput = "$serial$hostname$uuid"
# # Write-Host "Input: $concatenatedInput"

# # Gerar o hash SHA256
# $hash = Get-FileHash -InputStream ([System.IO.MemoryStream]::new([System.Text.Encoding]::UTF8.GetBytes($concatenatedInput))) -Algorithm SHA256
# Write-Host "Hash: $($hash.Hash)"


# Este script gera um UUID para o acg audit de forma que não colida com nenhum outro UUID

# Função para solicitar um nome com mais de 20 caracteres
function Get-ValidName {
    do {
        $name = Read-Host "Por favor, insira um nome com mais de 20 caracteres"
    } while ($name.Length -le 20)
    return $name
}

# Obter o número de série do disco rígido
$serial = (Get-WmiObject Win32_DiskDrive | Select-Object -ExpandProperty SerialNumber).TrimEnd('.') -join ""

# Obter o nome do computador
$hostname = $env:COMPUTERNAME

# Obter o UUID do computador
$uuid = (Get-WmiObject Win32_ComputerSystemProduct | Select-Object -ExpandProperty UUID).Trim()

# Concatenar os valores
$concatenatedInput = "$serial$hostname$uuid"

# Gerar o hash SHA256
$hash = Get-FileHash -InputStream ([System.IO.MemoryStream]::new([System.Text.Encoding]::UTF8.GetBytes($concatenatedInput))) -Algorithm SHA256

# Solicitar um nome válido
$name = Get-ValidName

# Caminho para salvar o arquivo no diretório especificado
$appDataPath = [System.IO.Path]::Combine($env:APPDATA, 'ACG Audit\acg audit files')
$filePath = [System.IO.Path]::Combine($appDataPath, 'etiqueta.txt')

# Criar o diretório se não existir
if (-not (Test-Path -Path $appDataPath)) {
    New-Item -ItemType Directory -Path $appDataPath | Out-Null
}

# Criar o conteúdo a ser salvo
$content = @"
$name
$($hash.Hash)
"@

# Salvar o conteúdo no arquivo
Set-Content -Path $filePath -Value $content

# Exibir no console o que está sendo salvo
Write-Host "O seguinte conteúdo foi salvo no arquivo $filePath :"
Write-Host $content
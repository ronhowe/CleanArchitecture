param(
    [switch]
    $Loop = $false
)

$ProgressPreference = "SilentlyContinue"

Clear-Host

do {
    Invoke-Pester -Path ".\Pester.ps1" -Output Detailed

    Start-Sleep -Seconds 3
}
while ($Loop)

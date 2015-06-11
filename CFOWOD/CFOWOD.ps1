# Modified from http://psget.net/GetPsGet.ps1 - Thank You!
function Install-CFOWOD {
    $ModulePaths = @($env:PSModulePath -split ';')

    $ExpectedUserModulePath = Join-Path -Path ([Environment]::GetFolderPath('MyDocuments')) -ChildPath WindowsPowerShell\Modules
    $Destination = $ModulePaths | Where-Object { $_ -eq $ExpectedUserModulePath }
    if (-not $Destination) {
        $Destination = $ModulePaths | Select-Object -Index 0
    }
    
    New-Item -Path ($Destination + "\CFOWOD\") -ItemType Directory -Force | Out-Null
    Write-Host 'Downloading CFOWOD from https://raw.githubusercontent.com/marckassay/CFO/master/CFOWOD/CFOWOD.psm1'
    $client = (New-Object Net.WebClient)
    $client.Proxy.Credentials = [System.Net.CredentialCache]::DefaultNetworkCredentials
    $client.DownloadFile("https://raw.githubusercontent.com/marckassay/CFO/master/CFOWOD/CFOWOD.psm1", $Destination + "\CFOWOD\CFOWOD.psm1")

    $executionPolicy = (Get-ExecutionPolicy)
    $executionRestricted = ($executionPolicy -eq "Restricted")
    if ($executionRestricted) {
        Write-Warning @"
Your execution policy is $executionPolicy, this means you will not be able import or use any scripts including modules.
To fix this change your execution policy to something like RemoteSigned.

        PS> Set-ExecutionPolicy RemoteSigned

For more information execute:

        PS> Get-Help about_execution_policies

"@
    }

    if (!$executionRestricted) {
        # ensure CFOWOD is imported from the location it was just installed to
        Import-Module -Name $Destination\CFOWOD
    }
    Write-Host "CFOWOD is installed and ready to use" -Foreground Green
    Write-Host @"
USAGE:
	PS> Get-WOD
	PS> Get-WOD "4/25/15"
	PS> Get-WOD "4/25/15 -2"
	PS> Get-WOD "4/13/15 +2"
	PS> Get-WOD "*"

"@ -Foreground Green
}

Install-CFOWOD
function Get-WOD
{
	[CmdletBinding()]
	param(
	  [Parameter(Mandatory=$False)]
	   [string]$Date
	)

    begin {

    }

    process {
        $proxy = New-WebServiceProxy -Uri http://cfo.cloudapp.net/Service1.svc?wsdl
        
        $response = $proxy.GetWOD()

        Write-Host $response.Title
        Write-Host "----------------"
        Write-Host $response.Body
    }

    end {

    }
}
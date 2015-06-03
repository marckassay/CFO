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
        $proxy = New-WebServiceProxy -Uri http://cfo.cloudapp.net/Service.svc?wsdl
        #$proxy = New-WebServiceProxy -Uri http://localhost:56159/Service.svc?wsdl
        
        $response = $proxy.GetWOD()

        Write-Host $response.Title
        Write-Host "----------------"
        Write-Host $response.Body
		Write-Host ""
    }

    end {

    }
}
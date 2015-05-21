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
        $proxy = New-WebServiceProxy -Uri http://localhost:54267/Service.svc?wsdl
        
        $response = $proxy.GetWOD()

        Write-Host $response.title
        Write-Host "----------------"
        Write-Host $response.body
    }

    end {

    }
}
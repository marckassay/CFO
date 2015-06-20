function Get-WOD
{
	[CmdletBinding()]
	param(
	  [Parameter(Mandatory=$False)]
	   [string]$DateEx
	)

    begin {

    }

    process {
        $proxy = New-WebServiceProxy -Uri http://cfo.cloudapp.net/Service.svc?wsdl
        #$proxy = New-WebServiceProxy -Uri http://localhost:56159/Service.svc?wsdl
        
        $response = $proxy.GetWOD($DateEx)
        
        ForEach ($element in $response) 
        {
            if($element.IsEmpty -ne "false")
            {
                Write-Host $element.Title -Foreground Green
                Write-Host "----------------" -Foreground Green
                Write-Host $element.Body 
                Write-Host ""
                Write-Host ""
            }
        }
    }

    end {

    }
}
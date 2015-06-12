# CFOService
This is a Microsoft Azure solution that scrapes data daily from crossfitorlando.com.  The data stored are WODs (Workout Of the Day).  With this solution active on Azure, clients can query cloud service to retrieve one WOD or multiple WODs.  The following is the service URL: http://cfo.cloudapp.net/Service.svc
<<<<<<< HEAD
=======

Currently data is stored dating back to 12/15/2014.
>>>>>>> 706a76c4eb510b1c3f32f3dca79bec6475b3da59

Currently data is stored dating back to 12/15/2014.

This solution consists of the following 4 projects:
## CFOWOD
This is a PowerShell module that is acting as a client to query the cloud service.  To install automatically, run the following script (PowerShell must be relaxed with security):

	(new-object Net.WebClient).DownloadString("https://raw.githubusercontent.com/marckassay/CFO/master/CFOWOD/CFOWOD.ps1") | iex

To install manually:
<<<<<<< HEAD
* Create a CFOWOD folder in your PowerShell module directory.  For an example: C:\Users\Marc\Documents\WindowsPowerShell\Modules\CFOWOD 
* Download CFOWOD.psm1 into the CFOWOD folder: https://raw.githubusercontent.com/marckassay/CFO/master/CFOWOD/CFOWOD.psm1
=======
	- Create a CFOWOD folder in your PowerShell module directory.  For an example: C:\Users\Marc\Documents\WindowsPowerShell\Modules\CFOWOD 
	- Download CFOWOD.psm1 into the CFOWOD folder: https://raw.githubusercontent.com/marckassay/CFO/master/CFOWOD/CFOWOD.psm1
>>>>>>> 706a76c4eb510b1c3f32f3dca79bec6475b3da59

## CloudService
This project is the Azure cloud service that hosts ServiceWebRole.

## ScraperWebJob
This WebJob project typically runs daily on Azure which scrapes the WOD web page on crossfitorlando.com.  This project uses HtmlAgility for scraping which will marshal the data into a WOD entity.  The entity is then stored into Azure Storage as a Table (NoSQL key-attribute data store). 

## ServiceWebRole
This is the SOAP web service project on Azure.  Interface contains `GetWOD` method:
	
	namespace ServiceWebRole
	{
		[ServiceContract]
		public interface IService
		{
			[OperationContract]
			IEnumerable<WOD> GetWOD(string DateEx);
		}
	}
	
	
The `DateEx` (date expression) parameter can take the following expressions:
	
	// returns a WOD for April 25th 2015
	"4/25/15"

	// returns WOD objects for April 23rd-25th of 2015.  
	"4/25/15 -2"

	// returns WOD objects for April 25th-27th of 2015. 
	"4/13/15 +2"

	// returns a random WOD object 
	"*"
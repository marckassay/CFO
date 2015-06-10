# CFOService
This is a Microsoft Azure solution that scrapes data daily from crossfitorlando.com.  The data is stored are WODs (Workout Of the Day).  With this solution active on Azure, clients can query cloud service to retrieve one WOD or multiple WODs.  The following is the service URL: http://cfo.cloudapp.net/Service.svc

Currently this project consists of the following 4 projects:
## CFOWOD
This is a PowerShell module that is acting a client to query the cloud service.

## CloudService
This is the Azure cloud service that hosts ServiceWebRole.

## ScraperWebJob
This WebJob typically runs daily on Azure which scrapes the WOD web page on crossfitorlando.com.  This project uses HtmlAgility for scraping which will marshal the data into a WOD entity.  The entity is then stored into Azure Storage as a Table (NoSQL key-attribute data store). 

## ServiceWebRole
This is a SOAP web service on Azure.  Interface contains GetWOD method:
	```
	namespace ServiceWebRole
	{
		[ServiceContract]
		public interface IService
		{
			[OperationContract]
			IEnumerable<WOD> GetWOD(string DateEx);
		}
	}
	```
	
The DateEx (date expression) parameter can take the following expressions:
	```
	// returns a WOD for April 25th 2015
	"4/25/15"

	// returns WOD objects for April 23rd-25th of 2015.  
	"4/25/15 -2"

	// returns WOD objects for April 25th-27th of 2015. 
	"4/13/15 +2"

	// returns a random WOD object 
	"*"
	```
	
Currently data is only stored dating back to 12/16/2014.